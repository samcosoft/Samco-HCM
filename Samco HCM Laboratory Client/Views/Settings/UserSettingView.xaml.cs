using DevExpress.Data.Filtering;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using Samco_HCM_Laboratory_Client.Classes;
using Samco_HCM_Shared;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.WindowsUI.Navigation;
using LabData;
using Samco_HCM_Laboratory_Client.Views.Settings.UserSettingsView;

namespace Samco_HCM_Laboratory_Client.Views.Settings
{
    /// <summary>
    /// Interaction logic for UserSettingView.xaml
    /// </summary>
    public partial class UserSettingView : IDisposable
    {
        private readonly Session _session1 = new();
        private XPCollection<LabUsers>? _users;

        public UserSettingView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Load data
            _users = new XPCollection<LabUsers>(_session1, CriteriaOperator.And(new BinaryOperator(nameof(LabUsers.group), "O", BinaryOperatorType.NotEqual),
                new BinaryOperator("Oid", SamcoSoftShared.CurrentUser!.Oid, BinaryOperatorType.NotEqual)));
            UserGrid.ItemsSource = _users;
            if (SamcoAdd.UserIsAdmin() == false)
                UserSelectGrp.Visibility = Visibility.Collapsed;
            base.OnNavigatedTo(e);
        }

        public void Dispose()
        {
            _session1.Dispose();
        }

        private void UserGridUnbound(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Role")
            {
                var user = (LabUsers)UserGrid.GetRowByListIndex(e.ListSourceRowIndex);
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
                Content = new UserInfoView(new LabUsers(_session1)),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newUserWindow.ShowDialog() == true) _users!.Reload();
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
                Content = new UserInfoView((LabUsers)UserGrid.SelectedItem),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newUserWindow.ShowDialog() == true) _users!.Reload();
        }

        private void DeleteUser_OnClick(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem == null)
            {
                MainNotify.ShowWarning("خطا در حذف کاربر", "کاربری انتخاب نشده است.");
                return;
            }
            var selUser = (LabUsers)UserGrid.SelectedItem;
            if (WinUIMessageBox.Show($"آیا از حذف کاربر {selUser.username} اطمینان دارید؟", "حذف کاربر", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) != MessageBoxResult.Yes) return;
            selUser.Delete();
            _users!.Reload();
        }
    }
}
