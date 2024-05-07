using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using FontAwesome6;
using HandyControl.Themes;
using Theme = DevExpress.Xpf.Core.Theme;
using ThemeManager = DevExpress.Xpf.Core.ThemeManager;

namespace Samco_HCM
{
    public partial class App
    {
        static App()
        {
            CompatibilitySettings.AllowThemePreload = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var splashScreenViewModel = new DXSplashScreenViewModel
            {
                Title = "Samco® Healthcare Management System",
                Subtitle = Assembly.GetExecutingAssembly().GetName().Version + " - آزمایشی",
                Logo = new Uri("pack://application:,,,/Images/HCM.png", UriKind.RelativeOrAbsolute),
                Copyright = $"©Copyright Samco® Software Group 2017 - {DateTime.Today.Year} . All Rights Reserved.",
                IsIndeterminate = true,
                Status = "در حال بارگذاری..."
            };
            SplashScreenManager.Create(() => new HCMSplash(), splashScreenViewModel).ShowOnStartup();
            ThemeManager.PreloadThemeResource(Theme.Win11SystemName);
            ApplicationThemeHelper.ApplicationThemeName = Theme.Win11SystemName;
            FontAwesome6.Fonts.FontAwesomeFonts.LoadStyles(new Uri("pack://application:,,,/Fonts/"),EFontAwesomeStyle.Brands,EFontAwesomeStyle.Solid);
            base.OnStartup(e);
            ThemedWindow.RoundCorners = true;
        }

        internal void SwitchToDarkTheme()
        {
            if (HandyControl.Themes.ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark) return;
            HandyControl.Themes.ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            ApplicationThemeHelper.ApplicationThemeName = Theme.Win11DarkName;
        }
 //internal void SwitchToLightTheme()
 //       {
 //           if (HandyControl.Themes.ThemeManager.Current.AccentColor != accent)
 //           {
 //               HandyControl.Themes.ThemeManager.Current.AccentColor = accent;
 //           }
 //       }
        internal void SwitchToLightTheme()
        {
            if (HandyControl.Themes.ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light) return;
            HandyControl.Themes.ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            ApplicationThemeHelper.ApplicationThemeName = Theme.Win11LightName;
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var culture = new CultureInfo("fa-IR");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            ApplicationThemeHelper.SaveApplicationThemeName();
        }
    }
}
