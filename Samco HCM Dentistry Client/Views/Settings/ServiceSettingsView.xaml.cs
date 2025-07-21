using DentData;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using Samco_HCM_Dentistry_Client.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Editors;

namespace Samco_HCM_Dentistry_Client.Views;

/// <summary>
/// Interaction logic for ServiceSettingsView.xaml
/// </summary>
public partial class ServiceSettingsView : IDisposable
{
    private readonly Session _session1 = new();

    public ServiceSettingsView()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        //Load Data
        InsuranceList.ItemsSource = new XPCollection<InsuranceTypes>(_session1);
        ServiceListGrid.ItemsSource = new XPCollection<Services>(_session1);

        var serviceCollection = new XPCollection<Services>(_session1).ToList();
        SvcCbx.ItemsSource = serviceCollection;
        ServiceSelCbx.ItemsSource = serviceCollection;
        PriceListGrid.ItemsSource = new XPCollection<Prices>(_session1);
    }
 
    public void Dispose()
    {
        _session1.Dispose();
    }

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
        if (ServiceListGrid.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در ویرایش خدمت", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }
        var newServiceWindows = new WinUIDialogWindow("ویرایش خدمت")
        {
            FlowDirection = FlowDirection.RightToLeft,
            Content = new NewServiceView(((Services)ServiceListGrid.SelectedItem).Oid),
            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
            FontSize = 16
        };
        if (newServiceWindows.ShowDialog() == true) LoadData();
    }

    private void DelServiceBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ServiceListGrid.SelectedItem is null)
        {
            MainNotify.ShowError("خطا در حذف خدمت", "لطفاً یک مورد را انتخاب نمایید.");
            return;
        }
        // Show confirmation
        if (WinUIMessageBox.Show(this,
                "آیا از حذف این خدمت مطمئن هستید؟ تمامی تعرفه‌های تعریف شده برای این خدمت نیز حذف خواهند شد.",
                "تأیید حذف خدمت", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Adorner, true) ==
            MessageBoxResult.Yes)
        {
            var selIns = _session1.GetObjectByKey<Services>(((Services)ServiceListGrid.SelectedItem).Oid);
            selIns.Delete();
            LoadData();
        }
    }

    private void ServiceListGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ServiceListGrid.SelectedItem is not null)
        {
            EditServiceBtn_Click(null, new RoutedEventArgs());
        }
    }

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
        InsCbx.SelectedItem = selPrice.Insurance;
        SvcCbx.SelectedItem = selPrice.Service;
        Price1Bx.Text = selPrice.Price1.ToString();
        Price2Bx.Text = selPrice.Price2.ToString();
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

        var selPrice = _session1.Query<Prices>().SingleOrDefault(x => x.Insurance.Oid == ((InsuranceTypes)InsCbx.SelectedItem).Oid && x.Service.Oid == ((Services)SvcCbx.SelectedItem).Oid);
        if (selPrice != null)
        {
            // update existing price
            selPrice.Price1 = int.Parse(Price1Bx.Text);
            selPrice.Price2 = int.Parse(Price2Bx.Text);
            selPrice.Save();
        }
        else
        {
            // New Price
            var newPrice = new Prices(_session1)
            {
                Insurance = (InsuranceTypes)InsCbx.SelectedItem,
                Service = (Services)SvcCbx.SelectedItem,
                Price1 = int.Parse(Price1Bx.Text),
                Price2 = int.Parse(Price2Bx.Text)
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