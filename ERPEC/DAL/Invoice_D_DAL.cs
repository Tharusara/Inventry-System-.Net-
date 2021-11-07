using ERPEC.Common;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPEC.DAL
{
    public class Invoice_D_DAL
    {
        public InvoiceDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, InvoiceDOM Invoice_D)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            string Query = "";
            Invoice_D.Items.ToList().ForEach(itm =>
            {
                Dictionary<string, object> SQLparams = new Dictionary<string, object>();

                SQLparams.Add("@INV_D_H_Id", Invoice_D.INV_HID);
                SQLparams.Add("@INV_Item_Id", itm.Item_ID);
                SQLparams.Add("@INV_Itm_Qty", itm.Item_Qty);
                SQLparams.Add("@INV_Itm_Rate", itm.Item_Price);
                
                Query = Common.Functions.QueryBuilder.BuildInsert("Invoice_D", "INV_D_Id", SQLparams);
                Invoice_D.INV_D_ID = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            });



            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return Invoice_D;
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
            SQLparams.Add("@INV_D_Id", GRN_H_ID);

            string Query = @"DELETE FROM Invoice_D WHERE INV_D_Id = @INV_D_Id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }
    }
}