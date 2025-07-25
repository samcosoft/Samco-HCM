using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using Samco_HCM_Laboratory_Client.Classes;
using NavigationEventArgs = DevExpress.Xpf.WindowsUI.Navigation.NavigationEventArgs;

namespace Samco_HCM_Laboratory_Client.Views
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
            ProdVer.Content = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            CopyRightLbl.Text = $"Copyright 2018 - {DateTime.Now.Year} Samco Software Group - Ayandeh Kavan Vorna Corp";
            base.OnNavigatedTo(e);
        }
    }
}
