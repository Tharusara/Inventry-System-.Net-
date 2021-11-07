using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class StockMovementDOM
    {
        //StockMovement
        public long SM_Id { get; set; }
        public int SM_Type { get; set; }
        public long SM_Item { get; set; }
        public long SM_Location { get; set; }
        public DateTime SM_Date { get; set; }
        public decimal SM_Bal { get; set; }

        //StockMovementTypes
        public int SMT_Id { get; set; }
        public string SMT_Code { get; set; }
        public string SMT_Description { get; set; }
        public bool SMT_StockPlus { get; set; }
    }
}