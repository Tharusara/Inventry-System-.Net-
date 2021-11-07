using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class GRNDOM
    {
        //GRN_H
        public long GRN_H_Id { get; set; }
        public string GRN_H_Code { get; set; }
        public DateTime GRN_H_Date { get; set; }
        public decimal GRN_H_Amount { get; set; }
        public long GRN_H_Supplier { get; set; }
        public int GRN_H_Location { get; set; }
        public long? GRN_H_Source { get; set; }
        public int GRN_H_User { get; set; }

        //GRN_D

        public long GRN_D_Id { get; set; }
        public long GRN_D_H_id { get; set; }
        public long GRN_D_Item { get; set; }
        public decimal GRN_D_ItemCost { get; set; }
        public decimal GRN_D_Qty { get; set; }

        //public ItemGridDOM[] Mylist { get; set; }
        public List<ItemGridDOM> Items { get; set; }
    }
}