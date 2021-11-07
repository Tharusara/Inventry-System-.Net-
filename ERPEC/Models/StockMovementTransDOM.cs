using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class StockMovementTransDOM
    {
        public long SMT_ID { get; set; }
        public long SMT_Item { get; set; }
        public decimal SMT_Qty { get; set; }
        public int SMT_Location { get; set; }
        public int SMT_Type { get; set; }
        public long SMT_Doc { get; set; }
        public string SMT_Doc_No { get; set; }
    }
}