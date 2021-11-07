using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models.Reports
{
    public class BINCARDDOM
    {
        public long SMT_ID { get; set; }
        public long SMT_Item { get; set; }
        public string ITM_Code { get; set; }
        public string ITM_Name { get; set; }
        public string SMT_Doc { get; set; }
        public decimal SMT_Qty { get; set; }
        public int Location { get; set; }
        public DateTime Trans_Date { get; set; }
    }
}