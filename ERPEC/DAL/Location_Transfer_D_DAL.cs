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
    public class Location_Transfer_D_DAL
    {
        public LocationTransferDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, LocationTransferDOM LT_D)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            string Query = "";
            LT_D.Items.ToList().ForEach(itm =>
            {
                Dictionary<string, object> SQLparams = new Dictionary<string, object>();

                SQLparams.Add("@TRANS_D_H_id", LT_D.TRANS_H_Id);
                SQLparams.Add("@TRANS_D_item", itm.Item_ID);
                SQLparams.Add("@TRANS_D_price", itm.Item_Price);
                SQLparams.Add("@TRANS_D_qty", itm.Item_Qty);


                Query = Common.Functions.QueryBuilder.BuildInsert("Location_Transfer_D", "TRANS_D_id", SQLparams);
                LT_D.TRANS_D_Id = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            });



            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return LT_D;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long LT_H_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@TRANS_D_H_id", LT_H_ID);

            string Query = @"DELETE FROM Location_Transfer_D WHERE TRANS_D_H_id = @TRANS_D_H_id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        
    }
}