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
        }

        private void ApprovePOAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((PurchaseOrder)View.CurrentObject).Status = PurchaseOrder.StatusEnum.Approved;
            ((PurchaseOrder)View.CurrentObject).ApprovedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            ((PurchaseOrder)View.CurrentObject).Save();
        }

        private void CancelPOAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((PurchaseOrder)View.CurrentObject).Status = PurchaseOrder.StatusEnum.Declined;
            ((PurchaseOrder)View.CurrentObject).ApprovedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            ((PurchaseOrder)View.CurrentObject).Save();
        }
    }
}
