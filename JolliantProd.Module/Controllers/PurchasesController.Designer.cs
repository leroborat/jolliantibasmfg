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
            // PurchasesController
            // 
            this.Actions.Add(this.CreatePOAction);
            this.Actions.Add(this.DeclineRequest);
            this.Actions.Add(this.ApprovePOAction);
            this.Actions.Add(this.CancelPOAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreatePOAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DeclineRequest;
        private DevExpress.ExpressApp.Actions.SimpleAction ApprovePOAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CancelPOAction;
    }
}
