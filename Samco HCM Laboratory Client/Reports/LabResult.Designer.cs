namespace Samco_HCM_Laboratory_Client.Reports
{
    partial class LabResult
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabResult));
            DevExpress.XtraPrinting.Shape.ShapeRectangle shapeRectangle1 = new DevExpress.XtraPrinting.Shape.ShapeRectangle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.ResultTable = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.UniversityLab = new DevExpress.XtraReports.UI.XRLabel();
            this.ShabakehLab = new DevExpress.XtraReports.UI.XRLabel();
            this.UniIconBx = new DevExpress.XtraReports.UI.XRPictureBox();
            this.XrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.dateBx = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ageBx = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.nameBx = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.VisitIDBx = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrShape1 = new DevExpress.XtraReports.UI.XRShape();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.XrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.XrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.CopyrightLab = new DevExpress.XtraReports.UI.XRLabel();
            this.dataModel = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.Age = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.ResultTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.ResultTable});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 1000F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 254F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ResultTable
            // 
            this.ResultTable.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.ResultTable.Dpi = 254F;
            this.ResultTable.Font = new DevExpress.Drawing.DXFont("IRANSansX", 9.75F);
            this.ResultTable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.ResultTable.Name = "ResultTable";
            this.ResultTable.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.ResultTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.XrTableRow2});
            this.ResultTable.SizeF = new System.Drawing.SizeF(331.0566F, 63.49998F);
            this.ResultTable.StylePriority.UseBorders = false;
            this.ResultTable.StylePriority.UseFont = false;
            this.ResultTable.BeforePrint += new DevExpress.XtraReports.UI.BeforePrintEventHandler(this.ResultTable_BeforePrint);
            // 
            // XrTableRow2
            // 
            this.XrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.XrTableCell2});
            this.XrTableRow2.Dpi = 254F;
            this.XrTableRow2.Name = "XrTableRow2";
            this.XrTableRow2.Weight = 1D;
            // 
            // XrTableCell2
            // 
            this.XrTableCell2.Dpi = 254F;
            this.XrTableCell2.Name = "XrTableCell2";
            this.XrTableCell2.Text = "XrTableCell2";
            this.XrTableCell2.Weight = 1D;
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
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.UniversityLab,
            this.ShabakehLab,
            this.UniIconBx,
            this.XrTable1,
            this.XrLabel1,
            this.XrShape1});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 300F;
            this.PageHeader.Name = "PageHeader";
            // 
            // UniversityLab
            // 
            this.UniversityLab.Dpi = 254F;
            this.UniversityLab.Font = new DevExpress.Drawing.DXFont("IRANSansX ExtraBold", 11F);
            this.UniversityLab.LocationFloat = new DevExpress.Utils.PointFloat(310F, 60.00004F);
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
            this.ShabakehLab.LocationFloat = new DevExpress.Utils.PointFloat(310F, 118.4201F);
            this.ShabakehLab.Name = "ShabakehLab";
            this.ShabakehLab.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.ShabakehLab.SizeF = new System.Drawing.SizeF(1620F, 58.41998F);
            this.ShabakehLab.StylePriority.UseFont = false;
            this.ShabakehLab.StylePriority.UseTextAlignment = false;
            this.ShabakehLab.Text = "شبکه بهداشت و درمان شهرستان {0} - آزمایشگاه مرکز بهداشت {1}";
            this.ShabakehLab.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // UniIconBx
            // 
            this.UniIconBx.Dpi = 254F;
            this.UniIconBx.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("UniIconBx.ImageSource"));
            this.UniIconBx.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 34F);
            this.UniIconBx.Name = "UniIconBx";
            this.UniIconBx.SizeF = new System.Drawing.SizeF(220F, 220F);
            this.UniIconBx.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // XrTable1
            // 
            this.XrTable1.Dpi = 254F;
            this.XrTable1.Font = new DevExpress.Drawing.DXFont("Vazirmatn", 9.900001F);
            this.XrTable1.LocationFloat = new DevExpress.Utils.PointFloat(310F, 190F);
            this.XrTable1.Name = "XrTable1";
            this.XrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.XrTableRow1});
            this.XrTable1.SizeF = new System.Drawing.SizeF(1620F, 64F);
            this.XrTable1.StylePriority.UseFont = false;
            // 
            // XrTableRow1
            // 
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.dateBx,
            this.XrTableCell10,
            this.ageBx,
            this.XrTableCell12,
            this.nameBx,
            this.XrTableCell11,
            this.VisitIDBx,
            this.XrTableCell1});
            this.XrTableRow1.Dpi = 254F;
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.Weight = 1D;
            // 
            // dateBx
            // 
            this.dateBx.Dpi = 254F;
            this.dateBx.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[visitDate]")});
            this.dateBx.Font = new DevExpress.Drawing.DXFont("B Yekan", 9.900001F);
            this.dateBx.Name = "dateBx";
            this.dateBx.StylePriority.UseFont = false;
            this.dateBx.StylePriority.UseTextAlignment = false;
            this.dateBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.dateBx.TextFormatString = "{0:yyyy\'/\'MM\'/\'dd}";
            this.dateBx.Weight = 1.0396319919822203D;
            // 
            // XrTableCell10
            // 
            this.XrTableCell10.Dpi = 254F;
            this.XrTableCell10.Name = "XrTableCell10";
            this.XrTableCell10.StylePriority.UseTextAlignment = false;
            this.XrTableCell10.Text = "تاریخ آزمایش:";
            this.XrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.XrTableCell10.Weight = 0.98346617831848238D;
            // 
            // ageBx
            // 
            this.ageBx.Dpi = 254F;
            this.ageBx.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Age]")});
            this.ageBx.Font = new DevExpress.Drawing.DXFont("B Yekan", 9.900001F);
            this.ageBx.Name = "ageBx";
            this.ageBx.StylePriority.UseFont = false;
            this.ageBx.StylePriority.UseTextAlignment = false;
            this.ageBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.ageBx.Weight = 0.69308796253032412D;
            // 
            // XrTableCell12
            // 
            this.XrTableCell12.Dpi = 254F;
            this.XrTableCell12.Name = "XrTableCell12";
            this.XrTableCell12.StylePriority.UseTextAlignment = false;
            this.XrTableCell12.Text = "سن:";
            this.XrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.XrTableCell12.Weight = 0.33643836851043885D;
            // 
            // nameBx
            // 
            this.nameBx.Dpi = 254F;
            this.nameBx.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Patient].[Name]")});
            this.nameBx.Name = "nameBx";
            this.nameBx.StylePriority.UseTextAlignment = false;
            this.nameBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.nameBx.Weight = 1.7558229295752947D;
            // 
            // XrTableCell11
            // 
            this.XrTableCell11.Dpi = 254F;
            this.XrTableCell11.Name = "XrTableCell11";
            this.XrTableCell11.StylePriority.UseTextAlignment = false;
            this.XrTableCell11.Text = "نام و نام خانوادگی:";
            this.XrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.XrTableCell11.Weight = 1.3005364712725593D;
            // 
            // VisitIDBx
            // 
            this.VisitIDBx.Dpi = 254F;
            this.VisitIDBx.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Patient].[Oid]")});
            this.VisitIDBx.Font = new DevExpress.Drawing.DXFont("B Yekan", 9.900001F);
            this.VisitIDBx.Name = "VisitIDBx";
            this.VisitIDBx.StylePriority.UseFont = false;
            this.VisitIDBx.StylePriority.UseTextAlignment = false;
            this.VisitIDBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.VisitIDBx.Weight = 0.924117306165654D;
            // 
            // XrTableCell1
            // 
            this.XrTableCell1.Dpi = 254F;
            this.XrTableCell1.Name = "XrTableCell1";
            this.XrTableCell1.StylePriority.UseTextAlignment = false;
            this.XrTableCell1.Text = "کد پذیرش:";
            this.XrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.XrTableCell1.Weight = 0.79528517912121688D;
            // 
            // XrLabel1
            // 
            this.XrLabel1.Dpi = 254F;
            this.XrLabel1.Font = new DevExpress.Drawing.DXFont("Vazirmatn", 10F);
            this.XrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(917.8413F, 0F);
            this.XrLabel1.Name = "XrLabel1";
            this.XrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.XrLabel1.SizeF = new System.Drawing.SizeF(390.7119F, 58.42F);
            this.XrLabel1.StylePriority.UseFont = false;
            this.XrLabel1.StylePriority.UseTextAlignment = false;
            this.XrLabel1.Text = "بسمه تعالی";
            this.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            this.XrShape1.SizeF = new System.Drawing.SizeF(2000F, 280F);
            this.XrShape1.StylePriority.UseBorders = false;
            this.XrShape1.StylePriority.UseBorderWidth = false;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrLine2,
            this.XrPageInfo1,
            this.CopyrightLab});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 70.00004F;
            this.PageFooter.Name = "PageFooter";
            // 
            // XrLine2
            // 
            this.XrLine2.Dpi = 254F;
            this.XrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.XrLine2.Name = "XrLine2";
            this.XrLine2.SizeF = new System.Drawing.SizeF(2000.267F, 7.9375F);
            // 
            // XrPageInfo1
            // 
            this.XrPageInfo1.Dpi = 254F;
            this.XrPageInfo1.Font = new DevExpress.Drawing.DXFont("IRANSansX", 10F);
            this.XrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(1746F, 11.58004F);
            this.XrPageInfo1.Name = "XrPageInfo1";
            this.XrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.XrPageInfo1.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.XrPageInfo1.StylePriority.UseFont = false;
            this.XrPageInfo1.StylePriority.UseTextAlignment = false;
            this.XrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.XrPageInfo1.TextFormatString = "صفحه {0} از {1}";
            // 
            // CopyrightLab
            // 
            this.CopyrightLab.Dpi = 254F;
            this.CopyrightLab.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Italic);
            this.CopyrightLab.LocationFloat = new DevExpress.Utils.PointFloat(0F, 19.99994F);
            this.CopyrightLab.Name = "CopyrightLab";
            this.CopyrightLab.Padding = new DevExpress.XtraPrinting.PaddingInfo(5F, 5F, 0F, 0F, 254F);
            this.CopyrightLab.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.CopyrightLab.SizeF = new System.Drawing.SizeF(1099.508F, 50F);
            this.CopyrightLab.StylePriority.UseFont = false;
            this.CopyrightLab.Text = "Samco® HCM Laboratory Client";
            // 
            // dataModel
            // 
            this.dataModel.DataSource = typeof(global::LabData.LabVisits);
            this.dataModel.Name = "dataModel";
            // 
            // Age
            // 
            this.Age.Expression = "DateDiffYear([Patient].[BirthDate], LocalDateTimeNow())";
            this.Age.Name = "Age";
            // 
            // LabResult
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.Age});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.dataModel});
            this.DataSource = this.dataModel;
            this.Dpi = 254F;
            this.Margins = new DevExpress.Drawing.DXMargins(50F, 50F, 50F, 50F);
            this.PageHeightF = 1480F;
            this.PageWidthF = 2100F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A5Rotated;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.SnapGridSize = 25F;
            this.Version = "25.1";
            this.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(this.LabResult_PrintProgress);
            ((System.ComponentModel.ISupportInitialize)(this.ResultTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable ResultTable;
        private DevExpress.XtraReports.UI.XRTableRow XrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell2;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable XrTable1;
        private DevExpress.XtraReports.UI.XRTableRow XrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell dateBx;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell ageBx;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell nameBx;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell VisitIDBx;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell1;
        private DevExpress.XtraReports.UI.XRLabel XrLabel1;
        private DevExpress.XtraReports.UI.XRShape XrShape1;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRPageInfo XrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel CopyrightLab;
        private DevExpress.XtraReports.UI.XRPictureBox UniIconBx;
        private DevExpress.XtraReports.UI.XRLabel UniversityLab;
        private DevExpress.XtraReports.UI.XRLabel ShabakehLab;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource dataModel;
        private DevExpress.XtraReports.UI.CalculatedField Age;
        private DevExpress.XtraReports.UI.XRLine XrLine2;
    }
}
