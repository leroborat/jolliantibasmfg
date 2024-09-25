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
    public class WastagesDisposal : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public WastagesDisposal(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DateReported = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string remarks;
        string nameOfEmployees;
        ChargedToEnum chargedTo;
        TypeEnum type;
        decimal amount;
        decimal unitPrice;
        UnitOfMeasure uOM;
        double quantity;
        string productCode;
        Product product;
        string gPNo;
        DateTime dateReported;

        public DateTime DateReported
        {
            get => dateReported;
            set => SetPropertyValue(nameof(DateReported), ref dateReported, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string GPNo
        {
            get => gPNo;
            set => SetPropertyValue(nameof(GPNo), ref gPNo, value);
        }


        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set
            {
                SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    UOM = Product.ProductionUOM;
                    ProductCode = Product.InternalReference;
                    UnitPrice = Product.Cost;
                    if (Product.CanBeSold)
                    {
                        Type = TypeEnum.FG;
                    }
                    else
                    {
                        Type = TypeEnum.WIP;
                    }
                }
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductCode
        {
            get => productCode;
            set => SetPropertyValue(nameof(ProductCode), ref productCode, value);
        }


        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }


        public UnitOfMeasure UOM
        {
            get => uOM;
            set => SetPropertyValue(nameof(UOM), ref uOM, value);
        }


        public decimal UnitPrice
        {
            get => unitPrice;
            set
            {
                SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    Amount = UnitPrice * Convert.ToDecimal(Quantity);
                }
            }
        }


        public decimal Amount
        {
            get => amount;
            set => SetPropertyValue(nameof(Amount), ref amount, value);
        }

        public enum TypeEnum
        {
            WIP,
            FG
        }


        public TypeEnum Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }

        public enum ChargedToEnum
        {
            Company,
            Employee
        }

        [RuleRequiredField()]
        public ChargedToEnum ChargedTo
        {
            get => chargedTo;
            set => SetPropertyValue(nameof(ChargedTo), ref chargedTo, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NameOfEmployees
        {
            get => nameOfEmployees;
            set => SetPropertyValue(nameof(NameOfEmployees), ref nameOfEmployees, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Remarks
        {
            get => remarks;
            set => SetPropertyValue(nameof(Remarks), ref remarks, value);
        }

    }
}