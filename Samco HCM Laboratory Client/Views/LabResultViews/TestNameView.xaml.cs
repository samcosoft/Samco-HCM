using System.Collections.Generic;
using DevExpress.Xpf.Editors;
using LabData;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using DevExpress.Xpf.LayoutControl;

namespace Samco_HCM_Laboratory_Client.Views.LabResultViews;

/// <summary>
/// Interaction logic for TestGroup.xaml
/// </summary>
public partial class TestNameView
{
    public TestNameView(TestGroupData data)
    {
        InitializeComponent();
        DataContext = data;

        foreach (var testName in data.TestCards)
        {
            LabTestList.Children.Add(LoadResultPanel(testName));
        }
    }

    private LayoutItem LoadResultPanel(TestCard test)
    {
        var returnLayout = new LayoutItem { Label = $"{test.TestName.name}:", DataContext = test };
        switch (test.TestName.dataType)
        {
            case 0:
                var newSpin = new SpinEdit { AllowNullInput = true, MaxHeight = 30, DataContext = test };
                // Set Validation rules
                if (!string.IsNullOrEmpty(test.TestName.nlRange)) newSpin.Tag = test.TestName.nlRange;
                if (!string.IsNullOrEmpty(test.Result))
                {
                    newSpin.Value = decimal.TryParse(test.Result, out var result) ? result : 0;
                }
                else
                    newSpin.Value = decimal.TryParse(test.TestName.defValue, out var result) ? result : 0;
                newSpin.Validate += NewSpin_Validate;
                newSpin.EditValueChanged += (_, _) => test.Result = newSpin.Value.ToString(CultureInfo.InvariantCulture);
                returnLayout.Content = newSpin;
                break;
            case 1:
                var newCombo = new ComboBoxEdit
                {
                    IsTextEditable = false,
                    DataContext = test,
                    Tag = test.TestName.nlRange
                };
                newCombo.Items.AddRange(test.TestName.itmList.Split("|").Select(x => new ComboBoxEditItem { Content = x }).ToArray<object>());
                newCombo.Text = !string.IsNullOrEmpty(test.Result) ? test.Result : test.TestName.defValue;

                newCombo.SelectedIndexChanged += (_, _) => test.Result = newCombo.Text;
                newCombo.Validate += NewCombo_Validate;
                returnLayout.Content = newCombo;
                break;
            case 2:
                var newText = new TextEdit
                {
                    Tag = test.TestName.nlRange,
                    DataContext = test,
                    Text = !string.IsNullOrEmpty(test.Result) ? test.Result : test.TestName.defValue
                };

                newText.EditValueChanged += (_, _) => test.Result = newText.Text;
                newText.Validate += NewText_Validate;
                returnLayout.Content = newText;
                break;
        }

        //Redundant case, should not happen
        return returnLayout;
    }
    private void NewText_Validate(object sender, ValidationEventArgs e)
    {
        if (sender is not TextEdit { DataContext: TestCard test } textBx || e.Value == null || string.IsNullOrEmpty(test.TestName.defValue)) return;

        textBx.Background = e.Value.ToString() != test.TestName.defValue ? Brushes.DarkRed : null;
    }

    private void NewCombo_Validate(object sender, ValidationEventArgs e)
    {
        if (sender is not ComboBoxEdit { DataContext: TestCard test } comboBx || e.Value == null || string.IsNullOrEmpty(test.TestName.defValue)) return;

        comboBx.Background = comboBx.Text != test.TestName.defValue ? Brushes.DarkRed : null;
    }

    private void NewSpin_Validate(object sender, ValidationEventArgs e)
    {
        if (sender is not SpinEdit { DataContext: TestCard test } spinBx || e.Value == null || string.IsNullOrEmpty(test.TestName.defValue)) return;

        var nlRange = test.TestName.nlRange?.Split('|');
        if (nlRange is { Length: < 2 }) return;
        var minVal = decimal.TryParse(nlRange?[0], out var min) ? min : decimal.MinValue;
        var maxVal = decimal.TryParse(nlRange?[1], out var max) ? max : decimal.MaxValue;

        if ((decimal)e.Value < minVal || (decimal)e.Value > maxVal)
        {
            spinBx.Background = Brushes.DarkRed;
        }
        else
        {
            spinBx.Background = null;
        }
    }
}

public class TestGroupData(string groupName, List<TestCard> testCards)
{
    public string GroupName { get; set; } = groupName;
    public List<TestCard> TestCards { get; set; } = testCards;
}