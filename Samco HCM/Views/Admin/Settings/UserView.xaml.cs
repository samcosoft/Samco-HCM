using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM.Classes;
using Samco_HCM.Views.Settings.UserViews;
using Samco_HCM_Shared;

namespace Samco_HCM.Views.Settings
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : IDisposable
    {
        private readonly Session _session1 = new();
        private XPCollection<Users> _users;
        public UserView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Load data
            _users = new XPCollection<Users>(_session1, CriteriaOperator.And(new BinaryOperator(nameof(Users.group), "O", BinaryOperatorType.NotEqual),
                new BinaryOperator("Oid", SamcoSoftShared.CurrentUser!.Oid, BinaryOperatorType.NotEqual)));
            UserGrid.ItemsSource = _users;
            if (SamcoAdd.UserIsAdmin() == false)
                UserSelectGrp.Visibility = Visibility.Collapsed;
            base.OnNavigatedTo(e);
        }

        public void Dispose()
        {
            _session1?.Dispose();
        }

        private void UserGridUnbound(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Role")
            {
                var user = (Users)UserGrid.GetRowByListIndex(e.ListSourceRowIndex);
                e.Value = user.group switch
                {
                    "O" => "مدیر سیستم",
                    "A" => "مدیر",
                    "U" => "کاربر منشی",
                    _ => "ناشناس"
                };
            }
        }

        private void NewUser_OnClick(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new WinUIDialogWindow("تعریف کاربر جدید")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new UserInfo(new Users(_session1)),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newUserWindow.ShowDialog() == true) _users.Reload();
        }
        private void UserGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserGrid.SelectedItem != null)
            {
                EditUser_OnClick(sender, e);
            }
        }

        private void EditUser_OnClick(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem == null)
            {
                MainNotify.ShowWarning("خطا در ویرایش کاربر", "کاربری انتخاب نشده است.");
                return;
            }
            var newUserWindow = new WinUIDialogWindow("تعریف کاربر جدید")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new UserInfo((Users)UserGrid.SelectedItem),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newUserWindow.ShowDialog() == true) _users.Reload();
        }

        private void DeleteUser_OnClick(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem == null)
            {
                MainNotify.ShowWarning("خطا در حذف کاربر", "کاربری انتخاب نشده است.");
                return;
            }
            var selUser = (Users)UserGrid.SelectedItem;
            if (WinUIMessageBox.Show($"آیا از حذف کاربر {selUser.username} اطمینان دارید؟", "حذف کاربر", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) != MessageBoxResult.Yes) return;
            selUser.Delete();
            _users.Reload();
        }

    }
}
