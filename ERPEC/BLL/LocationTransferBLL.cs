using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class LocationTransferBLL
    {
        LocationTransferDAL _LTDAL = new LocationTransferDAL();

        public LocationTransferDOM saveLT(LocationTransferDOM LT)
        {
            return _LTDAL.Save(null, null, LT);
        }

        public List<LocationTransferDOM> getAllLTs()
        {
            return _LTDAL.GetAllLTs(null, null);
        }

        public LocationTransferDOM selectGRN(long LT_ID)
        {
            return _LTDAL.SelectLT(null, null, LT_ID);
        }
    }
}