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
    public class GRNDAL
    {
       
        public GRNDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, GRNDOM GRN)
        {
            GRN_H_DAL _GRN_H = new GRN_H_DAL();
            GRN_D_DAL _GRN_D = new GRN_D_DAL();
            PO_H_DAL _PO_H = new PO_H_DAL();
            StockMovementDAL _ST_M = new StockMovementDAL();
            StockMovement_Trans_DAL _SM_Trans = new StockMovement_Trans_DAL();

            GRNDOM _GRNH, _GRND = new GRNDOM();
            StockMovementDOM _SM = new StockMovementDOM();
            StockMovementTransDOM _SMTrans = new StockMovementTransDOM();

            bool dispose = SqlCon == null;


            using(SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting))
            {
                if (dispose)
                {
                    SqlCon.Open();
                }

                using (SqlTransaction transaction = SqlCon.BeginTransaction())
                {
                    try
                    {
                        string Query = @"SELECT * FROM GRN_H WHERE GRN_H_id = " + GRN.GRN_H_Id;
                        DataTable GRN_H = DBCon.GetData(SqlCon, transaction, Query, null);

                        if (GRN_H != null && GRN_H.Rows.Count > 0)
                        {
                            _GRNH = _GRN_H.Save(SqlCon, transaction, GRN);
                            GRN.GRN_H_Id = _GRNH.GRN_H_Id;
                            _GRN_D.Delete(SqlCon, transaction, _GRNH.GRN_H_Id);
                            _GRND = _GRN_D.Save(SqlCon, transaction, GRN);
                        }
                        else
                        {
                            _GRNH = _GRN_H.Save(SqlCon, transaction, GRN);
                            GRN.GRN_H_Id = _GRNH.GRN_H_Id;
                            GRN.GRN_H_Code = _GRNH.GRN_H_Code;
                            _GRND = _GRN_D.Save(SqlCon, transaction, GRN);

                            if (_GRNH.GRN_H_Source != 0)
                            {
                                _PO_H.CompletePO(SqlCon, transaction, (long)_GRNH.GRN_H_Source);
                            }
                            _GRND.Items.ForEach(itm => {
                                _SM.SM_Type = Common.Properties.Database.ERP.SM_Types.GRN;
                                _SM.SM_Item = itm.Item_ID;
                                _SM.SM_Location = _GRNH.GRN_H_Location;
                                _SM.SM_Date = _GRNH.GRN_H_Date;
                                _SM.SM_Bal = itm.Item_Qty;

                                _ST_M.Save(SqlCon, transaction, _SM,true);

                                _SMTrans.SMT_Item = itm.Item_ID;
                                _SMTrans.SMT_Qty = itm.Item_Qty;
                                _SMTrans.SMT_Location = _GRNH.GRN_H_Location;
                                _SMTrans.SMT_Type = Common.Properties.Database.ERP.SM_Types.GRN;
                                _SMTrans.SMT_Doc = _GRNH.GRN_H_Id;
                                _SMTrans.SMT_Doc_No = _GRNH.GRN_H_Code;

                                _SM_Trans.Save(SqlCon, transaction, _SMTrans);

                            });
                        }
                       
                        transaction.Commit();
                    }
                    catch(Exception e1)
                    {
                        transaction.Rollback();
                        throw e1;
                    }
                }
            }
            return GRN;
        }

        public List<GRNDOM> GetAllGRNS (SqlConnection SqlCon, SqlTransaction SqlTran)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<GRNDOM> GRNH = new List<GRNDOM>();

            string Query = @"SELECT * FROM GRN_H";
            DataTable DT_GRNH = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_GRNH != null && DT_GRNH.Rows.Count > 0)
            {
                DT_GRNH.AsEnumerable().ToList().ForEach(r =>
                {
                    GRNH.Add(new GRNDOM()
                    {
                        GRN_H_Id = r.Field<long>("GRN_H_id"),
                        GRN_H_Code = r.Field<string>("GRN_H_code"),
                        GRN_H_Date = r.Field<DateTime>("GRN_H_Date"),
                        GRN_H_Amount = r.Field<decimal>("GRN_H_amount"),
                        GRN_H_Supplier = r.Field<long>("GRN_H_supplier"),
                        GRN_H_Location = r.Field<int>("GRN_H_location"),
                        GRN_H_Source = r.Field<long>("GRN_H_source"),
                        GRN_H_User = r.Field<int>("GRN_H_user")
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return GRNH;
        }

        public GRNDOM SelectGRN(SqlConnection SqlCon, SqlTransaction SqlTran, long GRN_ID)
        {

            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            GRNDOM GRNH = new GRNDOM();
            List<ItemGridDOM> ITM = new List<ItemGridDOM>();

            string Query = @"SELECT * FROM GRN_H WHERE GRN_H_id=" + GRN_ID;
            string Query_D = @"  SELECT GD.*,IM.ITEM_code,IM.ITEM_name FROM GRN_D GD " +
                                 "JOIN ItemMaster IM ON " +
                                 "GD.GRN_D_item_id=IM.ITEM_id " +
                                 "WHERE GRN_D_h_id=" + GRN_ID;

            DataTable DT_GRNH = DBCon.GetData(SqlCon, SqlTran, Query, null);
            DataTable DT_GRND = DBCon.GetData(SqlCon, SqlTran, Query_D, null);

            GRNH.GRN_H_Id = DT_GRNH.Rows[0].Field<long>("GRN_H_id");
            GRNH.GRN_H_Code = DT_GRNH.Rows[0].Field<string>("GRN_H_code");
            GRNH.GRN_H_Date = DT_GRNH.Rows[0].Field<DateTime>("GRN_H_Date");
            GRNH.GRN_H_Amount = DT_GRNH.Rows[0].Field<decimal>("GRN_H_amount");
            GRNH.GRN_H_Supplier = DT_GRNH.Rows[0].Field<long>("GRN_H_supplier");
            GRNH.GRN_H_Location = DT_GRNH.Rows[0].Field<int>("GRN_H_location");
            GRNH.GRN_H_Source = DT_GRNH.Rows[0].Field<long>("GRN_H_source");
            GRNH.GRN_H_User = DT_GRNH.Rows[0].Field<int>("GRN_H_user");

            if (DT_GRND != null && DT_GRND.Rows.Count > 0)
            {
                DT_GRND.AsEnumerable().ToList().ForEach(r =>
                {
                    ITM.Add(new ItemGridDOM()
                    {
                        Item_ID = r.Field<long>("GRN_D_item_id"),
                        Item_Code = r.Field<string>("ITEM_code"),
                        Item_Name = r.Field<string>("ITEM_name"),
                        Item_Qty = r.Field<decimal>("GRN_D_item_qty"),
                        Item_Price = r.Field<decimal>("GRN_D_item_cost"),
                        Item_Value = r.Field<decimal>("GRN_D_item_cost") * r.Field<decimal>("GRN_D_item_qty")
                    });
                });
                GRNH.Items = ITM;
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return GRNH;

        }



    }
}