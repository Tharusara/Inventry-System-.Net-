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
    public class ReOrderDAL
    {
        public ReOrderDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, ReOrderDOM RO)
        {
            ReOrder_H_DAL _RO_H = new ReOrder_H_DAL();
            ReOrder_D_DAL _RO_D = new ReOrder_D_DAL();
            PODDAL _PODAL = new PODDAL();



            ReOrderDOM _ROH, _ROD = new ReOrderDOM();
            PurchaseOrderDOM _PO = new PurchaseOrderDOM();

             bool dispose = SqlCon == null;
             using (SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting))
             {
                 if (dispose)
                 {
                     SqlCon.Open();
                 }

                 using (SqlTransaction transaction = SqlCon.BeginTransaction())
                 {
                     try
                     {
                         string Query = @"SELECT * FROM ReOrder_H WHERE RO_ID = " + RO.RO_ID;
                         DataTable DT_RO_H = DBCon.GetData(SqlCon, transaction, Query, null);

                         if (DT_RO_H != null && DT_RO_H.Rows.Count > 0)
                         {
                             _ROH=_RO_H.Save(SqlCon, transaction, RO);
                             RO.RO_ID = _ROH.RO_ID;
                             RO.RO_Code = _ROH.RO_Code;
                             _RO_D.Delete(SqlCon, transaction, _ROH.RO_ID);
                             _RO_D.Save(SqlCon, transaction, RO);
                         }
                         else
                         {
                             _ROH = _RO_H.Save(SqlCon, transaction, RO);
                             RO.RO_ID = _ROH.RO_ID;
                             RO.RO_Code = _ROH.RO_Code;
                             _RO_D.Save(SqlCon, transaction, RO);

                             //OP Save
                             _PO.PO_H_Location = _ROH.RO_Location;
                             _PO.PO_H_Supplier = 20;
                             _PO.Items = RO.Items;
                             _PODAL.Save(SqlCon, transaction,_PO);

                         }
                        

                     }
                     catch (Exception e1)
                     {
                         transaction.Rollback();
                         throw e1;
                     }
                 }
             }
             return RO;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long RO_ID)
        {
            ReOrder_H_DAL _RO_H = new ReOrder_H_DAL();
            ReOrder_D_DAL _RO_D = new ReOrder_D_DAL();

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
                        _RO_D.Delete(SqlCon, transaction, RO_ID);
                        _RO_H.Delete(SqlCon, transaction, RO_ID);

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

        public List<ReOrderDOM> GetAllROS(SqlConnection SqlCon, SqlTransaction SqlTran)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<ReOrderDOM> RO = new List<ReOrderDOM>();

            string Query = @"SELECT * FROM ReOrder_H";
            DataTable DT_RO = DBCon.GetData(SqlCon, SqlTran, Query, null);

            var d = DT_RO.Rows.Count;

            if (DT_RO != null && DT_RO.Rows.Count > 0)
            {
                DT_RO.AsEnumerable().ToList().ForEach(r =>
                {
                    long a = r.Field<long>("RO_ID");
                    string code = r.Field<string>("RO_Code");
                    RO.Add(new ReOrderDOM()
                    {
                        RO_ID = r.Field<long>("RO_ID"),
                        RO_Code = r.Field<string>("RO_Code"),
                        RO_Date = r.Field<DateTime>("RO_Date").Date,
                        RO_Location = r.Field<int>("RO_Location"),
                        RO_Amount = r.Field<decimal>("RO_Amount")
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return RO;
        }

        public ReOrderDOM SelectRO(SqlConnection SqlCon, SqlTransaction SqlTran, long RO_ID)
        {

            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            ReOrderDOM RO = new ReOrderDOM();
            List<ItemGridDOM> ITM = new List<ItemGridDOM>();

            string Query = @"SELECT * FROM ReOrder_H WHERE RO_ID=" + RO_ID;
            string Query_D = @"SELECT RD.*,IM.ITEM_code,IM.ITEM_name,IM.ITEM_cost FROM Reorder_D RD " +
                              "JOIN ItemMaster IM ON " +
                              "RD.RO_D_Item=IM.ITEM_id " +
                              "WHERE RD.RO_D_H=" + RO_ID;

            DataTable DT_ROH = DBCon.GetData(SqlCon, SqlTran, Query, null);
            DataTable DT_ROD = DBCon.GetData(SqlCon, SqlTran, Query_D, null);

            RO.RO_ID = DT_ROH.Rows[0].Field<long>("RO_ID");
            RO.RO_Code = DT_ROH.Rows[0].Field<string>("RO_Code");
            RO.RO_Date = DT_ROH.Rows[0].Field<DateTime>("RO_Date");
            RO.RO_Location = DT_ROH.Rows[0].Field<int>("RO_Location");
            RO.RO_Amount = DT_ROH.Rows[0].Field<decimal>("RO_Amount");

            if (DT_ROD != null && DT_ROD.Rows.Count > 0)
            {
                DT_ROD.AsEnumerable().ToList().ForEach(r =>
                {
                    ITM.Add(new ItemGridDOM()
                    {
                        Item_ID = r.Field<long>("RO_D_Item"),
                        Item_Code = r.Field<string>("ITEM_code"),
                        Item_Name = r.Field<string>("ITEM_name"),
                        Item_Price=r.Field<decimal>("ITEM_cost"),
                        Item_Qty = r.Field<decimal>("RO_D_Qty"),
                        Item_Value = r.Field<decimal>("ITEM_cost") * r.Field<decimal>("RO_D_Qty"),
                    });
                });
                RO.Items = ITM;
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return RO;

        }
    }
}