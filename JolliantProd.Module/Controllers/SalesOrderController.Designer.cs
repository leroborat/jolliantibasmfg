namespace JolliantProd.Module.Controllers
{
    partial class SalesOrderController
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.simpleAction1 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleAction2 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateInventoryAdjustmentAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateInventoryReturnAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateScrapAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateInternalTransferAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.GenerateInvoiceLinesAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateInvoiceAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateCustomerPaymentAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.GenerateTripLinesAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.POSummaryAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.UpdateLotExpiry = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TripSetToDraft = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SetToDraftSO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SetToDraftInvoiceAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleAction1
            // 
            this.simpleAction1.Caption = "Confirm Order";
            this.simpleAction1.ConfirmationMessage = "Do you want to confirm this order?";
            this.simpleAction1.Id = "SOConfirmAction";
            this.simpleAction1.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.SalesOrder);
            this.simpleAction1.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.simpleAction1.ToolTip = null;
            this.simpleAction1.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.simpleAction1.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
            // 
            // simpleAction2
            // 
            this.simpleAction2.Caption = "Confirm Trip";
            this.simpleAction2.ConfirmationMessage = "Are you sure you want to confirm trip?";
            this.simpleAction2.Id = "ValidateTripAction";
            this.simpleAction2.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Trip);
            this.simpleAction2.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.simpleAction2.ToolTip = null;
            this.simpleAction2.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.simpleAction2.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction2_Execute);
            // 
            // ValidateInventoryAdjustmentAction
            // 
            this.ValidateInventoryAdjustmentAction.Caption = "Validate Inventory Adjustment";
            this.ValidateInventoryAdjustmentAction.ConfirmationMessage = null;
            this.ValidateInventoryAdjustmentAction.Id = "ValidateInventoryAdjustmentAction";
            this.ValidateInventoryAdjustmentAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.InventoryAdjustment);
            this.ValidateInventoryAdjustmentAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateInventoryAdjustmentAction.ToolTip = null;
            this.ValidateInventoryAdjustmentAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateInventoryAdjustmentAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateInventoryAdjustmentAction_Execute);
            // 
            // ValidateInventoryReturnAction
            // 
            this.ValidateInventoryReturnAction.Caption = "Validate Inventory Return";
            this.ValidateInventoryReturnAction.ConfirmationMessage = null;
            this.ValidateInventoryReturnAction.Id = "ValidateInventoryReturnAction";
            this.ValidateInventoryReturnAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.InventoryReturn);
            this.ValidateInventoryReturnAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateInventoryReturnAction.ToolTip = null;
            this.ValidateInventoryReturnAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateInventoryReturnAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateInventoryReturnAction_Execute);
            // 
            // ValidateScrapAction
            // 
            this.ValidateScrapAction.Caption = "Validate Scrap Action";
            this.ValidateScrapAction.ConfirmationMessage = null;
            this.ValidateScrapAction.Id = "ValidateScrapAction";
            this.ValidateScrapAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.InventoryScrap);
            this.ValidateScrapAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateScrapAction.ToolTip = null;
            this.ValidateScrapAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateScrapAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateScrapAction_Execute);
            // 
            // ValidateInternalTransferAction
            // 
            this.ValidateInternalTransferAction.Caption = "Validate Internal Transfer";
            this.ValidateInternalTransferAction.ConfirmationMessage = null;
            this.ValidateInternalTransferAction.Id = "ValidateInternalTransferAction";
            this.ValidateInternalTransferAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.InternalTransfer);
            this.ValidateInternalTransferAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateInternalTransferAction.ToolTip = null;
            this.ValidateInternalTransferAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateInternalTransferAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateInternalTransferAction_Execute);
            // 
            // GenerateInvoiceLinesAction
            // 
            this.GenerateInvoiceLinesAction.Caption = "Generate Invoice Lines From Trips";
            this.GenerateInvoiceLinesAction.ConfirmationMessage = null;
            this.GenerateInvoiceLinesAction.Id = "GenerateInvoiceLinesAction";
            this.GenerateInvoiceLinesAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Invoice);
            this.GenerateInvoiceLinesAction.ToolTip = null;
            this.GenerateInvoiceLinesAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GenerateInvoiceLinesAction_Execute);
            // 
            // ValidateInvoiceAction
            // 
            this.ValidateInvoiceAction.Caption = "Validate Invoice";
            this.ValidateInvoiceAction.ConfirmationMessage = null;
            this.ValidateInvoiceAction.Id = "ValidateInvoiceAction";
            this.ValidateInvoiceAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Invoice);
            this.ValidateInvoiceAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateInvoiceAction.ToolTip = null;
            this.ValidateInvoiceAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateInvoiceAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateInvoiceAction_Execute);
            // 
            // ValidateCustomerPaymentAction
            // 
            this.ValidateCustomerPaymentAction.Caption = "Validate Customer Payment";
            this.ValidateCustomerPaymentAction.ConfirmationMessage = null;
            this.ValidateCustomerPaymentAction.Id = "ValidateCustomerPaymentAction";
            this.ValidateCustomerPaymentAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.CustomerPayment);
            this.ValidateCustomerPaymentAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateCustomerPaymentAction.ToolTip = null;
            this.ValidateCustomerPaymentAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateCustomerPaymentAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateCustomerPaymentAction_Execute);
            // 
            // GenerateTripLinesAction
            // 
            this.GenerateTripLinesAction.Caption = "Generate Trip Lines From Linked Lots";
            this.GenerateTripLinesAction.ConfirmationMessage = null;
            this.GenerateTripLinesAction.Id = "GenerateTripLinesAction";
            this.GenerateTripLinesAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.TripLine);
            this.GenerateTripLinesAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.GenerateTripLinesAction.ToolTip = null;
            this.GenerateTripLinesAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.GenerateTripLinesAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GenerateTripLinesAction_Execute);
            // 
            // POSummaryAction
            // 
            this.POSummaryAction.Caption = "Generate PO Summary";
            this.POSummaryAction.Category = "Reports";
            this.POSummaryAction.ConfirmationMessage = null;
            this.POSummaryAction.Id = "POSummaryAction";
            this.POSummaryAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.SalesOrder);
            this.POSummaryAction.ToolTip = null;
            this.POSummaryAction.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.POSummaryAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.POSummaryAction_Execute);
            // 
            // UpdateLotExpiry
            // 
            this.UpdateLotExpiry.Caption = "Update Hotta Rice Lot Expiry";
            this.UpdateLotExpiry.ConfirmationMessage = "This is a long running process. Do not close iBAS until it finishes cleaning up t" +
    "he data. Do you want to proceed?";
            this.UpdateLotExpiry.Id = "UpdateLotExpiry";
            this.UpdateLotExpiry.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Lot);
            this.UpdateLotExpiry.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.UpdateLotExpiry.ToolTip = null;
            this.UpdateLotExpiry.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.UpdateLotExpiry.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateLotExpiry_Execute);
            // 
            // TripSetToDraft
            // 
            this.TripSetToDraft.Caption = "Set to Draft";
            this.TripSetToDraft.ConfirmationMessage = "Are you sure you want to proceed?";
            this.TripSetToDraft.Id = "TripSetToDraft";
            this.TripSetToDraft.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Trip);
            this.TripSetToDraft.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TripSetToDraft.ToolTip = null;
            this.TripSetToDraft.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.TripSetToDraft.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TripSetToDraft_Execute);
            // 
            // SetToDraftSO
            // 
            this.SetToDraftSO.Caption = "Set To Draft";
            this.SetToDraftSO.ConfirmationMessage = "Are you sure?";
            this.SetToDraftSO.Id = "SOSetToDraft";
            this.SetToDraftSO.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.SalesOrder);
            this.SetToDraftSO.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.SetToDraftSO.ToolTip = null;
            this.SetToDraftSO.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.SetToDraftSO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SetToDraftSO_Execute);
            // 
            // SetToDraftInvoiceAction
            // 
            this.SetToDraftInvoiceAction.Caption = "Cancel Invoice";
            this.SetToDraftInvoiceAction.ConfirmationMessage = null;
            this.SetToDraftInvoiceAction.Id = "5b04dc92-be01-4187-8945-c0897517ac6a";
            this.SetToDraftInvoiceAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Invoice);
            this.SetToDraftInvoiceAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.SetToDraftInvoiceAction.ToolTip = null;
            this.SetToDraftInvoiceAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.SetToDraftInvoiceAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SetToDraftInvoiceAction_Execute);
            // 
            // SalesOrderController
            // 
            this.Actions.Add(this.simpleAction1);
            this.Actions.Add(this.simpleAction2);
            this.Actions.Add(this.ValidateInventoryAdjustmentAction);
            this.Actions.Add(this.ValidateInventoryReturnAction);
            this.Actions.Add(this.ValidateScrapAction);
            this.Actions.Add(this.ValidateInternalTransferAction);
            this.Actions.Add(this.GenerateInvoiceLinesAction);
            this.Actions.Add(this.ValidateInvoiceAction);
            this.Actions.Add(this.ValidateCustomerPaymentAction);
            this.Actions.Add(this.GenerateTripLinesAction);
            this.Actions.Add(this.POSummaryAction);
            this.Actions.Add(this.UpdateLotExpiry);
            this.Actions.Add(this.TripSetToDraft);
            this.Actions.Add(this.SetToDraftSO);
            this.Actions.Add(this.SetToDraftInvoiceAction);
            this.Activated += new System.EventHandler(this.SalesOrderController_Activated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleAction1;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleAction2;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateInventoryAdjustmentAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateInventoryReturnAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateScrapAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateInternalTransferAction;
        private DevExpress.ExpressApp.Actions.SimpleAction GenerateInvoiceLinesAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateInvoiceAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateCustomerPaymentAction;
        private DevExpress.ExpressApp.Actions.SimpleAction GenerateTripLinesAction;
        private DevExpress.ExpressApp.Actions.SimpleAction POSummaryAction;
        private DevExpress.ExpressApp.Actions.SimpleAction UpdateLotExpiry;
        private DevExpress.ExpressApp.Actions.SimpleAction TripSetToDraft;
        private DevExpress.ExpressApp.Actions.SimpleAction SetToDraftSO;
        private DevExpress.ExpressApp.Actions.SimpleAction SetToDraftInvoiceAction;
    }
}
