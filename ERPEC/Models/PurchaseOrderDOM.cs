using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class PurchaseOrderDOM
    {
        //PurchaseOrder_H
        public long PO_H_ID { get; set; }
        public string PO_H_Code { get; set; }
        public DateTime PO_H_Date { get; set; }
        public decimal PO_H_Amount { get; set; }
        public int PO_H_Location { get; set; }
        public long PO_H_Supplier { get; set; }
        public int PO_H_User { get; set; }
        public bool PO_H_isComplete { get; set; }


        //PurchaseOrder_D
        public long PO_D_ID { get; set; }
        public long PO_D_H_ID { get; set; }
        public long PO_D_Item { get; set; }
        public decimal PO_D_Cost { get; set; }
        public decimal PO_D_Qty { get; set; }

        public List<ItemGridDOM> Items { get; set; }


    }
}