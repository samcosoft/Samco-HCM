using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HCMData;
using LabData;
using Samco_HCM_Laboratory_Client.Classes;
using Samco_HCM_Shared;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NavigationEventArgs = DevExpress.Xpf.WindowsUI.Navigation.NavigationEventArgs;
using PatientInfo = LabData.PatientInfo;

namespace Samco_HCM_Laboratory_Client.Views;

/// <summary>
/// Interaction logic for NewVisitView.xaml
/// </summary>
public partial class NewVisitView : IDisposable
{
    private Session _session1 = new();
    private XPServerCollectionSource? _visitsCollection;
    private PatientInfo? _patientInfo;
    private LabVisits? _selVisit;
    public NewVisitView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        _session1 = new Session();
        // Initialize or load data for the view here
        InsuranceList.ItemsSource = new XPCollection<LabInsuranceType>(_session1);
        TestListToken.ItemsSource = new XPCollection<TestName>(_session1, CriteriaOperator.Parse("parent Is Null"));

        _visitsCollection = new XPServerCollectionSource(_session1, _session1.GetClassInfo(typeof(LabVisits)));

        FactorGrid.ItemsSource = _visitsCollection;
        TestTempGroup.Children.Clear();
        foreach (var button in _session1.Query<TestTemplate>().ToList().Select(itm => new SimpleButton
        {
            Content = itm.Name,
            Tag = itm,
            ToolTip = $"میانبر: {((Key)itm.ShortKey).ToString()}",
            Style = (Style)FindResource("ButtonEdit")
        }))
        {
            button.Click += (s, _) =>
            {
                var template = (TestTemplate)((Button)s).Tag;
                foreach (var testName in template.TestNames.Where(x => x.parent == null).ToList().Where(testName => !TestListToken.SelectedItems.Contains(testName)))
                {
                    TestListToken.SelectedItems.Add(testName);
                }
            };
            TestTempGroup.Children.Add(button);
        }
        MelliCodeBx.Focus();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        _session1.Dispose();
    }

    private void MelliCodeBx_OnValidate(object sender, ValidationEventArgs e)
    {
        if (e.Value == null || e.Value.ToString()?.Length < 10)
        {
            e.SetError("لطفاً کد ملی صحیح را وارد نمایید.");
            GetInfoBtn.IsEnabled = false;
        }
        else
        {
            e.SetError(null);
            GetInfoBtn.IsEnabled = true;
        }
    }

    private void GetInfoBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var patient = _session1.FindObject<PatientInfo>(new BinaryOperator(nameof(PatientInfo.MelliCode), MelliCodeBx.Text));

        if (patient != null)
        {
            _patientInfo = patient;
            VisitInfoGrp.DataContext = _patientInfo;
            InsuranceList.SelectedItem = _patientInfo.Insurance;
            FactorGrid.FilterCriteria = new BinaryOperator("Patient.MelliCode", _patientInfo.MelliCode);
            TestListToken.Focus();
        }
        else
        {
            if (SamcoSoftShared.LoadedSettings!.ActiveClient && SamcoAdd.CheckNetwork())
            {
                try
                {
                    using var remoteSession = new Session(SamcoAdd.RemoteDatalayer);
                    var remotePatient =
                        remoteSession.FindObject<HCMData.PatientInfo>(
                            new BinaryOperator(nameof(HCMData.PatientInfo.melliCode), MelliCodeBx.Text));
                    if (remotePatient != null)
                    {
                        _patientInfo = new PatientInfo(_session1)
                        {
                            MelliCode = remotePatient.melliCode,
                            Name = remotePatient.name,
                            Phone = remotePatient.mobile,
                            Insurance = _session1.FindObject<LabInsuranceType>(new BinaryOperator("serverID",
                                remotePatient.Oid))
                        };
                        VisitInfoGrp.DataContext = _patientInfo;
                        InsuranceList.SelectedItem = _patientInfo.Insurance;
                        BirthDateBx.Focus();
                    }
                }
                catch (Exception)
                {
                    _patientInfo = new PatientInfo(_session1)
                    {
                        MelliCode = MelliCodeBx.Text
                    };
                    VisitInfoGrp.DataContext = _patientInfo;
                    NameBx.Focus();
                }
            }
            else
            {
                _patientInfo = new PatientInfo(_session1)
                {
                    MelliCode = MelliCodeBx.Text
                };
                VisitInfoGrp.DataContext = _patientInfo;
                NameBx.Focus();
            }
        }

        _selVisit = null;
        PatientDataGroup.IsEnabled = true;
    }
    private void NameBx_OnValidate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value == null ? "لطفاً نام و نام خانوادگی را وارد کنید." : null);
    }

    private void BirthDateBx_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (BirthDateBx.SelectedDate != null)
        {
            AgeBx.Text = (DateTime.Now.Year - BirthDateBx.SelectedDate.Value.Year).ToString();
        }
    }

    private void AgeBx_OnValidate(object sender, ValidationEventArgs e)
    {
        if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()))
        {
            var calcDate = DateTime.Now.AddYears(-Convert.ToInt32(e.Value.ToString()));
            if (BirthDateBx.SelectedDate != calcDate)
                BirthDateBx.SelectedDate = calcDate;
        }
    }

    private void InsuranceList_OnValidate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value == null ? "لطفاً بیمه را انتخاب کنید." : null);
    }
    private void TestListToken_OnValidate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value == null ? "لطفاً حداقل یک آزمایش را انتخاب کنید." : null);
    }
    private void InsuranceList_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
    {
        CalculatePrice();
    }
    private void IsFreeVisit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        CalculatePrice();
    }

    private void TestListToken_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        CalculatePrice();
    }

    private void TestListToken_OnKeyDown(object sender, KeyEventArgs e)
    {
        var labTemplate = _session1.FindObject<TestTemplate>(new BinaryOperator("ShortKey", (byte)e.Key));
        if (labTemplate != null)
        {
            foreach (var testName in labTemplate.TestNames)
            {
                if (!TestListToken.SelectedItems.Contains(testName))
                {
                    TestListToken.SelectedItems.Add(testName);
                }
            }
        }
    }
    private void CalculatePrice()
    {
        var testList = TestListToken.SelectedItems.Cast<TestName>().ToList();
        var insurance = (LabInsuranceType)InsuranceList.SelectedItem;

        if (testList.Count == 0 || insurance == null) return;

        var franchise = insurance.franchises / 100;

        var testPriceTable = new DataTable();
        testPriceTable.Columns.Add("TestName",typeof(string));
        testPriceTable.Columns.Add("RealPrice", typeof(int));
        testPriceTable.Columns.Add("InsuredPrice", typeof(int));

        foreach (var test in testList)
        {
            //Get test price from insurance list
            var testPrice = _session1.Query<TestPrice>().Where(x => x.insType.Oid == insurance.Oid &&
                                                                x.testName.Oid == test.Oid);
            testPriceTable.Rows.Add(test.name, test.realPrice, testPrice.Any() ? testPrice.FirstOrDefault()!.price : Math.Round((decimal)(test.realPrice * franchise)));
        }

        var sumRealPrice = testList.Sum(test => test.realPrice);

        SumRealPriceBx.Text = sumRealPrice.ToString();

        var sumInsuredPrice = testPriceTable
            .AsEnumerable()
            .Sum(row => Convert.ToInt32(row["InsuredPrice"]));

        FranchiesLab.Text = sumInsuredPrice.ToString();
        
        testPriceTable.Rows.Add("حق فنی", insurance.fanniPrice.ToString());

        sumInsuredPrice += insurance.fanniPrice;

        SumPriceLab.Text = IsFreeVisit.IsChecked.GetValueOrDefault(false) ? "0" : sumInsuredPrice.ToString();

        RealPriceGrid.ItemsSource = testPriceTable;
        RealPriceGrid.RefreshData();
    }

    private void FactorGrid_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.NewItem == null) return;
        try
        {
            _selVisit = (LabVisits)e.NewItem;
            TestListBx.Text = string.Join(" - ", _selVisit.TestCards.Select(x => x.TestName).Where(x => x.parent == null).Select(x => x.name).ToList());
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void NameBxAddressBx_OnGotFocus(object sender, RoutedEventArgs e)
    {
        try
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void NameBxAddressBx_OnLostFocus(object sender, RoutedEventArgs e)
    {
        try
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void SaveBillBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (_patientInfo == null || NameBx.HasValidationError || InsuranceList.HasValidationError || _patientInfo.BirthDate == null || TestListToken.HasValidationError)
        {
            MainNotify.ShowWarning("خطا در ثبت قبض", "لطفا اطلاعات خواسته شده را وارد کنید");
            return;
        }

        _patientInfo.Insurance = (LabInsuranceType)InsuranceList.SelectedItem;
        _session1.Save(_patientInfo);

        if (_selVisit == null)
        {
            _selVisit = new LabVisits(_session1)
            {
                Patient = _patientInfo,
                insType = InsuranceList.SelectedItem as LabInsuranceType,
                isFree = IsFreeVisit.IsChecked.GetValueOrDefault(false),
                visitDate = DateTime.Now,
                user = _session1.GetObjectByKey<LabUsers>(SamcoSoftShared.CurrentUser!.Oid)
            };
        }
        else
        {
            _selVisit.Patient = _patientInfo;
            _selVisit.insType = InsuranceList.SelectedItem as LabInsuranceType;
            _selVisit.isFree = IsFreeVisit.IsChecked.GetValueOrDefault(false);
        }

        if (IsFreeVisit.IsChecked.GetValueOrDefault(false))
        {
            switch (IsFreeDisc.IsChecked)
            {
                case true:
                    _selVisit.description = "ایراپن";
                    break;
                case false:
                    _selVisit.description = "بارداری";
                    break;
                case null:
                    _selVisit.description = "سایر";
                    break;
            }
        }

        _selVisit.Save();

        var testList = TestListToken.SelectedItems.Cast<TestName>().ToList();
        testList.AddRange(testList.SelectMany(x => x.TestNameCollection).ToList());

        if (_selVisit.TestCards.Any())
        {
            var testsToRemove = _selVisit.TestCards.Where(x => testList.All(y => x.TestName.Oid != y.Oid)).ToList();

            foreach (var testName in testsToRemove.ToList())
            {
                _selVisit.TestCards.Remove(testName);
            }
        }
        var testsToAdd = testList.Where(x => _selVisit.TestCards.All(y => y.TestName.Oid != x.Oid)).ToList();

        foreach (var testName in testsToAdd)
        {
            _selVisit.TestCards.Add(new TestCard(_session1) { TestName = testName });
        }

        if (!_selVisit.paid) _selVisit.paid = _selVisit.isFree;
        _selVisit.Price = Convert.ToInt32(SumPriceLab.Text);

        _selVisit.Save();
        MainNotify.ShowSuccess("ثبت قبض", "قبض با موفقیت ثبت شد");
        ResetFields();
    }

    private void CancelBillBtn_OnClick(object sender, RoutedEventArgs e)
    {
        ResetFields();
    }

    private void ResetFields()
    {
        _patientInfo = null;
        _selVisit = null;
        NameBx.Text = string.Empty;
        AgeBx.Text = string.Empty;
        TestListToken.SelectedItems.Clear();
        IsFreeVisit.IsChecked = false;
        RealPriceGrid.ItemsSource = null;
        PatientDataGroup.IsEnabled = false;
        FactorGrid.FilterCriteria = null;
        InsuranceList.SelectedItem = null;
        SumRealPriceBx.Text = string.Empty;
        SumPriceLab.Text = string.Empty;
        FranchiesLab.Text = string.Empty;
        MelliCodeBx.Text = string.Empty;
        MelliCodeBx.Focus();
    }

    private void GridRefBtn_OnClick(object sender, RoutedEventArgs e)
    {
        SamcoAdd.UpdateBillsData();
        _visitsCollection = new XPServerCollectionSource(_session1, _session1.GetClassInfo(typeof(LabVisits)));
        //FactorGrid.ItemsSource = _visitsCollection;
        FactorGrid.RefreshData();
        MainNotify.ShowSuccess("تازه سازی", "لیست با موفقیت به روزرسانی شد");
    }

    private void GridEditBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (FactorGrid.SelectedItem == null)
        {
            MainNotify.ShowWarning("خطا در ویرایش قبض", "لطفاً یک قبض را برای ویرایش انتخاب کنید");
            return;
        }

        ResetFields();

        _selVisit = (LabVisits)FactorGrid.SelectedItem;
        _patientInfo = _selVisit.Patient;
        if (_selVisit.paid && !_selVisit.isFree)
        {
            MainNotify.ShowWarning("خطا در ویرایش قبض", "این قبض پرداخت شده است. شما نمی‌توانید آن را ویرایش کنید");
            return;
        }
        if (_selVisit.Patient != null)
        {
            MelliCodeBx.Text = _selVisit.Patient.MelliCode;
            VisitInfoGrp.DataContext = _selVisit.Patient;
            InsuranceList.SelectedItem = _selVisit.insType;
        }

        foreach (var testName in _selVisit.TestCards.Select(x => x.TestName).Where(x => x.parent == null).ToList())
        {
            TestListToken.SelectedItems.Add(testName);
        }

        IsFreeVisit.IsChecked = _selVisit.isFree;

        if (_selVisit.isFree)
        {
            IsFreeDisc.IsChecked = _selVisit.description switch
            {
                "ایراپن" => true,
                "بارداری" => false,
                "سایر" => null,
                _ => IsFreeDisc.IsChecked
            };
        }
        CalculatePrice();
        PatientDataGroup.IsEnabled = true;
        TestListToken.Focus();
    }

    private void DelBillBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (FactorGrid.SelectedItem == null)
        {
            MainNotify.ShowWarning("خطا در حذف قبض", "لطفاً یک قبض را برای حذف انتخاب کنید");
            return;
        }

        var selectedVisit = (LabVisits)FactorGrid.SelectedItem;
        if (selectedVisit is { paid: true, isFree: false })
        {
            MainNotify.ShowWarning("خطا در حذف قبض", "این قبض پرداخت شده است. شما نمی‌توانید آن را حذف کنید");
            return;
        }
        if (WinUIMessageBox.Show("آیا از حذف قبض انتخاب شده مطمئن هستید؟", "تأیید حذف قبض", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == MessageBoxResult.No) return;
        if (!selectedVisit.isFree)
        {
            var remoteSession = new Session(SamcoAdd.RemoteDatalayer);
            //Delete remoteBill if exists
            var remoteBill = remoteSession.FindObject<RemoteBill>(new BinaryOperator("RemoteBillId", selectedVisit.Oid));
            remoteBill?.Delete();
        }

        selectedVisit.Delete();
        MainNotify.ShowSuccess("حذف قبض", "قبض با موفقیت حذف شد");
        ResetFields();
        _visitsCollection!.Reload();
        FactorGrid.RefreshData();
    }

    public void Dispose()
    {
        _session1.Dispose();
        _visitsCollection?.Dispose();
    }
}