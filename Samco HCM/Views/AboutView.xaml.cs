using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using Samco_HCM.Classes;
using NavigationEventArgs = DevExpress.Xpf.WindowsUI.Navigation.NavigationEventArgs;

namespace Samco_HCM.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LicStatLab.Text = SamcoAdd.License.Status.ToString();
            ProdVer.Content = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            CopyRightLbl.Text = "Copyright 2018 - 2024 Samco Software Group - Ayandeh Kavan Vorna Corp";
            base.OnNavigatedTo(e);
        }
        private void ActivateBtn_Click(object sender, RoutedEventArgs e)
        {
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
            activationDialog.ShowDialog();
        }
    }
}
