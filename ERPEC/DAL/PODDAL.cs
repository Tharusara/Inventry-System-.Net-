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
    public class PODDAL
    {
        public PurchaseOrderDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, PurchaseOrderDOM PO)
        {
            PO_H_DAL _PO_H = new PO_H_DAL();
            PO_D_DAL _PO_D = new PO_D_DAL();
            PurchaseOrderDOM _POH, _POD = new PurchaseOrderDOM();

            bool dispose = SqlCon == null;

            using (SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting))
            {
                if (dispose)
                {
                    SqlCon.Open();
                }
                SqlTransaction transaction;
                if (SqlTran == null)
                    transaction = SqlCon.BeginTransaction();
                else
                    transaction = SqlTran;
                //using (SqlTransaction transaction = SqlCon.BeginTransaction())
                using (transaction)
                {
                    try
                    {
                        string Query = @"SELECT * FROM PurchaseOrder_H WHERE PO_H_id = " + PO.PO_H_ID;
                        DataTable DT_PO_H = DBCon.GetData(SqlCon, transaction, Query, null);

                        if (DT_PO_H != null && DT_PO_H.Rows.Count > 0)
                        {
                            _POH = _PO_H.Save(SqlCon, transaction, PO);
                            PO.PO_H_ID = _POH.PO_H_ID;
                            PO.PO_H_Code = DT_PO_H.Rows[0].Field<string>("PO_H_code");
                            _PO_D.Delete(SqlCon, transaction, _POH.PO_H_ID);
                            _POD = _PO_D.Save(SqlCon, transaction, PO);

                        }
                        else
                        {
                            _POH = _PO_H.Save(SqlCon, transaction, PO);
                            PO.PO_H_ID = _POH.PO_H_ID;
                            PO.PO_H_Code = _POH.PO_H_Code;
                            _POD = _PO_D.Save(SqlCon, transaction, PO);
                        }

                        transaction.Commit();
                    }
                    catch (Exception e1)
                    {
                        transaction.Rollback();
                        throw e1;
                    }
                }
            }
            return PO;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long POH_ID)
        {
            PO_H_DAL _PO_H = new PO_H_DAL();
            PO_D_DAL _PO_D = new PO_D_DAL();

            using (SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting))
            {
                bool dispose = SqlCon == null;
                if (dispose)
                {
                    SqlCon.Open();
                }

                using (SqlTransaction transaction = SqlCon.BeginTransaction())
                {
                    try
                    {
                        _PO_D.Delete(SqlCon,transaction,POH_ID);
                        _PO_H.Delete(SqlCon,transaction,POH_ID);

                        transaction.Commit();
                    }
                    catch (Exception e1)
                    {
                        transaction.Rollback();
                        throw e1;
                    }
                }
            }
        
        }

        public List<PurchaseOrderDOM> GetAllPOS(SqlConnection SqlCon, SqlTransaction SqlTran, int? IsComplete)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<PurchaseOrderDOM> POH = new List<PurchaseOrderDOM>();
            
            string Query = @"SELECT * FROM PurchaseOrder_H";
            if(IsComplete!=null)
            Query += " WHERE PO_H_isComplete=" + IsComplete;
            DataTable DT_POH = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_POH != null && DT_POH.Rows.Count > 0)
            {
                DT_POH.AsEnumerable().ToList().ForEach(r =>
                {
                    POH.Add(new PurchaseOrderDOM()
                    {
                        PO_H_ID = r.Field<long>("PO_H_id"),
                        PO_H_Code = r.Field<string>("PO_H_code"),
                        PO_H_Date = r.Field<DateTime>("PO_H_date").Date,
                        PO_H_Amount = r.Field<decimal>("PO_H_amount"),
                        PO_H_Location = r.Field<int>("PO_H_location"),
                        PO_H_Supplier = r.Field<long>("PO_H_supplier"),
                        PO_H_User = r.Field<int>("PO_H_user")
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return POH;
        }

        public PurchaseOrderDOM SelectPO(SqlConnection SqlCon, SqlTransaction SqlTran, long POH_ID)
        {

            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            PurchaseOrderDOM POH = new PurchaseOrderDOM();
            List<ItemGridDOM> ITM = new List<ItemGridDOM>();

            string Query = @"SELECT * FROM PurchaseOrder_H WHERE PO_H_id=" + POH_ID;
            string Query_D = @"  SELECT PD.*,IM.ITEM_code,IM.ITEM_name FROM PurchaseOrder_D PD "+
                                 "JOIN ItemMaster IM ON "+
                                 "pd.PO_D_item=IM.ITEM_id "+
                                 "WHERE PO_D_h_id=" + POH_ID;

            DataTable DT_POH = DBCon.GetData(SqlCon, SqlTran, Query, null);
            DataTable DT_POD = DBCon.GetData(SqlCon, SqlTran, Query_D, null);

            POH.PO_H_ID = DT_POH.Rows[0].Field<long>("PO_H_id");
            POH.PO_H_Code = DT_POH.Rows[0].Field<string>("PO_H_code");
            POH.PO_H_Date = DT_POH.Rows[0].Field<DateTime>("PO_H_date");
            POH.PO_H_Amount = DT_POH.Rows[0].Field<decimal>("PO_H_amount");
            POH.PO_H_Location = DT_POH.Rows[0].Field<int>("PO_H_location");
            POH.PO_H_Supplier = DT_POH.Rows[0].Field<long>("PO_H_supplier");
            POH.PO_H_User = DT_POH.Rows[0].Field<int>("PO_H_user");

            if (DT_POD != null && DT_POD.Rows.Count > 0)
            {
                DT_POD.AsEnumerable().ToList().ForEach(r =>
                {
                    ITM.Add(new ItemGridDOM()
                    {
                        Item_ID = r.Field<long>("PO_D_item"),
                        Item_Code = r.Field<string>("ITEM_code"),
                        Item_Name = r.Field<string>("ITEM_name"),
                        Item_Qty = r.Field<decimal>("PO_D_qty"),
                        Item_Price = r.Field<decimal>("PO_D_cost"),
                        Item_Value = r.Field<decimal>("PO_D_cost") * r.Field<decimal>("PO_D_qty")
                    });
                });
                POH.Items = ITM;
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return POH;

        }

    }
}