﻿using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.ReportsV2;
using JolliantProd.Module.Reports;
using JolliantProd.Module.BusinessObjects;

namespace JolliantProd.Module {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class JolliantProdModule : ModuleBase {
        public JolliantProdModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            PredefinedReportsUpdater predefinedReportsUpdater = new PredefinedReportsUpdater(Application, objectSpace, versionFromDB);
            predefinedReportsUpdater.AddPredefinedReport<XtraReport2>("PO Summary Report", typeof(SalesOrderLine));
            predefinedReportsUpdater.AddPredefinedReport<XtraReport1>("JRC PJJ PJT PO Summary Report", typeof(SalesOrderLine));
            predefinedReportsUpdater.AddPredefinedReport<VendorReport>("Vendor List", typeof(Vendor));
            predefinedReportsUpdater.AddPredefinedReport<LotReport>("Purchaseable Products Lot Report", typeof(Lot));
            predefinedReportsUpdater.AddPredefinedReport<POBalanceReport>("PO Balance Report", typeof(PurchaseOrderLine));
            predefinedReportsUpdater.AddPredefinedReport<PurchasePriceHistoryReport>("PO Price History Report", typeof(PurchaseOrderLine));
            predefinedReportsUpdater.AddPredefinedReport<LotAgeWithdrawal>("Lot Aeging in Withdrawals Report", typeof(WithdrawalLineLot));
            predefinedReportsUpdater.AddPredefinedReport<Reports.FGInventoryReport>("FG Inventory Report", typeof(StockTransfer));
            predefinedReportsUpdater.AddPredefinedReport<InventoryByLocation>("Inventory By Location", typeof(StockTransfer));
            return new ModuleUpdater[] { updater, predefinedReportsUpdater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            application.LinkNewObjectToParentImmediately = true;
            
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
