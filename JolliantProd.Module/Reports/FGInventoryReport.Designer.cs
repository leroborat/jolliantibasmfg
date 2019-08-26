namespace JolliantProd.Module.Reports
{
    partial class FGInventoryReport
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
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FGInventoryReport));
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.InventoryDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.groupHeaderBand1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.WarehouseLocation = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.collectionDataSource1 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            this.collectionDataSource2 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            this.collectionDataSource3 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Visible = false;
            // 
            // InventoryDate
            // 
            this.InventoryDate.Description = "InventoryDate";
            this.InventoryDate.Name = "InventoryDate";
            this.InventoryDate.Type = typeof(System.DateTime);
            this.InventoryDate.ValueInfo = "2019-08-18";
            // 
            // groupHeaderBand1
            // 
            this.groupHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1});
            this.groupHeaderBand1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Product.ProductName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.groupHeaderBand1.HeightF = 25F;
            this.groupHeaderBand1.Name = "groupHeaderBand1";
            this.groupHeaderBand1.AfterPrint += new System.EventHandler(this.GroupHeaderBand1_AfterPrint);
            // 
            // xrLabel2
            // 
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", resources.GetString("xrLabel2.ExpressionBindings"))});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(313.125F, 0F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(116.25F, 23F);
            xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel2.Summary = xrSummary4;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextFormatString = "{0:#.00}";
            this.xrLabel2.SummaryCalculated += new DevExpress.XtraReports.UI.TextFormatEventHandler(this.XrLabel2_SummaryCalculated);
            // 
            // xrLabel1
            // 
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Product].[ProductName]")});
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(290.625F, 23F);
            this.xrLabel1.Text = "xrLabel1";
            // 
            // WarehouseLocation
            // 
            this.WarehouseLocation.Description = "Location";
            dynamicListLookUpSettings1.DataSource = this.collectionDataSource2;
            dynamicListLookUpSettings1.DisplayMember = "DisplayName";
            dynamicListLookUpSettings1.SortMember = null;
            dynamicListLookUpSettings1.ValueMember = "Oid";
            this.WarehouseLocation.LookUpSettings = dynamicListLookUpSettings1;
            this.WarehouseLocation.Name = "WarehouseLocation";
            this.WarehouseLocation.Type = typeof(System.Guid);
            this.WarehouseLocation.ValueInfo = "00000000-0000-0000-0000-000000000000";
            // 
            // xrLabel3
            // 
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum(Iif([DestinationLocation].[Oid] == ?WarehouseLocation  And GetDate([Date])" +
                    " == GetDate(?InventoryDate),[Quantity],0))")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(441.8749F, 0F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 23F);
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel3.Summary = xrSummary3;
            this.xrLabel3.Text = "xrLabel2";
            this.xrLabel3.TextFormatString = "{0:#.00}";
            // 
            // xrLabel4
            // 
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", resources.GetString("xrLabel4.ExpressionBindings"))});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(677.0833F, 0F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(100F, 23F);
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel4.Summary = xrSummary2;
            this.xrLabel4.Text = "xrLabel2";
            this.xrLabel4.TextFormatString = "{0:#.00}";
            // 
            // xrLabel5
            // 
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum(Iif([SourceLocation].[Oid] == ?WarehouseLocation  And GetDate([Date]) == G" +
                    "etDate(?InventoryDate),[Quantity],0))")});
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(553.5417F, 1.999998F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(100F, 23F);
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel5.Summary = xrSummary1;
            this.xrLabel5.Text = "xrLabel2";
            this.xrLabel5.TextFormatString = "{0:#.00}";
            // 
            // xrTable1
            // 
            this.xrTable1.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(800F, 25F);
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell5});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "Finished Good";
            this.xrTableCell1.Weight = 1.9570310592651368D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "Beginning";
            this.xrTableCell2.Weight = 0.72656230926513676D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Received";
            this.xrTableCell3.Weight = 0.70312500000000011D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "Delivered";
            this.xrTableCell4.Weight = 0.69791717529296882D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "Ending";
            this.xrTableCell5.Weight = 0.91536445617675777D;
            // 
            // collectionDataSource1
            // 
            this.collectionDataSource1.CriteriaString = "[Date] > #2019-01-01# And [Date] <= AddDays(?InventoryDate, 1) And [Product.CanBe" +
    "Sold] = True And ([DestinationLocation.Oid] = ?WarehouseLocation Or [SourceLocat" +
    "ion.Oid] = ?WarehouseLocation)";
            this.collectionDataSource1.Name = "collectionDataSource1";
            this.collectionDataSource1.ObjectTypeName = "JolliantProd.Module.BusinessObjects.StockTransfer";
            this.collectionDataSource1.TopReturnedRecords = 0;
            // 
            // collectionDataSource2
            // 
            this.collectionDataSource2.Name = "collectionDataSource2";
            this.collectionDataSource2.ObjectTypeName = "JolliantProd.Module.BusinessObjects.WarehouseLocation";
            this.collectionDataSource2.TopReturnedRecords = 0;
            // 
            // collectionDataSource3
            // 
            this.collectionDataSource3.CriteriaString = "[Date] > #2019-01-01# And [Date] <= ?InventoryDate And [Product.CanBeSold] = True" +
    " And [SourceLocation.Oid] = ?WarehouseLocation";
            this.collectionDataSource3.Name = "collectionDataSource3";
            this.collectionDataSource3.ObjectTypeName = "JolliantProd.Module.BusinessObjects.StockTransfer";
            this.collectionDataSource3.TopReturnedRecords = 0;
            // 
            // FGInventoryReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.groupHeaderBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.collectionDataSource1,
            this.collectionDataSource2,
            this.collectionDataSource3});
            this.DataSource = this.collectionDataSource1;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(25, 25, 100, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.InventoryDate,
            this.WarehouseLocation});
            this.Version = "19.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionDataSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter InventoryDate;
        private DevExpress.XtraReports.UI.GroupHeaderBand groupHeaderBand1;
        private DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource2;
        private DevExpress.XtraReports.Parameters.Parameter WarehouseLocation;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.Persistent.Base.ReportsV2.CollectionDataSource collectionDataSource3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
    }
}
