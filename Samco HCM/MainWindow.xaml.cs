using System;
using System.Linq;
using HandyControl.Tools;
using System.Windows;
using DevExpress.Xpf.Core;
using DevExpress.Xpo;
using HCMData;
using DevExpress.Xpf.WindowsUI;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using HandyControl.Themes;
using Samco_HCM.Views;
using Samco_HCM_Shared.Classes;
using Samco_HCM.Classes;
using Samco_HCM.Views.Settings;
using Samco_HCM_Shared;
using FontFamily = System.Windows.Media.FontFamily;
using System.Timers;
using System.Windows.Media.Imaging;

namespace Samco_HCM
{
    public partial class MainWindow
    {
        private readonly Timer _timer = new(1000);

        public MainWindow()
        {
            InitializeComponent();
            ConfigHelper.Instance?.SetLang("fa-IR");
            if (HandyControl.Themes.ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark)
                ThemeButton.IsChecked = true;

            _timer.Elapsed += ShiftTimer;
            _timer.Enabled = true;
        }

        private void ShiftTimer(object sender, ElapsedEventArgs e)
        {
            if (SamcoSoftShared.LoadedSettings == null) return;
            try
            {
                var startShift = SamcoSoftShared.LoadedSettings!.StartShiftTime ?? new DateTime(2001, 1, 1, 8, 0, 0);
                var endShift = SamcoSoftShared.LoadedSettings!.EndShiftTime ?? new DateTime(2001, 1, 1, 20, 0, 0);

                if (DateTime.Now.TimeOfDay >= startShift.TimeOfDay && DateTime.Now.TimeOfDay < endShift.TimeOfDay)
                {
                    SamcoAdd.OffShift = false;
                }
                else
                {
                    SamcoAdd.OffShift = true;
                }
            }
            catch (Exception)
            {
                //Ignored
            }
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
            if (SamcoSoftShared.LoadedSettings?.AppThemeName != null) ApplicationThemeHelper.ApplicationThemeName = SamcoSoftShared.LoadedSettings?.AppThemeName;
            //Validate License
            if (SamcoAdd.ValidateLicense(out var isTrial) == false || isTrial)
            {
                WaitIndic.IsSplashScreenShown = false;
                SplashScreenManager.CloseAll();
                var activationDialog = new DXDialogWindow("فعالسازی برنامه")
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Content = new LicenseManagerView(),
                    FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                    FontSize = 16,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    MaxHeight = 500,
                    MaxWidth = 820,
                    ResizeMode = ResizeMode.NoResize,
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Images/Lock.png"))
                };
                if (activationDialog.ShowDialog() == false)
                {
                    Application.Current.Shutdown();
                    return;
                }
                SamcoSoftShared.LoadedSettings!.Save();
            }

            if (CheckDatabase() == false)
            {
                //Setup new database
                var dataSett = new SettingView();
                NavFrame.Navigate(dataSett);
                WaitIndic.IsSplashScreenShown = false;
                return;
            }

            //Login / Create User
            using (var session1 = new Session())
            {
                var usr = session1.Query<Users>();
                if (!usr.Any())
                {
                    var adminUser = new Users(session1) { username = "admin", password = BCrypt.Net.BCrypt.EnhancedHashPassword("admin"), group = "Owner", realname = "مدیر سیستم" };
                    adminUser.Save();
                }
            }

            WaitIndic.IsSplashScreenShown = false;
            SplashScreenManager.CloseAll();
            if (LoginUser() == false)
            {
                return;
            }

            //Save backup from setting file
            SamcoSoftShared.LoadedSettings!.SaveBackup();
        }
        private void CreateLoadSetting(string settingDir)
        {
            var settingFile = settingDir + "\\Settings.json";
            //Set Setting File Path
            if (File.Exists(settingFile))
            {
                try
                {
                    SamcoSoftShared.LoadedSettings = SamcoSoftShared.ReadFromJsonFile<HCMSettings>(settingFile);
                }
                catch (Exception)
                {
                    if (File.Exists(settingDir + "\\Settings.old")) File.Delete(settingDir + "\\Settings.old");
                    File.Move(settingFile, settingDir + "\\Settings.old");

                    //check if backup available
                    if (File.Exists(settingDir + "\\Settings.bck"))
                        if (WinUIMessageBox.Show(this, "اطلاعات تنظیمات سیستم مخدوش شده است. نسخه پشتیبان یافت شد. آیا مایل به استفاده از نسخه پشتیبان هستید؟", "بازیابی تنظیمات", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading, FloatingMode.Adorner) == MessageBoxResult.Yes)
                        {
                            File.Move(settingDir + "\\Settings.bck", settingFile);
                            CreateLoadSetting(settingDir);
                            return;
                        }

                    SamcoSoftShared.LoadedSettings = new HCMSettings();
                }
            }
            else
            {
                if (Directory.Exists(settingDir) == false) Directory.CreateDirectory(settingDir!);

                //Check for old setting file
                var oldSettingPath = settingDir + "\\Settings.xml";

                if (File.Exists(oldSettingPath))
                {
                    try
                    {
                        var oldSetting = ReadFromXmlFile<OldSettings>(oldSettingPath);
                        //Transfer all data
                        SamcoSoftShared.LoadedSettings = new HCMSettings
                        {
                            AppThemeName = oldSetting.AppThemeName,
                            DatabaseFilePath = oldSetting.DatabaseFilePath,
                            DatabaseType = Enum.Parse<HCMSettings.DatabaseTypes>(oldSetting.DatabaseType.ToString()),
                            DatabaseName = oldSetting.DatabaseName,
                            DatabasePassword = oldSetting.DatabasePassword,
                            DatabaseUserId = oldSetting.DatabaseUserId,
                            EndShiftTime = oldSetting.EndShiftTime,
                            IsClinic = oldSetting.IsClinic,
                            MarkazAddress = oldSetting.MarkazAddress,
                            MarkazName = oldSetting.MarkazName,
                            MarkazPhone = oldSetting.MarkazPhone,
                            PersonnelRole = oldSetting.PersonnelRole,
                            ServerAddress = oldSetting.ServerAddress,
                            SettingVersion = oldSetting.SettingVersion,
                            ShahrestanName = oldSetting.ShahrestanName,
                            StartShiftTime = oldSetting.StartShiftTime,
                            UniversityName = oldSetting.UniversityName
                        };
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                SamcoSoftShared.LoadedSettings ??= new HCMSettings();
            }

            SamcoSoftShared.LoadedSettings!.SettingDirectory = settingDir;
        }
        private bool CheckDatabase()
        {
            if (SamcoSoftShared.LoadedSettings!.ConnectionString != null)
            {
                string errorMessage = null;
                if (SamcoSoftShared.LoadDatabase(SamcoSoftShared.LoadedSettings.ConnectionString, ref errorMessage)) return true;

                WaitIndic.IsSplashScreenShown = false;
                WinUIMessageBox.Show(GetWindow(this), "در ارتباط با پایگاه داده مشکل زیر رخ داده است. لطفاً تنظیمات پایگاه داده را دوباره بررسی کنید." + "\n" + ('\n' + errorMessage), "بازگردانی تنظیمات", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Adorner, true);
                return false;
            }

            return false;
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
                ProfilePic.Source = SamcoSoftShared.LoadImage(SamcoSoftShared.CurrentUser!.Avatar);
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
                return (T)serializer.ReadObject(reader, true);
            }
            finally
            {
                reader.Close();
                xmlFile.Close();
            }
        }

        private void SetShiftButton_Click(object sender, RoutedEventArgs e)
        {
            var personnelSelector = new WinUIDialogWindow("انتخاب پرسنل شیفت")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new PersonnelSelectorView(),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            personnelSelector.ShowDialog();
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
    }
}
