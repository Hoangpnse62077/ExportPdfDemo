using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPDF
{
    /// <summary>
    /// PurchaseOrderExportInMailModel
    /// </summary>
    public class PurchaseOrderExportInMailModel
    {
        /// <summary>
        /// PoNumber
        /// </summary>
        public string PoNumber { get; set; }

        /// <summary>
        /// PoDateOnUtc
        /// </summary>
        public string PoDateOnUtc { get; set; }

        /// <summary>
        /// DeliveryDateOnUtc
        /// </summary>
        public string DeliveryDateOnUtc { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Terms
        /// </summary>
        public string Terms { get; set; }

        /// <summary>
        /// VendorName
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// AddressLineOne
        /// </summary>
        public string AddressLineOne { get; set; }

        /// <summary>
        /// AddressLineTwo
        /// </summary>
        public string AddressLineTwo { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// ZipCode
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// VendorName
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public decimal SubTotal { get; set; }

        public List<PoDetailInMailModel> ListPoDetailInMail { get; set; }

        public PurchaseOrderExportInMailModel(string poNumber, string poDateOnUtc, string deliveryDateOnUtc, string comments,
            string terms, string vendorName, string addressLineOne, string addressLineTwo, string city, 
            string state, string zipCode, string phone, decimal total, decimal subTotal, List<PoDetailInMailModel> listPoDetailInMail)
        {
            PoNumber = poNumber;
            PoDateOnUtc = poDateOnUtc;
            DeliveryDateOnUtc = deliveryDateOnUtc;
            Comments = comments;
            Terms = terms;
            VendorName = vendorName;
            AddressLineOne = addressLineOne;
            AddressLineTwo = addressLineTwo;
            City = city;
            State = state;
            ZipCode = zipCode;
            Phone = phone;
            Total = total;
            SubTotal = subTotal;
            ListPoDetailInMail = listPoDetailInMail;
        }
    }

    /// <summary>
    /// PoDetailInMailModel
    /// </summary>
    public class PoDetailInMailModel
    {
        public PoDetailInMailModel(string sku, string description, int qtyOrder, decimal cost, decimal lineTotal)
        {
            Sku = sku;
            Description = description;
            QtyOrder = qtyOrder;
            Cost = cost;
            LineTotal = lineTotal;
        }

        /// <summary>
        /// Sku
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Quantity Ordered
        /// </summary>
        public int QtyOrder { get; set; }

        /// <summary>
        /// Cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// LineTotal
        /// </summary>
        public decimal LineTotal { get; set; }
    }
}
