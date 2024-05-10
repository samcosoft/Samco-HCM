namespace Samco_HCM.Reports
{
    partial class VisitReceipt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisitReceipt));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.XrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.ServiceTable = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.NameBx = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.InsBx = new DevExpress.XtraReports.UI.XRLabel();
            this.MelliBx = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageFooterBand1 = new DevExpress.XtraReports.UI.PageFooterBand();
            this.XrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.XrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.UniIconBx = new DevExpress.XtraReports.UI.XRPictureBox();
            this.HeaderLab = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.Nobat = new DevExpress.XtraReports.Parameters.Parameter();
            this.SetPrice = new DevExpress.XtraReports.Parameters.Parameter();
            this.PatName = new DevExpress.XtraReports.Parameters.Parameter();
            this.InsName = new DevExpress.XtraReports.Parameters.Parameter();
            this.MelliCode = new DevExpress.XtraReports.Parameters.Parameter();
            this.Doctor = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.ServiceTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrLabel4,
            this.XrLabel6,
            this.ServiceTable,
            this.XrLabel3,
            this.XrLabel8,
            this.NameBx,
            this.XrLabel16,
            this.XrLabel12,
            this.XrLabel11,
            this.XrLabel1,
            this.XrLabel2,
            this.InsBx,
            this.MelliBx,
            this.XrLine1});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 427F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // XrLabel4
            // 
            this.XrLabel4.Dpi = 254F;
            this.XrLabel4.Font = new DevExpress.Drawing.DXFont("IRANSansX", 11F);
            this.XrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(35.12811F, 75.00001F);
            this.XrLabel4.Name = "XrLabel4";
            this.XrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel4.SizeF = new System.Drawing.SizeF(258.8499F, 59.20466F);
            this.XrLabel4.StylePriority.UseFont = false;
            this.XrLabel4.StylePriority.UseTextAlignment = false;
            this.XrLabel4.Text = "خدمت دهنده:";
            this.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrLabel6
            // 
            this.XrLabel6.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
            this.XrLabel6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.XrLabel6.BorderWidth = 2F;
            this.XrLabel6.Dpi = 254F;
            this.XrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?Doctor")});
            this.XrLabel6.Font = new DevExpress.Drawing.DXFont("IRANSansX", 11F);
            this.XrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(295.1282F, 75.00001F);
            this.XrLabel6.Name = "XrLabel6";
            this.XrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel6.SizeF = new System.Drawing.SizeF(381.8034F, 59.20466F);
            this.XrLabel6.StylePriority.UseBorderDashStyle = false;
            this.XrLabel6.StylePriority.UseBorders = false;
            this.XrLabel6.StylePriority.UseBorderWidth = false;
            this.XrLabel6.StylePriority.UseFont = false;
            this.XrLabel6.StylePriority.UseTextAlignment = false;
            this.XrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // ServiceTable
            // 
            this.ServiceTable.Dpi = 254F;
            this.ServiceTable.Font = new DevExpress.Drawing.DXFont("Vazir", 10.2F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0)))});
            this.ServiceTable.LocationFloat = new DevExpress.Utils.PointFloat(35.00002F, 338.5F);
            this.ServiceTable.Name = "ServiceTable";
            this.ServiceTable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.ServiceTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.XrTableRow1});
            this.ServiceTable.SizeF = new System.Drawing.SizeF(640.0926F, 63.50003F);
            this.ServiceTable.StylePriority.UseFont = false;
            this.ServiceTable.StylePriority.UseTextAlignment = false;
            this.ServiceTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrTableRow1
            // 
            this.XrTableRow1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.XrTableCell1,
            this.XrTableCell2});
            this.XrTableRow1.Dpi = 254F;
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.StylePriority.UseBorders = false;
            this.XrTableRow1.Weight = 1D;
            // 
            // XrTableCell1
            // 
            this.XrTableCell1.Dpi = 254F;
            this.XrTableCell1.Multiline = true;
            this.XrTableCell1.Name = "XrTableCell1";
            this.XrTableCell1.StylePriority.UseTextAlignment = false;
            this.XrTableCell1.Text = "نام خدمت";
            this.XrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.XrTableCell1.Weight = 1.4942216172987517D;
            // 
            // XrTableCell2
            // 
            this.XrTableCell2.Dpi = 254F;
            this.XrTableCell2.Multiline = true;
            this.XrTableCell2.Name = "XrTableCell2";
            this.XrTableCell2.StylePriority.UseTextAlignment = false;
            this.XrTableCell2.Text = "تعداد";
            this.XrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.XrTableCell2.Weight = 0.50577838270124831D;
            // 
            // XrLabel3
            // 
            this.XrLabel3.Dpi = 254F;
            this.XrLabel3.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.XrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(423.0429F, 267.1422F);
            this.XrLabel3.Name = "XrLabel3";
            this.XrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel3.SizeF = new System.Drawing.SizeF(90F, 55F);
            this.XrLabel3.StylePriority.UseFont = false;
            this.XrLabel3.StylePriority.UseTextAlignment = false;
            this.XrLabel3.Text = "مبلغ:";
            this.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // XrLabel8
            // 
            this.XrLabel8.Dpi = 254F;
            this.XrLabel8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?SetPrice")});
            this.XrLabel8.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.XrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(513.0429F, 267.1422F);
            this.XrLabel8.Name = "XrLabel8";
            this.XrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel8.SizeF = new System.Drawing.SizeF(161.9571F, 55F);
            this.XrLabel8.StylePriority.UseFont = false;
            this.XrLabel8.StylePriority.UseTextAlignment = false;
            this.XrLabel8.Text = "XrLabel8";
            this.XrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // NameBx
            // 
            this.NameBx.Dpi = 254F;
            this.NameBx.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?PatName")});
            this.NameBx.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.NameBx.LocationFloat = new DevExpress.Utils.PointFloat(205.1715F, 142.1422F);
            this.NameBx.Name = "NameBx";
            this.NameBx.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.NameBx.SizeF = new System.Drawing.SizeF(469.7439F, 55F);
            this.NameBx.StylePriority.UseFont = false;
            this.NameBx.StylePriority.UseTextAlignment = false;
            this.NameBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // XrLabel16
            // 
            this.XrLabel16.Dpi = 254F;
            this.XrLabel16.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.XrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(34.99998F, 142.1422F);
            this.XrLabel16.Multiline = true;
            this.XrLabel16.Name = "XrLabel16";
            this.XrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel16.SizeF = new System.Drawing.SizeF(170F, 55F);
            this.XrLabel16.StylePriority.UseFont = false;
            this.XrLabel16.StylePriority.UseTextAlignment = false;
            this.XrLabel16.Text = "نام فرد:";
            this.XrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // XrLabel12
            // 
            this.XrLabel12.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
            this.XrLabel12.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.XrLabel12.BorderWidth = 2F;
            this.XrLabel12.Dpi = 254F;
            this.XrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?Nobat")});
            this.XrLabel12.Font = new DevExpress.Drawing.DXFont("IRANSansX", 12F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.XrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(295F, 0F);
            this.XrLabel12.Name = "XrLabel12";
            this.XrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel12.SizeF = new System.Drawing.SizeF(381.8034F, 75.00001F);
            this.XrLabel12.StylePriority.UseBorderDashStyle = false;
            this.XrLabel12.StylePriority.UseBorders = false;
            this.XrLabel12.StylePriority.UseBorderWidth = false;
            this.XrLabel12.StylePriority.UseFont = false;
            this.XrLabel12.StylePriority.UseTextAlignment = false;
            this.XrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrLabel11
            // 
            this.XrLabel11.Dpi = 254F;
            this.XrLabel11.Font = new DevExpress.Drawing.DXFont("IRANSansX", 12F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0)))});
            this.XrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(35F, 0F);
            this.XrLabel11.Name = "XrLabel11";
            this.XrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel11.SizeF = new System.Drawing.SizeF(258.8499F, 75F);
            this.XrLabel11.StylePriority.UseFont = false;
            this.XrLabel11.StylePriority.UseTextAlignment = false;
            this.XrLabel11.Text = "نوبت شما:";
            this.XrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // XrLabel1
            // 
            this.XrLabel1.Dpi = 254F;
            this.XrLabel1.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.XrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(35.1281F, 267.1421F);
            this.XrLabel1.Name = "XrLabel1";
            this.XrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel1.SizeF = new System.Drawing.SizeF(170F, 55F);
            this.XrLabel1.StylePriority.UseFont = false;
            this.XrLabel1.StylePriority.UseTextAlignment = false;
            this.XrLabel1.Text = "نام بیمه:";
            this.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // XrLabel2
            // 
            this.XrLabel2.Dpi = 254F;
            this.XrLabel2.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.XrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(34.99999F, 197.1422F);
            this.XrLabel2.Name = "XrLabel2";
            this.XrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.XrLabel2.SizeF = new System.Drawing.SizeF(170F, 69.99989F);
            this.XrLabel2.StylePriority.UseFont = false;
            this.XrLabel2.StylePriority.UseTextAlignment = false;
            this.XrLabel2.Text = "کد ملی:";
            this.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // InsBx
            // 
            this.InsBx.Dpi = 254F;
            this.InsBx.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?InsName")});
            this.InsBx.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.InsBx.LocationFloat = new DevExpress.Utils.PointFloat(205.1715F, 267.1421F);
            this.InsBx.Name = "InsBx";
            this.InsBx.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.InsBx.SizeF = new System.Drawing.SizeF(217.8714F, 55F);
            this.InsBx.StylePriority.UseFont = false;
            this.InsBx.StylePriority.UseTextAlignment = false;
            this.InsBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // MelliBx
            // 
            this.MelliBx.Dpi = 254F;
            this.MelliBx.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?MelliCode")});
            this.MelliBx.Font = new DevExpress.Drawing.DXFont("Vazir", 9.900001F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(178)))});
            this.MelliBx.LocationFloat = new DevExpress.Utils.PointFloat(205.128F, 197.1422F);
            this.MelliBx.Name = "MelliBx";
            this.MelliBx.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.MelliBx.SizeF = new System.Drawing.SizeF(469.872F, 69.99989F);
            this.MelliBx.StylePriority.UseFont = false;
            this.MelliBx.StylePriority.UseTextAlignment = false;
            this.MelliBx.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // XrLine1
            // 
            this.XrLine1.Dpi = 254F;
            this.XrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 134.2047F);
            this.XrLine1.Name = "XrLine1";
            this.XrLine1.SizeF = new System.Drawing.SizeF(700F, 7.9375F);
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
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageFooterBand1
            // 
            this.PageFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrLabel5,
            this.XrPageInfo1,
            this.XrLine2});
            this.PageFooterBand1.Dpi = 254F;
            this.PageFooterBand1.HeightF = 65F;
            this.PageFooterBand1.Name = "PageFooterBand1";
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
            // XrLine2
            // 
            this.XrLine2.Dpi = 254F;
            this.XrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.XrLine2.Name = "XrLine2";
            this.XrLine2.SizeF = new System.Drawing.SizeF(700F, 7.9375F);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.UniIconBx,
            this.HeaderLab,
            this.XrLine3});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 230F;
            this.PageHeader.Name = "PageHeader";
            // 
            // UniIconBx
            // 
            this.UniIconBx.Dpi = 254F;
            this.UniIconBx.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("UniIconBx.ImageSource"));
            this.UniIconBx.LocationFloat = new DevExpress.Utils.PointFloat(520.0001F, 0F);
            this.UniIconBx.Name = "UniIconBx";
            this.UniIconBx.SizeF = new System.Drawing.SizeF(160F, 220F);
            this.UniIconBx.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
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
            // XrLine3
            // 
            this.XrLine3.Dpi = 254F;
            this.XrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 220F);
            this.XrLine3.Name = "XrLine3";
            this.XrLine3.SizeF = new System.Drawing.SizeF(700F, 10F);
            // 
            // Nobat
            // 
            this.Nobat.Description = "Show person position.";
            this.Nobat.Name = "Nobat";
            this.Nobat.Type = typeof(int);
            this.Nobat.ValueInfo = "1";
            this.Nobat.Visible = false;
            // 
            // SetPrice
            // 
            this.SetPrice.Description = "Parameter1";
            this.SetPrice.Name = "SetPrice";
            this.SetPrice.Type = typeof(int);
            this.SetPrice.ValueInfo = "0";
            // 
            // PatName
            // 
            this.PatName.AllowNull = true;
            this.PatName.Description = "Patient Name";
            this.PatName.Name = "PatName";
            // 
            // InsName
            // 
            this.InsName.AllowNull = true;
            this.InsName.Description = "Insurance Name";
            this.InsName.Name = "InsName";
            // 
            // MelliCode
            // 
            this.MelliCode.AllowNull = true;
            this.MelliCode.Name = "MelliCode";
            // 
            // Doctor
            // 
            this.Doctor.Description = "Parameter1";
            this.Doctor.Name = "Doctor";
            this.Doctor.Type = typeof(int);
            this.Doctor.ValueInfo = "0";
            // 
            // VisitReceipt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooterBand1,
            this.PageHeader});
            this.Dpi = 254F;
            this.Margins = new DevExpress.Drawing.DXMargins(0F, 0F, 0F, 0F);
            this.PageHeight = 1000;
            this.PageWidth = 700;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Nobat,
            this.SetPrice,
            this.PatName,
            this.InsName,
            this.MelliCode,
            this.Doctor});
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.RequestParameters = false;
            this.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.RightToLeftLayout = DevExpress.XtraReports.UI.RightToLeftLayout.Yes;
            this.RollPaper = true;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.SnapGridSize = 25F;
            this.Version = "23.2";
            ((System.ComponentModel.ISupportInitialize)(this.ServiceTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRLabel XrLabel4;
        private DevExpress.XtraReports.UI.XRLabel XrLabel6;
        private DevExpress.XtraReports.UI.XRTable ServiceTable;
        private DevExpress.XtraReports.UI.XRTableRow XrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell XrTableCell2;
        private DevExpress.XtraReports.UI.XRLabel XrLabel3;
        private DevExpress.XtraReports.UI.XRLabel XrLabel8;
        private DevExpress.XtraReports.UI.XRLabel NameBx;
        private DevExpress.XtraReports.UI.XRLabel XrLabel16;
        private DevExpress.XtraReports.UI.XRLabel XrLabel12;
        private DevExpress.XtraReports.UI.XRLabel XrLabel11;
        private DevExpress.XtraReports.UI.XRLabel XrLabel1;
        private DevExpress.XtraReports.UI.XRLabel XrLabel2;
        private DevExpress.XtraReports.UI.XRLabel InsBx;
        private DevExpress.XtraReports.UI.XRLabel MelliBx;
        private DevExpress.XtraReports.UI.XRLine XrLine1;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooterBand1;
        private DevExpress.XtraReports.UI.XRLabel XrLabel5;
        private DevExpress.XtraReports.UI.XRPageInfo XrPageInfo1;
        private DevExpress.XtraReports.UI.XRLine XrLine2;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRPictureBox UniIconBx;
        private DevExpress.XtraReports.UI.XRLabel HeaderLab;
        private DevExpress.XtraReports.UI.XRLine XrLine3;
        private DevExpress.XtraReports.Parameters.Parameter Nobat;
        private DevExpress.XtraReports.Parameters.Parameter SetPrice;
        private DevExpress.XtraReports.Parameters.Parameter PatName;
        private DevExpress.XtraReports.Parameters.Parameter InsName;
        private DevExpress.XtraReports.Parameters.Parameter MelliCode;
        private DevExpress.XtraReports.Parameters.Parameter Doctor;
    }
}
