using ERPEC.Common;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPEC.DAL
{
    public class ReOrder_D_DAL
    {
        public ReOrderDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, ReOrderDOM ROD_D)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            string Query = "";
            ROD_D.Items.ToList().ForEach(itm =>
            {
                Dictionary<string, object> SQLparams = new Dictionary<string, object>();

                SQLparams.Add("@RO_D_H", ROD_D.RO_ID);
                SQLparams.Add("@RO_D_Item", itm.Item_ID);
                SQLparams.Add("@RO_D_Qty", itm.Item_Qty);

                Query = Common.Functions.QueryBuilder.BuildInsert("Reorder_D", "RO_D_Id", SQLparams);
                ROD_D.ROD_ID = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            });



            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return ROD_D;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long RO_H_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@RO_D_H", RO_H_ID);

            string Query = @"DELETE FROM Reorder_D WHERE RO_D_H = @RO_D_H";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }
    }
}