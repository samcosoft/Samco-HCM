using System;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using HCMData;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using DevExpress.Xpf.WindowsUI;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Themes;
using Samco_HCM.Classes;
using Samco_HCM_Shared;

namespace Samco_HCM.Views;

/// <summary>
/// Interaction logic for LoginView.xaml
/// </summary>
public partial class LoginView
{
    public LoginView()
    {
        InitializeComponent();
        using (var session1 = new Session())
        {
            if (session1.Query<Users>().Count() == 1) FirstUserNotif.Visibility = Visibility.Visible;
            foreach (var user in session1.Query<Users>().ToList().Where(user => string.IsNullOrEmpty(user.realname)))
            {
                user.realname = user.username;
                user.Save();
            }
            UserBx.ItemsSource = session1.Query<Users>().Select(x => new LoginUser(x)).ToList();
        }

        if (SamcoSoftShared.LoadedSettings?.UniversityIcon != null) LogoImage.Source = SamcoSoftShared.LoadImage(SamcoSoftShared.LoadedSettings.UniversityIcon);

        UserBx.Focus();

        if (ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark)
            ThemeButton.IsChecked = true;
    }
    private void LoginBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(UserBx.Text) || string.IsNullOrEmpty(PassBx.Password))
        {
            MainNotify.ShowError("خطای ورود", "لطفاً نام کاربری و کلمه عبور را وارد کنید.");
            return;
        }

        using var session1 = new Session();
        var selUser = session1.FindObject<Users>(new BinaryOperator(nameof(Users.username), UserBx.EditValue.ToString()));

        //Check and update old system password
        if (GetMd5(PassBx.Password) == selUser.password)
        {
            selUser.password = BCrypt.Net.BCrypt.EnhancedHashPassword(PassBx.Password);
            selUser.Save();
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(PassBx.Password, selUser.password))
        {
            MainNotify.ShowError("خطای ورود", "نام کاربری و یا کلمه عبور اشتباه وارد شده است.");
            return;
        }

        //Save Last login time
        selUser.lastLogin = DateTime.Now;
        selUser.Save();

        SamcoSoftShared.CurrentUser = new LoggedUser(selUser);
        Application.Current.Resources["CurrentUserName"] = selUser.realname;

        var parentWindow = (WinUIDialogWindow)Parent;
        parentWindow.DialogResult = true;
    }

    private void ExitBtn_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = (WinUIDialogWindow)Parent;
        parentWindow.DialogResult = false;
    }

    private static string GetMd5(string s)
    {
        var builder = new StringBuilder();

        foreach (var b in MD5.HashData(Encoding.UTF8.GetBytes(s)))
            builder.Append(b.ToString("x2").ToLower());

        return builder.ToString();
    }

    private class LoginUser(Users user)
    {
        public string Username { get; set; } = user.username;
        public string RealName { get; set; } = user.realname;
    }

    private void ThemeButton_OnChecked(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).SwitchToDarkTheme();
    }

    private void ThemeButton_OnUnchecked(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).SwitchToLightTheme();
    }
}