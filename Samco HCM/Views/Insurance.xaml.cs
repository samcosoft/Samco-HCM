using System;
using DevExpress.Xpo;
using HCMData;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpf.WindowsUI.Navigation;
using System.Windows;
using Samco_HCM.Classes;
using System.ComponentModel;
using System.Globalization;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Navigation;
using Microsoft.VisualBasic;
using Samco_HCM_Shared;
using DevExpress.Xpf.Grid;

namespace Samco_HCM.Views;

/// <summary>
/// Interaction logic for Insurance.xaml
/// </summary>
public partial class Insurance : IDisposable
{
    private insuranceType _selInsurance;
    private HealthServices _selService;
    private List<EquipmentList> _equipmentList;
    private List<AddonService> _addonServices;
    private Session _session1;
    private RemoteBill _remoteBill;
    public Insurance()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        _session1 = new Session();
        if (e.Parameter.GetType() == typeof(RemoteBill))
        {
            // Load information for remote billing
            _remoteBill = (RemoteBill)e.Parameter;
            _selService = _session1.GetObjectByKey<HealthServices>(_remoteBill.ServiceType);
            // Set Insurance
            _selInsurance = _session1.GetObjectByKey<insuranceType>(_remoteBill.InsId);
            MelliCodeBx.Text = _remoteBill.MelliCode;
            SelectedInsNameBx.Text = _selInsurance.name;
            // Set Name
            NameBx.Text = _remoteBill.Name;
            // Set Price
            PriceBx.Text = _remoteBill.Price.ToString();
            // Disable controls
            InsuranceBar.IsEnabled = false;
            InfoGroup.IsEnabled = false;
            EquipServiceColumn.IsEnabled = false;
        }
        else
        {
            // Load selected service name
            _selService = _session1.GetObjectByKey<HealthServices>(e.Parameter);
            NumberBx.Visibility = _selService.group == "emerg" ? Visibility.Visible : Visibility.Collapsed;
            // enable controls
            MelliCodeBx.Text = string.Empty;
            NameBx.Text = string.Empty;
            MobileNumBx.Text = string.Empty;
            PriceBx.Text = string.Empty;
            InsuranceBar.IsEnabled = true;
            InfoGroup.IsEnabled = true;
            _remoteBill = null;
            EquipServiceColumn.IsEnabled = true;
        }
        SelServiceBx.Text = _selService.name;
        // Load role box
        if (_selService.ProviderRole is not null)
        {
            var personnelList = new List<Personnel>();
            foreach (var shiftPersonnel in from person in _selService.ProviderRole.Split(';')
                                           where SamcoAdd.ShiftList.ContainsKey(person)
                                           let ShiftPersonnel1 = SamcoAdd.ShiftList[person]
                                           where ShiftPersonnel1 != null
                                           select person.ToList())
                personnelList.AddRange(from itm in shiftPersonnel
                                       select _session1.GetObjectByKey<Personnel>(itm));
            PersonnelSelBx.ItemsSource = personnelList;
        }
        // Load Equipment List
        EquipCombo.ItemsSource = new XPCollection<Equipments>(_session1);
        // Load Default equipments
        _equipmentList = [];
        if (_selService.DefEquips.Any())
        {
            _equipmentList.AddRange(_selService.DefEquips.Select(itm => new EquipmentList(_session1) { EquipName = itm.EquipName, Count = itm.Count }));
        }
        EquipGrid.ItemsSource = _equipmentList;

        // Load service list
        var serviceList = _session1.Query<HealthServices>().Where(itm => itm.ProviderRole == _selService.ProviderRole &&
                                                                           !itm.HealthServicesCollection.Any() && itm.PricesCollection.Any() && itm.Oid != _selService.Oid).ToList();

        ServiceCombo.ItemsSource = serviceList;
        _addonServices = [];
        ServiceGrid.ItemsSource = _addonServices;

        // MelliCode Bx Item source
        var melliCodeCol = _session1.Query<PatientInfo>().Select(x => x.melliCode).Distinct().ToList();
        MelliCodeBx.ItemsSource = melliCodeCol;

        LoadInsurance();

        base.OnNavigatedTo(e);
    }

    private void LoadInsurance()
    {
        // Load tiles
        var recTileList = _session1.Query<insuranceType>().ToList();
        if (recTileList.Count == 0)
        {
            SelectedInsNameBx.Text = "شما باید ابتدا بیمه‌های لازم را تعریف کنید.‌";
            InsuranceBar.IsEnabled = false;
            return;
        }
        InsuranceBar.Items.Clear();
        foreach (var insurance in recTileList)
        {
            var fInsTile = SamcoAdd.GetTile(insurance);
            fInsTile.Name = $"InsTile{insurance.Oid}";
            fInsTile.Click += InsuranceTileClick;
            InsuranceBar.Items.Add(fInsTile);
        }

        // Check if it is friday
        if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
        {
            SamcoAdd.OffShift = true;
        }
        CloseDayCbx.IsChecked = SamcoAdd.OffShift;
        NumberBx.Value = 1;
    }

    private void InsuranceTileClick(object sender, EventArgs e)
    {
        _selInsurance = _session1.GetObjectByKey<insuranceType>(int.Parse(((TileBarItem)sender).Tag.ToString()!));
        SelectedInsNameBx.Text = _selInsurance.name;
        GetTotalPrice();
    }

    private void PrevInfoBtn_Click(object sender, RoutedEventArgs e)
    {
        var lastPerson = _session1.Query<Visits>().LastOrDefault();
        if (lastPerson == null)
            return;
        if (lastPerson.patient is not null)
        {
            MelliCodeBx.Text = lastPerson.patient.melliCode;
            NameBx.Text = lastPerson.patient.name;
            MobileNumBx.Text = lastPerson.patient.mobile;
        }

        _selInsurance = lastPerson.insType;
        SelectedInsNameBx.Text = _selInsurance.name;
        GetTotalPrice();
    }

    private int GetPrice(HealthServices getService, int count)
    {
        if (_selInsurance == null)
            return 0;
        // Get price
        var selPrice = _session1.Query<Prices>().SingleOrDefault(x => x.service.Oid == getService.Oid && x.insType.Oid == _selInsurance.Oid);
        // Check price defined before
        if (selPrice is not null)
        {
            if (CloseDayCbx.IsChecked == false)
            {
                // Select price 1
                return selPrice.price1 * count;
            }

            // Select Price 2
            return selPrice.price2 * count;
        }

        return 0;
    }

    private int GetTotalPrice(bool includeServices = true)
    {
        if (_selInsurance == null) return 0;

        // Get price
        var servicePrice = GetPrice(_selService, int.Parse(NumberBx.Value.ToString(CultureInfo.InvariantCulture)));

        if (servicePrice == 0)
        {
            MainNotify.ShowWarning("عدم تعریف تعرفه", $"برای خدمت {_selService.name} تعرفه‌ای تعریف نشده است.");
        }

        if (includeServices)
        {  // calculate other services
            if (_addonServices.Count != 0)
            {
                servicePrice += _addonServices.Sum(itm => GetPrice(itm.ServiceName, itm.Count));
            }
        }

        // calculate Equipment price
        if (_equipmentList.Count != 0)
        {
            servicePrice += _equipmentList.Sum(itm => itm.EquipName.Price * itm.Count);
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

        return servicePrice;
    }

    private void MelliCodeBx_Validate(object sender, ValidationEventArgs e)
    {
        if (e.Value is null)
        {
            e.SetError("وارد نمودن کد ملی الزامی است.");
        }
        else if (e.Value.ToString()!.Length < 10)
        {
            e.SetError("کد ملی را به صورت عدد ده رقمی وارد کنید.");
        }
        else
        {
            e.SetError(default);
            // Get name if existed
            var patInfo = _session1.FindObject<PatientInfo>(new BinaryOperator("melliCode", e.Value.ToString()));
            if (patInfo?.name == null) return;

            // Load latest information
            NameBx.Text = patInfo.name;
            MobileNumBx.Text = patInfo.mobile;
        }
    }

    private void PriceBx_Validate(object sender, ValidationEventArgs e)
    {
        if (e.Value is null)
        {
            e.SetError("انتخاب بیمه یا وارد نمودن مبلغ دریافتی الزامی است.");
            ButtonGroup.IsEnabled = false;
        }
        else
        {
            ButtonGroup.IsEnabled = true;
            e.SetError(default);
        }
    }
    private void PersonnelSelBx_Validate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value is null ? "انتخاب خدمت دهنده الزامی است." : default(object));
    }
    private void NumberBx_EditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        GetTotalPrice();
    }
    private void CloseDayCbx_EditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        SamcoAdd.OffShift = (bool)e.NewValue;
        GetTotalPrice();
    }

    #region Equipment-Services

    private void AddEquipmentBtn_Click(object sender, RoutedEventArgs e)
    {
        if (EquipCombo.SelectedItem is null)
            return;
        _equipmentList.Add(new EquipmentList(_session1) { EquipName = (Equipments)EquipCombo.SelectedItem, Count = Convert.ToInt32(EquipCount.Value) });
        EquipCombo.Clear();
        EquipCount.Value = 1;
        EquipCombo.Focus();
        EquipGrid.RefreshData();
        GetTotalPrice();
    }

    private void DelEquipmentBtn_Click(object sender, RoutedEventArgs e)
    {
        EquipCombo.Clear();
        EquipCount.Value = 1;
        EquipCombo.Focus();
        if (EquipGrid.SelectedItem is null)
            return;
        _equipmentList.Remove((EquipmentList)EquipGrid.SelectedItem);
        EquipGrid.RefreshData();
        GetTotalPrice();
    }

    private void AddServiceBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ServiceCombo.SelectedItem is null)
            return;

        _addonServices.Add(new AddonService((HealthServices)ServiceCombo.SelectedItem, Convert.ToInt32(ServiceCount.Value)));
        ServiceCombo.Clear();
        ServiceCount.Value = 1;
        ServiceCombo.Focus();
        ServiceGrid.RefreshData();
        GetTotalPrice();
    }

    private void DelServiceBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ServiceGrid.SelectedItem is null)
            return;
        _addonServices.Remove((AddonService)ServiceGrid.SelectedItem);
        ServiceCombo.Clear();
        ServiceCount.Value = 1;
        ServiceCombo.Focus();
        ServiceGrid.RefreshData();
        GetTotalPrice();
    }


    private void EquipmentServiceGrid_OnRowUpdated(object sender, RowEventArgs e)
    {
        GetTotalPrice();
    }

    #endregion

    private void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        if (SaveRecord() == false)
            return;
        ((MainWindow)Application.Current.MainWindow)!.NavFrame.Journal.GoHome();
    }
    private bool SaveRecord()
    {
        if (MelliCodeBx.HasValidationError | PriceBx.HasValidationError | PersonnelSelBx.HasValidationError)
        {
            MainNotify.ShowWarning("خطا در ثبت اطلاعات", "لطفاً اطلاعات خواسته شده را به صورت کامل وارد کنید.");
            return false;
        }

        // Check if remote price
        if (_remoteBill is not null)
        {
            using var session2 = new Session();
            var sellBill = session2.GetObjectByKey<RemoteBill>(_remoteBill.Oid);
            sellBill.IsPaid = true;
            sellBill.Save();
        }

        // Save Patient info
        var patInfo = _session1.FindObject<PatientInfo>(new BinaryOperator("melliCode", MelliCodeBx.Text));
        if (patInfo is null)
        {
            patInfo = new PatientInfo(_session1) { melliCode = MelliCodeBx.Text, name = NameBx.Text, mobile = MobileNumBx.Text, insurance = _session1.GetObjectByKey<insuranceType>(_selInsurance.Oid) };
            patInfo.Save();
        }
        else
        {
            patInfo.name = NameBx.Text;
            patInfo.mobile = MobileNumBx.Text;
            patInfo.insurance = _session1.GetObjectByKey<insuranceType>(_selInsurance.Oid);
            patInfo.Save();
        }

        // Save bill
        var newVisit = new Visits(_session1)
        {
            insType = _session1.GetObjectByKey<insuranceType>(_selInsurance.Oid),
            service = _session1.GetObjectByKey<HealthServices>(_selService.Oid),
            patient = patInfo,
            price = GetTotalPrice(false),
            user = _session1.GetObjectByKey<Users>(SamcoSoftShared.CurrentUser!.Oid),
            visitDate = DateAndTime.Now,
            isFullPrice = CloseDayCbx.IsChecked.GetValueOrDefault(false),
            Personnel = (Personnel)PersonnelSelBx.SelectedItem
        };

        newVisit.Save();

        // Save equipments
        if (_equipmentList.Count != 0)
        {
            foreach (var itm in _equipmentList)
            {
                itm.Visit = newVisit;
                itm.Save();
            }
        }

        // add more services
        for (int i = 1, loopTo = int.Parse(NumberBx.Value.ToString(CultureInfo.InvariantCulture)) - 1; i <= loopTo; i++)
        {
            newVisit = new Visits(_session1)
            {
                insType = _session1.GetObjectByKey<insuranceType>(_selInsurance.Oid),
                service = _session1.GetObjectByKey<HealthServices>(_selService.Oid),
                patient = patInfo,
                price = GetTotalPrice(false),
                user = _session1.GetObjectByKey<Users>(SamcoSoftShared.CurrentUser!.Oid),
                visitDate = DateAndTime.Now,
                isFullPrice = CloseDayCbx.IsChecked.GetValueOrDefault(false),
                Personnel = (Personnel)PersonnelSelBx.SelectedItem
            };

            newVisit.Save();
        }

        if (_addonServices.Count != 0)
        {
            foreach (var itm in _addonServices)
            {
                for (int i = 0, loopTo = itm.Count - 1; i <= loopTo; i++)
                {
                    var addVisit = new Visits(_session1)
                    {
                        insType = _session1.GetObjectByKey<insuranceType>(_selInsurance.Oid),
                        service = _session1.GetObjectByKey<HealthServices>(itm.ServiceName.Oid),
                        patient = patInfo,
                        price = GetPrice(itm.ServiceName, itm.Count),
                        user = _session1.GetObjectByKey<Users>(SamcoSoftShared.CurrentUser!.Oid),
                        visitDate = DateAndTime.Now,
                        isFullPrice = CloseDayCbx.IsChecked.GetValueOrDefault(false),
                        Personnel = (Personnel)PersonnelSelBx.SelectedItem
                    };

                    addVisit.Save();
                }
            }
        }
        return true;
    }
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        _session1?.Dispose();
        base.OnNavigatedFrom(e);
    }
    public void Dispose()
    {
        _session1?.Dispose();
        ((IDisposable)InsuranceBar)?.Dispose();
    }
}
public class AddonService : INotifyPropertyChanged
{
    private HealthServices _serviceName;
    public HealthServices ServiceName
    {
        get => _serviceName;
        set
        {
            _serviceName = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServiceName)));
        }
    }

    private int _count;
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        }
    }

    public AddonService(HealthServices getService, int serviceCount)
    {
        ServiceName = getService;
        Count = serviceCount;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}