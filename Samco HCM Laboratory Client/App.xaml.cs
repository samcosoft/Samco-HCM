using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using FontAwesome6;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace Samco_HCM_Laboratory_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            CompatibilitySettings.UseLightweightThemes = true;
            ApplicationThemeHelper.Preload(PreloadCategories.Core);

            var splashScreenViewModel = new DXSplashScreenViewModel
            {
                Title = "Samco® Healthcare Management System",
                Subtitle = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                Logo = new Uri("pack://application:,,,/Images/Logo.png", UriKind.RelativeOrAbsolute),
                Copyright = $"©Copyright Samco® Software Group 2017 - {DateTime.Today.Year} . All Rights Reserved.",
                IsIndeterminate = true,
                Status = "در حال بارگذاری..."
            };
            SplashScreenManager.Create(() => new LaboratorySplash(), splashScreenViewModel).ShowOnStartup(false);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var culture = new CultureInfo("fa-IR");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            FontAwesome6.Fonts.FontAwesomeFonts.LoadStyles(new Uri("pack://application:,,,/Fonts/"), EFontAwesomeStyle.Brands, EFontAwesomeStyle.Solid);
            ThemedWindow.RoundCorners = true;
            ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11SystemColors.Name;
        }

        internal void SwitchToDarkTheme()
        {
            if (HandyControl.Themes.ThemeManager.Current.ApplicationTheme == HandyControl.Themes.ApplicationTheme.Dark) return;
            HandyControl.Themes.ThemeManager.Current.ApplicationTheme = HandyControl.Themes.ApplicationTheme.Dark;
            ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11Dark.Name;
        }

        internal void SwitchToLightTheme()
        {
            if (HandyControl.Themes.ThemeManager.Current.ApplicationTheme == HandyControl.Themes.ApplicationTheme.Light) return;
            HandyControl.Themes.ThemeManager.Current.ApplicationTheme = HandyControl.Themes.ApplicationTheme.Light;
            ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11Light.Name;
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            ApplicationThemeHelper.SaveApplicationThemeName();
        }
    }
}
