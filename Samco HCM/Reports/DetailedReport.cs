using DevExpress.XtraReports.UI;
using System;
using System.Drawing;
using DevExpress.Xpo;
using HandyControl.Tools;
using Samco_HCM_Shared;
using HCMData;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Samco_HCM.Reports
{
    public partial class DetailedReport : XtraReport
    {
        public DetailedReport(DateTime startDate, DateTime endDate, List<Visits> visitList)
        {
            InitializeComponent();
            using var session1 = new Session();
            // Load Information
            if (SamcoSoftShared.LoadedSettings!.IsClinic)
            {
                UniversityLab.Text = SamcoSoftShared.LoadedSettings!.MarkazName;
                ShabakehLab.Text = SamcoSoftShared.LoadedSettings!.MarkazAddress + (string.IsNullOrEmpty(SamcoSoftShared.LoadedSettings!.MarkazPhone) ? " - " + SamcoSoftShared.LoadedSettings!.MarkazPhone : string.Empty);
            }
            else
            {
                UniversityLab.Text += SamcoSoftShared.LoadedSettings!.UniversityName;
                ShabakehLab.Text = string.Format(ShabakehLab.Text, SamcoSoftShared.LoadedSettings!.ShahrestanName, SamcoSoftShared.LoadedSettings!.MarkazName);
            }
            repDateLabel.Text = string.Format(repDateLabel.Text, new PersianDateTime(startDate).ShamsiDate, startDate.ToString("hh:MM"), new PersianDateTime(endDate).ShamsiDate, endDate.ToString("hh:MM"));
            // Load university Icon if available
            try
            {
                UniIconBx.Image = Image.FromStream(new MemoryStream(SamcoSoftShared.LoadedSettings!.UniversityIcon!));
            }
            catch (Exception)
            {
                // ignored
            }
            // Add services statistics
            ServiceTab.BeginInit();
            var svcList = visitList.GroupBy(x => x.service);
            foreach (var svc in svcList)
            {
                var newRow = new XRTableRow();
                newRow.Cells.Add(new XRTableCell { Text = svc.Key.name });
                newRow.Cells.Add(new XRTableCell { Text = svc.Count().ToString() });
                ServiceTab.Rows.Add(newRow);
            }
            ServiceTab.AdjustSize();
            ServiceTab.EndInit();

            // Add Equipment statistics
            EquipTab.BeginInit();
            var equipmentList = visitList.SelectMany(x => x.EquipmentLists).GroupBy(x => x.EquipName).ToList();
            foreach (var itm in equipmentList)
            {
                var newRow = new XRTableRow();
                var equipmentCount = itm.Sum(x => x.Count);
                newRow.Cells.Add(new XRTableCell { Text = itm.Key.Name });
                newRow.Cells.Add(new XRTableCell { Text = equipmentCount.ToString() });
                newRow.Cells.Add(new XRTableCell { Text = (equipmentCount * itm.Key.Price).ToString() });
                EquipTab.Rows.Add(newRow);
            }

            // Add sum report
            var sumRow = EquipTab.InsertRowBelow(default);
            sumRow.Cells[0].Text = @"جمع کل";
            sumRow.Cells[1].Text = equipmentList.Sum(x => x.Key.Price * x.Sum(y => y.Count)) + @" تومان";
            sumRow.Cells[2].Visible = false;

            EquipTab.AdjustSize();
            EquipTab.EndInit();

            // Add Personnel names
            PersonnelTab.BeginInit();
            var personnelList = visitList.GroupBy(x => x.Personnel).Select(x => new { PersonName = x.Key.Name, ServiceVisit = x.GroupBy(svc => svc.service) }).OrderBy(x => x.PersonName);
            foreach (var person in personnelList)
            {
                var serviceRowCollection = new List<XRTableRow>();
                var totalPrice = 0;
                foreach (var svc in person.ServiceVisit)
                {
                    var newRow = new XRTableRow();
                    newRow.Cells.Add(new XRTableCell());
                    newRow.Cells.Add(new XRTableCell { Text = svc.Key.name });
                    newRow.Cells.Add(new XRTableCell { Text = svc.Count().ToString() });
                    var price = svc.Sum(x => x.price - x.EquipmentLists.Sum(y => y.EquipName.Price * y.Count));
                    totalPrice += price;
                    newRow.Cells.Add(new XRTableCell { Text = price.ToString() });
                    serviceRowCollection.Add(newRow);
                }

                var sumPersonRow = new XRTableRow();
                sumPersonRow.Cells.Add(new XRTableCell());
                sumPersonRow.Cells.Add(new XRTableCell { Text = @"جمع کل" });
                sumPersonRow.Cells.Add(new XRTableCell { Text = totalPrice + @" تومان" });
                sumPersonRow.Cells.Add(new XRTableCell { Visible = false });
                serviceRowCollection.Add(sumPersonRow);

                serviceRowCollection.First().Cells[0].Text = person.PersonName;
                serviceRowCollection.First().Cells[0].RowSpan = person.ServiceVisit.Count() + 1;
                PersonnelTab.Rows.AddRange(serviceRowCollection.ToArray());
            }
            PersonnelTab.AdjustSize();
            PersonnelTab.EndInit();
        }

        private void PersonnelTab_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PersonnelTab.WidthF = PageWidth - Margins.Left - Margins.Right - 25;
        }
    }
}
