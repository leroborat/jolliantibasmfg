namespace JolliantProd.Module.Controllers
{
    partial class ProductionController
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
            this.PlanMO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ValidateProductionTransfer = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.MarkComplete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.MarkWorkOrderDone = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.MarkProductionCompleted = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PlanMO
            // 
            this.PlanMO.Caption = "Plan";
            this.PlanMO.ConfirmationMessage = null;
            this.PlanMO.Id = "PlanMO";
            this.PlanMO.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.ManufacturingOrder);
            this.PlanMO.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.PlanMO.ToolTip = null;
            this.PlanMO.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.PlanMO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PlanMO_Execute);
            // 
            // ValidateProductionTransfer
            // 
            this.ValidateProductionTransfer.Caption = "Validate";
            this.ValidateProductionTransfer.ConfirmationMessage = null;
            this.ValidateProductionTransfer.Id = "ValidateProductionTransfer";
            this.ValidateProductionTransfer.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.ProductionTransfer);
            this.ValidateProductionTransfer.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ValidateProductionTransfer.ToolTip = null;
            this.ValidateProductionTransfer.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ValidateProductionTransfer.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidateProductionTransfer_Execute);
            // 
            // MarkComplete
            // 
            this.MarkComplete.Caption = "Mark Complete";
            this.MarkComplete.ConfirmationMessage = null;
            this.MarkComplete.Id = "MarkComplete";
            this.MarkComplete.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.WithdrawalRequest);
            this.MarkComplete.ToolTip = null;
            this.MarkComplete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.MarkComplete_Execute);
            // 
            // MarkWorkOrderDone
            // 
            this.MarkWorkOrderDone.Caption = "Mark As Done";
            this.MarkWorkOrderDone.ConfirmationMessage = null;
            this.MarkWorkOrderDone.Id = "MarkWorkOrderDone";
            this.MarkWorkOrderDone.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.WorkOrder);
            this.MarkWorkOrderDone.ToolTip = null;
            this.MarkWorkOrderDone.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.MarkWorkOrderDone_Execute);
            // 
            // MarkProductionCompleted
            // 
            this.MarkProductionCompleted.Caption = "Mark Production Completed";
            this.MarkProductionCompleted.ConfirmationMessage = null;
            this.MarkProductionCompleted.Id = "MarkProductionCompleted";
            this.MarkProductionCompleted.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.ManufacturingOrder);
            this.MarkProductionCompleted.ToolTip = null;
            this.MarkProductionCompleted.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.MarkProductionCompleted_Execute);
            // 
            // ProductionController
            // 
            this.Actions.Add(this.PlanMO);
            this.Actions.Add(this.ValidateProductionTransfer);
            this.Actions.Add(this.MarkComplete);
            this.Actions.Add(this.MarkWorkOrderDone);
            this.Actions.Add(this.MarkProductionCompleted);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PlanMO;
        private DevExpress.ExpressApp.Actions.SimpleAction ValidateProductionTransfer;
        private DevExpress.ExpressApp.Actions.SimpleAction MarkComplete;
        private DevExpress.ExpressApp.Actions.SimpleAction MarkWorkOrderDone;
        private DevExpress.ExpressApp.Actions.SimpleAction MarkProductionCompleted;
    }
}
