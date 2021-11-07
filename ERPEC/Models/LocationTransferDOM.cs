using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class LocationTransferDOM
    {
        //Location_Transfer_H
        public long TRANS_H_Id { get; set; }
        public string TRANS_H_Code { get; set; }
        public DateTime TRANS_H_Date { get; set; }
        public int TRANS_H_From { get; set; }
        public int TRANS_H_To { get; set; }
        public decimal TRANS_H_Value { get; set; }
        public string TRANS_H_Narration { get; set; }
        public int TRANS_H_User { get; set; }

        //Location_Transfer_D
        public long TRANS_D_Id { get; set; }
        public long TRANS_D_H_Id { get; set; }
        public long TRANS_D_Item { get; set; }
        public decimal TRANS_D_price { get; set; }
        public decimal TRANS_D_Qty { get; set; }

        public List<ItemGridDOM> Items { get; set; }

    }
}