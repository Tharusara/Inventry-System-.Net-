using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class ItemGridDOM
    {
        public long Item_ID { get; set; }
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }
        public decimal Item_Price { get; set; }
        public decimal Item_Qty { get; set; }
        public decimal Item_Value { get; set; }

    }
}