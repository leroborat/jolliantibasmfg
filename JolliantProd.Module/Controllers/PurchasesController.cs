using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using JolliantProd.Module.BusinessObjects;

namespace JolliantProd.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PurchasesController : ViewController
    {
        public PurchasesController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void CreatePOAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((MaterialRequest)View.CurrentObject).Save();
            var thisOid = ((MaterialRequest)View.CurrentObject).Oid.ToString();

            IObjectSpace os = Application.CreateObjectSpace(); // Create IObjectSpace or use the existing one, e.g. View.ObjectSpace, if it is suitable for your scenario.

            MaterialRequest thisMR = os.FindObject<MaterialRequest>(new BinaryOperator("Oid", thisOid));
            PurchaseOrder obj = os.CreateObject<PurchaseOrder>();
            thisMR.PurchaseOrder = obj;
            thisMR.Status = MaterialRequest.RequestStatusEnum.Approved;

            foreach (MaterialRequestLine item in thisMR.MaterialRequestLines)
            {
                PurchaseOrderLine pol = os.CreateObject<PurchaseOrderLine>();
                pol.Product = item.Product;
                pol.Quantity = item.Quantity;
                obj.PurchaseOrderLines.Add(pol);
            }

            DetailView dv = Application.CreateDetailView(os, obj);//Specify the IsRoot parameter if necessary.
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.ShowViewParameters.CreatedView = dv;
        }

        private void DeclineRequest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((MaterialRequest)View.CurrentObject).Status = MaterialRequest.RequestStatusEnum.Denied;
            ((MaterialRequest)View.CurrentObject).Save();
            ObjectSpace.CommitChanges();
        }

        private void ApprovePOAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((PurchaseOrder)View.CurrentObject).Status = PurchaseOrder.StatusEnum.Approved;
            ((PurchaseOrder)View.CurrentObject).ApprovedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            ((PurchaseOrder)View.CurrentObject).Save();
            ObjectSpace.CommitChanges();
            Receiving receiving = ObjectSpace.CreateObject<Receiving>();
            receiving.PurchaseOrder = ((PurchaseOrder)View.CurrentObject);
            receiving.ActualDeliveryDate = ((PurchaseOrder)View.CurrentObject).DeliveryDate;
            ObjectSpace.CommitChanges();
        }

        private void CancelPOAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((PurchaseOrder)View.CurrentObject).Status = PurchaseOrder.StatusEnum.Declined;
            ((PurchaseOrder)View.CurrentObject).ApprovedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            ((PurchaseOrder)View.CurrentObject).Save();
            ObjectSpace.CommitChanges();
        }

        private void ValidateReceiptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((Receiving)View.CurrentObject);
            thisView.Save();
            WarehouseLocation warehouseLocation;
            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Vendor"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Vendor";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.VendorLocation;
                ObjectSpace.CommitChanges();
            }

            foreach (ReceivedLine item in thisView.ReceivedLines)
            {
                if (item.PurchaseQuantityReceived <= 0)
                {
                    continue;
                }
                StockTransfer st = ObjectSpace.CreateObject<StockTransfer>();
                st.SourceLocation = warehouseLocation;
                st.DestinationLocation = thisView.StorageLocation;
                if(item.Lot != null)
                {
                    st.Lot = item.Lot;
                }
                st.Quantity = item.StockingQuantityReceived;
                st.UOM = item.StorageUOM;
                st.Product = item.Product;
                st.Reference = thisView.Series;
                ObjectSpace.CommitChanges();
                if (item?.LotExpiry != null)
                {
                    st.Lot.ExpirationDate = item.LotExpiry;
                }
                st.Lot.UpdateStockOnHand(true);
                ObjectSpace.CommitChanges();
            }
            //Set status to Validated
            thisView.Status = Receiving.StatusEnum.Validated;
            //Set Employee who handled
            thisView.ProcessedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            //Save
            thisView.Save();
            ObjectSpace.CommitChanges();
        }
    }
}
