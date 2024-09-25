namespace JolliantProd.Module.Controllers
{
    partial class ReceivingSummaryReportController
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
            this.GenerateReceivingSummaryAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GenerateReceivingSummaryAction
            // 
            this.GenerateReceivingSummaryAction.Caption = "Generate Receiving Summary";
            this.GenerateReceivingSummaryAction.Category = "";
            this.GenerateReceivingSummaryAction.ConfirmationMessage = null;
            this.GenerateReceivingSummaryAction.Id = "GenerateReceivingSummary";
            this.GenerateReceivingSummaryAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.ReceivingReportSummary);
            this.GenerateReceivingSummaryAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.GenerateReceivingSummaryAction.ToolTip = null;
            this.GenerateReceivingSummaryAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.GenerateReceivingSummaryAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GenerateReceivingSummaryAction_Execute);
            // 
            // ReceivingSummaryReportController
            // 
            this.Actions.Add(this.GenerateReceivingSummaryAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction GenerateReceivingSummaryAction;
    }
}
