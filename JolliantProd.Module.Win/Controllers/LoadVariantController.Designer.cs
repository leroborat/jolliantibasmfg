namespace JolliantProd.Module.Win.Controllers
{
    partial class LoadVariantController
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
            this.LoadVariantAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LoadProductAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LoadFgAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.UpdateLotCountDB = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LoadPurchaseableProduct = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LoadVariantAction
            // 
            this.LoadVariantAction.Caption = "Load From Excel";
            this.LoadVariantAction.ConfirmationMessage = null;
            this.LoadVariantAction.Id = "60a54fac-ebcb-40a9-a2e9-d0d9396557b7";
            this.LoadVariantAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Variant);
            this.LoadVariantAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.LoadVariantAction.ToolTip = null;
            this.LoadVariantAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.LoadVariantAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LoadVariantAction_Execute);
            // 
            // LoadProductAction
            // 
            this.LoadProductAction.Caption = "Import Products from Template";
            this.LoadProductAction.ConfirmationMessage = null;
            this.LoadProductAction.Id = "4537853d-4d8f-48db-a963-adf7b01260bc";
            this.LoadProductAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Product);
            this.LoadProductAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.LoadProductAction.ToolTip = null;
            this.LoadProductAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.LoadProductAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LoadProductAction_Execute);
            // 
            // LoadFgAction
            // 
            this.LoadFgAction.Caption = "Import FG Template";
            this.LoadFgAction.ConfirmationMessage = null;
            this.LoadFgAction.Id = "LoadFgAction";
            this.LoadFgAction.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.FinishedGoodLoader);
            this.LoadFgAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.LoadFgAction.ToolTip = null;
            this.LoadFgAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.LoadFgAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LoadFgAction_Execute);
            // 
            // UpdateLotCountDB
            // 
            this.UpdateLotCountDB.Caption = "Update Lot Count in Database";
            this.UpdateLotCountDB.ConfirmationMessage = null;
            this.UpdateLotCountDB.Id = "UpdateLotCountDB";
            this.UpdateLotCountDB.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.FinishedGoodLoader);
            this.UpdateLotCountDB.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.UpdateLotCountDB.ToolTip = null;
            this.UpdateLotCountDB.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.UpdateLotCountDB.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateLotCountDB_Execute);
            // 
            // LoadPurchaseableProduct
            // 
            this.LoadPurchaseableProduct.Caption = "Import Purchaseable Products from Template";
            this.LoadPurchaseableProduct.ConfirmationMessage = null;
            this.LoadPurchaseableProduct.Id = "LoadPurchaseableProduct";
            this.LoadPurchaseableProduct.TargetObjectType = typeof(JolliantProd.Module.BusinessObjects.Product);
            this.LoadPurchaseableProduct.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.LoadPurchaseableProduct.ToolTip = null;
            this.LoadPurchaseableProduct.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.LoadPurchaseableProduct.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LoadPurchaseableProduct_Execute);
            // 
            // LoadVariantController
            // 
            this.Actions.Add(this.LoadVariantAction);
            this.Actions.Add(this.LoadProductAction);
            this.Actions.Add(this.LoadFgAction);
            this.Actions.Add(this.UpdateLotCountDB);
            this.Actions.Add(this.LoadPurchaseableProduct);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction LoadVariantAction;
        private DevExpress.ExpressApp.Actions.SimpleAction LoadProductAction;
        private DevExpress.ExpressApp.Actions.SimpleAction LoadFgAction;
        private DevExpress.ExpressApp.Actions.SimpleAction UpdateLotCountDB;
        private DevExpress.ExpressApp.Actions.SimpleAction LoadPurchaseableProduct;
    }
}
