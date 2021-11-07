using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class ItemMasterDOM
    {
        public long ITEM_id { get; set; }
        public string ITEM_code { get; set; }
        public string ITEM_name { get; set; }
        public int ITEM_cat { get; set; }
        public decimal ITEM_price { get; set; }
        public decimal ITEM_cost { get; set; }
        public decimal ITEM_minqty { get; set; }
        public bool ITEM_active { get; set; }
        public string ITEM_activechq { get; set; }

    }
}