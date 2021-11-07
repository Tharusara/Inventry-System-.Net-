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
    public class ItemMasterDAL
    {
        public ItemMasterDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, ItemMasterDOM Item_Master)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@ITEM_code", Item_Master.ITEM_code);
            SQLparams.Add("@ITEM_name", Item_Master.ITEM_name);
            SQLparams.Add("@ITEM_catagory", Item_Master.ITEM_cat);
            SQLparams.Add("@ITEM_selling_price", Item_Master.ITEM_price);
            SQLparams.Add("@ITEM_cost", Item_Master.ITEM_cost);
            SQLparams.Add("@ITEM_min_qty", Item_Master.ITEM_minqty);
            SQLparams.Add("@ITEM_active", Item_Master.ITEM_activechq == "true" ? true : false);


            string Query = @"SELECT * FROM ItemMaster WHERE ITEM_id = " + Item_Master.ITEM_id;
            DataTable ItemMaster = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (ItemMaster != null && ItemMaster.Rows.Count > 0)
            {
                Query = Common.Functions.QueryBuilder.BuildUpdate("ItemMaster", "ITEM_id", Item_Master.ITEM_id, SQLparams);

                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {
                Query = Common.Functions.QueryBuilder.BuildInsert("ItemMaster", "ITEM_id", SQLparams);
                Item_Master.ITEM_id = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return Item_Master;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long Item_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@ITEM_id", Item_ID);

            string Query = @"DELETE FROM ItemMaster WHERE ItemMaster = @ITEM_id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        public List<ItemMasterDOM> GetAll(SqlConnection SqlCon, SqlTransaction SqlTran,bool? IsActive,long? Item_ID,string Item_Code)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<ItemMasterDOM> ItemMaster = new List<ItemMasterDOM>();

            string Query = @"SELECT * FROM ItemMaster";
            if (IsActive == true)
                Query += " WHERE ITEM_active=1";
            else if (Item_ID != null)
                Query += " WHERE ITEM_id=" + Item_ID;
            else if (!string.IsNullOrEmpty(Item_Code))
                Query += " WHERE ITEM_code='" + Item_Code+"'";

            DataTable DT_ItemMaster = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_ItemMaster != null && DT_ItemMaster.Rows.Count > 0)
            {
                DT_ItemMaster.AsEnumerable().ToList().ForEach(r =>
                {
                    ItemMaster.Add(new ItemMasterDOM()
                    {
                        ITEM_id = r.Field<long>("ITEM_id"),
                        ITEM_code = r.Field<string>("ITEM_code"),
                        ITEM_name = r.Field<string>("ITEM_name"),
                        ITEM_cat = r.Field<int>("ITEM_catagory"),
                        ITEM_price = r.Field<decimal>("ITEM_selling_price"),
                        ITEM_cost = r.Field<decimal>("ITEM_cost"),
                        ITEM_minqty = r.Field<decimal>("ITEM_min_qty"),
                        ITEM_active = r.Field<bool>("ITEM_active"),
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return ItemMaster;
        }
        public List<ItemMasterDOM> GetActive(SqlConnection SqlCon, SqlTransaction SqlTran)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<ItemMasterDOM> ItemMaster = new List<ItemMasterDOM>();

            string Query = @"  SELECT * FROM ItemMaster WHERE ITEM_active=1";
            DataTable DT_ItemMaster = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_ItemMaster != null && DT_ItemMaster.Rows.Count > 0)
            {
                DT_ItemMaster.AsEnumerable().ToList().ForEach(r =>
                {
                    ItemMaster.Add(new ItemMasterDOM()
                    {
                        ITEM_id = r.Field<long>("ITEM_id"),
                        ITEM_code = r.Field<string>("ITEM_code"),
                        ITEM_name = r.Field<string>("ITEM_name"),
                        ITEM_cat = r.Field<int>("ITEM_catagory"),
                        ITEM_price = r.Field<decimal>("ITEM_selling_price"),
                        ITEM_cost = r.Field<decimal>("ITEM_cost"),
                        ITEM_minqty = r.Field<decimal>("ITEM_min_qty"),
                        ITEM_active = r.Field<bool>("ITEM_active"),
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return ItemMaster;
        }

        public List<ItemGridDOM> getItems(SqlConnection SqlCon, SqlTransaction SqlTran, int Item_Loc, long? item_id, string item_code)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<ItemGridDOM> ITM = new List<ItemGridDOM>();

            string Query = @"SELECT IM.ITEM_id,IM.ITEM_code,IM.ITEM_name,IM.ITEM_selling_price,SM.SM_Bal,IM.ITEM_active FROM StockMovement SM" +
                             " JOIN ItemMaster IM ON SM.SM_Item=IM.ITEM_id" +
                             " WHERE SM.SM_Location=" + Item_Loc+" AND SM.SM_Bal>0";
            
            if (item_id != null)
                Query += " AND ITEM_id=" + item_id;
            else if (!string.IsNullOrEmpty(item_code))
                Query += " AND ITEM_code LIKE" + "'%" + item_code + "%'";
            Query += " AND ITEM_active=1";

            DataTable DT_Data = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_Data != null && DT_Data.Rows.Count > 0)
            {
                DT_Data.AsEnumerable().ToList().ForEach(r =>
                {
                    ITM.Add(new ItemGridDOM()
                    {
                        Item_ID = r.Field<long>("ITEM_id"),
                        Item_Code = r.Field<string>("ITEM_code"),
                        Item_Name = r.Field<string>("ITEM_name"),
                        Item_Qty = r.Field<decimal>("SM_Bal"),
                        Item_Price = r.Field<decimal>("ITEM_selling_price"),
                    });
                });

            }
            return ITM;
        }

        public List<ItemGridDOM> getReorderItems(SqlConnection SqlCon, SqlTransaction SqlTran, int? Item_Loc)
        {
            List<ItemGridDOM> ITM = new List<ItemGridDOM>();
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            string Query = @"SELECT IM.ITEM_id,IM.ITEM_code,IM.ITEM_name,SM.SM_Bal,IM.ITEM_selling_price FROM dbo.StockMovement SM" +
                            " JOIN dbo.ItemMaster IM ON SM.SM_Item=IM.ITEM_id" +
                            " WHERE SM.SM_Location=" + Item_Loc + "AND SM.SM_Bal<=IM.ITEM_min_qty";

            DataTable DT_Data = DBCon.GetData(SqlCon, SqlTran, Query, null);
            if (DT_Data != null && DT_Data.Rows.Count > 0)
            {
                DT_Data.AsEnumerable().ToList().ForEach(r =>
                {
                    ITM.Add(new ItemGridDOM()
                    {
                        Item_ID = r.Field<long>("ITEM_id"),
                        Item_Code = r.Field<string>("ITEM_code"),
                        Item_Name = r.Field<string>("ITEM_name"),
                        Item_Qty = r.Field<decimal>("SM_Bal"),
                        Item_Price = r.Field<decimal>("ITEM_selling_price"),
                    });
                });

            }
            return ITM;
        }

    

       


    }
}