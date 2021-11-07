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
    public class Location_Transfer_H_DAL
    {
        public LocationTransferDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, LocationTransferDOM LT_H)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            decimal tot = 0;
            LT_H.Items.ToList().ForEach(x =>
            {
                tot += x.Item_Value;

            });
            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@TRANS_H_code", "");
            SQLparams.Add("@TRANS_H_date", System.DateTime.Now.Date);
            SQLparams.Add("@TRANS_H_from", LT_H.TRANS_H_From);
            SQLparams.Add("@TRANS_H_to", LT_H.TRANS_H_To);
            SQLparams.Add("@TRANS_H_value", tot);
            //SQLparams.Add("@TRANS_H_narration", LT_H.TRANS_H_Narration);
            SQLparams.Add("@TRANS_H_narration", "");
            SQLparams.Add("@TRANS_H_user", 1);

            DataTable code_rec = DBCon.GetData(SqlCon, SqlTran, "SELECT DOC_LastNo FROM Documnets WHERE DOC_id=4", null);
            int last_no = code_rec.Rows[0].Field<int>("DOC_LastNo") + 1;
            string code = "LT" + last_no.ToString().PadLeft(7, '0');
            SQLparams["@TRANS_H_code"] = code;

            string Query = Common.Functions.QueryBuilder.BuildInsert("Location_Transfer_H", "TRANS_H_id", SQLparams);
            LT_H.TRANS_H_Id = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            LT_H.TRANS_H_Code = code;
            LT_H.TRANS_H_Date = (DateTime)SQLparams["@TRANS_H_date"];

            string Update = "UPDATE Documnets SET DOC_LastNo= " + last_no + " WHERE DOC_id=4";
            DBCon.ExecuteNonQuery(SqlCon, SqlTran, Update, null);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return LT_H;
        }
    }
}