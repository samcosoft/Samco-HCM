using DevExpress.Data.Filtering;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using HandyControl.Data;
using LabData;
using Samco_HCM_Laboratory_Client.Classes;
using Samco_HCM_Laboratory_Client.Reports;
using Samco_HCM_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NavigationEventArgs = DevExpress.Xpf.WindowsUI.Navigation.NavigationEventArgs;

namespace Samco_HCM_Laboratory_Client.Views;

/// <summary>
/// Interaction logic for StatisticsView.xaml
/// </summary>
public partial class StatisticsView : IDisposable
{
    private DateTime _fromDate, _toDate;
    private Session? _session1;
    public StatisticsView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        _session1 = new Session();
        // Set default date values
        ToDateBx.SelectedDateTime = DateTime.Now;
        FromDateBx.SelectedDateTime = DateTime.Today;

        // Add user list
        UserSelCbx.Items.Clear();
        foreach (var x in _session1.Query<LabUsers>())
            UserSelCbx.Items.Add(new ComboBoxEditItem { Content = x.realname, Tag = x.Oid });
        UserSelCbx.ShowCustomItems = true;

        // Set permission
        if (SamcoAdd.UserIsAdmin())
        {
            UserSelLab.Visibility = Visibility.Visible;
            UserSelCbx.SelectAllItems();
            VisitGrid.View.ShowFilterPanelMode = DevExpress.Xpf.Grid.ShowFilterPanelMode.Default;
        }
        else
        {
            UserSelLab.Visibility = Visibility.Collapsed;
            UserSelCbx.SelectedItem =
                UserSelCbx.Items.Cast<LabUsers>().Where(x => x.Oid == SamcoSoftShared.CurrentUser!.Oid);
            VisitGrid.View.ShowFilterPanelMode = DevExpress.Xpf.Grid.ShowFilterPanelMode.Never;
        }
        LoadGridData();
    }

    public void Dispose()
    {
        _session1?.Dispose();
    }
    private void LoadGridData()
    {
        var serverData = new XPServerCollectionSource(_session1, _session1!.GetClassInfo<LabVisits>(), new BetweenOperator("visitDate", _fromDate, _toDate));
        VisitGrid.ItemsSource = serverData;
        VisitGrid.RefreshData();
        SetGridCriteria();
    }

    private void SetGridCriteria()
    {
        if (UserSelCbx.SelectedItems.Count < UserSelCbx.Items.Count && UserSelCbx.SelectedItems.Count > 0)
        {
            var criteriaList = UserSelCbx.SelectedItems.Cast<ComboBoxEditItem>().
                Select(x => new BinaryOperator("user.Oid", x.Tag.ToString())).Cast<CriteriaOperator>().ToList();

            VisitGrid.FilterCriteria = CriteriaOperator.Or(criteriaList);
        }
        else if (UserSelCbx.SelectedItems.Count == 0)
        {
            // Show nothing
            VisitGrid.FilterCriteria = new BinaryOperator("user.Oid", "-1");
        }
        else
        {
            // No filtering
            VisitGrid.FilterCriteria = null;
        }
        CalculateStatistics();
    }

    private void VisitGrid_FilterChanged(object sender, RoutedEventArgs e)
    {
        CalculateStatistics();
    }

    private void CalculateStatistics()
    {
        // calculate statistics information
        StatPanel.Children.Clear();
        var todayInfo = new List<LabVisits>();
        if (VisitGrid.VisibleRowCount > 0)
        {
            for (int i = 0, loopTo = VisitGrid.VisibleRowCount - 1; i <= loopTo; i++)
                todayInfo.Add((LabVisits)VisitGrid.GetRow(VisitGrid.GetRowHandleByVisibleIndex(i)));
        }

        var todayTests = todayInfo.SelectMany(x => x.TestCards).Select(x => x.TestName)
            .Where(x => x.parent == null).GroupBy(x => x.name).ToList();
        // Show daily information
        foreach (var test in todayTests)
        {
            var testGroup = new LayoutGroup { Header = test.Key };
            testGroup.Children.Add(new LayoutItem { Content = new TextEdit { Text = test.Count().ToString() } });
            StatPanel.Children.Add(testGroup);
        }

        // Calculate current money
        var totalPrice = todayInfo.Where(x => x.paid).Sum(x => x.Price);
        var totalPriceGroup = new LayoutGroup { Header = "مبلغ دریافتی" };
        totalPriceGroup.Children.Add(new LayoutItem { Content = new TextEdit { Text = totalPrice.ToString() } });
        StatPanel.Children.Add(totalPriceGroup);
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
        VisitGrid.View.ShowPrintPreviewDialog(Window.GetWindow(this));
    }

    private void PrintStatList(object sender, RoutedEventArgs e)
    {
        var todayInfo = new List<LabVisits>();
        if (VisitGrid.VisibleRowCount > 0)
        {
            for (int i = 0, loopTo = VisitGrid.VisibleRowCount - 1; i <= loopTo; i++)
                todayInfo.Add((LabVisits)VisitGrid.GetRow(VisitGrid.GetRowHandleByVisibleIndex(i)));
        }
        var labReport = new LabReport(todayInfo, _fromDate, _toDate);
        using var printTool = new ReportPrintTool(labReport);
        printTool.ShowRibbonPreviewDialog();
    }

    private void PrintWorkBtn_Click(object sender, RoutedEventArgs e)
    {
        WaitIndic.IsSplashScreenShown = true;
        var newReport = new XtraReport();
        var reportTask = Task.Run(async () =>
        {
            foreach (var testGroup in _session1.Query<TestGroup>().ToList())
            {
                var workSheet = new LabWork(testGroup, _fromDate, _toDate, out var hasData);
                if (hasData)
                {
                    await workSheet.CreateDocumentAsync();
                    newReport.Pages.AddRange(workSheet.Pages);
                }
            }
        });
        reportTask.Wait();
        WaitIndic.IsSplashScreenShown = false;
        using var printTool = new ReportPrintTool(newReport);
        printTool.ShowRibbonPreviewDialog();
    }
}