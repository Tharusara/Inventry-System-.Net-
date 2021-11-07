using ERPEC.Common;
using ERPEC.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPEC.DAL.Reports
{
    public class BINCARDDAL
    {
        public List<BINCARDDOM> getBinCardData(SqlConnection SqlCon, SqlTransaction SqlTran,long Item,int Location,DateTime fromdate,DateTime todate)
        {

            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<BINCARDDOM> BCR = new List<BINCARDDOM>();

            string Query = @"SELECT SMTR.*,IM.ITEM_code,IM.ITEM_name FROM StockMovement_Trans SMTR " +
                             "JOIN ItemMaster IM ON SMTR.SMT_Item=IM.ITEM_id " +
                             "WHERE SMTR.SMT_Item=" + Item + " AND SMT_Location=" + Location+
                             " AND SMTR.SMT_trans_date BETWEEN '"+fromdate+"' AND '"+todate+"'";
            DataTable DT_BCR = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_BCR != null && DT_BCR.Rows.Count > 0)
            {
                DT_BCR.AsEnumerable().ToList().ForEach(r =>
                    {
                        BCR.Add(new BINCARDDOM() {
                            SMT_ID = r.Field<long>("SMT_ID"),
                            SMT_Item = r.Field<long>("SMT_Item"),
                            ITM_Name = r.Field<string>("ITEM_name"),
                            ITM_Code = r.Field<string>("ITEM_code"),
                            SMT_Qty = r.Field<decimal>("SMT_Qty"),
                            SMT_Doc = r.Field<string>("SMT_doc_code"),
                            Location = r.Field<int>("SMT_Location"),
                            Trans_Date = r.Field<DateTime>("SMT_trans_date"),
                        });
                    });
            }
                
            return BCR.OrderBy(x=>x.Trans_Date).ToList();
        }
    }
}