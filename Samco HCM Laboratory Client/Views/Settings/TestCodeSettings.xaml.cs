using System;
using System.Windows;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using LabData;

namespace Samco_HCM_Laboratory_Client.Views.Settings;

/// <summary>
/// Interaction logic for TestCodeSettings.xaml
/// </summary>
public partial class TestCodeSettings : IDisposable
{
    private readonly Session _session = new();
    public TestCodeSettings()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        MachineCodeGrid.ItemsSource = new XPCollection<TestName>(_session, CriteriaOperator.Parse("parent Is null"));
    }

    public void Dispose()
    {
        _session.Dispose();
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        ((MainWindow)Application.Current.MainWindow!).NavFrame.Journal.GoHome();
    }
}