using System;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM.Classes;
using Samco_HCM_Shared;

namespace Samco_HCM.Views.Settings.UserViews
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo:IDisposable
    {
        private readonly Session _session1 = new();
        private readonly Users _selUser;

        public UserInfo()
        {
            InitializeComponent();
            _selUser = _session1.GetObjectByKey<Users>(SamcoSoftShared.CurrentUser!.Oid);
            DataContext = _selUser;
            UserImageBx.EditValue = _selUser.photo;
            AccountType.SelectedIndex = _selUser.group is "O" or "A" ? 0 : 1;
            CurrentPassGrp.Visibility = Visibility.Visible;
            UserBx.IsEnabled = false;
            AccountType.IsEnabled = false;
            CancelBtnGroup.Visibility = Visibility.Collapsed;
        }

        public UserInfo(Users selUser)
        {
            InitializeComponent();
            _selUser = selUser;
            DataContext = _selUser;
            UserImageBx.EditValue = _selUser.photo;
            AccountType.SelectedIndex = _selUser.group is "O" or "A" ? 0 : 1;
        }

        private void UserBx_OnValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null)
            {
                e.SetError("وارد کردن نام کاربری الزامیست.", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                return;
            }
            if (e.Value.ToString()!.Length < 5)
            {
                e.SetError("نام کاربری باید حداقل 5 کاراکتر باشد.",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                return;
            }
            e.SetError(default);
        }

        private void SaveInfo_OnClick(object sender, RoutedEventArgs e)
        {
            if (UserBx.HasValidationError)
            {
                MainNotify.ShowError("خطا در ثبت اطلاعات", "نام کاربری معتبر نیست.");
                return;
            }

            if (CurrentPassGrp.IsVisible)
            {
                if (string.IsNullOrEmpty(CurrentPassBx.Password))
                {
                    MainNotify.ShowError("خطا در ثبت اطلاعات", "وارد نمودن کلمه عبور فعلی برای ثبت تغییرات کاربر فعلی الزامیست.");
                    return;
                }
                if (!BCrypt.Net.BCrypt.EnhancedVerify(CurrentPassBx.Password, _selUser.password))
                {
                    MainNotify.ShowError("خطا در ثبت اطلاعات", "رمز عبور فعلی اشتباه است.");
                    return;
                }
            }
            //Check for unique username
            var prevUser = _session1.Query<Users>().FirstOrDefault(x => x.username == _selUser.username);
            if (prevUser != null && prevUser.Oid != _selUser.Oid)
            {
                MainNotify.ShowError("خطا در ثبت اطلاعات", "کاربر دیگری با همین نام کاربری وجود دارد.");
                return;
            }
            if (string.IsNullOrEmpty(PassBx.Password) == false)
            {
                if (PassBx.Password != ConfPassBx.Password)
                {
                    MainNotify.ShowError("خطا در ثبت اطلاعات", "کلمه عبور و تکرار آن برابر نیستند.");
                    return;
                }
                _selUser.password = BCrypt.Net.BCrypt.EnhancedHashPassword(PassBx.Password);
            }
            _selUser.photo = (byte[])UserImageBx.EditValue;
            _selUser.group = ((ComboBoxEditItem)AccountType.SelectedItem).Tag.ToString();
            _selUser.Save();

            MainNotify.ShowSuccess("ثبت اطلاعات", "اطلاعات کاربر با موفقیت ثبت شدند.");
            if (Parent.GetType() == typeof(WinUIDialogWindow))
                ((WinUIDialogWindow)Parent).DialogResult = true;
        }

        private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Parent.GetType() == typeof(WinUIDialogWindow))
                ((WinUIDialogWindow)Parent).DialogResult = false;
        }

        public void Dispose()
        {
            _session1?.Dispose();
        }
    }
}
