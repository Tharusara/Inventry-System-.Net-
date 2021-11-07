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
    public class PO_H_DAL
    {
        public PurchaseOrderDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, PurchaseOrderDOM PO_H)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            decimal tot = 0;
            PO_H.Items.ToList().ForEach(x =>
            {
                tot += x.Item_Value;
            });

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@PO_H_code", "");
            SQLparams.Add("@PO_H_date", System.DateTime.Now.Date);
            SQLparams.Add("@PO_H_amount", tot);
            SQLparams.Add("@PO_H_location", PO_H.PO_H_Location);
            SQLparams.Add("@PO_H_supplier", PO_H.PO_H_Supplier);
            //SQLparams.Add("@PO_H_user", PO_H.PO_H_User);
            SQLparams.Add("@PO_H_user", 1);
            SQLparams.Add("@PO_H_isComplete",0);

            string Query = @"SELECT * FROM PurchaseOrder_H WHERE PO_H_id = " + PO_H.PO_H_ID;
            DataTable DT_POH = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (DT_POH != null && DT_POH.Rows.Count > 0)
            {
                SQLparams["@PO_H_code"] = DT_POH.Rows[0].Field<string>("PO_H_code");
                Query = Common.Functions.QueryBuilder.BuildUpdate("PurchaseOrder_H", "PO_H_id", PO_H.PO_H_ID, SQLparams);
                DBCon.ExecuteNonQuery(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {
                DataTable code_rec = DBCon.GetData(SqlCon, SqlTran, "SELECT DOC_LastNo FROM Documnets WHERE DOC_id=2", null);
                int last_no = code_rec.Rows[0].Field<int>("DOC_LastNo") + 1;
                string code = "PO" + last_no.ToString().PadLeft(7, '0');
                SQLparams["@PO_H_code"] = code;

                Query = Common.Functions.QueryBuilder.BuildInsert("dbo.PurchaseOrder_H", "PO_H_id", SQLparams);
                PO_H.PO_H_ID = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
                PO_H.PO_H_Code = code;

                string Update = "UPDATE Documnets SET DOC_LastNo= " + last_no + " WHERE DOC_id=2";
                DBCon.ExecuteNonQuery(SqlCon, SqlTran, Update, null);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return PO_H;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long POH_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@PO_H_id", POH_ID);

            string Query = @"DELETE FROM PurchaseOrder_H WHERE PO_H_id = @PO_H_id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        public void CompletePO(SqlConnection SqlCon, SqlTransaction SqlTran, long POH_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@PO_H_isComplete", 1);

            string Query = @"SELECT * FROM PurchaseOrder_H WHERE PO_H_id = " + POH_ID;
            DataTable DT_POH = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (DT_POH != null && DT_POH.Rows.Count > 0)
            {
                Query = Common.Functions.QueryBuilder.BuildUpdate("PurchaseOrder_H", "PO_H_id", POH_ID, SQLparams);
                DBCon.ExecuteNonQuery(SqlCon, SqlTran, Query, SQLparams);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        //public List<PurchaseOrderDOM> GetAllPOs(SqlConnection SqlCon, SqlTransaction SqlTran)
        //{
        //    bool dispose = SqlCon == null;
        //    SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
        //    if (dispose)
        //    {
        //        SqlCon.Open();
        //    }

        //    List<PurchaseOrderDOM> POH = new List<PurchaseOrderDOM>();

        //    string Query = @"SELECT * FROM PurchaseOrder_H";

        //    DataTable DT_POH = DBCon.GetData(SqlCon, SqlTran, Query, null);

        //    if (DT_POH != null && DT_POH.Rows.Count > 0)
        //    {
        //        DT_POH.AsEnumerable().ToList().ForEach(r =>
        //        {
        //            POH.Add(new PurchaseOrderDOM()
        //            {
        //                PO_H_ID = r.Field<long>("PO_H_id"),
        //                PO_H_Code = r.Field<string>("PO_H_code"),
        //                PO_H_Date = r.Field<DateTime>("PO_H_date"),
        //                PO_H_Amount = r.Field<decimal>("PO_H_amount"),
        //                PO_H_Location = r.Field<int>("PO_H_location"),
        //                PO_H_Supplier = r.Field<long>("PO_H_supplier"),
        //                PO_H_User = r.Field<int>("PO_H_user"),
        //            });
        //        });
        //    }

        //    if (dispose)
        //    {
        //        SqlCon.Close();
        //        SqlCon.Dispose();
        //    }

        //    return POH;
        //}
    }
}