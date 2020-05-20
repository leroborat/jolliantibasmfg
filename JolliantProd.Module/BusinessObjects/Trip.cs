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
using AggregatedAttribute = DevExpress.Xpo.AggregatedAttribute;

namespace JolliantProd.Module.BusinessObjects
{
    //RuleCombinationOfPropertiesIsUnique("RuleUniqueTripSO", DefaultContexts.Save, "SalesOrder, TripNumber")
    [DefaultClassOptions]
    public class Trip : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Trip(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TripDateTime = DateTime.Now;
            XPQuery<WarehouseLocation> partnerLocations = new XPQuery<WarehouseLocation>(Session);
            var list = from c in partnerLocations
                        where c.LocationType == WarehouseLocation.LocationTypeEnum.CustomerLocation
                        select c;

            if (list != null)
            {
                DestinationLocation = list.First<WarehouseLocation>();
            }

            Encoder = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;


        }



        WarehouseLocation sourceLocation;
        Invoice invoice;
        string truckPlateNumber;
        string truckDriver;
        string encoder;
        [Persistent(nameof(QuantityDelivered))]
        double quantityDelivered;
        WarehouseLocation destinationLocation;
        TripStatusEnum tripStatus;
        [Persistent(nameof(PONumber))]
        string pONumber;
        [Persistent(nameof(DisplayName))]
        string displayName;
        DateTime tripDateTime;
        int tripNumber;
        SalesOrder salesOrder;

        [Association("SalesOrder-Trips"), RuleRequiredField()]
        public SalesOrder SalesOrder
        {
            get => salesOrder;
            set
            {
                SetPropertyValue(nameof(SalesOrder), ref salesOrder, value);

            }
        }


        [PersistentAlias(nameof(displayName))]
        public string DisplayName
        {
            get
            {
                if (!IsLoading && !IsSaving)
                {
                    try
                    {
                        if (PONumber != null)
                        {
                            displayName = "Trip# " + Convert.ToString(TripNumber) + " - " + TripDateTime.ToShortDateString() + " for PO# " + PONumber;
                        }
                    }
                    catch (Exception)
                    {


                    }

                }


                return displayName;
            }
        }


        [PersistentAlias(nameof(pONumber))]
        public string PONumber
        {
            get
            {
                pONumber = SalesOrder.PurchaseOrderNumber;
                return pONumber;
            }
        }


        [RuleRequiredField()]
        public int TripNumber
        {
            get => tripNumber;
            set => SetPropertyValue(nameof(TripNumber), ref tripNumber, value);
        }

        [RuleRequiredField()]
        public DateTime TripDateTime
        {
            get => tripDateTime;
            set => SetPropertyValue(nameof(TripDateTime), ref tripDateTime, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TruckDriver
        {
            get => truckDriver;
            set => SetPropertyValue(nameof(TruckDriver), ref truckDriver, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TruckPlateNumber
        {
            get => truckPlateNumber;
            set => SetPropertyValue(nameof(TruckPlateNumber), ref truckPlateNumber, value);
        }


        [PersistentAlias(nameof(quantityDelivered))]
        public double QuantityDelivered
        {
            get
            {
                quantityDelivered = 0;
                foreach (TripLine item in TripLines)
                {
                    quantityDelivered += item.QuantityDone;
                }
                return quantityDelivered;
            }
        }


        [Association("Trip-TripLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<TripLine> TripLines
        {
            get
            {
                return GetCollection<TripLine>(nameof(TripLines));
            }
        }

        public enum TripStatusEnum
        {
            Draft,
            Validated
        }

        
        [RuleRequiredField()]
        public WarehouseLocation SourceLocation
        {
            get => sourceLocation;
            set => SetPropertyValue(nameof(SourceLocation), ref sourceLocation, value);
        }


        [RuleRequiredField()]
        public WarehouseLocation DestinationLocation
        {
            get => destinationLocation;
            set => SetPropertyValue(nameof(DestinationLocation), ref destinationLocation, value);
        }

        public TripStatusEnum TripStatus
        {
            get => tripStatus;
            set => SetPropertyValue(nameof(TripStatus), ref tripStatus, value);
        }



        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Encoder
        {
            get => encoder;
            set => SetPropertyValue(nameof(Encoder), ref encoder, value);
        }

        
        [Association("Invoice-Trips")]
        public Invoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        [Action(Caption = "Generate Assigned Lots", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void ActionMethod()
        {
            //            For each product where Quantity < Quantity Delivered

            if (SalesOrder != null)
            {
                var UnfulfilledOrders = SalesOrder.SalesOrderLines.Where(x => x.QuantityDelivered < x.Quantity);
                foreach (var item in UnfulfilledOrders)
                {

                    TripLine tl = new TripLine(Session);
                    tl.Trip = this;
                    tl.Product = item.Product;
                    tl.Location = this.SourceLocation;
                    

                    var query = new XPQuery<Lot>(Session).Where(x => x.StockOnHand >= 1 && x.Product == tl.Product)
                        .OrderBy(x => x.ExpirationDate);

                    double productcount = 0;

                    foreach (var lot in query)
                    {
                        TripLineDetail tld = new TripLineDetail(Session);
                        tld.TripLine = tl;
                        tld.LotCode = lot;
                        tld.From = tl.Location;
                        if (tld.AvailableQuantity <= 0)
                        {
                            Session.Delete(tld);
                            continue;
                        }

                        Session.Save(tld);
                        Session.Save(tl);
                        Session.Save(this);

                        if (tl.PendingDemand < tld.AvailableQuantity)
                        {
                            tld.QuantityDone = tl.PendingDemand;
                            
                        } else
                        {
                            tld.QuantityDone = tld.AvailableQuantity;
                        }
                        
                        productcount += tld.QuantityDone;

                        if (productcount >= tl.PendingDemand)
                        {
                            break;
                        }

                      
                    }

                }
            }
                        //Create Trip Line
                        //Set Product
                        //Set Location

                        //Select Lots Available Quantity > 0
                        //Max of Quantity Ordered
                        //Sort by Expiration Date

                        //Create Trip Detail
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            //this.PersistentProperty = "Paid";
        }

    }

    public class TripLine : BaseObject
    {
        public TripLine(Session session) : base(session)
        { }

        
        [Association("Trip-TripLines")]
        public Trip Trip
        {
            get => trip;
            set => SetPropertyValue(nameof(Trip), ref trip, value);
        }

        WarehouseLocation location;
        [Persistent(nameof(QuantityDone))]
        double quantityDone;
        [Persistent(nameof(PendingDemand))]
        double pendingDemand;

        Trip trip;
        Product product;
        [RuleRequiredField()]


        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        
        public WarehouseLocation Location
        {
            get => location;
            set => SetPropertyValue(nameof(Location), ref location, value);
        }


        [PersistentAlias(nameof(pendingDemand))]
        public double PendingDemand
        {
            get
            {
                double orders = 0;
                foreach (SalesOrderLine item in Trip.SalesOrder.SalesOrderLines)
                {
                    if (item.Product == Product)
                    {
                        orders += item.Quantity;
                    }
                }

                double confirmeddels = 0;

                foreach (Trip item in Trip.SalesOrder.Trips)
                {
                    if (item.TripStatus == Trip.TripStatusEnum.Validated)
                    {
                        foreach (TripLine tripLine in item.TripLines)
                        {
                            if (tripLine.Product == Product)
                            {
                                confirmeddels += tripLine.QuantityDone;
                            }
                        }
                    }
                }

                double currentTripPending = 0;
                if (this.Trip.TripStatus == Trip.TripStatusEnum.Draft)
                {
                    foreach (TripLine item in Trip.TripLines)
                    {
                        if (item.Product == Product)
                        {
                            currentTripPending += item.QuantityDone;
                        }
                    }

                }

                var returns = (from c in Trip.SalesOrder.SalesOrderLines
                               where c.Product == Product
                               select c.QuantityReturned).Sum();

                pendingDemand = orders - confirmeddels - currentTripPending + returns;

                return pendingDemand;
            }
        }

        
        [PersistentAlias(nameof(quantityDone))]
        public double QuantityDone
        {
            get {
                quantityDone = (from c in TripLineDetails
                                select c.QuantityDone).Sum();
                return quantityDone; }
        }
     
        [Association("TripLine-TripLineDetails"), Aggregated()]
        public XPCollection<TripLineDetail> TripLineDetails
        {
            get
            {
                return GetCollection<TripLineDetail>(nameof(TripLineDetails));
            }
        }

        [Association("TripLine-Lots")]
        public XPCollection<Lot> Lots
        {
            get
            {
                return GetCollection<Lot>(nameof(Lots));
            }
        }
    }

    [RuleCombinationOfPropertiesIsUnique("UniqueLot", DefaultContexts.Save, "LotCode, TripLine")]
    public class TripLineDetail : BaseObject
    {
        public TripLineDetail(Session session) : base(session)
        { }

        WarehouseLocation from;
        double quantityDone;
        [Persistent(nameof(AvailableQuantity))]
        double availableQuantity;
        Lot lotCode;
        TripLine tripLine;

        [Association("TripLine-TripLineDetails")]
        public TripLine TripLine
        {
            get => tripLine;
            set => SetPropertyValue(nameof(TripLine), ref tripLine, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation From
        {
            get => from;
            set => SetPropertyValue(nameof(From), ref from, value);
        }

        [RuleRequiredField()]
        public Lot LotCode
        {
            get => lotCode;
            set => SetPropertyValue(nameof(LotCode), ref lotCode, value);
        }



        [PersistentAlias(nameof(availableQuantity))]
        public double AvailableQuantity
        {
            get {
                if (TripLine != null)
                {
                    if (TripLine.Product.Tracking == Product.TrackingEnum.TrackByLot ||
                        TripLine.Product.Tracking == Product.TrackingEnum.TrackBySerial)
                    {
                        if (LotCode != null && From != null)
                        {
                            var smoves = new XPQuery<StockTransfer>(Session);
                            var TotalIn = (from a in smoves
                                           where a.DestinationLocation == From &&
                                           a.Lot == LotCode
                                           select a.Quantity).Sum();

                            var TotalOut = (from a in smoves
                                            where a.SourceLocation == From &&
                                            a.Lot == LotCode
                                            select a.Quantity).Sum();
                            availableQuantity = TotalIn - TotalOut;
                        }
                    } else
                    {
                        var smoves = new XPQuery<StockTransfer>(Session);
                        var TotalIn = (from a in smoves
                                       where a.DestinationLocation == From
                                       select a.Quantity).Sum();

                        var TotalOut = (from a in smoves
                                        where a.SourceLocation == From 
                                        select a.Quantity).Sum();
                        availableQuantity = TotalIn - TotalOut;
                    }
                }
                return availableQuantity; }
        }


        [RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.LessThanOrEqual,
    "AvailableQuantity", ParametersMode.Expression)]
        public double QuantityDone
        {
            get => quantityDone;
            set => SetPropertyValue(nameof(QuantityDone), ref quantityDone, value);
        }


    }

}