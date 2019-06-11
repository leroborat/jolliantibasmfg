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
            // ProductionController
            // 
            this.Actions.Add(this.PlanMO);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PlanMO;
    }
}
