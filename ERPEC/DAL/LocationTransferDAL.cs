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
    public class LocationTransferDAL
    {
        public LocationTransferDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, LocationTransferDOM LT)
        {
            Location_Transfer_H_DAL _LT_H = new Location_Transfer_H_DAL();
            Location_Transfer_D_DAL _LT_D = new Location_Transfer_D_DAL();
            StockMovementDAL _ST_M = new StockMovementDAL();
            StockMovement_Trans_DAL _SM_Trans = new StockMovement_Trans_DAL();

            LocationTransferDOM _LTH, _LTD = new LocationTransferDOM();
            StockMovementDOM _SM = new StockMovementDOM();
            StockMovementTransDOM _SMTrans = new StockMovementTransDOM();

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
                        string Query = @"SELECT * FROM Location_Transfer_H WHERE TRANS_H_id = " + LT.TRANS_H_Id;
                        DataTable DT_LT_H = DBCon.GetData(SqlCon, transaction, Query, null);

                        if (DT_LT_H != null && DT_LT_H.Rows.Count > 0)
                        {
                            _LTH = _LT_H.Save(SqlCon, transaction, LT);
                            LT.TRANS_H_Id = _LTH.TRANS_H_Id;
                            _LT_D.Delete(SqlCon, transaction, _LTH.TRANS_H_Id);
                            _LTD = _LT_D.Save(SqlCon, transaction, LT);
                        }
                        else
                        {
                            _LTH = _LT_H.Save(SqlCon, transaction, LT);
                            LT.TRANS_H_Id = _LTH.TRANS_H_Id;
                            LT.TRANS_H_Code = _LTH.TRANS_H_Code;
                            _LTD = _LT_D.Save(SqlCon, transaction, LT);


                            //LocationIN
                            _LTD.Items.ForEach(itm =>
                            {
                                _SM.SM_Type = Common.Properties.Database.ERP.SM_Types.LTI;
                                _SM.SM_Item = itm.Item_ID;
                                _SM.SM_Location = _LTH.TRANS_H_To;
                                _SM.SM_Date = _LTH.TRANS_H_Date;
                                _SM.SM_Bal = itm.Item_Qty;
                                
                                _ST_M.Save(SqlCon, transaction, _SM,true);

                                _SMTrans.SMT_Item = itm.Item_ID;
                                _SMTrans.SMT_Qty = itm.Item_Qty;
                                _SMTrans.SMT_Location = _LTH.TRANS_H_To;
                                _SMTrans.SMT_Type = Common.Properties.Database.ERP.SM_Types.LTI;
                                _SMTrans.SMT_Doc = _LTH.TRANS_H_Id;
                                _SMTrans.SMT_Doc_No = _LTH.TRANS_H_Code;

                                _SM_Trans.Save(SqlCon, transaction, _SMTrans);
                            });

                            //LocationOut
                            _LTD.Items.ForEach(itm =>
                            {
                                _SM.SM_Type = Common.Properties.Database.ERP.SM_Types.LTO;
                                _SM.SM_Item = itm.Item_ID;
                                _SM.SM_Location = _LTH.TRANS_H_From;
                                _SM.SM_Date = _LTH.TRANS_H_Date;
                                _SM.SM_Bal = itm.Item_Qty;

                                _ST_M.Save(SqlCon, transaction, _SM,false);

                                _SMTrans.SMT_Item = itm.Item_ID;
                                _SMTrans.SMT_Qty = itm.Item_Qty;
                                _SMTrans.SMT_Location = _LTH.TRANS_H_From;
                                _SMTrans.SMT_Type = Common.Properties.Database.ERP.SM_Types.LTO;
                                _SMTrans.SMT_Doc = _LTH.TRANS_H_Id;
                                _SMTrans.SMT_Doc_No = _LTH.TRANS_H_Code;

                                _SM_Trans.Save(SqlCon, transaction, _SMTrans);
                            });
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
            return LT;
        }

        public List<LocationTransferDOM> GetAllLTs(SqlConnection SqlCon, SqlTransaction SqlTran)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<LocationTransferDOM> LTH = new List<LocationTransferDOM>();

            string Query = @"SELECT * FROM Location_Transfer_H";
            DataTable DT_LTH = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_LTH != null && DT_LTH.Rows.Count > 0)
            {
                DT_LTH.AsEnumerable().ToList().ForEach(r =>
                {
                    LTH.Add(new LocationTransferDOM()
                    {
                        TRANS_H_Id = r.Field<long>("TRANS_H_id"),
                        TRANS_H_Code = r.Field<string>("TRANS_H_code"),
                        TRANS_H_Date = r.Field<DateTime>("TRANS_H_date"),
                        TRANS_H_From = r.Field<int>("TRANS_H_from"),
                        TRANS_H_To = r.Field<int>("TRANS_H_to"),
                        TRANS_H_Value = r.Field<decimal>("TRANS_H_value"),
                        TRANS_H_Narration = r.Field<string>("TRANS_H_narration"),
                        TRANS_H_User = r.Field<int>("TRANS_H_user")
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return LTH;
        }

        public LocationTransferDOM SelectLT(SqlConnection SqlCon, SqlTransaction SqlTran, long LTH_ID)
        {

            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            LocationTransferDOM LTH = new LocationTransferDOM();
            List<ItemGridDOM> ITM = new List<ItemGridDOM>();

            string Query = @"SELECT * FROM Location_Transfer_H WHERE TRANS_H_id=" + LTH_ID;
            string Query_D = @"  SELECT LTD.*,IM.ITEM_code,IM.ITEM_name FROM Location_Transfer_D LTD " +
                                 "JOIN ItemMaster IM ON " +
                                 "LTD.TRANS_D_item=IM.ITEM_id " +
                                 "WHERE LTD.TRANS_D_H_id=" + LTH_ID;

            DataTable DT_LTH = DBCon.GetData(SqlCon, SqlTran, Query, null);
            DataTable DT_LTD = DBCon.GetData(SqlCon, SqlTran, Query_D, null);

            LTH.TRANS_H_Id = DT_LTH.Rows[0].Field<long>("TRANS_H_id");
            LTH.TRANS_H_Code = DT_LTH.Rows[0].Field<string>("TRANS_H_code");
            LTH.TRANS_H_Date = DT_LTH.Rows[0].Field<DateTime>("TRANS_H_date");
            LTH.TRANS_H_From = DT_LTH.Rows[0].Field<int>("TRANS_H_from");
            LTH.TRANS_H_To = DT_LTH.Rows[0].Field<int>("TRANS_H_to");
            LTH.TRANS_H_Value = DT_LTH.Rows[0].Field<decimal>("TRANS_H_value");
            LTH.TRANS_H_Narration = DT_LTH.Rows[0].Field<string>("TRANS_H_narration");
            LTH.TRANS_H_User = DT_LTH.Rows[0].Field<int>("TRANS_H_user");

            if (DT_LTD != null && DT_LTD.Rows.Count > 0)
            {
                DT_LTD.AsEnumerable().ToList().ForEach(r =>
                {
                    ITM.Add(new ItemGridDOM()
                    {
                        Item_ID = r.Field<long>("TRANS_D_item"),
                        Item_Code = r.Field<string>("ITEM_code"),
                        Item_Name = r.Field<string>("ITEM_name"),
                        Item_Qty = r.Field<decimal>("TRANS_D_qty"),
                        Item_Price = r.Field<decimal>("TRANS_D_price"),
                        Item_Value = r.Field<decimal>("TRANS_D_price") * r.Field<decimal>("TRANS_D_qty")
                    });
                });
                LTH.Items = ITM;
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return LTH;

        }
    }
}