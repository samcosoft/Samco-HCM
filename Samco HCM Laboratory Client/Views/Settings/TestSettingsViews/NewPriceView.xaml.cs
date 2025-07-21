using System;
using System.Collections.ObjectModel;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using LabData;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Editors;

namespace Samco_HCM_Laboratory_Client.Views.Settings.TestSettingsViews;

/// <summary>
/// Interaction logic for NewPriceView.xaml
/// </summary>
public partial class NewPriceView : IDisposable
{
    private readonly Session _session1 = new();
    private readonly ObservableCollection<TestPrice> _testPrices = new();
    public NewPriceView()
    {
        InitializeComponent();
        TestCbx.ItemsSource = _session1.Query<TestName>().Where(x => x.parent == null).ToList();
        PriceGrid.ItemsSource = _testPrices;
    }

    private void TestCbx_OnValidate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value == null ? "لطفاً یک آزمایش را انتخاب کنید." : null);
    }

    private void TestCbx_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        //Loading prices for the selected test
        _testPrices.Clear();
        var insurances = _session1.Query<LabInsuranceType>().ToList();
        var selectedTest = _session1.GetObjectByKey<TestName>((int)e.NewValue);

        foreach (var labInsuranceType in insurances)
        {
            if(selectedTest.TestPrices.Any(x=>x.insType.name==labInsuranceType.name))
                _testPrices.Add(selectedTest.TestPrices.First(x => x.insType.name == labInsuranceType.name));
            else
            {
                var newPrice = new TestPrice(_session1)
                {
                    testName = selectedTest,
                    insType = labInsuranceType,
                    price = selectedTest.TestPrices.FirstOrDefault(x => x.Oid == labInsuranceType.Oid)?.price ?? 0
                };
                _testPrices.Add(newPrice);
            }
        }
    }

    private void SavePriceBtn_Click(object sender, RoutedEventArgs e)
    {
        //Validation
        foreach (var testPrice in _testPrices)
        {
            testPrice.Save();
        }
        ((WinUIDialogWindow)Parent).DialogResult = true;
    }

    private void CancelBtn_Click(object sender, RoutedEventArgs e)
    {
        ((WinUIDialogWindow)Parent).DialogResult = false;
    }

    public void Dispose()
    {
        _session1.Dispose();
    }
}