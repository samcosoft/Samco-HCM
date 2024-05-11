using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM.Classes;
using Samco_HCM.Reports;
using NavigationEventArgs = DevExpress.Xpf.WindowsUI.Navigation.NavigationEventArgs;

namespace Samco_HCM.Views;

/// <summary>
/// Interaction logic for BillEditorView.xaml
/// </summary>
public partial class BillEditorView: IDisposable
{
    private Session _session1;
    private Visits _selVisit;
    public XPServerCollectionSource VisitCollection { get; set; }
    public XPCollection<insuranceType> InsuranceDataSource { get; set; }
    public XPCollection<HealthServices> ServicesDataSource { get; set; }
    public BillEditorView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        _session1 = new Session();
        InsuranceDataSource = new XPCollection<insuranceType>(_session1);
        ServicesDataSource = new XPCollection<HealthServices>(_session1,
            CriteriaOperator.Parse("[HealthServicesCollection].Count = 0"));
        VisitCollection = new XPServerCollectionSource(_session1, _session1.GetClassInfo<Visits>());

        VisitGrid.ItemsSource = VisitCollection;
        ServiceSelCbx.ItemsSource = ServicesDataSource;
        InsuranceSelCbx.ItemsSource = InsuranceDataSource;

        base.OnNavigatedTo(e);
    }

    private void VisitGrid_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
    {
        if (VisitGrid.SelectedItem is null)
            return;
        _selVisit = (Visits)VisitGrid.SelectedItem;
        InsuranceSelCbx.SelectedItem = _selVisit.insType;
        ServiceSelCbx.SelectedItem = _selVisit.service;
        PriceBx.Text = _selVisit.price.ToString();
        CloseDayCbx.IsChecked = _selVisit.isFullPrice;
        EquipGrid.ItemsSource = _selVisit.EquipmentLists;
        // Load Equipment List
        EquipCombo.ItemsSource = new XPCollection<Equipments>(_session1);

        // Load role box
        if (_selVisit.service.ProviderRole is not null)
        {
            var personnelList = new List<Personnel>();

            foreach (var role in _selVisit.service.ProviderRole.Split(';'))
            {
                if(SamcoAdd.ShiftList.ContainsKey(role))
                    personnelList.AddRange(SamcoAdd.ShiftList[role].Select(x=>_session1.GetObjectByKey<Personnel>(x)));
            }

            PersonnelSelBx.ItemsSource = personnelList;
        }

        PersonnelSelBx.SelectedItem = _selVisit.Personnel;
    }
    private void ComboBx_SelectedIndexChanged(object sender, RoutedEventArgs e)
    {
        GetPrice();
    }

    private void CloseDayCbx_EditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        GetPrice();
    }

    private void GetPrice()
    {
        // Get price
        var servicePrice = 0;
        if(InsuranceSelCbx.EditValue == null) return;
        try
        {
            var selPrice = _session1.Query<Prices>().SingleOrDefault(x => x.service.Oid == _selVisit.service.Oid && x.insType.Oid == (int)InsuranceSelCbx.EditValue);
            // Check price defined before
            if (selPrice is not null)
            {
                servicePrice = CloseDayCbx.IsChecked == false ? selPrice.price1 : selPrice.price2;
            }

            // calculate Equipment price
            if (_selVisit.EquipmentLists.Count != 0)
            {
                servicePrice += _selVisit.EquipmentLists.Sum(itm => itm.EquipName.Price * itm.Count);
            }

            if (servicePrice == 0)
            {
                // Check if service is available
                MainNotify.ShowInformation("عدم تعریف تعرفه", "لطفاً مبلغ را به صورت دستی وارد کنید.");
                PriceBx.IsReadOnly = false;
            }
            else
            {
                PriceBx.IsReadOnly = true;
                PriceBx.Text = servicePrice.ToString();
            }
        }
        catch (Exception)
        {
            PriceBx.IsReadOnly = true;
            PriceBx.Text = servicePrice.ToString();
        }
        
    }

    #region Equipment

    private void AddEquipmentBtn_Click(object sender, RoutedEventArgs e)
    {
        if (EquipCombo.SelectedItem is null)
            return;
        if (_selVisit == null)
        {
            MainNotify.ShowWarning("خطا در افزودن تجهیزات", "لطفاً ابتدا یک خدمت را از لیست انتخاب کنید.");
            return;
        }
        _selVisit.EquipmentLists.Add(new EquipmentList(_session1) { EquipName = (Equipments)EquipCombo.SelectedItem, Count = Convert.ToInt32(EquipCount.Value) });
        //_selVisit.Save();
        EquipCombo.Clear();
        EquipCount.Value = 1;
        EquipCombo.Focus();
        EquipGrid.RefreshData();
        GetPrice();
    }

    private void DelEquipmentBtn_Click(object sender, RoutedEventArgs e)
    {
        if (EquipGrid.SelectedItem is null)
            return;
        _selVisit.EquipmentLists.Remove((EquipmentList)EquipGrid.SelectedItem);
        //_selVisit.Save();
        EquipCombo.Clear();
        EquipCount.Value = 1;
        EquipCombo.Focus();        
        EquipGrid.RefreshData();
        GetPrice();
    }

    private void EquipmentServiceGrid_OnRowUpdated(object sender, RowEventArgs e)
    {
        GetPrice();
    }

    #endregion

    private void PriceBx_Validate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value is null ? "انتخاب بیمه یا وارد نمودن مبلغ دریافتی الزامی است." : null);
    }
    private void PersonnelSelBx_Validate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value is null ? "انتخاب خدمت دهنده الزامی است." : default(object));
    }

    private void SaveInfoButtonClick(object sender, RoutedEventArgs e)
    {
        if (VisitGrid.GetSelectedRowHandles().Length == 0)
        {
            MainNotify.ShowWarning("خطا در ویرایش","لطفاً یک قبض را انتخاب کنید.");
            return;
        }

        if (PriceBx.HasValidationError | PersonnelSelBx.HasValidationError)
        {
            MainNotify.ShowWarning("خطا در ثبت اطلاعات", "لطفاً اطلاعات خواسته شده را به صورت کامل وارد کنید.");
            return;
        }
        _selVisit.insType = (insuranceType)InsuranceSelCbx.SelectedItem;
        _selVisit.service = (HealthServices)ServiceSelCbx.SelectedItem;
        _selVisit.price = int.Parse(PriceBx.Text);
        _selVisit.isFullPrice = CloseDayCbx.IsChecked.GetValueOrDefault(false);
        _selVisit.Personnel = (Personnel)PersonnelSelBx.SelectedItem;
        _selVisit.Save();
        VisitCollection.Reload();
        VisitGrid.RefreshData();
        MainNotify.ShowSuccess("ثبت اطلاعات", "اطلاعات با موفقیت ثبت شدند.");
    }
    private void PrintButtonClick(object sender, RoutedEventArgs e)
    {
        if (VisitGrid.GetSelectedRowHandles().Length == 0)
        {
            MainNotify.ShowWarning("خطا در ویرایش","لطفاً یک قبض را انتخاب کنید.");
            return;
        }
        SaveInfoButtonClick(sender,e);
        // Create report for print
        var serviceList = new List<AddonService> { new(_selVisit.service, 1) };
        var report = new VisitReceipt(serviceList);
        report.Parameters["Nobat"].Value = 0;
        report.Parameters["Doctor"].Value = PersonnelSelBx.Text;
        report.Parameters["SetPrice"].Value = PriceBx.Text;
        report.Parameters["InsName"].Value = _selVisit.insType.name;
        report.Parameters["PatName"].Value = _selVisit.patient.name;
        report.Parameters["MelliCode"].Value = _selVisit.patient.melliCode;
        try
        {
            PrintHelper.PrintDirect(report);
            MainNotify.ShowInformation("صدور قبض","قبض صادر شد.");
        }
        catch (Exception ex)
        {
            MainNotify.ShowError("خطا در چاپ قبض", "چاپگر نصب و تنظیم نیست.");
            MainNotify.ShowError("خطا در چاپ قبض", ex.ToString());
        }
    }

    private void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (VisitGrid.GetSelectedRowHandles().Length == 0)
        {
            MainNotify.ShowWarning("خطا در حذف","لطفاً قبل از حذف قبض یکی را انتخاب کنید.");
            return;
        }

        if (WinUIMessageBox.Show(this, "آیا از حذف این قبض‌ها مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.No, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading, FloatingMode.Adorner) == MessageBoxResult.Yes)
        {
            var rowHand = VisitGrid.GetSelectedRowHandles();
            foreach (var getVisit in from itmID in rowHand
                                     select (Visits)VisitGrid.GetRow(itmID))
            {
                using var deleteSession = new Session();
                var delRow = deleteSession.GetObjectByKey<Visits>(getVisit.Oid);
                delRow.Delete();
            } 
            VisitCollection.Reload();
            VisitGrid.RefreshData();
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        _session1?.Dispose();
        VisitCollection?.Dispose();
        InsuranceDataSource?.Dispose();
        ServicesDataSource?.Dispose();
        base.OnNavigatedFrom(e);
    }

    public void Dispose()
    {
        _session1?.Dispose();
        VisitCollection?.Dispose();
        InsuranceDataSource?.Dispose();
        ServicesDataSource?.Dispose();
    }

}