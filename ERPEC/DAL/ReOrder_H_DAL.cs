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
    public class ReOrder_H_DAL
    {
        public ReOrderDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, ReOrderDOM RO_H)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            decimal tot = 0;
            RO_H.Items.ToList().ForEach(x =>
            {
                tot += x.Item_Value;
            });

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@RO_Code", "");
            SQLparams.Add("@RO_Date", System.DateTime.Now.Date);
            SQLparams.Add("@RO_Location", RO_H.RO_Location);
            SQLparams.Add("@RO_Amount", tot);

            string Query = @"SELECT * FROM ReOrder_H WHERE RO_ID = " + RO_H.RO_ID;
            DataTable DT_RO_H = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (DT_RO_H != null && DT_RO_H.Rows.Count > 0)
            {
                SQLparams.Add("@RO_Code", RO_H.RO_Code);
                Query = Common.Functions.QueryBuilder.BuildUpdate("ReOrder_H", "RO_ID", RO_H.RO_ID, SQLparams);

                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {

                DataTable code_rec = DBCon.GetData(SqlCon, SqlTran, "SELECT DOC_LastNo FROM Documnets WHERE DOC_id=5", null);
                int last_no = code_rec.Rows[0].Field<int>("DOC_LastNo") + 1;
                string code = "RO" + last_no.ToString().PadLeft(7, '0');
                SQLparams["@RO_Code"] = code;

                Query = Common.Functions.QueryBuilder.BuildInsert("ReOrder_H", "RO_ID", SQLparams);
                RO_H.RO_ID = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
                RO_H.RO_Code = code;
                RO_H.RO_Date = (DateTime)SQLparams["@RO_Date"];

                string Update = "UPDATE Documnets SET DOC_LastNo= " + last_no + " WHERE DOC_id=5";
                DBCon.ExecuteNonQuery(SqlCon, SqlTran, Update, null);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return RO_H;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long RO_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@RO_ID", RO_ID);

            string Query = @"DELETE FROM ReOrder_H WHERE RO_ID = @RO_ID";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

    }
}