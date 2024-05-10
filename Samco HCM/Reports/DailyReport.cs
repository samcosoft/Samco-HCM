using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using HCMData;
using Samco_HCM_Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using HandyControl.Tools;

namespace Samco_HCM.Reports;

public partial class DailyReport : XtraReport
{

    public DailyReport(IReadOnlyCollection<Visits> visitInfo, DateTime? fromDate = null, DateTime? toDate = null)
    {
        InitializeComponent();
        using var session1 = new Session();
        // Load Information
        HeaderLab.Text = SamcoSoftShared.LoadedSettings!.IsClinic ? SamcoSoftShared.LoadedSettings!.MarkazName :
            string.Format(HeaderLab.Text, SamcoSoftShared.LoadedSettings!.UniversityName, SamcoSoftShared.LoadedSettings!.ShahrestanName, SamcoSoftShared.LoadedSettings!.MarkazName);

        // Get services
        var svcQuery = session1.Query<HealthServices>();
        var svcList = svcQuery.Where(srv => srv.group != "emerg" && srv.parent == null).ToList();


        foreach (var svc in svcList)
        {
            var svcNum = visitInfo.Count(visit => visit.service.Oid == svc.Oid || visit.service.parent != null && visit.service.parent.Oid == svc.Oid);

            if (svcNum > 0)
            {
                var newSvcRow = new XRTableRow();
                newSvcRow.Cells.Add(new XRTableCell { Text = svc.name });
                newSvcRow.Cells.Add(new XRTableCell { Text = svcNum.ToString() });
                repTable.Rows.Add(newSvcRow);
            }
        }

        // Get emergency services
        var emergRow = new XRTableRow();
        emergRow.Cells.Add(new XRTableCell { Text = @"خدمات اورژانس" });
        emergRow.Cells.Add(new XRTableCell { Text = visitInfo.Count(x => x.service.group== "emerg").ToString() });
        repTable.Rows.Add(emergRow);
        // Calculate current money
        
        priceBx.Text = visitInfo.Sum(x => x.price).ToString();

        titleLab.Text = @"آمار خدمات";

        // Set name and date
        nameLabel.Text = SamcoSoftShared.CurrentUser!.RealName;
        if (fromDate != null)
        {
            fromDateLabel.Text = @"از " + new PersianDateTime(fromDate.Value).ShamsiDate + @" ساعت " + fromDate.GetValueOrDefault(DateTime.Today).ToShortTimeString();
        }
        else
        {
            fromDateLabel.Text = @"از " + new PersianDateTime(SamcoSoftShared.CurrentUser!.LastLoginTime.GetValueOrDefault(DateTime.Today)).ShamsiDate + @" ساعت " + SamcoSoftShared.CurrentUser.LastLoginTime.GetValueOrDefault(DateTime.Today).ToShortTimeString();
        }
        if (toDate != default)
        {
            toDateLabel.Text = @" تا " + new PersianDateTime(toDate.Value).ShamsiDate + @" ساعت " + toDate.GetValueOrDefault(DateTime.Today).ToShortTimeString();
        }
        else
        {
            toDateLabel.Text = @" تا " + new PersianDateTime(DateTime.Now).ShamsiDate + @" ساعت " + DateTime.Now.ToShortTimeString();
        }
        // Load university Icon if available
        try
        {
            UniIconBx.Image = Image.FromStream(new MemoryStream(SamcoSoftShared.LoadedSettings!.UniversityIcon!));
        }
        catch (Exception)
        {
            // ignored
        }
    }
}