using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models.Reports
{
    public class STOCKDOM
    {
        public long Item_ID { get; set; }
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }
        public string Location { get; set; }
        public decimal Qty { get; set; }
    }
}