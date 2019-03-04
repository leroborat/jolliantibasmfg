namespace JolliantProd.Module.Reports
{
    partial class XtraReport1
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.PODate = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPivotGrid2 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.xrPivotGridField1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField2 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField3 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField5 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.pivotGridField5 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.GroupHeader3 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPivotGrid3 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.xrPivotGridField6 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField7 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField8 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField10 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.pivotGridField4 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.GroupHeader4 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPivotGrid1 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.fieldProductProductName = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldQuantity1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.pivotGridField1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.pivotGridField2 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.pivotGridField3 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.pivotGridField6 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.SubBand1 = new DevExpress.XtraReports.UI.SubBand();
            this.xrPivotGridField4 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField9 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.collectionDataSource2 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            this.collectionDataSource3 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            this.collectionDataSource1 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            this.collectionDataSource4 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4});
            this.TopMargin.Name = "TopMargin";
            // 
            // xrLabel5
            // 
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?PODate")});
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 66.99999F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(429.1667F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "xrLabel5";
            this.xrLabel5.TextFormatString = "Purchase Order Date: {0:MM/dd/yyyy}";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Italic);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 21.83333F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(287.5F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "This is a system generated report.";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 48.33333F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.HeightF = 0.8332316F;
            this.Detail.Name = "Detail";
            // 
            // PODate
            // 
            this.PODate.Description = "PO Date";
            this.PODate.Name = "PODate";
            this.PODate.Type = typeof(System.DateTime);
            this.PODate.ValueInfo = "2019-03-04";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrPivotGrid2});
            this.GroupHeader2.HeightF = 151.6666F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            this.GroupHeader2.SubBands.AddRange(new DevExpress.XtraReports.UI.SubBand[] {
            this.SubBand1});
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "PJJ";
            // 
            // xrPivotGrid2
            // 
            this.xrPivotGrid2.Appearance.Cell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.Appearance.CustomTotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.Appearance.FieldHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.Appearance.FieldValue.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.Appearance.FieldValueGrandTotal.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.Appearance.FieldValueTotal.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.Appearance.GrandTotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.Appearance.Lines.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.Appearance.TotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid2.DataSource = this.collectionDataSource2;
            this.xrPivotGrid2.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.xrPivotGridField1,
            this.xrPivotGridField2,
            this.xrPivotGridField3,
            this.xrPivotGridField4,
            this.xrPivotGridField5,
            this.pivotGridField5});
            this.xrPivotGrid2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 33F);
            this.xrPivotGrid2.Name = "xrPivotGrid2";
            this.xrPivotGrid2.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivotGrid2.OptionsPrint.PrintFilterHeaders = DevExpress.Utils.DefaultBoolean.False;
            this.xrPivotGrid2.OptionsView.ShowColumnHeaders = false;
            this.xrPivotGrid2.OptionsView.ShowDataHeaders = false;
            this.xrPivotGrid2.SizeF = new System.Drawing.SizeF(640F, 107F);
            // 
            // xrPivotGridField1
            // 
            this.xrPivotGridField1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField1.AreaIndex = 0;
            this.xrPivotGridField1.Caption = "Product";
            this.xrPivotGridField1.FieldName = "Product.ProductName";
            this.xrPivotGridField1.Name = "xrPivotGridField1";
            this.xrPivotGridField1.Width = 300;
            // 
            // xrPivotGridField2
            // 
            this.xrPivotGridField2.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField2.AreaIndex = 0;
            this.xrPivotGridField2.Caption = "Quantity";
            this.xrPivotGridField2.FieldName = "Quantity";
            this.xrPivotGridField2.Name = "xrPivotGridField2";
            // 
            // xrPivotGridField3
            // 
            this.xrPivotGridField3.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField3.AreaIndex = 0;
            this.xrPivotGridField3.FieldName = "SalesOrder.DeliveryAddress.DeliveryAddress";
            this.xrPivotGridField3.Name = "xrPivotGridField3";
            // 
            // xrPivotGridField5
            // 
            this.xrPivotGridField5.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField5.AreaIndex = 2;
            this.xrPivotGridField5.FieldName = "SalesOrder.PurchaseOrderNumber";
            this.xrPivotGridField5.Name = "xrPivotGridField5";
            // 
            // pivotGridField5
            // 
            this.pivotGridField5.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField5.AreaIndex = 3;
            this.pivotGridField5.FieldName = "SalesOrder.ScheduledDeliveryDate";
            this.pivotGridField5.Name = "pivotGridField5";
            this.pivotGridField5.ValueFormat.FormatString = "MM/dd/yyyy";
            this.pivotGridField5.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            // 
            // GroupHeader3
            // 
            this.GroupHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrPivotGrid3});
            this.GroupHeader3.HeightF = 140.8334F;
            this.GroupHeader3.Name = "GroupHeader3";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "PJT";
            // 
            // xrPivotGrid3
            // 
            this.xrPivotGrid3.Appearance.Cell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.Appearance.CustomTotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.Appearance.FieldHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.Appearance.FieldValue.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.Appearance.FieldValueGrandTotal.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.Appearance.FieldValueTotal.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.Appearance.GrandTotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.Appearance.Lines.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.Appearance.TotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid3.DataSource = this.collectionDataSource3;
            this.xrPivotGrid3.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.xrPivotGridField6,
            this.xrPivotGridField7,
            this.xrPivotGridField8,
            this.xrPivotGridField9,
            this.xrPivotGridField10,
            this.pivotGridField4});
            this.xrPivotGrid3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 36.66667F);
            this.xrPivotGrid3.Name = "xrPivotGrid3";
            this.xrPivotGrid3.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivotGrid3.OptionsPrint.PrintFilterHeaders = DevExpress.Utils.DefaultBoolean.False;
            this.xrPivotGrid3.OptionsView.ShowColumnHeaders = false;
            this.xrPivotGrid3.OptionsView.ShowDataHeaders = false;
            this.xrPivotGrid3.SizeF = new System.Drawing.SizeF(640F, 90F);
            // 
            // xrPivotGridField6
            // 
            this.xrPivotGridField6.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField6.AreaIndex = 0;
            this.xrPivotGridField6.Caption = "Product";
            this.xrPivotGridField6.FieldName = "Product.ProductName";
            this.xrPivotGridField6.Name = "xrPivotGridField6";
            this.xrPivotGridField6.Width = 300;
            // 
            // xrPivotGridField7
            // 
            this.xrPivotGridField7.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField7.AreaIndex = 0;
            this.xrPivotGridField7.Caption = "Quantity";
            this.xrPivotGridField7.FieldName = "Quantity";
            this.xrPivotGridField7.Name = "xrPivotGridField7";
            // 
            // xrPivotGridField8
            // 
            this.xrPivotGridField8.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField8.AreaIndex = 0;
            this.xrPivotGridField8.FieldName = "SalesOrder.DeliveryAddress.DeliveryAddress";
            this.xrPivotGridField8.Name = "xrPivotGridField8";
            // 
            // xrPivotGridField10
            // 
            this.xrPivotGridField10.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField10.AreaIndex = 2;
            this.xrPivotGridField10.FieldName = "SalesOrder.PurchaseOrderNumber";
            this.xrPivotGridField10.Name = "xrPivotGridField10";
            // 
            // pivotGridField4
            // 
            this.pivotGridField4.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField4.AreaIndex = 3;
            this.pivotGridField4.FieldName = "SalesOrder.ScheduledDeliveryDate";
            this.pivotGridField4.Name = "pivotGridField4";
            this.pivotGridField4.ValueFormat.FormatString = "MM/dd/yyyy";
            this.pivotGridField4.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            // 
            // GroupHeader4
            // 
            this.GroupHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrPivotGrid1});
            this.GroupHeader4.HeightF = 136.6667F;
            this.GroupHeader4.Level = 2;
            this.GroupHeader4.Name = "GroupHeader4";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "JRC\r\n";
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.Appearance.Cell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.Appearance.CustomTotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.Appearance.FieldHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.Appearance.FieldValue.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.Appearance.FieldValueGrandTotal.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.Appearance.FieldValueTotal.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.Appearance.GrandTotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.Appearance.Lines.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.Appearance.TotalCell.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.xrPivotGrid1.DataSource = this.collectionDataSource1;
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.fieldProductProductName,
            this.fieldQuantity1,
            this.pivotGridField1,
            this.pivotGridField2,
            this.pivotGridField3,
            this.pivotGridField6});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivotGrid1.OptionsPrint.PrintFilterHeaders = DevExpress.Utils.DefaultBoolean.False;
            this.xrPivotGrid1.OptionsView.ShowColumnHeaders = false;
            this.xrPivotGrid1.OptionsView.ShowDataHeaders = false;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(640F, 107F);
            // 
            // fieldProductProductName
            // 
            this.fieldProductProductName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldProductProductName.AreaIndex = 0;
            this.fieldProductProductName.Caption = "Product";
            this.fieldProductProductName.FieldName = "Product.ProductName";
            this.fieldProductProductName.Name = "fieldProductProductName";
            this.fieldProductProductName.Width = 300;
            // 
            // fieldQuantity1
            // 
            this.fieldQuantity1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldQuantity1.AreaIndex = 0;
            this.fieldQuantity1.Caption = "Quantity";
            this.fieldQuantity1.FieldName = "Quantity";
            this.fieldQuantity1.Name = "fieldQuantity1";
            // 
            // pivotGridField1
            // 
            this.pivotGridField1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField1.AreaIndex = 0;
            this.pivotGridField1.FieldName = "SalesOrder.DeliveryAddress.DeliveryAddress";
            this.pivotGridField1.Name = "pivotGridField1";
            // 
            // pivotGridField2
            // 
            this.pivotGridField2.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField2.AreaIndex = 1;
            this.pivotGridField2.FieldName = "SalesOrder.Type";
            this.pivotGridField2.Name = "pivotGridField2";
            // 
            // pivotGridField3
            // 
            this.pivotGridField3.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField3.AreaIndex = 2;
            this.pivotGridField3.FieldName = "SalesOrder.PurchaseOrderNumber";
            this.pivotGridField3.Name = "pivotGridField3";
            // 
            // pivotGridField6
            // 
            this.pivotGridField6.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField6.AreaIndex = 3;
            this.pivotGridField6.FieldName = "SalesOrder.ScheduledDeliveryDate";
            this.pivotGridField6.Name = "pivotGridField6";
            this.pivotGridField6.ValueFormat.FormatString = "MM/dd/yyyy";
            this.pivotGridField6.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            // 
            // xrLabel6
            // 
            this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum([Quantity])")});
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(241.6667F, 10F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Total Chilled";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(129.1667F, 10F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "Total Chilled:";
            // 
            // SubBand1
            // 
            this.SubBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel7,
            this.xrLabel6});
            this.SubBand1.HeightF = 36.66682F;
            this.SubBand1.Name = "SubBand1";
            // 
            // xrPivotGridField4
            // 
            this.xrPivotGridField4.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField4.AreaIndex = 1;
            this.xrPivotGridField4.FieldName = "SalesOrder.Type";
            this.xrPivotGridField4.Name = "xrPivotGridField4";
            // 
            // xrPivotGridField9
            // 
            this.xrPivotGridField9.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField9.AreaIndex = 1;
            this.xrPivotGridField9.FieldName = "SalesOrder.Type";
            this.xrPivotGridField9.Name = "xrPivotGridField9";
            // 
            // collectionDataSource2
            // 
            this.collectionDataSource2.CriteriaString = "[SalesOrder.SupplierCode.SuppCode.SuppCode] = \'PJJ\' And [SalesOrder.PurchaseOrder" +
    "Date] = ?PODate";
            this.collectionDataSource2.Name = "collectionDataSource2";
            this.collectionDataSource2.ObjectTypeName = "JolliantProd.Module.BusinessObjects.SalesOrderLine";
            this.collectionDataSource2.TopReturnedRecords = 0;
            // 
            // collectionDataSource3
            // 
            this.collectionDataSource3.CriteriaString = "[SalesOrder.SupplierCode.SuppCode.SuppCode] = \'PJT\' And [SalesOrder.PurchaseOrder" +
    "Date] = ?PODate";
            this.collectionDataSource3.Name = "collectionDataSource3";
            this.collectionDataSource3.ObjectTypeName = "JolliantProd.Module.BusinessObjects.SalesOrderLine";
            this.collectionDataSource3.TopReturnedRecords = 0;
            // 
            // collectionDataSource1
            // 
            this.collectionDataSource1.CriteriaString = "[SalesOrder.SupplierCode.SuppCode.SuppCode] = \'JRC\' And [SalesOrder.PurchaseOrder" +
    "Date] = ?PODate";
            this.collectionDataSource1.Name = "collectionDataSource1";
            this.collectionDataSource1.ObjectTypeName = "JolliantProd.Module.BusinessObjects.SalesOrderLine";
            this.collectionDataSource1.TopReturnedRecords = 0;
            // 
            // collectionDataSource4
            // 
            this.collectionDataSource4.CriteriaString = "[SalesOrder.SupplierCode.SuppCode.SuppCode] In (\'JRC\', \'PJJ\') And [SalesOrder.Pur" +
    "chaseOrderDate] = ?PODate";
            this.collectionDataSource4.Name = "collectionDataSource4";
            this.collectionDataSource4.ObjectTypeName = "JolliantProd.Module.BusinessObjects.SalesOrderLine";
            this.collectionDataSource4.TopReturnedRecords = 0;
            // 
            // XtraReport1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.GroupHeader2,
            this.GroupHeader3,
            this.GroupHeader4});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.collectionDataSource4,
            this.collectionDataSource1,
            this.collectionDataSource3,
            this.collectionDataSource2});
            this.DataSource = this.collectionDataSource4;
            this.DefaultPrinterSettingsUsing.UseLandscape = true;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 100, 48);
            this.PageHeight = 1250;
            this.PageWidth = 2100;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.PODate});
            this.Version = "18.2";
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter PODate;
        private DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource2;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid2;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField2;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField3;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField4;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField5;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader3;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid3;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField6;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField7;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField8;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField9;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField10;
        private DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField pivotGridField5;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField pivotGridField4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldProductProductName;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldQuantity1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField pivotGridField1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField pivotGridField2;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField pivotGridField3;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField pivotGridField6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource4;
        private DevExpress.XtraReports.UI.SubBand SubBand1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
    }
}
