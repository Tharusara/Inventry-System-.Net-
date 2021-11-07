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
    public class GRN_H_DAL
    {
        public GRNDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, GRNDOM GRN_H)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }
            decimal tot=0;
            GRN_H.Items.ToList().ForEach(x =>
            {
                tot += x.Item_Value;
            });
            
            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@GRN_H_code", "");
            SQLparams.Add("@GRN_H_Date", System.DateTime.Now.Date);
            SQLparams.Add("@GRN_H_amount", tot);
            SQLparams.Add("@GRN_H_supplier", GRN_H.GRN_H_Supplier);
            SQLparams.Add("@GRN_H_location", GRN_H.GRN_H_Location);
            SQLparams.Add("@GRN_H_source", GRN_H.GRN_H_Source??0);
            SQLparams.Add("@GRN_H_user", 1);

            string Query = @"SELECT * FROM GRN_H WHERE GRN_H_id = " + GRN_H.GRN_H_Id;
            DataTable GRNH = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (GRNH != null && GRNH.Rows.Count > 0)
            {
                SQLparams.Add("@GRN_H_code", GRN_H.GRN_H_Code);
                Query = Common.Functions.QueryBuilder.BuildUpdate("GRN_H", "GRN_H_id", GRN_H.GRN_H_Id, SQLparams);

                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {

                DataTable code_rec = DBCon.GetData(SqlCon, SqlTran, "SELECT DOC_LastNo FROM Documnets WHERE DOC_id=1", null);
                int last_no = code_rec.Rows[0].Field<int>("DOC_LastNo") + 1;
                string code = "GRN" + last_no.ToString().PadLeft(7, '0');
                SQLparams["@GRN_H_code"] = code;
                                
                Query = Common.Functions.QueryBuilder.BuildInsert("dbo.GRN_H", "GRN_H_id", SQLparams);
                GRN_H.GRN_H_Id = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
                GRN_H.GRN_H_Code = code;
                GRN_H.GRN_H_Date = (DateTime)SQLparams["@GRN_H_Date"];

                string Update = "UPDATE Documnets SET DOC_LastNo= " +last_no+ " WHERE DOC_id=1";
                DBCon.ExecuteNonQuery(SqlCon, SqlTran, Update, null);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return GRN_H;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long GRN_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@GRN_H_id", GRN_ID);

            string Query = @"DELETE FROM GRN_H WHERE GRN_H_id = @GRN_H_id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        

       
    }
}