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
    public class Invoice_H_DAL
    {
        public InvoiceDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, InvoiceDOM Invoice_H)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }
            decimal tot = 0;
            Invoice_H.Items.ToList().ForEach(x =>
            {
                tot += x.Item_Value;
            });

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@INV_H_Code", "");
            SQLparams.Add("@INV_H_Date", System.DateTime.Now.Date);
            SQLparams.Add("@INV_H_Location", Invoice_H.INV_Location);
            SQLparams.Add("@INV_H_Amount", tot);
            SQLparams.Add("@INV_H_User", 1);

            string Query = @"SELECT * FROM Invoice_H WHERE INV_H_Id = " + Invoice_H.INV_HID;
            DataTable DT_InvoiceH = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (DT_InvoiceH != null && DT_InvoiceH.Rows.Count > 0)
            {
                SQLparams.Add("@INV_H_Code", Invoice_H.INV_Code);
                Query = Common.Functions.QueryBuilder.BuildUpdate("Invoice_H", "INV_H_Id", Invoice_H.INV_HID, SQLparams);

                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {

                DataTable code_rec = DBCon.GetData(SqlCon, SqlTran, "SELECT DOC_LastNo FROM Documnets WHERE DOC_id=3", null);
                int last_no = code_rec.Rows[0].Field<int>("DOC_LastNo") + 1;
                string code = "INV" + last_no.ToString().PadLeft(7, '0');
                SQLparams["@INV_H_Code"] = code;

                Query = Common.Functions.QueryBuilder.BuildInsert("Invoice_H", "INV_H_Id", SQLparams);
                Invoice_H.INV_HID = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
                Invoice_H.INV_Code = code;
                Invoice_H.INV_Date = (DateTime)SQLparams["@INV_H_Date"];

                string Update = "UPDATE Documnets SET DOC_LastNo= " + last_no + " WHERE DOC_id=3";
                DBCon.ExecuteNonQuery(SqlCon, SqlTran, Update, null);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return Invoice_H;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long INV_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@INV_H_Id", INV_ID);

            string Query = @"DELETE FROM Invoice_H WHERE INV_H_Id = @INV_H_Id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }
    }
}