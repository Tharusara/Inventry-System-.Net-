using ERPEC.Common;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPEC.DAL
{
    public class StockMovement_Trans_DAL
    {
        public StockMovementTransDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, StockMovementTransDOM StockMovementTrans)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@SMT_Item", StockMovementTrans.SMT_Item);
            SQLparams.Add("@SMT_Qty", StockMovementTrans.SMT_Qty);
            SQLparams.Add("@SMT_Location", StockMovementTrans.SMT_Location);
            SQLparams.Add("@SMT_Type", StockMovementTrans.SMT_Type); ;
            SQLparams.Add("@SMT_doc", StockMovementTrans.SMT_Doc);
            SQLparams.Add("@SMT_doc_code", StockMovementTrans.SMT_Doc_No);
            SQLparams.Add("@SMT_trans_date", System.DateTime.Now.Date);

            string Query = Common.Functions.QueryBuilder.BuildInsert("StockMovement_Trans", "SMT_ID", SQLparams);
            StockMovementTrans.SMT_ID = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return StockMovementTrans;


        }
    }
}