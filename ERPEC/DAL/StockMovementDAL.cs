using ERPEC.Common;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPEC.DAL
{
    public class StockMovementDAL
    {
        public StockMovementDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, StockMovementDOM StockMovement,bool Plus)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@SM_Type", StockMovement.SM_Type);
            SQLparams.Add("@SM_Item", StockMovement.SM_Item);
            SQLparams.Add("@SM_Location", StockMovement.SM_Location);
            SQLparams.Add("@SM_Date", StockMovement.SM_Date);;
            SQLparams.Add("@SM_Qty", StockMovement.SM_Bal);

            string Query = @"SELECT * FROM StockMovement WHERE SM_Item = " + StockMovement.SM_Item;
            Query += " AND SM_Location = " + StockMovement.SM_Location;
            DataTable DT_StockMovement = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (DT_StockMovement != null && DT_StockMovement.Rows.Count > 0)
            {
                string Query2 = @" UPDATE StockMovement SET SM_Bal=";
  
                if (Plus)
                    Query2 += "SM_Bal+" + StockMovement.SM_Bal;
                else
                {

                    decimal balnce = DT_StockMovement.Rows[0].Field<decimal>("SM_Bal") - StockMovement.SM_Bal;
                    balnce = balnce < 0 ? 0 : balnce;

                    Query2 += balnce;
                    //if (balnce == 0)
                    //    Query2 += 0;
                    //else
                    //    Query2 += "SM_Bal-" + StockMovement.SM_Bal;
                }

                Query2 += "  WHERE SM_Item=" + StockMovement.SM_Item + " AND SM_Location=" + StockMovement.SM_Location;
                DBCon.ExecuteNonQuery(SqlCon, SqlTran, Query2,null);
            }

            else
            {
                Query = Common.Functions.QueryBuilder.BuildInsert("StockMovement", "SM_ID", SQLparams);
                StockMovement.SM_Id = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }

            //string Query = Common.Functions.QueryBuilder.BuildInsert("StockMovement", "SM_ID", SQLparams);
            //    StockMovement.SM_Id = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return StockMovement;
        }
    }
}