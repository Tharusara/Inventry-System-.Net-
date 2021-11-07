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
    public class CategoryMasterDAL
    {
        public CategoryMasterDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, CategoryMasterDOM Category_Master)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@CAT_code", Category_Master.CAT_code);
            SQLparams.Add("@CAT_name", Category_Master.CAT_Name);
            string Query = @"SELECT * FROM Category_Master WHERE CAT_Id = " + Category_Master.CAT_id;
            DataTable CategoryMaster = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (CategoryMaster != null && CategoryMaster.Rows.Count > 0)
            {
                Query = Common.Functions.QueryBuilder.BuildUpdate("Category_Master", "CAT_Id", Category_Master.CAT_id, SQLparams);

                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {
                Query = Common.Functions.QueryBuilder.BuildInsert("Category_Master", "CAT_Id", SQLparams);
                Category_Master.CAT_id = (int)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return Category_Master;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long Cat_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@CAT_Id", Cat_ID);

            string Query = @"DELETE FROM Category_Master WHERE SUP_id = @CAT_Id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        public List<CategoryMasterDOM> GetAllCategories(SqlConnection SqlCon, SqlTransaction SqlTran,long? Cat_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<CategoryMasterDOM> CategoryMaster = new List<CategoryMasterDOM>();

            string Query = @"SELECT * FROM Category_Master";
            if (Cat_ID != null)
                Query += " WHERE CAT_id=" + Cat_ID;
            DataTable DT_CategoryMaster = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_CategoryMaster != null && DT_CategoryMaster.Rows.Count > 0)
            {
                DT_CategoryMaster.AsEnumerable().ToList().ForEach(r =>
                {
                    CategoryMaster.Add(new CategoryMasterDOM()
                    {
                        CAT_id = r.Field<int>("CAT_id"),
                        CAT_code = r.Field<string>("CAT_code"),
                        CAT_Name = r.Field<string>("CAT_name"),
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return CategoryMaster;
        }

        public string getCatagory(SqlConnection SqlCon, SqlTransaction SqlTran,int Cat_Id)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }
            string Query = @"SELECT * FROM Category_Master WHERE CAT_Id = " + Cat_Id;
            DataTable CategoryMaster = DBCon.GetData(SqlCon, SqlTran, Query, null);
            string Cat_name = CategoryMaster.AsEnumerable().FirstOrDefault().Field<string>("CAT_name");
            return Cat_name;
        }

        
    }
}