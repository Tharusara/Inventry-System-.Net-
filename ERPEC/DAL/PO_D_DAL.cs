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
    public class PO_D_DAL
    {
        public PurchaseOrderDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, PurchaseOrderDOM PO_D)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }
            string Query = "";

            PO_D.Items.ForEach(itm => {
                Dictionary<string, object> SQLparams = new Dictionary<string, object>();

                SQLparams.Add("@PO_D_h_id", PO_D.PO_H_ID);
                SQLparams.Add("@PO_D_item", itm.Item_ID);
                SQLparams.Add("@PO_D_cost", itm.Item_Price);
                SQLparams.Add("@PO_D_qty", itm.Item_Qty);

                Query = Common.Functions.QueryBuilder.BuildInsert("dbo.PurchaseOrder_D", "PO_D_id", SQLparams);
                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            });
            


            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return PO_D;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long PO_H_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@PO_D_h_id", PO_H_ID);

            string Query = @"DELETE FROM PurchaseOrder_D WHERE PO_D_h_id = @PO_D_h_id";
            DBCon.ExecuteNonQuery(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

    }
}