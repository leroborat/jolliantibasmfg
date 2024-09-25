using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProductionRoute : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProductionRoute(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        string routeName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RouteName
        {
            get => routeName;
            set => SetPropertyValue(nameof(RouteName), ref routeName, value);
        }

        [Association("ProductionRoute-RouteOperations"), DevExpress.Xpo.Aggregated()]
        public XPCollection<RouteOperation> RouteOperations
        {
            get
            {
                return GetCollection<RouteOperation>(nameof(RouteOperations));
            }
        }
    }

    [RuleCombinationOfPropertiesIsUnique("RouteOpSequence", DefaultContexts.Save, "Name, Sequence")]
    public class RouteOperation : BaseObject
    {
        public RouteOperation(Session session) : base(session)
        { }


        ProductionRoute productionRoute;
        int sequence;
        string description;
        int idealDurationInMinutes;
        WorkCenter workCenter;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }


        public int Sequence
        {
            get => sequence;
            set => SetPropertyValue(nameof(Sequence), ref sequence, value);
        }


        public WorkCenter WorkCenter
        {
            get => workCenter;
            set => SetPropertyValue(nameof(WorkCenter), ref workCenter, value);
        }


        public int IdealDurationInMinutes
        {
            get => idealDurationInMinutes;
            set => SetPropertyValue(nameof(IdealDurationInMinutes), ref idealDurationInMinutes, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        
        [Association("ProductionRoute-RouteOperations")]
        public ProductionRoute ProductionRoute
        {
            get => productionRoute;
            set => SetPropertyValue(nameof(ProductionRoute), ref productionRoute, value);
        }


    }
}