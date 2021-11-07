using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class LocationMasterBLL
    {
        LocationMasterDAL _LocationMasterDAL = new LocationMasterDAL();

        public LocationMasterDOM saveLocation(LocationMasterDOM LocationMaster)
        {
            return _LocationMasterDAL.Save(null, null, LocationMaster);
        }

        public void deleteLocation(int LocationMasterId) 
        {
            _LocationMasterDAL.Delete(null, null, LocationMasterId);
        }

        public List<LocationMasterDOM> getAllLocations(bool? IsActive,int? locaID)
        {
            return _LocationMasterDAL.GetAllLocations(null, null, IsActive, locaID);
        }
    }
}