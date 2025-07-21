using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM.Classes;
using Samco_HCM.Views.Settings.ServiceInsuranceViews;
using Samco_HCM_Shared;

namespace Samco_HCM.Views.Settings;

/// <summary>
/// Interaction logic for ServiceInsuranceView.xaml
/// </summary>
public partial class ServiceInsuranceView : IDisposable
{
    private readonly Session _session1 = new();
    private bool _disposed;

    public ServiceInsuranceView()
    {
        InitializeComponent();
        if (SamcoSoftShared.LoadedSettings!.PersonnelRole == null)
        {
            SamcoSoftShared.LoadedSettings.PersonnelRole = ["پزشک", "دندانپزشک", "پرستار", "ماما", "بهیار"];
        }
        LoadData();
    }

    private void LoadData()
    {
        //Load Data
        PriceListGrid.ItemsSource = new XPCollection<Prices>(_session1);
        InsList.ItemsSource = new XPCollection<insuranceType>(_session1);
        InsCbx.ItemsSource = new XPCollection<insuranceType>(_session1);
        EquipList.ItemsSource = new XPCollection<Equipments>(_session1);
        ServiceList.ItemsSource = new XPCollection<HealthServices>(_session1);
        var serviceCollection = new XPCollection<HealthServices>(_session1).Where(x => x.HealthServicesCollection.Count == 0).ToList();
        SvcCbx.ItemsSource = serviceCollection;
        ServiceSelCbx.ItemsSource = serviceCollection;
        RoleList.ItemsSource = SamcoSoftShared.LoadedSettings!.PersonnelRole;
        ShiftStartTime.DateTime = SamcoSoftShared.LoadedSettings.StartShiftTime ?? new DateTime(2001, 1, 1, 8, 0, 0);
        ShiftEndTime.DateTime = SamcoSoftShared.LoadedSettings.EndShiftTime ?? new DateTime(2001, 1, 1, 8, 0, 0);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        SamcoSoftShared.LoadedSettings!.StartShiftTime = ShiftStartTime.DateTime;
        SamcoSoftShared.LoadedSettings!.EndShiftTime = ShiftEndTime.DateTime;
        SamcoSoftShared.LoadedSettings!.Save();
        base.OnNavigatedFrom(e);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _session1?.Dispose();
        }
        _disposed = true;
    }

    #region Insurance Editor
    private void NewInsBtn_Click(object sender, RoutedEventArgs e)
    {
        var newInsWindows = new WinUIDialogWindow("تعریف بیمه جدید")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewInsuranceView(),
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };
        //newInsWindows.ShowDialog();
        if (newInsWindows.ShowDialog() == true) LoadData();
    }
    private void EditInsBtn_Click(object sender, RoutedEventArgs e)
    {
        if (InsList.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در ویرایش بیمه", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }
        var newInsWindows = new WinUIDialogWindow("ویرایش بیمه")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewInsuranceView(int.Parse(InsList.EditValue.ToString()!)),
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };
        if (newInsWindows.ShowDialog() == true) LoadData();
    }

    private void InsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (InsList.SelectedItem is not null)
        {
            EditInsBtn_Click(null, new RoutedEventArgs());
        }
    }
    private void DelInsBtn_Click(object sender, RoutedEventArgs e)
    {
        if (InsList.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در حذف بیمه", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }
        // Show confirmation
        if (WinUIMessageBox.Show(this, "آیا از حذف این بیمه مطمئن هستید؟ تمامی تعرفه‌های تعریف شده برای این بیمه نیز حذف خواهند شد.", "تأیید حذف بیمه", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Adorner, true) == MessageBoxResult.Yes)
        {
            var selIns = _session1.GetObjectByKey<insuranceType>(InsList.EditValue);
            selIns.Delete();
            ((XPCollection)InsList.ItemsSource).Reload();
            LoadData();
        }
    }
    #endregion

    #region Service Editor
    private void NewServiceBtn_Click(object sender, RoutedEventArgs e)
    {
        var newServiceWindows = new WinUIDialogWindow("تعریف خدمت جدید")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewServiceView(),
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };
        if (newServiceWindows.ShowDialog() == true) LoadData();
    }

    private void EditServiceBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ServiceList.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در ویرایش خدمت", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }
        var newServiceWindows = new WinUIDialogWindow("ویرایش خدمت")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewServiceView(int.Parse(ServiceList.EditValue.ToString()!)),
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };
        if (newServiceWindows.ShowDialog() == true) LoadData();
    }

    private void ServiceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ServiceList.SelectedItem is not null)
        {
            EditServiceBtn_Click(null, new RoutedEventArgs());
        }
    }

    private void DelServiceBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ServiceList.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در حذف خدمت", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }

        var selSvc = _session1.GetObjectByKey<HealthServices>(ServiceList.EditValue);
        // check if it has children
        var serviceList = selSvc.HealthServicesCollection.Select(x => x.name).ToList();
        if (!selSvc.HealthServicesCollection.Any() || WinUIMessageBox.Show(this,
                "با حذف این خدمت تمام زیرمجموعه‌های آن و تعرفه‌های تعریف شده برای آن‌ها نیز حذف خواهند شد. آیا از حذف این خدمت مطمئن هستید؟" +
                string.Join(Environment.NewLine, serviceList), "تأیید حذف خدمت", MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation, MessageBoxResult.No,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Adorner, true) !=
            MessageBoxResult.Yes) return;
        // Show confirmation
        selSvc.Delete();
        LoadData();
    }
    #endregion

    #region Role Editor

    private void NewRoleBtn_Click(object sender, RoutedEventArgs e)
    {
        var newRoleWindows = new WinUIDialogWindow("تعریف نقش جدید")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewRoleView(),
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };

        if (newRoleWindows.ShowDialog() != true) return;
        RoleList.ItemsSource = null;
        RoleList.ItemsSource = SamcoSoftShared.LoadedSettings!.PersonnelRole;
    }
    private void EditRoleBtn_Click(object sender, RoutedEventArgs e)
    {
        if (RoleList.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در ویرایش نقش", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }
        var newRoleWindows = new WinUIDialogWindow("ویرایش نقش")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewRoleView(RoleList.EditValue.ToString()),
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };

        if (newRoleWindows.ShowDialog() != true) return;
        RoleList.ItemsSource = null;
        RoleList.ItemsSource = SamcoSoftShared.LoadedSettings!.PersonnelRole;
    }

    private void RoleList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (RoleList.SelectedItem is not null)
        {
            EditRoleBtn_Click(null, new RoutedEventArgs());
        }
    }
    private void DelRoleBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (RoleList.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در حذف نقش", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }

        SamcoSoftShared.LoadedSettings!.PersonnelRole!.Remove(RoleList.SelectedItem.ToString());
        RoleList.ItemsSource = null;
        RoleList.ItemsSource = SamcoSoftShared.LoadedSettings!.PersonnelRole!;
    }

    #endregion

    #region Equipment Editor

    private void NewEquipBtn_Click(object sender, RoutedEventArgs e)
    {
        var newEquipWindows = new WinUIDialogWindow("تعریف تجهیزات جدید")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewEquipmentView(),
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };
        if (newEquipWindows.ShowDialog() == true) LoadData();
    }
    private void EditEquipBtn_Click(object sender, RoutedEventArgs e)
    {
        if (EquipList.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در ویرایش تجهیزات", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }
        var newEquipWindows = new WinUIDialogWindow("ویرایش تجهیزات")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewEquipmentView(),
            //TODO:ChangeLine
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };
        if (newEquipWindows.ShowDialog() == true) LoadData();
    }
    private void EquipList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (EquipList.SelectedItem is not null)
        {
            EditEquipBtn_Click(null, new RoutedEventArgs());
        }
    }
    private void DelEquipBtn_Click(object sender, RoutedEventArgs e)
    {
        if (EquipList.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در حذف تجهیزات", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }
        // Show confirmation
        if (WinUIMessageBox.Show(this, "آیا از حذف این تجهیزات مطمئن هستید؟", "تأیید حذف تجهیزات",
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Adorner, true) !=
            MessageBoxResult.Yes) return;
        ((Equipments)EquipList.SelectedItem).Delete();
        LoadData();
    }
    #endregion

    #region Price Editor

    private void ServiceSelCbx_SelectedIndexChanged(object sender, RoutedEventArgs e)
    {
        PriceListGrid.FilterCriteria = ServiceSelCbx.SelectedItem is not null ? new BinaryOperator("service.Oid", ServiceSelCbx.EditValue.ToString()) : null;
    }

    private void NewPriceBtn_Click(object sender, RoutedEventArgs e)
    {
        EditPriceBtn.IsEnabled = false;
        NewPriceBox.Visibility = Visibility.Visible;
    }

    private void EditPriceBtn_Click(object sender, RoutedEventArgs e)
    {
        if (PriceListGrid.SelectedItems.Count == 0)
        {
            MainNotify.ShowError("خطا در ویرایش تعرفه", "لطفاً یک تعرفه را از لیست بالا انتخاب نمایید.");
            return;
        }
        // load data in fields
        var selPrice = (Prices)PriceListGrid.SelectedItem;
        InsCbx.SelectedItem = selPrice.insType;
        SvcCbx.SelectedItem = selPrice.service;
        Price1Bx.Text = selPrice.price1.ToString();
        Price2Bx.Text = selPrice.price2.ToString();
        // Show box
        EditPriceBtn.IsEnabled = false;
        NewPriceBox.Visibility = Visibility.Visible;
    }

    private void PriceListGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (PriceListGrid.SelectedItems.Count > 0)
        {
            EditPriceBtn_Click(sender, e);
        }
    }

    private void DeletePriceBtn_Click(object sender, RoutedEventArgs e)
    {
        if (PriceListGrid.SelectedItems.Count == 0)
        {
            MainNotify.ShowError("خطا در حذف تعرفه", "لطفاً یک تعرفه را از لیست بالا انتخاب نمایید.");
            return;
        }
        // Show confirmation
        if (WinUIMessageBox.Show(this, "آیا از حذف این تعرفه مطمئن هستید؟", "تأیید حذف تعرفه", MessageBoxButton.YesNo, MessageBoxImage.Exclamation,
                MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Adorner, true) == MessageBoxResult.Yes)
        {
            var selPrice = _session1.GetObjectByKey<Prices>(((Prices)PriceListGrid.SelectedItem).Oid.ToString());
            selPrice.Delete();
            LoadData();
        }
    }
    #endregion

    #region New Price Box

    private void InsCbx_Validate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value is null ? "انتخاب بیمه الزامی است." : null);
    }

    private void SvcCbx_Validate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value is null ? "انتخاب خدمت الزامی است." : null);
    }

    private void PriceBx_Validate(object sender, ValidationEventArgs e)
    {
        e.SetError(e.Value is null ? "وارد نمودن مبلغ قابل پرداخت الزامی است." : null);
    }

    private void SavePriceBtn_Click(object sender, RoutedEventArgs e)
    {
        if (InsCbx.HasValidationError || SvcCbx.HasValidationError || Price1Bx.HasValidationError || Price2Bx.HasValidationError)
            return;
        // Check if it is existed before update

        var selPrice = _session1.Query<Prices>().SingleOrDefault(x => x.insType.Oid == ((insuranceType)InsCbx.SelectedItem).Oid && x.service.Oid == ((HealthServices)SvcCbx.SelectedItem).Oid);
        if (selPrice != null)
        {
            // update existing price
            selPrice.price1 = int.Parse(Price1Bx.Text);
            selPrice.price2 = int.Parse(Price2Bx.Text);
            selPrice.Save();
        }
        else
        {
            // New Price
            var newPrice = new Prices(_session1)
            {
                insType = (insuranceType)InsCbx.SelectedItem,
                service = (HealthServices)SvcCbx.SelectedItem,
                price1 = int.Parse(Price1Bx.Text),
                price2 = int.Parse(Price2Bx.Text)
            };
            newPrice.Save();
        }
        LoadData();
        EditPriceBtn.IsEnabled = true;
        NewPriceBox.Visibility = Visibility.Collapsed;
    }

    private void CancelBtn_Click(object sender, RoutedEventArgs e)
    {
        EditPriceBtn.IsEnabled = true;
        NewPriceBox.Visibility = Visibility.Collapsed;
    }

    #endregion
}
