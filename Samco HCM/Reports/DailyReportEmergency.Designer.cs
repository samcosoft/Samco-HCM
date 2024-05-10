namespace Samco_HCM.Reports
{
    partial class DailyReportEmergency
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.repTable = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand1 = new DevExpress.XtraReports.UI.SubBand();
            this.XrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.priceBx = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.XrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.titleLab = new DevExpress.XtraReports.UI.XRLabel();
            this.toDateLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.fromDateLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.nameLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.HeaderLab = new DevExpress.XtraReports.UI.XRLabel();
            this.UniIconBx = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.XrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.XrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.XrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.repTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.repTable});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 50F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.SubBands.AddRange(new DevExpress.XtraReports.UI.SubBand[] {
            this.SubBand1});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // repTable
            // 
            this.repTable.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.repTable.BorderWidth = 2F;
            this.repTable.Dpi = 254F;
            this.repTable.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0)))});
            this.repTable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.repTable.Name = "repTable";
            this.repTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.XrTableRow1});
            this.repTable.SizeF = new System.Drawing.SizeF(700F, 50F);
            this.repTable.StylePriority.UseBorders = false;
            this.repTable.StylePriority.UseBorderWidth = false;
            this.repTable.StylePriority.UseFont = false;
            this.repTable.StylePriority.UseTextAlignment = false;
            this.repTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrTableRow1
            // 
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.XrTableCell1,
            this.XrTableCell2,
            this.xrTableCell4});
            this.XrTableRow1.Dpi = 254F;
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.Weight = 1D;
            // 
            // XrTableCell1
            // 
            this.XrTableCell1.Dpi = 254F;
            this.XrTableCell1.Name = "XrTableCell1";
            this.XrTableCell1.Text = "نام خدمت";
            this.XrTableCell1.Weight = 1.526885649097989D;
            // 
            // XrTableCell2
            // 
            this.XrTableCell2.Dpi = 254F;
            this.XrTableCell2.Name = "XrTableCell2";
            this.XrTableCell2.Text = "تعداد";
            this.XrTableCell2.Weight = 1.5268857620098311D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "خدمت دهنده";
            this.xrTableCell4.Weight = 1.5268857620098311D;
            // 
            // SubBand1
            // 
            this.SubBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrTable1});
            this.SubBand1.Dpi = 254F;
            this.SubBand1.HeightF = 60F;
            this.SubBand1.Name = "SubBand1";
            // 
            // XrTable1
            // 
            this.XrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.XrTable1.BorderWidth = 2F;
            this.XrTable1.Dpi = 254F;
            this.XrTable1.Font = new DevExpress.Drawing.DXFont("Vazir", 12F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0)))});
            this.XrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.XrTable1.Name = "XrTable1";
            this.XrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.XrTableRow2});
            this.XrTable1.SizeF = new System.Drawing.SizeF(700F, 60F);
            this.XrTable1.StylePriority.UseBorders = false;
            this.XrTable1.StylePriority.UseBorderWidth = false;
            this.XrTable1.StylePriority.UseFont = false;
            this.XrTable1.StylePriority.UseTextAlignment = false;
            this.XrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrTableRow2
            // 
            this.XrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.XrTableCell3,
            this.priceBx});
            this.XrTableRow2.Dpi = 254F;
            this.XrTableRow2.Name = "XrTableRow2";
            this.XrTableRow2.Weight = 1D;
            // 
            // XrTableCell3
            // 
            this.XrTableCell3.Dpi = 254F;
            this.XrTableCell3.Name = "XrTableCell3";
            this.XrTableCell3.Text = "مبلغ:";
            this.XrTableCell3.Weight = 1.526885649097989D;
            // 
            // priceBx
            // 
            this.priceBx.Dpi = 254F;
            this.priceBx.Name = "priceBx";
            this.priceBx.Text = "-----";
            this.priceBx.Weight = 1.5268857620098311D;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 4F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrLine3,
            this.titleLab,
            this.toDateLabel,
            this.XrLine1,
            this.fromDateLabel,
            this.XrLabel2,
            this.nameLabel,
            this.XrLabel11,
            this.HeaderLab,
            this.UniIconBx});
            this.ReportHeader.Dpi = 254F;
            this.ReportHeader.HeightF = 405F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // XrLine3
            // 
            this.XrLine3.Dpi = 254F;
            this.XrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 220F);
            this.XrLine3.Name = "XrLine3";
            this.XrLine3.SizeF = new System.Drawing.SizeF(700F, 15F);
            // 
            // titleLab
            // 
            this.titleLab.Dpi = 254F;
            this.titleLab.Font = new DevExpress.Drawing.DXFont("Vazir", 12F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0)))});
            this.titleLab.LocationFloat = new DevExpress.Utils.PointFloat(30F, 240F);
            this.titleLab.Name = "titleLab";
            this.titleLab.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.titleLab.SizeF = new System.Drawing.SizeF(650F, 50F);
            this.titleLab.StylePriority.UseFont = false;
            this.titleLab.StylePriority.UseTextAlignment = false;
            this.titleLab.Text = "-----";
            this.titleLab.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // toDateLabel
            // 
            this.toDateLabel.Dpi = 254F;
            this.toDateLabel.Font = new DevExpress.Drawing.DXFont("IRANSansX", 10F);
            this.toDateLabel.LocationFloat = new DevExpress.Utils.PointFloat(395F, 340F);
            this.toDateLabel.Name = "toDateLabel";
            this.toDateLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.toDateLabel.SizeF = new System.Drawing.SizeF(285F, 50F);
            this.toDateLabel.StylePriority.UseFont = false;
            this.toDateLabel.StylePriority.UseTextAlignment = false;
            this.toDateLabel.Text = "-----";
            this.toDateLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrLine1
            // 
            this.XrLine1.Dpi = 254F;
            this.XrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 390F);
            this.XrLine1.Name = "XrLine1";
            this.XrLine1.SizeF = new System.Drawing.SizeF(700F, 15F);
            // 
            // fromDateLabel
            // 
            this.fromDateLabel.Dpi = 254F;
            this.fromDateLabel.Font = new DevExpress.Drawing.DXFont("IRANSansX", 10F);
            this.fromDateLabel.LocationFloat = new DevExpress.Utils.PointFloat(110F, 340F);
            this.fromDateLabel.Name = "fromDateLabel";
            this.fromDateLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.fromDateLabel.SizeF = new System.Drawing.SizeF(285F, 50F);
            this.fromDateLabel.StylePriority.UseFont = false;
            this.fromDateLabel.StylePriority.UseTextAlignment = false;
            this.fromDateLabel.Text = "-----";
            this.fromDateLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrLabel2
            // 
            this.XrLabel2.Dpi = 254F;
            this.XrLabel2.Font = new DevExpress.Drawing.DXFont("IRANSansX", 8F);
            this.XrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(30F, 340F);
            this.XrLabel2.Name = "XrLabel2";
            this.XrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel2.SizeF = new System.Drawing.SizeF(80F, 50F);
            this.XrLabel2.StylePriority.UseFont = false;
            this.XrLabel2.StylePriority.UseTextAlignment = false;
            this.XrLabel2.Text = "تاریخ:";
            this.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // nameLabel
            // 
            this.nameLabel.Dpi = 254F;
            this.nameLabel.Font = new DevExpress.Drawing.DXFont("IRANSansX", 10F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.nameLabel.LocationFloat = new DevExpress.Utils.PointFloat(250F, 290F);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.nameLabel.SizeF = new System.Drawing.SizeF(430F, 50F);
            this.nameLabel.StylePriority.UseFont = false;
            this.nameLabel.StylePriority.UseTextAlignment = false;
            this.nameLabel.Text = "-----";
            this.nameLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrLabel11
            // 
            this.XrLabel11.Dpi = 254F;
            this.XrLabel11.Font = new DevExpress.Drawing.DXFont("IRANSansX", 8F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.XrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(30F, 290F);
            this.XrLabel11.Name = "XrLabel11";
            this.XrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel11.SizeF = new System.Drawing.SizeF(220F, 50F);
            this.XrLabel11.StylePriority.UseFont = false;
            this.XrLabel11.StylePriority.UseTextAlignment = false;
            this.XrLabel11.Text = "نام پذیرش دهنده:";
            this.XrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // HeaderLab
            // 
            this.HeaderLab.Dpi = 254F;
            this.HeaderLab.Font = new DevExpress.Drawing.DXFont("Vazir", 8.249999F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0)))});
            this.HeaderLab.LocationFloat = new DevExpress.Utils.PointFloat(30F, 0F);
            this.HeaderLab.Multiline = true;
            this.HeaderLab.Name = "HeaderLab";
            this.HeaderLab.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.HeaderLab.SizeF = new System.Drawing.SizeF(475F, 220F);
            this.HeaderLab.StylePriority.UseFont = false;
            this.HeaderLab.StylePriority.UseTextAlignment = false;
            this.HeaderLab.Text = "دانشگاه علوم پزشکی و\r\nخدمات بهداشتی و درمانی {0} \r\nشبکه بهداشت و درمان شهرستان {1" +
    "} \r\nمرکز بهداشت {2}";
            this.HeaderLab.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // UniIconBx
            // 
            this.UniIconBx.Dpi = 254F;
            this.UniIconBx.LocationFloat = new DevExpress.Utils.PointFloat(520F, 0F);
            this.UniIconBx.Name = "UniIconBx";
            this.UniIconBx.SizeF = new System.Drawing.SizeF(160F, 220F);
            this.UniIconBx.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrLine2,
            this.XrPageInfo1,
            this.XrLabel5});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 65F;
            this.PageFooter.Name = "PageFooter";
            // 
            // XrLine2
            // 
            this.XrLine2.Dpi = 254F;
            this.XrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.XrLine2.Name = "XrLine2";
            this.XrLine2.SizeF = new System.Drawing.SizeF(700F, 15F);
            // 
            // XrPageInfo1
            // 
            this.XrPageInfo1.Dpi = 254F;
            this.XrPageInfo1.Font = new DevExpress.Drawing.DXFont("IRANSansX", 8.1F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.XrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 14.99986F);
            this.XrPageInfo1.Name = "XrPageInfo1";
            this.XrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.XrPageInfo1.SizeF = new System.Drawing.SizeF(339.2001F, 50F);
            this.XrPageInfo1.StylePriority.UseFont = false;
            this.XrPageInfo1.StylePriority.UseTextAlignment = false;
            this.XrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // XrLabel5
            // 
            this.XrLabel5.Dpi = 254F;
            this.XrLabel5.Font = new DevExpress.Drawing.DXFont("Microsoft Sans Serif", 8.1F);
            this.XrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(350.0001F, 14.99986F);
            this.XrLabel5.Name = "XrLabel5";
            this.XrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel5.SizeF = new System.Drawing.SizeF(349.9999F, 50F);
            this.XrLabel5.StylePriority.UseFont = false;
            this.XrLabel5.StylePriority.UseTextAlignment = false;
            this.XrLabel5.Text = "Samco® Software Group";
            this.XrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // DailyReportEmergency
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.Dpi = 254F;
            this.Margins = new DevExpress.Drawing.DXMargins(0F, 0F, 0F, 4F);
            this.PageHeight = 10;
            this.PageWidth = 700;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.RequestParameters = false;
            this.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.RightToLeftLayout = DevExpress.XtraReports.UI.RightToLeftLayout.Yes;
            this.RollPaper = true;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.SnapGridSize = 25F;
            this.Version = "23.2";
            ((System.ComponentModel.ISupportInitialize)(this.repTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable repTable;
        private DevExpress.XtraReports.UI.XRTableRow XrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.SubBand SubBand1;
        private DevExpress.XtraReports.UI.XRTable XrTable1;
        private DevExpress.XtraReports.UI.XRTableRow XrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell priceBx;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLine XrLine3;
        private DevExpress.XtraReports.UI.XRLabel titleLab;
        private DevExpress.XtraReports.UI.XRLabel toDateLabel;
        private DevExpress.XtraReports.UI.XRLine XrLine1;
        private DevExpress.XtraReports.UI.XRLabel fromDateLabel;
        private DevExpress.XtraReports.UI.XRLabel XrLabel2;
        private DevExpress.XtraReports.UI.XRLabel nameLabel;
        private DevExpress.XtraReports.UI.XRLabel XrLabel11;
        private DevExpress.XtraReports.UI.XRLabel HeaderLab;
        private DevExpress.XtraReports.UI.XRPictureBox UniIconBx;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRLine XrLine2;
        private DevExpress.XtraReports.UI.XRPageInfo XrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel XrLabel5;
    }
}
