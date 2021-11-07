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
    public class LocationMasterDAL
    {
        public LocationMasterDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, LocationMasterDOM Location_Master)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();

            SQLparams.Add("@LOC_code", Location_Master.LocationMasterCode);
            SQLparams.Add("@LOC_name", Location_Master.LocationMasterName);
            SQLparams.Add("@LOC_Address", Location_Master.LocationMasterAddress);
            SQLparams.Add("@LOC_active", Location_Master.LocationMasterChq=="true"?true:false);

            string Query = @"SELECT * FROM LocationMaster WHERE LOC_id = " + Location_Master.LocationMasterId;
            DataTable LocationMaster = DBCon.GetData(SqlCon, SqlTran, Query, SQLparams);

            if (LocationMaster != null && LocationMaster.Rows.Count > 0)
            {
                Query = Common.Functions.QueryBuilder.BuildUpdate("LocationMaster", "LOC_id", Location_Master.LocationMasterId, SQLparams);

                DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }
            else
            {
                Query = Common.Functions.QueryBuilder.BuildInsert("dbo.LocationMaster", "LOC_id", SQLparams);
                Location_Master.LocationMasterId = (int)DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return Location_Master;
        }

        public void Delete(SqlConnection SqlCon, SqlTransaction SqlTran, int Loc_ID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            Dictionary<string, object> SQLparams = new Dictionary<string, object>();
            SQLparams.Add("@Loc_id", Loc_ID);

            string Query = @"DELETE FROM LocationMaster WHERE LOC_id = @LOC_id";
            DBCon.ExecuteScalar(SqlCon, SqlTran, Query, SQLparams);

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        public List<LocationMasterDOM> GetAllLocations(SqlConnection SqlCon, SqlTransaction SqlTran,bool? IsActive,int ?LocID)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<LocationMasterDOM> ItemMaster = new List<LocationMasterDOM>();

            string Query = @"SELECT * FROM LocationMaster";
            if(LocID!=null)
                Query += " WHERE LOC_id="+LocID;
            else if (IsActive == true)
                Query += " WHERE LOC_active=1";
            DataTable DT_LocationMaster = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_LocationMaster != null && DT_LocationMaster.Rows.Count > 0)
            {
                DT_LocationMaster.AsEnumerable().ToList().ForEach(r =>
                {
                    ItemMaster.Add(new LocationMasterDOM()
                    {
                        LocationMasterId = r.Field<int>("LOC_id"),
                        LocationMasterCode = r.Field<string>("LOC_code"),
                        LocationMasterName = r.Field<string>("LOC_name"),
                        LocationMasterAddress = r.Field<string>("LOC_Address"),
                        LocationMasterActive = r.Field<bool>("LOC_active"),
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


    }
}