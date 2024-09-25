using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using JolliantProd.Module.BusinessObjects;

namespace JolliantProd.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AuditController : ViewController
    {
        public AuditController()
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

        private void GenerateAuditEntriesSimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //var thisView = (AuditUsageReport)View.CurrentObject;
            //ObjectSpace.Delete(thisView.AuditUsageLines);
            //ObjectSpace.CommitChanges();
            //thisView.Save();

            //var myList = ObjectSpace.GetObjects<AuditDataItemPersistent>()
            //    .Where(x => x.ModifiedOn >= thisView.FromDate && x.ModifiedOn < thisView.ToDate.AddDays(1))
            //    .Where(x => x.OperationType == "ObjectCreated" || x.OperationType == "ObjectChanged");

            //Debug.Print(myList.Count().ToString());

            //foreach (var item in myList)
            //{
            //    var x = ObjectSpace.CreateObject<AuditUsageLine>();
            //    x.AuditUsageReport = thisView;
            //    x.ADIP = item;
            //    thisView.AuditUsageLines.Add(x);
            //}

            //ObjectSpace.CommitChanges();
            //thisView.Save();
            //thisView.Reload();
            //ObjectSpace.Refresh();

        }
    }
}
