namespace Samco_HCM_Laboratory_Client.Reports
{
    partial class LabWork
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabWork));
            DevExpress.XtraPrinting.Shape.ShapeRectangle shapeRectangle1 = new DevExpress.XtraPrinting.Shape.ShapeRectangle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.repTable = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.CopyrightLab = new DevExpress.XtraReports.UI.XRLabel();
            this.XrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.UniIconBx = new DevExpress.XtraReports.UI.XRPictureBox();
            this.XrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.UniversityLab = new DevExpress.XtraReports.UI.XRLabel();
            this.ShabakehLab = new DevExpress.XtraReports.UI.XRLabel();
            this.WorkTypeBx = new DevExpress.XtraReports.UI.XRLabel();
            this.DateBx = new DevExpress.XtraReports.UI.XRLabel();
            this.XrShape1 = new DevExpress.XtraReports.UI.XRShape();
            ((System.ComponentModel.ISupportInitialize)(this.repTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.repTable});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 1000F;
            this.Detail.MultiColumn.ColumnCount = 2;
            this.Detail.MultiColumn.ColumnSpacing = 10F;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnCount;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 254F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // repTable
            // 
            this.repTable.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.repTable.Dpi = 254F;
            this.repTable.Font = new DevExpress.Drawing.DXFont("IRANSansX", 9.900001F);
            this.repTable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.repTable.Name = "repTable";
            this.repTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.XrTableRow1});
            this.repTable.SizeF = new System.Drawing.SizeF(995F, 60F);
            this.repTable.StylePriority.UseBorders = false;
            this.repTable.StylePriority.UseFont = false;
            this.repTable.StylePriority.UseTextAlignment = false;
            this.repTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrTableRow1
            // 
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.XrTableCell1,
            this.XrTableCell2});
            this.XrTableRow1.Dpi = 254F;
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.Weight = 1D;
            // 
            // XrTableCell1
            // 
            this.XrTableCell1.Dpi = 254F;
            this.XrTableCell1.Font = new DevExpress.Drawing.DXFont("vazirmatn", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.XrTableCell1.Name = "XrTableCell1";
            this.XrTableCell1.StylePriority.UseFont = false;
            this.XrTableCell1.Text = "نام مراجعه کننده";
            this.XrTableCell1.Weight = 0.60399781629082916D;
            // 
            // XrTableCell2
            // 
            this.XrTableCell2.Dpi = 254F;
            this.XrTableCell2.Font = new DevExpress.Drawing.DXFont("vazirmatn", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.XrTableCell2.Name = "XrTableCell2";
            this.XrTableCell2.StylePriority.UseFont = false;
            this.XrTableCell2.Text = "آزمایشات";
            this.XrTableCell2.Weight = 1.3954545937041434D;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 50F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 50F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.CopyrightLab,
            this.XrPageInfo1});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 65F;
            this.PageFooter.Name = "PageFooter";
            // 
            // CopyrightLab
            // 
            this.CopyrightLab.Dpi = 254F;
            this.CopyrightLab.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Italic);
            this.CopyrightLab.LocationFloat = new DevExpress.Utils.PointFloat(900.4919F, 15F);
            this.CopyrightLab.Name = "CopyrightLab";
            this.CopyrightLab.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.CopyrightLab.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.CopyrightLab.SizeF = new System.Drawing.SizeF(1099.508F, 50F);
            this.CopyrightLab.StylePriority.UseFont = false;
            this.CopyrightLab.Text = "Samco® HCM Laboratory Client";
            // 
            // XrPageInfo1
            // 
            this.XrPageInfo1.Dpi = 254F;
            this.XrPageInfo1.Font = new DevExpress.Drawing.DXFont("IRANSansX", 9.900001F);
            this.XrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.580002F);
            this.XrPageInfo1.Name = "XrPageInfo1";
            this.XrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.XrPageInfo1.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.XrPageInfo1.StylePriority.UseFont = false;
            this.XrPageInfo1.StylePriority.UseTextAlignment = false;
            this.XrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.XrPageInfo1.TextFormatString = "صفحه {0} از {1}";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.UniIconBx,
            this.XrLabel1,
            this.UniversityLab,
            this.ShabakehLab,
            this.WorkTypeBx,
            this.DateBx,
            this.XrShape1});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 340F;
            this.PageHeader.Name = "PageHeader";
            // 
            // UniIconBx
            // 
            this.UniIconBx.Dpi = 254F;
            this.UniIconBx.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("UniIconBx.ImageSource"));
            this.UniIconBx.LocationFloat = new DevExpress.Utils.PointFloat(1730F, 60.00004F);
            this.UniIconBx.Name = "UniIconBx";
            this.UniIconBx.SizeF = new System.Drawing.SizeF(220F, 220F);
            this.UniIconBx.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // XrLabel1
            // 
            this.XrLabel1.Dpi = 254F;
            this.XrLabel1.Font = new DevExpress.Drawing.DXFont("Vazirmatn", 10F);
            this.XrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(632.8413F, 0F);
            this.XrLabel1.Name = "XrLabel1";
            this.XrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.XrLabel1.SizeF = new System.Drawing.SizeF(390.7119F, 58.42F);
            this.XrLabel1.StylePriority.UseFont = false;
            this.XrLabel1.StylePriority.UseTextAlignment = false;
            this.XrLabel1.Text = "بسمه تعالی";
            this.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // UniversityLab
            // 
            this.UniversityLab.Dpi = 254F;
            this.UniversityLab.Font = new DevExpress.Drawing.DXFont("IRANSansX ExtraBold", 11F);
            this.UniversityLab.LocationFloat = new DevExpress.Utils.PointFloat(25F, 60.00004F);
            this.UniversityLab.Name = "UniversityLab";
            this.UniversityLab.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.UniversityLab.SizeF = new System.Drawing.SizeF(1620F, 58.42002F);
            this.UniversityLab.StylePriority.UseFont = false;
            this.UniversityLab.StylePriority.UseTextAlignment = false;
            this.UniversityLab.Text = "دانشگاه علوم پزشکی و خدمات بهداشتی و درمانی ";
            this.UniversityLab.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // ShabakehLab
            // 
            this.ShabakehLab.Dpi = 254F;
            this.ShabakehLab.Font = new DevExpress.Drawing.DXFont("IRANSansX ExtraBold", 11F);
            this.ShabakehLab.LocationFloat = new DevExpress.Utils.PointFloat(25F, 118.4201F);
            this.ShabakehLab.Name = "ShabakehLab";
            this.ShabakehLab.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.ShabakehLab.SizeF = new System.Drawing.SizeF(1620F, 58.41998F);
            this.ShabakehLab.StylePriority.UseFont = false;
            this.ShabakehLab.StylePriority.UseTextAlignment = false;
            this.ShabakehLab.Text = "شبکه بهداشت و درمان شهرستان {0} - آزمایشگاه مرکز بهداشت {1}";
            this.ShabakehLab.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // WorkTypeBx
            // 
            this.WorkTypeBx.Dpi = 254F;
            this.WorkTypeBx.Font = new DevExpress.Drawing.DXFont("vazirmatn", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.WorkTypeBx.LocationFloat = new DevExpress.Utils.PointFloat(180.1332F, 243.16F);
            this.WorkTypeBx.Name = "WorkTypeBx";
            this.WorkTypeBx.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.WorkTypeBx.SizeF = new System.Drawing.SizeF(1221.503F, 50F);
            this.WorkTypeBx.StylePriority.UseFont = false;
            this.WorkTypeBx.StylePriority.UseTextAlignment = false;
            this.WorkTypeBx.Text = "Biochemistry";
            this.WorkTypeBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // DateBx
            // 
            this.DateBx.Dpi = 254F;
            this.DateBx.Font = new DevExpress.Drawing.DXFont("vazirmatn", 10F);
            this.DateBx.LocationFloat = new DevExpress.Utils.PointFloat(180.1332F, 193.16F);
            this.DateBx.Name = "DateBx";
            this.DateBx.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.DateBx.SizeF = new System.Drawing.SizeF(1221.503F, 50F);
            this.DateBx.StylePriority.UseFont = false;
            this.DateBx.StylePriority.UseTextAlignment = false;
            this.DateBx.Text = "برگه کار آزمایشگاه از تاریخ {0} تا تاریخ {1}";
            this.DateBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrShape1
            // 
            this.XrShape1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.XrShape1.BorderWidth = 2F;
            this.XrShape1.Dpi = 254F;
            this.XrShape1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.XrShape1.Name = "XrShape1";
            shapeRectangle1.Fillet = 5;
            this.XrShape1.Shape = shapeRectangle1;
            this.XrShape1.SizeF = new System.Drawing.SizeF(2000.267F, 330F);
            this.XrShape1.StylePriority.UseBorders = false;
            this.XrShape1.StylePriority.UseBorderWidth = false;
            // 
            // LabWork
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.PageHeader});
            this.Dpi = 254F;
            this.Margins = new DevExpress.Drawing.DXMargins(50F, 50F, 50F, 50F);
            this.PageHeightF = 1480F;
            this.PageWidthF = 2100F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A5Rotated;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.RightToLeftLayout = DevExpress.XtraReports.UI.RightToLeftLayout.Yes;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.SnapGridSize = 25F;
            this.Version = "25.1";
            this.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart;
            ((System.ComponentModel.ISupportInitialize)(this.repTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable repTable;
        private DevExpress.XtraReports.UI.XRTableRow XrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell2;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRPageInfo XrPageInfo1;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel WorkTypeBx;
        private DevExpress.XtraReports.UI.XRLabel DateBx;
        private DevExpress.XtraReports.UI.XRShape XrShape1;
        private DevExpress.XtraReports.UI.XRPictureBox UniIconBx;
        private DevExpress.XtraReports.UI.XRLabel XrLabel1;
        private DevExpress.XtraReports.UI.XRLabel UniversityLab;
        private DevExpress.XtraReports.UI.XRLabel ShabakehLab;
        private DevExpress.XtraReports.UI.XRLabel CopyrightLab;
    }
}
