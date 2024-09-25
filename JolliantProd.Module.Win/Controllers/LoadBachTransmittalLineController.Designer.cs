namespace JolliantProd.Module.Win.Controllers
{
    partial class LoadBachTransmittalLineController
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
            this.BatchTransmittalAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BatchTransmittalAction
            // 
            this.BatchTransmittalAction.Caption = "Load From Excel";
            this.BatchTransmittalAction.ConfirmationMessage = null;
            this.BatchTransmittalAction.Id = "BatchTransmittalAction";
            this.BatchTransmittalAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.BatchTransmittal);
            this.BatchTransmittalAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BatchTransmittalAction.ToolTip = null;
            this.BatchTransmittalAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BatchTransmittalAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BatchTransmittalAction_Execute);
            // 
            // LoadBachTransmittalLineController
            // 
            this.Actions.Add(this.BatchTransmittalAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BatchTransmittalAction;
    }
}
