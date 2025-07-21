using System;
using DevExpress.Xpo;

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

    public void Dispose()
    {
        _session.Dispose();
    }
}