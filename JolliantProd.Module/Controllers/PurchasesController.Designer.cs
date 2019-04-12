namespace JolliantProd.Module.Controllers
{
    partial class PurchasesController
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
            this.CreatePOAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DeclineRequest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApprovePOAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CancelPOAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateReceiptAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ProcessReceivingReturnAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AssignLotsAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateWithdrawal = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreatePOAction
            // 
            this.CreatePOAction.Caption = "Generate PO";
            this.CreatePOAction.ConfirmationMessage = null;
            this.CreatePOAction.Id = "CreatePOAction";
            this.CreatePOAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.MaterialRequest);
            this.CreatePOAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CreatePOAction.ToolTip = null;
            this.CreatePOAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CreatePOAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreatePOAction_Execute);
            // 
            // DeclineRequest
            // 
            this.DeclineRequest.Caption = "Decline Request";
            this.DeclineRequest.ConfirmationMessage = null;
            this.DeclineRequest.Id = "DeclineRequest";
            this.DeclineRequest.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.MaterialRequest);
            this.DeclineRequest.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.DeclineRequest.ToolTip = null;
            this.DeclineRequest.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.DeclineRequest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeclineRequest_Execute);
            // 
            // ApprovePOAction
            // 
            this.ApprovePOAction.Caption = "Approve PO";
            this.ApprovePOAction.ConfirmationMessage = null;
            this.ApprovePOAction.Id = "ApprovePOAction";
            this.ApprovePOAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.PurchaseOrder);
            this.ApprovePOAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ApprovePOAction.ToolTip = null;
            this.ApprovePOAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ApprovePOAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApprovePOAction_Execute);
            // 
            // CancelPOAction
            // 
            this.CancelPOAction.Caption = "Cancel PO";
            this.CancelPOAction.ConfirmationMessage = null;
            this.CancelPOAction.Id = "CancelPOAction";
            this.CancelPOAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.PurchaseOrder);
            this.CancelPOAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CancelPOAction.ToolTip = null;
            this.CancelPOAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CancelPOAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CancelPOAction_Execute);
            // 
            // ValidateReceiptAction
            // 
            this.ValidateReceiptAction.Caption = "Validate";
            this.ValidateReceiptAction.ConfirmationMessage = null;
            this.ValidateReceiptAction.Id = "ValidateReceiptAction";
            this.ValidateReceiptAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Receiving);
            this.ValidateReceiptAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateReceiptAction.ToolTip = null;
            this.ValidateReceiptAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateReceiptAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateReceiptAction_Execute);
            // 
            // ProcessReceivingReturnAction
            // 
            this.ProcessReceivingReturnAction.Caption = "Validate";
            this.ProcessReceivingReturnAction.ConfirmationMessage = null;
            this.ProcessReceivingReturnAction.Id = "ProcessReceivingReturnAction";
            this.ProcessReceivingReturnAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.ReceivingReturn);
            this.ProcessReceivingReturnAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ProcessReceivingReturnAction.ToolTip = null;
            this.ProcessReceivingReturnAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ProcessReceivingReturnAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProcessReceivingReturnAction_Execute);
            // 
            // AssignLotsAction
            // 
            this.AssignLotsAction.Caption = "Assign Lots";
            this.AssignLotsAction.ConfirmationMessage = null;
            this.AssignLotsAction.Id = "AssignLotsAction";
            this.AssignLotsAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Withdrawal);
            this.AssignLotsAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AssignLotsAction.ToolTip = null;
            this.AssignLotsAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AssignLotsAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AssignLotsAction_Execute);
            // 
            // ValidateWithdrawal
            // 
            this.ValidateWithdrawal.Caption = "Validate Withdrawal";
            this.ValidateWithdrawal.ConfirmationMessage = null;
            this.ValidateWithdrawal.Id = "ValidateWithdrawal";
            this.ValidateWithdrawal.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Withdrawal);
            this.ValidateWithdrawal.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateWithdrawal.ToolTip = null;
            this.ValidateWithdrawal.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateWithdrawal.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateWithdrawal_Execute);
            // 
            // PurchasesController
            // 
            this.Actions.Add(this.CreatePOAction);
            this.Actions.Add(this.DeclineRequest);
            this.Actions.Add(this.ApprovePOAction);
            this.Actions.Add(this.CancelPOAction);
            this.Actions.Add(this.ValidateReceiptAction);
            this.Actions.Add(this.ProcessReceivingReturnAction);
            this.Actions.Add(this.AssignLotsAction);
            this.Actions.Add(this.ValidateWithdrawal);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreatePOAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DeclineRequest;
        private DevExpress.ExpressApp.Actions.SimpleAction ApprovePOAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CancelPOAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateReceiptAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ProcessReceivingReturnAction;
        private DevExpress.ExpressApp.Actions.SimpleAction AssignLotsAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateWithdrawal;
    }
}
