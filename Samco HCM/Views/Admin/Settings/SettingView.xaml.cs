using System;
using System.Windows;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using Samco_HCM.Classes;
using Samco_HCM_Shared.Classes;
using SamcoSoft = Samco_HCM_Shared.SamcoSoftShared;

namespace Samco_HCM.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView
    {
        public SettingView()
        {
            InitializeComponent();
            DataContext = SamcoSoft.LoadedSettings;
        }

        private void SettingView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (SamcoSoft.LoadedSettings?.DatabaseType != HCMSettings.DatabaseTypes.NotSet)
            {
                switch (SamcoSoft.LoadedSettings?.DatabaseType)
                {
                    case HCMSettings.DatabaseTypes.NotSet:
                        DatabaseWarn.Visibility = Visibility.Visible;
                        break;
                    case HCMSettings.DatabaseTypes.MicrosoftSQLServer:
                        ServerButton.IsChecked = true;
                        MsSqlButton.IsSelected = true;
                        break;
                    case HCMSettings.DatabaseTypes.MicrosoftSQLLocal:
                        LocalRadioButton.IsChecked = true;
                        MsSqlLocal.IsSelected = true;
                        break;
                    case HCMSettings.DatabaseTypes.MySql:
                        ServerButton.IsChecked = true;
                        MySQlButton.IsSelected = true;
                        break;
                    case HCMSettings.DatabaseTypes.Access:
                        LocalRadioButton.IsChecked = true;
                        AccessLocal.IsSelected = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //Read Center Information
            if (SamcoSoft.LoadedSettings.IsClinic)
            {
                ClinicButton.IsChecked = true;
                ClinicBx.Text = SamcoSoft.LoadedSettings.MarkazName;
                AddressBx.Text = SamcoSoft.LoadedSettings.MarkazAddress;
                PhoneBx.Text = SamcoSoft.LoadedSettings.MarkazPhone;
                HealthCenterInfoBox.SelectedTabIndex = 1;
            }
            else
            {
                HealthCenterButton.IsChecked = true;
                UniversityNameBx.Text = SamcoSoft.LoadedSettings.UniversityName;
                ProvinceNameBx.Text = SamcoSoft.LoadedSettings.ShahrestanName;
                CenterNameBx.Text = SamcoSoft.LoadedSettings.MarkazName;
            }

            UniIconBx.EditValue = SamcoSoft.LoadedSettings.UniversityIcon;
        }

        private void BrowseBtn_Validate(object sender, ValidationEventArgs e)
        {
            if (LocalRadioButton.IsChecked != null && LocalRadioButton.IsChecked.Value && e.Value == null)
                e.SetError("انتخاب محل ایجاد پایگاه داده ضروری است.",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
                e.SetError(null);
        }

        private void BrowseClickBtn_Click(object sender, RoutedEventArgs e)
        {
            var saveFile = new DXSaveFileDialog
            {
                Title = "انتخاب محل پایگاه داده",
                Filter = AccessLocal.IsSelected ? "Access file (*.accdb)|*.accdb" : "Microsoft SQL|*.mdf"
            };
            BrowseBtn.Text = saveFile.ShowDialog().GetValueOrDefault() ? saveFile.FileName : string.Empty;
        }

        private void PasswordBoxEdit_Validate(object sender, ValidationEventArgs e)
        {
            if (AccessLocal.IsSelected && e.Value == null)
                e.SetError("وارد نمودن کلمه عبور برای پایگاه داده Access ضروری است.",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
                e.SetError(null);
        }

        private void ServerName_Validate(object sender, ValidationEventArgs e)
        {
            if (ServerButton.IsChecked.GetValueOrDefault() && e.Value == null)
                e.SetError("وارد کردن نام سرور ضروری است.", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
                e.SetError(null);
        }

        private void ServerDBName_Validate(object sender, ValidationEventArgs e)
        {
            if (ServerButton.IsChecked.GetValueOrDefault() && e.Value == null)
                e.SetError("وارد کردن نام پایگاه داده ضروری است.",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
                e.SetError(null);
        }

        private void ServerUser_Validate(object sender, ValidationEventArgs e)
        {
            if (ServerButton.IsChecked.GetValueOrDefault() && e.Value == null)
                e.SetError("وارد کردن نام کاربری برای اتصال به سرور ضروری است.",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
                e.SetError(null);
        }

        private void ServerPass_Validate(object sender, ValidationEventArgs e)
        {
            if (ServerButton.IsChecked.GetValueOrDefault() && e.Value == null)
                e.SetError("وارد کردن کلمه عبور برای اتصال به سرور ضروری است.",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
                e.SetError(null);
        }

        private void SetDataConBtn_Click(object sender, RoutedEventArgs e)
        {
            SamcoSoft.LoadedSettings!.IsClinic = ClinicButton.IsChecked.GetValueOrDefault();
            if (HealthCenterButton.IsChecked.GetValueOrDefault())
            {
                if (UniversityNameBx.HasValidationError || CenterNameBx.HasValidationError ||
                    ProvinceNameBx.HasValidationError)
                {
                    MainNotify.ShowError("خطا در اطلاعات مرکز",
                        "لطفاً اطلاعات مرکز را به طور کامل وارد کرده و سپس دوباره تلاش کنید.");
                    return;
                }

                //Save Center Information
                SamcoSoft.LoadedSettings.UniversityName = UniversityNameBx.Text;
                SamcoSoft.LoadedSettings.ShahrestanName = ProvinceNameBx.Text;
                SamcoSoft.LoadedSettings.MarkazName = CenterNameBx.Text;
                if (UniIconBx.HasImage) SamcoSoft.LoadedSettings.UniversityIcon = (byte[])UniIconBx.EditValue;
            }
            else
            {
                if (ClinicBx.HasValidationError)
                {
                    MainNotify.ShowError("خطا در اطلاعات مرکز",
                        "لطفاً اطلاعات مرکز درمانی را به طور کامل وارد کرده و سپس دوباره تلاش کنید.");
                    return;
                }

                //Save Center Information
                SamcoSoft.LoadedSettings.MarkazName = ClinicBx.Text;
                SamcoSoft.LoadedSettings.MarkazPhone = PhoneBx.Text;
                SamcoSoft.LoadedSettings.MarkazAddress = AddressBx.Text;
                SamcoSoft.LoadedSettings.UniversityIcon = (byte[])UniIconBx.EditValue;
            }

            //Creating connection string
            if (LocalRadioButton.IsChecked == true)
            {
                //Validate
                if (BrowseBtn.HasValidationError)
                {
                    MainNotify.ShowError("خطا در ارتباط با پایگاه داده", "لطفاً محل فایل پایگاه داده را انتخاب کنید.");
                    return;
                }

                switch (AccessLocal.IsSelected)
                {
                    case true when AcsPassBx.Text == null:
                        MainNotify.ShowError("خطا در ارتباط با پایگاه داده", "لطفاً کلمه عبور را جهت استفاده از پایگاه داده وارد کنید.");
                        return;
                    //Local
                    //Format: 0Type|1Server|2DatabaseName|3User|4Pass|5LocalDB
                    case true:
                        //Access Local Database
                        SamcoSoft.LoadedSettings.DatabaseType = HCMSettings.DatabaseTypes.Access;
                        break;
                    default:
                        {
                            //MSSQL
                            SamcoSoft.LoadedSettings.DatabaseType = HCMSettings.DatabaseTypes.MicrosoftSQLLocal;
                            SamcoSoft.LoadedSettings.ServerAddress = "(localdb)\\MSSQLLocalDB";
                            SamcoSoft.LoadedSettings.DatabaseName = "SamcoHCM";
                            break;
                        }
                }
            }
            else
            {
                //Validate
                if (ServerName.HasValidationError || ServerDbName.HasValidationError || ServerUser.HasValidationError ||
                    ServerPass.HasValidationError)
                {
                    MainNotify.ShowError("خطا در ارتباط با پایگاه داده", "لطفاً تمام اطلاعات خواسته شده را جهت استفاده از پایگاه داده وارد کنید.");
                    return;
                }

                //Server
                SamcoSoft.LoadedSettings.DatabaseType = MsSqlButton.IsSelected
                    ? HCMSettings.DatabaseTypes.MicrosoftSQLServer
                    : HCMSettings.DatabaseTypes.MySql;
            }

            string errorMessage = null;
            if (SamcoSoft.LoadHcmDatabase(SamcoSoft.LoadedSettings.ConnectionString!, false, ref errorMessage))
            {
                SamcoSoft.LoadedSettings.Save();
                WinUIMessageBox.Show(this, "ارتباط با پایگاه داده برقرار شد. لطفاً نرم افزار را یک بار دیگر اجرا کنید.",
                    "ارتباط با پایگاه داده", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None,
                    MessageBoxOptions.None, FloatingMode.Adorner, true);
                Application.Current.Shutdown();
            }
            else
            {
                MainNotify.ShowError("خطا در ارتباط با پایگاه داده", string.Format(
                     "امکان برقراری ارتباط با پایگاه داده وجود ندارد. ممکن است درایورهای مورد نیاز نصب نباشند و یا کلمه عبور را اشتباه وارد نموده‌اید. اطلاعات بیشتر:{0}{0}{1}",
                     '\n', errorMessage));
            }
        }

    }
}

