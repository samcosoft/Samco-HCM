using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using HandyControl.Tools;
using LabData;
using Samco_HCM_Shared;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Samco_HCM_Laboratory_Client.Reports;

public partial class LabWork : XtraReport
{
    public LabWork(TestGroup labGroup, DateTime fromDate, DateTime toDate, out bool hasData)
    {
        InitializeComponent();
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
        DateBx.Text = string.Format(DateBx.Text, new PersianDateTime(fromDate).ShamsiDate, fromDate.ToString("hh:MM"), new PersianDateTime(toDate).ShamsiDate, toDate.ToString("hh:MM"));
        // Load university Icon if available
        try
        {
            UniIconBx.Image = Image.FromStream(new MemoryStream(SamcoSoftShared.LoadedSettings.UniversityIcon!));
        }
        catch (Exception)
        {
            // ignored
        }

        WorkTypeBx.Text = labGroup.name;
        using var session = new Session();
        var filteredTests = session.Query<LabVisits>().Where(x => x.visitDate >= fromDate && x.visitDate <= toDate)
            .OrderBy(x => x.Oid)
            .SelectMany(x => x.TestCards).Where(x => x.TestName.group.Oid == labGroup.Oid)
            .OrderBy(x => x.TestName.printOrder)
            .Select(x => new { VisitId = x.Visit.Oid, PatientName = x.Visit.Patient.Name, TestName = x.TestName.name })
            .GroupBy(x => x.VisitId).ToList();
        if (filteredTests.Count == 0)
        {
            hasData = false;
            return;
        }

        repTable.BeginInit();
        var cellWidth = repTable.Rows[0].Cells[1].WidthF;
        foreach (var test in filteredTests)
        {
            // Create a new report band for each test
            var newRow = new XRTableRow();
            var resRow = new XRTableRow();
            newRow.Cells.Add(new XRTableCell { Text = test.First().PatientName, WidthF = 300});
            resRow.Cells.Add(new XRTableCell { Text = test.First().VisitId.ToString(), WidthF = 300});
            foreach (var itm in test)
            {
                newRow.Cells.Add(new XRTableCell { Text = itm.TestName, WordWrap = true, WidthF = cellWidth / test.Count() });
                resRow.Cells.Add(new XRTableCell { Text = " ", WidthF = cellWidth / test.Count() });
            }
            repTable.Rows.Add(newRow);
            repTable.Rows.Add(resRow);
        }
        //repTable.AdjustSize();


        repTable.EndInit();
        hasData = true;
    }
}