using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using DevExpress.Xpo;
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

            obj.Vendor = thisMR.Vendor;
            obj.DeliveryDate = thisMR.DeliveryDate;

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

            if (thisView.Series == null)
            {
                thisView.StorageLocation.NextIn += 1;
                thisView.Series = thisView.StorageLocation.Warehouse.WarehouseName + "-IN-" + thisView.StorageLocation.NextIn;
                ObjectSpace.CommitChanges();
            }
            
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

        private void ProcessReceivingReturnAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            var thisView = ((ReceivingReturn)View.CurrentObject);
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

            foreach (ReceivingReturnLine item in thisView.ReceivingReturnLines)
            {
                if (item.Quantity <= 0)
                {
                    continue;
                }
                StockTransfer st = ObjectSpace.CreateObject<StockTransfer>();
                st.DestinationLocation = warehouseLocation;
                st.SourceLocation = thisView.FromLocation;

                if (item.Lot != null)
                {
                    st.Lot = item.Lot;
                }
                st.Quantity = item.StockingQuantity;
                st.UOM = item.StorageUOM;
                st.Product = item.Product;
                st.Reference = "Return: " +  thisView.PurchaseOrder.PurchaseOrderNumber;
                ObjectSpace.CommitChanges();
                st.Lot.UpdateStockOnHand(true);
                ObjectSpace.CommitChanges();
            }
            //Set status to Validated
            thisView.Status = ReceivingReturn.StatusEnum.Validated;
            //Set Employee who handled
            thisView.ProcessedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            //Save
            thisView.Save();
            ObjectSpace.CommitChanges();
        }

        private void AssignLotsAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((Withdrawal)View.CurrentObject);

            foreach (WithdrawalLine item in thisView.WithdrawalLines)
            {
                if (item.ProcessedQuantity >= item.DemandQuantity)
                {
                    continue;
                }

                var lots = ObjectSpace.GetObjects<Lot>().Where(x => x.Product == item.Product &&
                x.StockOnHand > 0
                ).OrderBy(x => x.ExpirationDate);

                foreach (var thisLot in lots)
                {
                    var thisObj = ObjectSpace.CreateObject<WithdrawalLineLot>();
                    thisObj.WithdrawalLine = item;
                    thisObj.Lot = thisLot;
                    if (item.DemandQuantity < thisLot.StockOnHand)
                    {
                        Debug.WriteLine("More than");
                        thisObj.DoneQuantity = item.DemandQuantity - item.ProcessedQuantity;
                        break;
                    } else
                    {
                        Debug.WriteLine("Here");
                        thisObj.DoneQuantity = thisLot.StockOnHand;
                    }

                    ObjectSpace.CommitChanges();
                    if (item.ProcessedQuantity >= item.DemandQuantity)
                    {
                        break;
                    }
                }
            }
            ObjectSpace.CommitChanges();
        }

        private void ValidateWithdrawal_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            var thisView = ((Withdrawal)View.CurrentObject);
            WarehouseLocation warehouseLocation;
            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Production"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Production";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.Production;
                ObjectSpace.CommitChanges();
            }

            foreach (var item in thisView.WithdrawalLines)
            {
                foreach (var item2 in item.WithdrawalLineLots)
                {
                    if (item2.DoneQuantity <= 0)
                    {
                        continue;
                    }
                    StockTransfer st = ObjectSpace.CreateObject<StockTransfer>();
                    st.DestinationLocation = warehouseLocation;
                    st.SourceLocation = thisView.Location;

                    if (item2.Lot != null)
                    {
                        item2.Lot.KitchenPlan = thisView.KitchenPlan;
                        st.Lot = item2.Lot;
                    }
                    st.Quantity = item2.DoneQuantity;
                    st.UOM = item.UOM;
                    st.Product = item.Product;
                    st.Reference = "Move to Production: " + thisView.SeriesName;
                    ObjectSpace.CommitChanges();
                    st.Lot.UpdateStockOnHand(true);
                    ObjectSpace.CommitChanges();
                }
               
            }
            //Set status to Validated
            thisView.Status = Withdrawal.StatusEnum.Validated;
            //Set Employee who handled
            thisView.TransferredBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            //Save
            thisView.Save();
            ObjectSpace.CommitChanges();
        }

        private void SetToNewReceivedAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
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
                st.SourceLocation = thisView.StorageLocation;
                st.DestinationLocation = warehouseLocation;
                if (item.Lot != null)
                {
                    st.Lot = item.Lot;
                }
                st.Quantity = item.StockingQuantityReceived;
                st.UOM = item.StorageUOM;
                st.Product = item.Product;
                st.Reference = thisView.Series + " Reversal";
                ObjectSpace.CommitChanges();
                if (item?.LotExpiry != null)
                {
                    st.Lot.ExpirationDate = item.LotExpiry;
                }
                st.Lot.UpdateStockOnHand(true);
            }
            //Set status to Validated
            thisView.Status = Receiving.StatusEnum.New;
            //Set Employee who handled
            thisView.ProcessedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            //Save
            thisView.Save();
            ObjectSpace.CommitChanges();
        }

        private void POSetToDraft_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((PurchaseOrder)View.CurrentObject);
            ((PurchaseOrder)View.CurrentObject).Status = PurchaseOrder.StatusEnum.New;
            foreach (Receiving item in thisView.Receivings)
            {
                if (item.Status == Receiving.StatusEnum.Validated)
                {
                    WarehouseLocation warehouseLocation;
                    warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Vendor"));
                    if (warehouseLocation == null)
                    {
                        warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                        warehouseLocation.LocationName = "Vendor";
                        warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.VendorLocation;
                        ObjectSpace.CommitChanges();
                    }

                    foreach (ReceivedLine item2 in item.ReceivedLines)
                    {
                        if (item2.PurchaseQuantityReceived <= 0)
                        {
                            continue;
                        }
                        StockTransfer st = ObjectSpace.CreateObject<StockTransfer>();
                        st.SourceLocation = item.StorageLocation;
                        st.DestinationLocation = warehouseLocation;
                        if (item2.Lot != null)
                        {
                            st.Lot = item2.Lot;
                        }
                        st.Quantity = item2.StockingQuantityReceived;
                        st.UOM = item2.StorageUOM;
                        st.Product = item2.Product;
                        st.Reference = item.Series + " Reversal";
                        ObjectSpace.CommitChanges();
                        if (item2?.LotExpiry != null)
                        {
                            st.Lot.ExpirationDate = item2.LotExpiry;
                        }
                        st.Lot.UpdateStockOnHand(true);
                    }
                    item.Status = Receiving.StatusEnum.Cancelled;
                    //Set Employee who handled
                    item.ProcessedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
                    //Save
                    item.Save();
                    ObjectSpace.CommitChanges();
                }

                else if (item.Status == Receiving.StatusEnum.New)
                {
                    item.Status = Receiving.StatusEnum.Cancelled;
                    item.Save();
                }

                ObjectSpace.CommitChanges();
            }
        }

       
    }
}
