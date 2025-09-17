using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using LabData;
using Samco_HCM_Laboratory_Client.Classes;
using Samco_HCM_Laboratory_Client.Reports;
using Samco_HCM_Laboratory_Client.Views.LabResultViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Samco_HCM_Laboratory_Client.Views;

/// <summary>
/// Interaction logic for LabResultView.xaml
/// </summary>
public partial class LabResultView : IDisposable
{
    private Session _session = new();
    private LabVisits? _selVisit;

    public LabResultView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        FactorGrid.ItemsSource = new XPServerCollectionSource(_session, _session.GetClassInfo<LabVisits>());
        TestFilterCbx.ItemsSource = new XPCollection<TestGroup>(_session);
    }

    public void Dispose()
    {
        _session.Dispose();
    }

    private void FactorGrid_OnCustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
    {
        var testOid = e.GetListSourceFieldValue("Oid").ToString();
        if (!string.IsNullOrEmpty(testOid))
            e.Value = CompletedCheck(int.Parse(e.GetListSourceFieldValue("Oid").ToString()!), out _);
    }

    private void RefreshVisitGridBtn(object sender, RoutedEventArgs e)
    {
        ((XPServerCollectionSource)FactorGrid.ItemsSource).Reload();
        FactorGrid.RefreshData();
        MainNotify.ShowInformation("به روز رسانی", ".به روزرسانی با موفقیت انجام شد");
    }

    private bool CompletedCheck(int visitOid, out List<TestName> inCompleted)
    {
        var selVisit = _session.GetObjectByKey<LabVisits>(visitOid);
        var incompleteTests = selVisit.TestCards.Where(x => x.TestName.TestNameCollection.Count == 0 && string.IsNullOrEmpty(x.Result)).ToList();
        inCompleted = [];
        inCompleted.AddRange(incompleteTests.Select(x => x.TestName));
        return incompleteTests.Count == 0;
    }

    private void FactorGrid_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.NewItem != null)
        {
            _selVisit = e.NewItem as LabVisits;
            LoadTestList();
        }
    }

    private void LoadTestList()
    {
        LabTestList.Children.Clear();
        if (_selVisit == null) return;
        var testGroups = _session.Query<TestGroup>().OrderBy(x => x.printOrder).ToList();
        foreach (var group in testGroups)
        {
            var groupTests = _selVisit.TestCards.Where(x => ReferenceEquals(x.TestName.group, group) && x.TestName.parent == null)
                .OrderBy(x => x.TestName.printOrder).ToList();

            if (groupTests.Count == 0) continue;

            var testStack = new LayoutGroup { Orientation = Orientation.Vertical, ItemSpace = 10 };

            foreach (var test in groupTests)
            {
                if (test.TestName.TestNameCollection.Count == 0)
                {
                    var newTest = new TestNameView(new TestGroupData(test.TestName.name, [test]));
                    testStack.Children.Add(newTest);
                }
                else
                {
                    var testList = _selVisit.TestCards
                        .Where(x => x.TestName.parent != null && x.TestName.parent.Oid == test.TestName.Oid).ToList();
                    var newTest = new TestNameView(new TestGroupData(test.TestName.name, testList));
                    testStack.Children.Add(newTest);
                }
            }

            var testGroup = new LayoutGroup
            { Header = group.name, View = LayoutGroupView.GroupBox, Orientation = Orientation.Vertical };
            testGroup.Children.Add(testStack);
            LabTestList.Children.Add(testGroup);
        }
    }

    private void SaveResults()
    {
        var allTests = new List<DependencyObject>();
        SamcoAdd.PrintVisualTreeRecursive(ref allTests, LabTestList, typeof(LayoutItem));
        try
        {
            foreach (var test in allTests.Cast<LayoutItem>())
            {
                if (test.DataContext is not TestCard testCard) continue;
                testCard.Save();
            }
        }
        catch (Exception)
        {
            MainNotify.ShowError("خطا در ذخیره اطلاعات","لطفاً دوباره اطلاعات را ذخیره کنید.");
        }
       
    }

    private void SaveBillBtn_OnClick(object sender, RoutedEventArgs e)
    {
        SaveResults();
        MainNotify.ShowSuccess("ذخیره اطلاعات", "اطلاعات ذخیره شد.");
    }

    private void CancelBillBtn_OnClick(object sender, RoutedEventArgs e)
    {
        _session = new Session();
        var oldOid = _selVisit?.Oid;
        FactorGrid.ItemsSource = new XPServerCollectionSource(_session, _session.GetClassInfo<LabVisits>());
        if (oldOid != null)
        {
            FactorGrid.BeginSelection();
            FactorGrid.UnselectAll();
            for (var index = 0; index < FactorGrid.VisibleRowCount; index++)
            {
                var rowHandle = FactorGrid.GetRowHandleByVisibleIndex(index);
                var cellValue = FactorGrid.GetCellValue(rowHandle, "Oid");
                if (cellValue != null && cellValue.Equals(oldOid))
                    FactorGrid.SelectItem(rowHandle);
            }
            FactorGrid.EndSelection();
            FactorGrid.SelectedItem = _session.GetObjectByKey<LabVisits>(oldOid);
        }
    }

    private void QuickPrintBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selVisit == null)
        {
            MainNotify.ShowWarning("خطا در چاپ", "لطفاً یک ویزیت را انتخاب کنید");
            return;
        }

        if (!CheckPrint()) return;
        var printReport = new LabResult(_selVisit.Oid);
        WaitSplash.IsSplashScreenShown = true;
        var reportTask = Task.Run(async () =>
        {
            await printReport.CreateDocumentAsync();
            using var printTool = new ReportPrintTool(printReport);
            printTool.Print();
        });
        reportTask.Wait();
        WaitSplash.IsSplashScreenShown = false;
    }

    private void PrintBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selVisit == null)
        {
            MainNotify.ShowWarning("خطا در چاپ", "لطفاً یک ویزیت را انتخاب کنید");
            return;
        }

        if (!CheckPrint()) return;
        var printReport = new LabResult(_selVisit.Oid);
        WaitSplash.IsSplashScreenShown = true;
        var reportTask = Task.Run(async () =>
        {
            await printReport.CreateDocumentAsync();
            using var printTool = new ReportPrintTool(printReport);
            printTool.ShowRibbonPreviewDialog();
        });
        reportTask.Wait();
        WaitSplash.IsSplashScreenShown = false;
    }

    private bool CheckPrint()
    {
        if (_selVisit == null) return false;
        SaveResults();

#if !DEBUG
       if (!_selVisit.paid)
        {
            MainNotify.ShowWarning("خطا در چاپ", "هزینه آزمایشات پرداخت نشده است. شما مجاز به چاپ نتایج نیستید");
            return false;
        } 
#endif

        if (!CompletedCheck(_selVisit.Oid, out var list))
        {
            var testParameter = list.Select(x => x.name + (x.parent != null ? $" ({x.parent?.name})" : string.Empty));
            if (WinUIMessageBox.Show(this,
                    "آزمایشات زیر ثبت نشده‌اند. آیا از چاپ آزمایشات مطمئن هستید؟" + Environment.NewLine + Environment.NewLine +
                    string.Join(" - ", testParameter), "آزمایشات ناقص", MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation, MessageBoxResult.Cancel,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Adorner) ==
                MessageBoxResult.Cancel) return false;
        }

        return !_selVisit.IsPrinted ||
               WinUIMessageBox.Show("نتایج فعلی یک بار قبلاً چاپ شده‌اند. آیا مایل به چاپ دوباره آزمایشات هستید؟", "چاپ دوباره", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) != MessageBoxResult.No;
    }

    private void TestFilterCbx_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
    {
        if (e.NewValue != null)
        {
            var visitDataSource = (XPServerCollectionSource)FactorGrid.ItemsSource;
            visitDataSource.FixedFilterCriteria = new ContainsOperator(nameof(LabVisits.TestCards), new BinaryOperator("TestName.group.Oid", ((TestGroup)TestFilterCbx.SelectedItem).Oid));
        }
        else
        {
            var visitDataSource = (XPServerCollectionSource)FactorGrid.ItemsSource;
            visitDataSource.FixedFilterCriteria = null;
        }

        FactorGrid.RefreshData();
    }
}