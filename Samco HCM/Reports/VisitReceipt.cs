using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DevExpress.XtraReports.UI;
using Samco_HCM.Views;
using Samco_HCM_Shared;

namespace Samco_HCM.Reports
{
    public partial class VisitReceipt : XtraReport
    {
        public VisitReceipt(IEnumerable<AddonService> serviceList)
        {
            InitializeComponent();
            // Load Information
            HeaderLab.Text = SamcoSoftShared.LoadedSettings!.IsClinic ? SamcoSoftShared.LoadedSettings!.MarkazName :
                string.Format(HeaderLab.Text, SamcoSoftShared.LoadedSettings!.UniversityName, SamcoSoftShared.LoadedSettings!.ShahrestanName, SamcoSoftShared.LoadedSettings!.MarkazName);

            // Load university Icon if available
            try
            {
                UniIconBx.Image = Image.FromStream(new MemoryStream(SamcoSoftShared.LoadedSettings!.UniversityIcon!));
            }
            catch (Exception)
            {
                // ignored
            }
            ServiceTable.BeginInit();
            foreach (var itm in serviceList)
            {
                var newRow = ServiceTable.InsertRowBelow(null);
                newRow.Cells[0].Text = itm.ServiceName.name;
                newRow.Cells[1].Text = itm.Count.ToString();
            }
            ServiceTable.AdjustSize();
            ServiceTable.EndInit();
        }
    }
}
