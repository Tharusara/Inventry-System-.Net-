using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class InvoiceDOM
    {

        //Invoice_H
        public long INV_HID { get; set; }
        public string INV_Code { get; set; }
        public DateTime INV_Date { get; set; }
        public decimal INV_Value { get; set; }
        public int INV_Location { get; set; }
        public int INV_H_User { get; set; }

        //Invoice_D
        public long INV_D_ID { get; set; }
        public long INV_D_H_Id { get; set; }
        public long INV_D_Item_ID { get; set; }
        public decimal INV_D_Qty { get; set; }
        public decimal INV_D_ItemRate { get; set; }

        public List<ItemGridDOM> Items { get; set; }

    }
}