using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class ProductionController : ViewController
    {
        public ProductionController()
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

        private void PlanMO_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //Get Ratio
            var thisView = ((ManufacturingOrder)View.CurrentObject);
            var Ratio = thisView.QuantityToProduce / thisView.BillOfMaterial.Quantity;
            thisView.Status = ManufacturingOrder.StatusEnum.Planned;

            foreach (var item in thisView.BillOfMaterial.BomComponents)
            {
                var consumedMaterialLine = ObjectSpace.CreateObject<ConsumedMaterial>();
                consumedMaterialLine.Product = item.Component;
                consumedMaterialLine.UnitOfMeasure = item.ProductionUOM;
                consumedMaterialLine.ToConsume = item.Quantity * Ratio;
                consumedMaterialLine.Consumed = 0;
                thisView.ConsumedMaterials.Add(consumedMaterialLine);
            }

            

            var finishedGood = ObjectSpace.CreateObject<FinishedGood>();
            finishedGood.Product = thisView.Product;
            finishedGood.UnitOfMeasure = thisView.Product.UOM;
            finishedGood.Quantity = thisView.QuantityToProduce;
            thisView.FinishedGoods.Add(finishedGood);

            var routeList = thisView.Routing.RouteOperations.OrderBy(x => x.Sequence).ToList();

            var currentTime = thisView.ProductionStartDateTime;

            foreach (var item in routeList)
            {
                var workOrder = ObjectSpace.CreateObject<WorkOrder>();
                workOrder.PlannedDate = currentTime;
                currentTime = currentTime.AddMinutes(item.IdealDurationInMinutes);
                workOrder.PlannedDateTo = currentTime;
                workOrder.Status = WorkOrder.StatusEnum.Ready;
                workOrder.WorkCenter = item.WorkCenter;
                thisView.WorkOrders.Add(workOrder);
            }

            var withdrawalRequest = ObjectSpace.CreateObject<WithdrawalRequest>();
            withdrawalRequest.FromLocation = thisView.StockLocation;
            withdrawalRequest.ManufacturingOrder = thisView;
            withdrawalRequest.RequestedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            ObjectSpace.CommitChanges();

            foreach (var item in thisView.ConsumedMaterials)
            {
                var wLine = ObjectSpace.CreateObject<WithdrawalRequestLine>();
                wLine.Product = item.Product;
                wLine.Demand = item.ToConsume;
                wLine.ProductionUOM = item.UnitOfMeasure;
                wLine.StockingQuantity = wLine.Demand / wLine.Product.UOMRatioProduction;
                wLine.StockingUOM = wLine.Product.UOM;
                withdrawalRequest.WithdrawalRequestLines.Add(wLine);
            }

            withdrawalRequest.Status = WithdrawalRequest.StatusEnum.InProgress;
            thisView.WithdrawalRequests.Add(withdrawalRequest);
            ObjectSpace.CommitChanges();


            //Create Materials to Consume
            //Create WithdrawalRequests
        }

        private void ValidateProductionTransfer_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            var thisView = ((ProductionTransfer)View.CurrentObject);
            WarehouseLocation warehouseLocation;
            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Production"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Production";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.Production;
                ObjectSpace.CommitChanges();
            }

            foreach (var item in thisView.ProductionTransferLines)
            {
                foreach (var item2 in item.ProductionTransferLineLots)
                {
                    if (item2.StockingQuantity <= 0)
                    {
                        continue;
                    }
                    StockTransfer st = ObjectSpace.CreateObject<StockTransfer>();
                    st.DestinationLocation = warehouseLocation;
                    st.SourceLocation = thisView.Location;

                    if (item2.Lot != null)
                    {
                        st.Lot = item2.Lot;
                    }
                    st.Quantity = item2.StockingQuantity;
                    st.UOM = item.StockingUOM;
                    st.Product = item.Product;
                    st.Reference = "Moved to Production: " + thisView.ProcessedDate;
                    ObjectSpace.CommitChanges();
                    st.Lot.UpdateStockOnHand(true);
                    ObjectSpace.CommitChanges();
                }

            }
            //Set status to Validated
            thisView.Status = ProductionTransfer.StatusEnum.Validated;
            //Set Employee who handled
            thisView.ProcessedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            //Save
            thisView.Save();
            ObjectSpace.CommitChanges();
        }

        private void MarkComplete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((WithdrawalRequest)View.CurrentObject).Status = WithdrawalRequest.StatusEnum.Completed;
            ObjectSpace.CommitChanges();
        }

        private void MarkWorkOrderDone_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((WorkOrder)View.CurrentObject).Status = WorkOrder.StatusEnum.Finished;
            ((WorkOrder)View.CurrentObject).ManufacturingOrder.Status = ManufacturingOrder.StatusEnum.InProgress;
            ObjectSpace.CommitChanges();
        }

        private void MarkProductionCompleted_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((ManufacturingOrder)View.CurrentObject);
            thisView.Status = ManufacturingOrder.StatusEnum.Done;
            thisView.ProcessedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            thisView.FinishedDate = DateTime.Now;
            ObjectSpace.CommitChanges();

            WarehouseLocation warehouseLocation;
            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Production"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Production";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.Production;
                ObjectSpace.CommitChanges();
            }

            foreach (var item in thisView.FinishedGoods)
            {
                StockTransfer st = ObjectSpace.CreateObject<StockTransfer>();
                st.DestinationLocation = thisView.StockLocation;
                st.SourceLocation = warehouseLocation;

                Lot newLot = ObjectSpace.CreateObject<Lot>();
                newLot.Product = item.Product;
                newLot.LotCode = item.LotCode;
                newLot.ExpirationDate = item.ExpirationDate;
                newLot.InternalReference = thisView.SeriesName;
                st.Lot = newLot;
                st.Quantity = item.Quantity;
                st.UOM = item.UnitOfMeasure;
                st.Product = item.Product;
                st.Reference = "Moved FG to Stock: " + thisView.SeriesName;
                ObjectSpace.CommitChanges();
            }
        }
    }
}
