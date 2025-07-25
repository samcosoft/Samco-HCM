using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using HandyControl.Tools;
using LabData;
using Samco_HCM_Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Samco_HCM_Laboratory_Client.Reports
{
    public partial class LabReport : XtraReport
    {
        private readonly Session _session1 = new();
        public LabReport(List<LabVisits> todayInfo, DateTime fromDate, DateTime toDate)
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
            repDateLabel.Text = string.Format(repDateLabel.Text, new PersianDateTime(fromDate).ShamsiDate, fromDate.ToString("hh:MM"), new PersianDateTime(toDate).ShamsiDate, toDate.ToString("hh:MM"));
            // Load university Icon if available
            try
            {
                UniIconBx.Image = Image.FromStream(new MemoryStream(SamcoSoftShared.LoadedSettings.UniversityIcon!));
            }
            catch (Exception)
            {
                // ignored
            }

            repTable.BeginInit();
            // Load data
            var tests = todayInfo.Where(x => x.paid).SelectMany(x => x.TestCards)
                .Select(x => new { x.TestName, x.Visit }).Where(x => x.TestName.parent == null)
                .GroupBy(x => x.TestName.name).ToList();

            foreach (var test in tests)
            {
                // Create a new report band for each test
                var newRow = new XRTableRow();
                newRow.Cells.Add(new XRTableCell { Text = test.Key });
                newRow.Cells.Add(new XRTableCell { Text = test.Count().ToString() });
                newRow.Cells.Add(new XRTableCell { Text = test.Sum(x => GetPrice(x.TestName, x.Visit.insType)).ToString() });
                repTable.Rows.Add(newRow);
            }
            repTable.AdjustSize();
            repTable.EndInit();
        }

        private int GetPrice(TestName test, LabInsuranceType insurance)
        {
            return _session1.GetObjectByKey<TestName>(test.Oid).TestPrices
                .FirstOrDefault(x => x.insType.Oid == insurance.Oid)?.price ?? 0;
        }

        protected override void OnDisposing()
        {
            _session1.Dispose();
            base.OnDisposing();
        }

        private void LabReport_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            repTable.WidthF = PageWidth - Margins.Left - Margins.Right;
        }
    }
}
