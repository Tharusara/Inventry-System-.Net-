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
    public class STOCKDAL
    {
        public List<STOCKDOM> getStockData(SqlConnection SqlCon, SqlTransaction SqlTran,int? Location)
        {

            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<STOCKDOM> SDR = new List<STOCKDOM>();

            string Query = @"SELECT SM.SM_Item,IM.ITEM_code,IM.ITEM_name,LM.LOC_name,SM.SM_Bal FROM StockMovement SM " +
                             "JOIN ItemMaster IM ON SM.SM_Item=IM.ITEM_id " +
                             "JOIN LocationMaster LM ON SM.SM_Location=LM.LOC_id " +
                             "WHERE SM.SM_Location=" + Location;

            DataTable DT_SDR = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_SDR != null && DT_SDR.Rows.Count > 0)
            {
                DT_SDR.AsEnumerable().ToList().ForEach(r =>
                        {
                            if (!SDR.Any(x => x.Item_ID == r.Field<long>("SM_Item")))
                            {
                                SDR.Add(new STOCKDOM()
                                {
                                    Item_ID = r.Field<long>("SM_Item"),
                                    Item_Code = r.Field<string>("ITEM_code"),
                                    Item_Name = r.Field<string>("ITEM_name"),
                                    Location = r.Field<string>("LOC_name"),
                                    Qty = r.Field<decimal>("SM_Bal"),
                                });
                            }
                            else
                            {
                                STOCKDOM Rec = SDR.Where(x => x.Item_ID == r.Field<long>("SM_Item")).FirstOrDefault();
                                Rec.Qty += r.Field<decimal>("SM_Bal");
                            }
                          
                        });
            }
            return SDR;
        }

    }
}