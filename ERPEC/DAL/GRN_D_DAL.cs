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
    public class GRN_D_DAL
    {
        public GRNDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, GRNDOM GRN_D)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            string Query = "";
            GRN_D.Items.ToList().ForEach(itm => {
                Dictionary<string, object> SQLparams = new Dictionary<string, object>();

                SQLparams.Add("@GRN_D_h_id", GRN_D.GRN_H_Id);
                SQLparams.Add("@GRN_D_item_id", itm.Item_ID);
                SQLparams.Add("@GRN_D_item_cost", itm.Item_Price);
                SQLparams.Add("@GRN_D_item_qty", itm.Item_Qty);


                Query = Common.Functions.QueryBuilder.BuildInsert("dbo.GRN_D", "GRN_D_id", SQLparams);
                GRN_D.GRN_D_Id = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            });
           


            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return GRN_D;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long GRN_H_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@GRN_D_h_id", GRN_H_ID);

            string Query = @"DELETE FROM GRN_D WHERE GRN_H_id = @GRN_D_h_id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        



    }
}