using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using HandyControl.Data;
using HCMData;
using Samco_HCM.Classes;
using Samco_HCM.Reports;
using Samco_HCM_Shared;

namespace Samco_HCM.Views;

/// <summary>
/// Interaction logic for Statistics.xaml
/// </summary>
public partial class Statistics : IDisposable
{
    private DateTime _fromDate, _toDate;
    private Session _session1;
    public Statistics()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        _session1 = new Session();
        // Set default date values
        ToDateBx.SelectedDateTime = DateTime.Now;
        FromDateBx.SelectedDateTime = DateTime.Today;

        // Add user list
        UserSelCbx.Items.Clear();
        foreach (var x in _session1.Query<Users>())
            UserSelCbx.Items.Add(new ComboBoxEditItem { Content = x.realname, Tag = x.Oid });
        UserSelCbx.ShowCustomItems = true;

        // Set visibility
        if (e.Parameter.ToString() == "all")
        {
            DateSelGrp.Visibility = Visibility.Visible;
            DailyStatPanel.Caption = "آمار لیست";
        }
        else
        {
            DateSelGrp.Visibility = Visibility.Collapsed;
            DailyStatPanel.Caption = "آمار امروز تا این لحظه";
        }

        // Set permission
        if (SamcoAdd.UserIsAdmin())
        {
            UserSelLab.Visibility = Visibility.Visible;
            UserSelCbx.SelectAllItems();
            GridCont.View.ShowFilterPanelMode = DevExpress.Xpf.Grid.ShowFilterPanelMode.Default;
        }
        else
        {
            UserSelLab.Visibility = Visibility.Collapsed;
            UserSelCbx.SelectedItem =
                UserSelCbx.Items.Cast<Users>().Where(x => x.Oid == SamcoSoftShared.CurrentUser!.Oid);
            GridCont.View.ShowFilterPanelMode = DevExpress.Xpf.Grid.ShowFilterPanelMode.Never;
        }
        LoadGridData();
    }

    private void LoadGridData()
    {
        var serverData = new XPCollection<Visits>(_session1, new BetweenOperator("visitDate", _fromDate, _toDate));
        GridCont.ItemsSource = serverData;
        GridCont.RefreshData();
        SetGridCriteria();
    }
    private void SetGridCriteria()
    {
        if (UserSelCbx.SelectedItems.Count < UserSelCbx.Items.Count && UserSelCbx.SelectedItems.Count > 0)
        {
            var criteriaList = (from usr in UserSelCbx.SelectedItems.Cast<ComboBoxEditItem>()
                                                   select new BinaryOperator("user.Oid", usr.Tag.ToString())).Cast<CriteriaOperator>().ToList();


            GridCont.FilterCriteria = CriteriaOperator.Or([.. criteriaList]);
        }
        else if (UserSelCbx.SelectedItems.Count == 0)
        {
            // Show nothing
            GridCont.FilterCriteria = new BinaryOperator("user.Oid", "-1");
        }
        else
        {
            // No filtering
            GridCont.FilterCriteria = null;
        }
        CalculateStatistics();
    }

    private void GridCont_FilterChanged(object sender, RoutedEventArgs e)
    {
        CalculateStatistics();
    }

    private void CalculateStatistics()
    {
        // calculate statistics information
        StatPanel.Children.Clear();
        var todayInfo = new List<Visits>();
        if (GridCont.VisibleRowCount > 0)
        {
            for (int i = 0, loopTo = GridCont.VisibleRowCount - 1; i <= loopTo; i++)
                todayInfo.Add((Visits)GridCont.GetRow(GridCont.GetRowHandleByVisibleIndex(i)));
        }

        // Show daily information
        var svcQuery = _session1.Query<HealthServices>();
        // Get emergency services

        var emergencyPan = new TextEdit { Text = "0" };

        try
        {
            emergencyPan.Text = todayInfo.Count(x => x.service.group == "emerg").ToString();
        }
        catch (Exception)
        {
            // MessageBox.Show("خطا در شمارش خدمت اورژانس", "بروز خطای سیستمی", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading Or MessageBoxOptions.RightAlign)
        }

        var emergencyGroup = new LayoutGroup { Header = "خدمات اورژانس" };
        emergencyGroup.Children.Add(emergencyPan);
        StatPanel.Children.Add(emergencyGroup);

        var services = svcQuery.Where(x => x.group != "emerg" && x.parent == null);

        foreach (var svc in services)
        {
            var statTxt = new TextEdit { Text = "0" };
            var selServiceVisit = new List<Visits>();

            try
            {
                selServiceVisit = todayInfo.Where(x => x.service.Oid == svc.Oid ||
                                                       x.service.parent?.Oid == svc.Oid).ToList();
            }
            catch (Exception)
            {
                //Ignored
            }

            statTxt.Text = selServiceVisit.Count.ToString();
            var lg = new LayoutGroup { Header = svc.name };
            lg.Children.Add(statTxt);
            StatPanel.Children.Add(lg);
        }
        // Calculate current money
        var equipPrice = todayInfo.SelectMany(x => x.EquipmentLists).GroupBy(x => x.EquipName)
            .Sum(x => x.Key.Price * x.Sum(y => y.Count));

        var equipTxt = new TextEdit { Text = equipPrice.ToString() };
        var equipPriceGroup = new LayoutGroup { Header = "مبلغ تجهیزات" };
        equipPriceGroup.Children.Add(equipTxt);
        StatPanel.Children.Add(equipPriceGroup);

        var moneyTxt = new TextEdit { Text = (todayInfo.Sum(x => x.price) - equipPrice).ToString() };
        var servicePriceGroup = new LayoutGroup { Header = "مبلغ خدمات" };
        servicePriceGroup.Children.Add(moneyTxt);
        StatPanel.Children.Add(servicePriceGroup);
        // Add print Button
        var printStatBtn = new SimpleButton { Content = "چاپ آمار فعلی", Height = 30 };
        printStatBtn.Click += PrintStatList;
        StatPanel.Children.Add(printStatBtn);
        var printEmergBtn = new SimpleButton { Content = "چاپ آمار اورژانس", Height = 30 };
        printEmergBtn.Click += PrintEmergencyList;
        StatPanel.Children.Add(printEmergBtn);
    }

    private void DateBx_OnSelectedDateTimeChanged(object sender, FunctionEventArgs<DateTime?> e)
    {
        if (FromDateBx.SelectedDateTime is null || ToDateBx.SelectedDateTime is null) return;
        _fromDate = FromDateBx.SelectedDateTime.GetValueOrDefault();
        _toDate = ToDateBx.SelectedDateTime.GetValueOrDefault();
        if (_fromDate > _toDate)
        {
            MainNotify.ShowWarning("خطا در انتخاب تاریخ",
                "تاریخ شروع نمی‌تواند بزرگتر از تاریخ پایان گزارش باشد. لطفاً ابتدا تاریخ پایان گزارش را تعیین کنید.");
            FromDateBx.SelectedDateTime = ToDateBx.SelectedDateTime;
        }
        else
        {
            LoadGridData();
        }
    }
    private void UserSelCbx_EditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        SetGridCriteria();
    }

    private void CurrentRepBtn_Click(object sender, RoutedEventArgs e)
    {
        GridCont.View.ShowPrintPreviewDialog(Window.GetWindow(this));
    }

    private void PrintStatList(object sender, RoutedEventArgs e)
    {
        PrintReport(false);
    }

    private void PrintEmergencyList(object sender, RoutedEventArgs e)
    {
        PrintReport(true);
    }

    private void PrintReport(bool isEmergency)
    {
        var todayInfo = new List<Visits>();
        if (GridCont.VisibleRowCount > 0)
        {
            for (int i = 0, loopTo = GridCont.VisibleRowCount - 1; i <= loopTo; i++)
                todayInfo.Add((Visits)GridCont.GetRow(GridCont.GetRowHandleByVisibleIndex(i)));
        }

        if (!isEmergency)
        {
            var dailyReport = new DailyReport(todayInfo, _fromDate, _toDate);
            try
            {
                PrintHelper.PrintDirect(dailyReport);
                MainNotify.ShowInformation("صدور گزارش", "گزارش چاپ شد.");

            }
            catch (Exception ex)
            {
                MainNotify.ShowError("خطا در صدور گزارش", "چاپگر نصب و تنظیم نیست.");
                MainNotify.ShowError("خطا در صدور گزارش", ex.ToString());
            }
        }
        else
        {
            //Print Emergency
            var dailyReport = new DailyReportEmergency(todayInfo, _fromDate, _toDate);
            try
            {
                PrintHelper.PrintDirect(dailyReport);
                MainNotify.ShowInformation("صدور گزارش", "گزارش چاپ شد.");

            }
            catch (Exception ex)
            {
                MainNotify.ShowError("خطا در صدور گزارش", "چاپگر نصب و تنظیم نیست.");
                MainNotify.ShowError("خطا در صدور گزارش", ex.ToString());
            }
        }
    }
    private void ServiceRepBtn_Click(object sender, RoutedEventArgs e)
    {
        WaitIndicator.IsSplashScreenShown = true;

        Dispatcher.Invoke(() =>
          {
              List<Visits> visitList = [];
              if (GridCont.VisibleRowCount > 0)
              {
                  for (int i = 0, loopTo = GridCont.VisibleRowCount - 1; i <= loopTo; i++)
                      visitList.Add((Visits)GridCont.GetRow(GridCont.GetRowHandleByVisibleIndex(i)));
              }

              var report = new DetailedReport(_fromDate, _toDate, visitList);
              WaitIndicator.IsSplashScreenShown = false;
              PrintHelper.ShowRibbonPrintPreviewDialog((MainWindow)Application.Current.MainWindow, report);
          });
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        _session1?.Dispose();
        base.OnNavigatedFrom(e);
    }

    public void Dispose()
    {
        _session1?.Dispose();
    }
}