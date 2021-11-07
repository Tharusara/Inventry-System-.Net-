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
    public class SupplierMasterDAL
    {
        public SupplierMasterDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, SupplierMasterDOM Supplier_Master)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@SUP_code", Supplier_Master.SUP_code);
            SQLparams.Add("@SUP_name", Supplier_Master.SUP_name);
            SQLparams.Add("@SUP_address", Supplier_Master.SUP_address);
            SQLparams.Add("@SUP_contactno", Supplier_Master.SUP_contact);
            SQLparams.Add("@SUP_active", 1);

            string Query = @"SELECT * FROM SupplierMaster WHERE SUP_id = " + Supplier_Master.SUP_id;
            DataTable SupplierMaster = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (SupplierMaster != null && SupplierMaster.Rows.Count > 0)
            {
                Query = Common.Functions.QueryBuilder.BuildUpdate("SupplierMaster", "SUP_id", Supplier_Master.SUP_id, SQLparams);

                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {
                Query = Common.Functions.QueryBuilder.BuildInsert("dbo.SupplierMaster", "SUP_id", SQLparams);
                Supplier_Master.SUP_id = (long)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return Supplier_Master;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, long Supplier_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@SUP_id", Supplier_ID);

            string Query = @"DELETE FROM SupplierMaster WHERE SUP_id = @SUP_id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        public List<SupplierMasterDOM> GetAllSuppliers(SqlConnection SqlCon, SqlTransaction SqlTran,bool? IsActive)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<SupplierMasterDOM> SupplierMaster = new List<SupplierMasterDOM>();

            string Query = @"SELECT * FROM SupplierMaster";
            if (IsActive == true)
                Query += " WHERE SUP_active=1";
            DataTable DT_SupplierMaster = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_SupplierMaster != null && DT_SupplierMaster.Rows.Count > 0)
            {
                DT_SupplierMaster.AsEnumerable().ToList().ForEach(r =>
                {
                    SupplierMaster.Add(new SupplierMasterDOM()
                    {
                        SUP_id = r.Field<long>("SUP_id"),
                        SUP_code = r.Field<string>("SUP_code"),
                        SUP_name = r.Field<string>("SUP_name"),
                        SUP_address = r.Field<string>("SUP_address"),
                        SUP_contact = r.Field<string>("SUP_contactno"),
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return SupplierMaster;
        }
        public List<SupplierMasterDOM> GetSupplierCode(SqlConnection SqlCon, SqlTransaction SqlTran)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<SupplierMasterDOM> SupplierMaster = new List<SupplierMasterDOM>();

            string Query = @"SELECT ('SUP'+ RIGHT('000' + CAST((SELECT PREFIX_last FROM dbo.ERP_Prefix_Master WHERE PREFIX_id = 1) AS varchar(4)) , 4)) As Code FROM ERP_Prefix_Master WHERE PREFIX_id=1";
            DataTable DT_SupplierMaster = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_SupplierMaster != null && DT_SupplierMaster.Rows.Count > 0)
            {
                DT_SupplierMaster.AsEnumerable().ToList().ForEach(r =>
                {
                    SupplierMaster.Add(new SupplierMasterDOM()
                    {
                        SUP_code = r.Field<string>("Code"),
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return SupplierMaster;
        }
    }
}