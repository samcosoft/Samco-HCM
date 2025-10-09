using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HandyControl.Themes;
using HandyControl.Tools;
using LabData;
using Samco_HCM_Laboratory_Client.Classes;
using Samco_HCM_Laboratory_Client.Views;
using Samco_HCM_Laboratory_Client.Views.Settings;
using Samco_HCM_Shared;
using Samco_HCM_Shared.Classes;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using DevExpress.Xpf.Bars;

namespace Samco_HCM_Laboratory_Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{

    private readonly Timer _billTimer = new(TimeSpan.FromSeconds(10));
    private const string SettingFileName = "LabClientSettings";
    public MainWindow()
    {
        InitializeComponent();
        ConfigHelper.Instance?.SetLang("fa-IR");
        if (HandyControl.Themes.ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark)
            ThemeButton.IsChecked = true;

        _billTimer.Elapsed += BillTimer;

    }

    private void BillTimer(object? sender, ElapsedEventArgs e)
    {
        SamcoAdd.UpdateBillsData();
    }

    #region Change Theme
    private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;
    private void ThemeButton_OnChecked(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).SwitchToDarkTheme();
    }
    private void ThemeButton_OnUnchecked(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).SwitchToLightTheme();
    }
    #endregion
    private void MainWindow_OnContentRendered(object sender, EventArgs e)
    {
        var settingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Samco HCM";
        WaitIndic.IsSplashScreenShown = true;
        CreateLoadSetting(settingDir);
        //Set theme
        if (SamcoSoftShared.LoadedSettings?.AppThemeName != null) ApplicationThemeHelper.ApplicationThemeName = SamcoSoftShared.LoadedSettings.AppThemeName;

        if (!CheckDatabase())
        {
            SplashScreenManager.CloseAll();
            WaitIndic.IsSplashScreenShown = false;

            //Setup new database
            var dataSett = new SettingView();
            NavFrame.Navigate(dataSett);
            return;
        }

        //Login / Create User
        using (var session1 = new Session())
        {
            var usr = session1.Query<LabUsers>();
            if (!usr.Any())
            {
                var adminUser = new LabUsers(session1) { username = "admin", password = BCrypt.Net.BCrypt.EnhancedHashPassword("admin"), group = "O", realname = "مدیر سیستم" };
                adminUser.Save();
            }
        }

        WaitIndic.IsSplashScreenShown = false;
        SplashScreenManager.CloseAll();
        if (!LoginUser()) return;

        //Save backup from setting file
        SamcoSoftShared.LoadedSettings!.Save();
        SamcoSoftShared.LoadedSettings.SaveBackup();
        SamcoAdd.CheckNetwork();
        if (SamcoSoftShared.LoadedSettings.ActiveClient) _billTimer.Enabled = true;
    }
    private void CreateLoadSetting(string settingDir)
    {
        var settingFile = $"{settingDir}\\{SettingFileName}.json";
        //Set Setting File Path
        if (File.Exists(settingFile))
        {
            try
            {
                SamcoSoftShared.LoadedSettings = SamcoSoftShared.ReadFromJsonFile<HCMSettings>(settingFile);
            }
            catch (Exception)
            {
                if (File.Exists($"{settingDir}\\{SettingFileName}.old")) File.Delete(settingDir + $"\\{SettingFileName}.old");
                File.Move(settingFile, settingDir + $"\\{SettingFileName}.old");
                File.Move(settingFile, settingDir + $"\\{SettingFileName}.old");

                //check if backup available
                if (File.Exists(settingDir + $"\\{SettingFileName}.bck"))
                    if (WinUIMessageBox.Show(this, "اطلاعات تنظیمات سیستم مخدوش شده است. نسخه پشتیبان یافت شد. آیا مایل به استفاده از نسخه پشتیبان هستید؟", "بازیابی تنظیمات", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading, FloatingMode.Adorner) == MessageBoxResult.Yes)
                    {
                        File.Move(settingDir + $"\\{SettingFileName}.bck", settingFile);
                        CreateLoadSetting(settingDir);
                        return;
                    }

                SamcoSoftShared.LoadedSettings = new HCMSettings("LabClientSettings");
            }
        }
        else
        {
            if (!Directory.Exists(settingDir)) Directory.CreateDirectory(settingDir);

            //Check for old setting file
            var oldSettingPath = settingDir + "\\LabClient.xml";

            if (File.Exists(oldSettingPath))
            {
                try
                {
                    var oldSetting = ReadFromXmlFile<OldSettings>(oldSettingPath);
                    //Transfer all data
                    SamcoSoftShared.LoadedSettings = new HCMSettings("LabClientSettings")
                    {
                        AppThemeName = oldSetting.AppThemeName,
                        DatabaseFilePath = oldSetting.DatabaseFilePath,
                        DatabaseType = Enum.Parse<HCMSettings.DatabaseTypes>(oldSetting.DatabaseType.ToString()),
                        DatabaseName = oldSetting.DatabaseName,
                        DatabasePassword = oldSetting.DatabasePassword,
                        DatabaseUserId = oldSetting.DatabaseUserId,
                        MarkazName = oldSetting.MarkazName,
                        ServerAddress = oldSetting.ServerAddress,
                        SettingVersion = oldSetting.SettingVersion,
                        ShahrestanName = oldSetting.ShahrestanName,
                        UniversityName = oldSetting.UniversityName,
                        RemoteServerAddress = oldSetting.RemoteServerAddress,
                        RemoteDatabaseType = Enum.Parse<HCMSettings.DatabaseTypes>(oldSetting.RemoteDatabaseType.ToString()),
                        RemoteDatabaseName = oldSetting.RemoteDatabaseName,
                        RemoteDatabasePassword = oldSetting.RemoteDatabasePassword,
                        RemoteDatabaseUserId = oldSetting.RemoteDatabaseUserId,
                        ActiveClient = oldSetting.ActiveClient
                    };
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            SamcoSoftShared.LoadedSettings ??= new HCMSettings("LabClientSettings");
        }

        SamcoSoftShared.LoadedSettings!.SettingDirectory = settingDir;
    }
    private bool CheckDatabase()
    {
        if (SamcoSoftShared.LoadedSettings!.ConnectionString == null) return false;

        string? errorMessage = null;
        if (!SamcoAdd.LoadDatabase(SamcoSoftShared.LoadedSettings.ConnectionString, ref errorMessage))
        {
            WaitIndic.IsSplashScreenShown = false;
            SplashScreenManager.CloseAll();
            WinUIMessageBox.Show(GetWindow(this),
                "در ارتباط با پایگاه داده مشکل زیر رخ داده است. لطفاً تنظیمات پایگاه داده را دوباره بررسی کنید." +
                "\n" + ('\n' + errorMessage), "خطا در برقراری ارتباط", MessageBoxButton.OK, MessageBoxImage.Exclamation,
                MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign,
                FloatingMode.Adorner, true);
            return false;
        }

        if (SamcoSoftShared.LoadedSettings.ActiveClient)
        {
            if (SamcoSoftShared.LoadedSettings.RemoteConnectionString == null) return false;
            if (SamcoSoftShared.LoadRemoteHcmDatabase(SamcoSoftShared.LoadedSettings.RemoteConnectionString, true, ref errorMessage, out SamcoAdd.RemoteDatalayer)) return true;

            WaitIndic.IsSplashScreenShown = false;
            SplashScreenManager.CloseAll();
            WinUIMessageBox.Show(GetWindow(this),
                "در ارتباط با پایگاه داده پذیرش مشکل زیر رخ داده است. لطفاً تنظیمات پایگاه داده را دوباره بررسی کنید." +
                "\n" + ('\n' + errorMessage), "خطا در برقراری ارتباط", MessageBoxButton.OK, MessageBoxImage.Exclamation,
                MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign,
                FloatingMode.Adorner, true);
            return false;
        }

        return true;
    }
    private bool LoginUser()
    {
        var loginWindow = new LoginView();

        //Show Login Window
        var loginDial = new WinUIDialogWindow
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = loginWindow,
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };
        var result = loginDial.ShowDialog();
        if (result == false)
        {
            Application.Current.Shutdown();
            return false;
        }
        WaitIndic.IsSplashScreenShown = true;

        NavFrame.Navigate(new Dashboard());
        NavFrame.Journal.ClearNavigationHistory();
        NavFrame.Journal.ClearNavigationCache();

        //Load profile photo
        if (SamcoSoftShared.CurrentUser?.Avatar == null)
        {
            ProfilePic.Id = SamcoSoftShared.CurrentUser!.Username;
        }
        else
        {
            ProfilePic.Source = SamcoSoftShared.LoadImage(SamcoSoftShared.CurrentUser.Avatar);
        }

        WaitIndic.IsSplashScreenShown = false;
        return true;
    }
    public T ReadFromXmlFile<T>(string filePath)
    {
        var xmlFile = new FileStream(filePath, FileMode.Open);
        var reader = XmlDictionaryReader.CreateTextReader(xmlFile, new XmlDictionaryReaderQuotas());
        try
        {
            var serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(reader, true)!;
        }
        finally
        {
            reader.Close();
            xmlFile.Close();
        }
    }

    private void AboutBtn_Click(object sender, RoutedEventArgs e)
    {
        NavFrame.Navigate(new AboutView());
    }

    private void LogOutButton_Click(object sender, RoutedEventArgs e)
    {
        SamcoSoftShared.CurrentUser = null;
        Application.Current.Resources["CurrentUserName"] = string.Empty;
        ProfilePic.Id = string.Empty;
        ProfilePic.Source = null;
        LoginUser();
    }

    private void ServerConnectBtn_OnItemClick(object sender, ItemClickEventArgs e)
    {
        SamcoAdd.UpdateBillsData();
    }
}