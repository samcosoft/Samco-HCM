using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using LabData;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DevExpress.Data.Filtering;
using Samco_HCM_Laboratory_Client.Classes;

namespace Samco_HCM_Laboratory_Client.Views.Settings.TestSettingsViews;

/// <summary>
/// Interaction logic for NewTestView.xaml
/// </summary>
public partial class NewTestView : IDisposable
{
    private readonly Session _session = new();
    private readonly ObservableCollection<string> _values = [];
    private readonly TestName _selTest;

    public NewTestView()
    {
        InitializeComponent();
        _selTest = new TestName(_session);
        DataContext = _selTest;
        LoadValues();
    }

    public NewTestView(int testId)
    {
        InitializeComponent();
        _selTest = _session.GetObjectByKey<TestName>(testId);
        DataContext = _selTest;
        if (_selTest.parent != null)
        {
            ParentBx.IsEnabled = false;
            TestGroupBx.IsEnabled = false;
            RealPriceBx.IsEnabled = false;
        }

        LoadValues();
    }

    public NewTestView(TestName parentTest)
    {
        InitializeComponent();
        _selTest = new TestName(_session)
        {
            parent = _session.GetObjectByKey<TestName>(parentTest.Oid),
            group = _session.GetObjectByKey<TestGroup>(parentTest.group.Oid)
        };
        DataContext = _selTest;

        ParentBx.IsEnabled = false;
        TestGroupBx.IsEnabled = false;
        RealPriceBx.IsEnabled = false;

        LoadValues();
    }

    private void LoadValues()
    {
        TestGroupBx.ItemsSource = new XPCollection<TestGroup>(_session);

        ParentBx.ItemsSource = new XPCollection<TestName>(_session).Where(x => x.parent == null).ToList();

        _values.Clear();
        DefaultValueBx.Mask = null;
        DefaultValueBx.MaskType = MaskType.None;
        switch (_selTest.dataType)
        {
            case 0: //Numeric
                DataTypeBx.EditValue = "عددی";
                if (!string.IsNullOrEmpty(_selTest.nlRange) && _selTest.nlRange.Split("|").Length == 2)
                {
                    var nlRangeValues = _selTest.nlRange.Split("|");
                    MaxValueBx.Text = nlRangeValues[1];
                    MinValueBx.Text = nlRangeValues[0];
                }
                DefaultValueBx.Mask= "f2";
                DefaultValueBx.MaskType = MaskType.Numeric;
                break;
            case 1:
                DataTypeBx.EditValue = "انتخابی";
                if (!string.IsNullOrEmpty(_selTest.itmList))
                {
                    foreach (var item in _selTest.itmList.Split("|"))
                    {
                        _values.Add(item);
                    }
                    ValueList.ItemsSource = _values;
                    NlRangeCbx.ItemsSource = _values;
                    //NlRangeCbx.Text = _selTest.nlRange;
                }
                break;
            case 2:
                DataTypeBx.EditValue = "نوشتاری";
                break;
        }

        //Load parent test if exists
        if (_selTest.parent != null)
        {
            ParentBx.SelectedItem = _selTest.parent;
        }
        //Load test group if exists
        if (_selTest.group != null)
        {
            TestGroupBx.SelectedItem = _selTest.group;
        }
    }

    private void DataTypeBx_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        var value = e.NewValue.ToString();
        SelectionListBox.Visibility = Visibility.Collapsed;
        NumericNlRange.Visibility = Visibility.Collapsed;
        SelectionNlRange.Visibility = Visibility.Collapsed;
        TextNlRange.Visibility = Visibility.Collapsed;
        DefaultValueBx.AllowDefaultButton = false;
        switch (value)
        {
            case "عددی":
                NumericNlRange.Visibility = Visibility.Visible;
                break;
            case "انتخابی":
                DefaultValueBx.AllowDefaultButton = true;
                SelectionListBox.Visibility = Visibility.Visible;
                SelectionNlRange.Visibility = Visibility.Visible;
                break;
            case "نوشتاری":
                TextNlRange.Visibility = Visibility.Visible;
                break;
            default:
                SelectionListBox.Visibility = Visibility.Collapsed;
                SelectionNlRange.Visibility = Visibility.Collapsed;
                TextNlRange.Visibility = Visibility.Collapsed;
                NumericNlRange.Visibility = Visibility.Collapsed;
                break;
        }
    }

    private void TestNameBx_OnValidate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value == null ? "وارد نمودن نام آزمایش الزامیست" : null, DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
    }
    private void DataTypeBx_OnValidate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value == null ? "وارد نمودن نوع پاسخ الزامیست" : null, DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
    }
    private void TestGroupBx_OnValidate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value == null && _selTest.parent == null ? "وارد نمودن گروه آزمایش الزامیست" : null, DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
    }
    private void RealPriceBx_OnValidate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value == null ? "وارد نمودن قیمت واقعی الزامیست" : null, DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
        if (e.Value?.ToString() != string.Empty && int.Parse(e.Value?.ToString()!) < 0)
        {
            e.SetError("قیمت واقعی نمی تواند منفی باشد", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
        }
    }
    public void Dispose()
    {
        _session.Dispose();
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        ((WinUIDialogWindow)Parent).DialogResult = false;
    }

    private void ValueList_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        ValueEditBtn.Text = e.NewValue.ToString();
    }

    private void NewValueBtn(object sender, RoutedEventArgs e)
    {
        ValueEditBtn.Text = string.Empty;
    }

    private void SaveValueBtn(object sender, RoutedEventArgs e)
    {
        if (!_values.Contains(ValueEditBtn.Text)) _values.Add(ValueEditBtn.Text);
        ValueEditBtn.Text = string.Empty;
        ValueList.ItemsSource = _values;
        NlRangeCbx.ItemsSource = _values;
    }

    private void DeleteValueBtn(object sender, RoutedEventArgs e)
    {
        if (ValueList.SelectedItem == null) return;
        var selValue = ValueList.SelectedItem.ToString();
        if (_values.Contains(selValue!))
        {
            _values.Remove(selValue!);
        }
        ValueList.ItemsSource = _values;
        NlRangeCbx.ItemsSource = _values;
    }

    private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (TestNameBx.HasValidationError || TestGroupBx.HasValidationError || RealPriceBx.HasValidationError)
        {
            MainNotify.ShowError("خطا در ثبت اطلاعات", "لطفاً اطلاعات خواسته شده را را وارد نمایید");
            return;
        }

        //Name validation
        var existTest = _session.FindObject<TestName>(new BinaryOperator("name", _selTest.name));
        if (existTest != null && ReferenceEquals(existTest.parent, _selTest.parent) && existTest.Oid != _selTest.Oid)
        {
            MainNotify.ShowError("خطا در ثبت اطلاعات", "آزمایش تکراری است. لطفاً یک نام دیگر وارد نمایید");
            return;
        }

        _selTest.parent = null;
        if (ParentBx.SelectedItem != null)
        {
            _selTest.parent = (TestName)ParentBx.SelectedItem;
        }

        _selTest.group = null;
        if (TestGroupBx.SelectedItem != null)
        {
            _selTest.group = (TestGroup)TestGroupBx.SelectedItem;
        }

        switch (_selTest.dataType)
        {
            case 0: //Numeric
                _selTest.nlRange = $"{MinValueBx.Text}|{MaxValueBx.Text}";
                _selTest.itmList = string.Empty;
                break;
            case 1: //Selection
                _selTest.itmList = string.Join("|", _values);
                break;
            case 2: //Text
                _selTest.nlRange = NlRangeTextBx.Text;
                _selTest.itmList = string.Empty;
                break;
        }
        if (_selTest.printOrder == 0) _selTest.printOrder = _selTest.parent == null ? _session.Query<TestName>().Count() : _selTest.parent.TestNameCollection.Count;
        _selTest.Save();
        ((WinUIDialogWindow)Parent).DialogResult = true;
    }
}