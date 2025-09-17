using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraReports.UI;
using LabData;
using Samco_HCM_Shared;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Samco_HCM_Laboratory_Client.Reports;

public partial class LabResult : XtraReport
{
    private readonly Session _session1;
    private readonly LabVisits _visit;
    public LabResult(int visitId)
    {
        InitializeComponent();
        _session1 = new Session();
        // Load Information
        if (SamcoSoftShared.LoadedSettings!.IsClinic)
        {
            UniversityLab.Text = SamcoSoftShared.LoadedSettings.MarkazName;
            ShabakehLab.Text = SamcoSoftShared.LoadedSettings.MarkazAddress + (string.IsNullOrEmpty(SamcoSoftShared.LoadedSettings.MarkazPhone) ? " - " + SamcoSoftShared.LoadedSettings.MarkazPhone : string.Empty);
        }
        else
        {
            UniversityLab.Text += SamcoSoftShared.LoadedSettings.UniversityName;
            ShabakehLab.Text = string.Format(ShabakehLab.Text, SamcoSoftShared.LoadedSettings.ShahrestanName, SamcoSoftShared.LoadedSettings.MarkazName);
        }
        // Load university Icon if available
        try
        {
            UniIconBx.Image = Image.FromStream(new MemoryStream(SamcoSoftShared.LoadedSettings.UniversityIcon!));
        }
        catch (Exception)
        {
            // ignored
        }

        _visit = _session1.GetObjectByKey<LabVisits>(visitId);
        dataModel.DataSource = _visit;
        dataModel.Fill();

        ResultTable.Rows.Clear();
        ResultTable.BeginInit();
        var testGroup = new XPCollection<TestGroup>(_session1, null, new SortProperty("printOrder", SortingDirection.Ascending));
        foreach (var group in testGroup)
        {
            var selTest = _visit.TestCards.Where(x => ReferenceEquals(x.TestName.group, group) && x.TestName.parent == null)
                .OrderBy(x => x.TestName.printOrder).ToList();

            if (selTest.Count == 0) continue;
            var newGroupRow = new XRTableRow();
            newGroupRow.Cells.Add(new XRTableCell
            {
                Text = group.name,
                Font = new Font("Times New Roman", 14,
                    FontStyle.Bold | FontStyle.Italic)
            });
            ResultTable.Rows.Add(newGroupRow);
            foreach (var test in selTest)
            {
                if (test.TestName.TestNameCollection.Count == 0)
                {
                    var indicator = GetIndicator(test.TestName, test.Result, out var normalRange);
                    ResultTable.Rows.Add(GetTestRow(test.TestName.name, test.Result ?? test.TestName.defValue, indicator, test.TestName.unit,
                        normalRange));
                }
                else
                {
                    var titleRow = new XRTableRow();
                    titleRow.Cells.Add(new XRTableCell
                    {
                        Text = test.TestName.name,
                        Font = new Font("Vazirmatn", 10, FontStyle.Bold)
                    });
                    ResultTable.Rows.Add(titleRow);

                    //Get sublist of tests
                    var testList = _visit.TestCards
                        .Where(x => x.TestName.parent != null && x.TestName.parent.Oid == test.TestName.Oid)
                        .OrderBy(x => x.TestName.printOrder).ToList();

                    foreach (var subTest in testList)
                    {
                        var indicator = GetIndicator(subTest.TestName, subTest.Result, out var normalRange);
                        ResultTable.Rows.Add(GetTestRow(subTest.TestName.name, subTest.Result ?? subTest.TestName.defValue, indicator, subTest.TestName.unit,
                            normalRange));
                    }
                }
            }
            //add empty row for separation
            ResultTable.Rows.Add(new XRTableRow());
        }

        ResultTable.AdjustSize();
        ResultTable.EndInit();
    }

    private string? GetIndicator(TestName test, string? result, out string? normalRange)
    {
        normalRange = !string.IsNullOrEmpty(test.nlRange) ? test.nlRange.Replace("|", " - ") : null;
        if (result == null) return string.Empty;
        switch (test.dataType)
        {
            case 0:
                if (GetMaxMin(test.nlRange, out var max, out var min))
                {
                    decimal.TryParse(result, CultureInfo.InvariantCulture, out var decimalResult);
                    if (decimalResult > max) return "H";
                    if (decimalResult < min) return "L";
                }
                return string.Empty;
            case 1:
            case 2:
                if (!string.IsNullOrEmpty(normalRange) && test.nlRange != result) return "Abnormal";
                return test.nlRange;
            default:
                normalRange = "نامشخص";
                return "نامشخص";
        }
    }

    private bool GetMaxMin(string normalRange, out decimal max, out decimal min)
    {
        max = 0;
        min = 0;
        if (string.IsNullOrEmpty(normalRange) || normalRange.Split("|").Length < 2) return false;
        return decimal.TryParse(normalRange.Split("|")[1], out max) && decimal.TryParse(normalRange.Split("|")[0], out min);
    }

    private XRTableRow GetTestRow(string name, string? result, string? indicator, string? unit, string? normalRange)
    {
        var newRow = new XRTableRow();
        newRow.Cells.Add(new XRTableCell { Text = name });
        newRow.Cells.Add(new XRTableCell { Text = result ?? "نتیجه ای ثبت نشده است" });
        newRow.Cells.Add(new XRTableCell { Text = indicator ?? "نتیجه ای ثبت نشده است" });
        newRow.Cells.Add(new XRTableCell { Text = unit ?? "N/A" });
        newRow.Cells.Add(new XRTableCell { Text = normalRange ?? "N/A" });
        return newRow;
    }

    private void ResultTable_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
    {
        ResultTable.WidthF = PageWidth - Margins.Left - Margins.Right;
    }

    private void LabResult_PrintProgress(object sender, DevExpress.XtraPrinting.PrintProgressEventArgs e)
    {
        _visit.IsPrinted = true;
        _visit.Save();
    }

    protected override void OnDisposing()
    {
        _session1.Dispose();
        base.OnDisposing();
    }
}