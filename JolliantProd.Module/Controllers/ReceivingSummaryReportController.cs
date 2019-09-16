using System;
using System.Collections.Generic;
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
    public partial class ReceivingSummaryReportController : ViewController
    {
        public ReceivingSummaryReportController()
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

        private void GenerateReceivingSummaryAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((ReceivingReportSummary)View.CurrentObject);
            ObjectSpace.Delete(thisView.RRSummaryLines);
            thisView.Save();
            ObjectSpace.CommitChanges();

            var myList = ObjectSpace.GetObjects<ReceivedLine>().Where(x => x.Receiving.ActualDeliveryDate >= thisView.From)
                .Where(x => x.Receiving.ActualDeliveryDate <= thisView.To)
                .Where(x => x.PurchaseQuantityReceived > 0)
                .Where(x => x.Receiving.Status == Receiving.StatusEnum.Validated);


            foreach (var item in myList)
            {
                var myItem = ObjectSpace.CreateObject<RRSumarryLine>();
                myItem.ReceivingReportSummary = thisView;
                myItem.ReceivedLine = item;
            }

            thisView.Save();
            ObjectSpace.CommitChanges();

            thisView.RRSummaryLines.Reload();
            thisView.Reload();
            
            View.Refresh();
        }
    }
}
