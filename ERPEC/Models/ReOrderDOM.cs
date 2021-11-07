using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class ReOrderDOM
    {
        //Reorder_H
        public long RO_ID { get; set; }
        public string RO_Code { get; set; }
        public DateTime RO_Date { get; set; }
        public int RO_Location { get; set; }
        public decimal RO_Amount { get; set; }

        //Reorder_D
        public long ROD_ID { get; set; }
        public long ROD_H_ID { get; set; }
        public long ROD_Item { get; set; }
        public decimal ROD_Qty { get; set; }

        public List<ItemGridDOM> Items { get; set; }

    }
}