using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI.Navigation;
using HCMData;

namespace Samco_HCM.Classes
{
    /// <summary>
    /// Interaction logic for MaximizedTile.xaml
    /// </summary>
    public partial class MaximizedTile
    {

        public static readonly DependencyProperty HealthServicesList =
            DependencyProperty.Register(
                nameof(ServiceList),
                typeof(List<HealthServices>),
                typeof(UserControl));

        public List<HealthServices> ServiceList
        {
            get => (List<HealthServices>)GetValue(HealthServicesList);
            set => SetValue(HealthServicesList, value);
        }

        public MaximizedTile()
        {
            InitializeComponent();
        }

        public MaximizedTile(List<HealthServices> servicesList)
        {
            InitializeComponent();
            foreach (var itm in servicesList)
            {
                var newBtn = new SimpleButton { Content = itm.name };
                Navigation.SetNavigateTo(newBtn, "Insurance");
                Navigation.SetNavigationParameter(newBtn, itm.Oid);
                ItemPanel.Children.Add(newBtn);
            }
        }
    }
}
