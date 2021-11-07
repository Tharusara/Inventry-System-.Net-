using ERPEC.BLL;
using ERPEC.Common;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPEC.Controllers
{
    [LoginAuthorize]
    public class LocationMasterController : Controller
    {
        LocationMasterBLL _LocationMasterBLL = new LocationMasterBLL();
        UserMasterBLL _UserMasterBLL = new UserMasterBLL();
        // GET: LocationMaster
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveLocation(LocationMasterDOM Location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Location.LocationMasterName))
                    throw new Exception("Please Enter Name");
                if (string.IsNullOrWhiteSpace(Location.LocationMasterAddress))
                    throw new Exception("Please Enter Address");
                _LocationMasterBLL.saveLocation(Location);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "Item saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult GetLocations(int? start, int? length,bool?IsActive,int? Loc_ID)
        {
            try
            {
                var Locations = _LocationMasterBLL.getAllLocations(IsActive, Loc_ID).OrderBy(x => x.LocationMasterCode).ToList();
                int totRecords = Locations.Count;

                if (start != null)
                {
                    Locations = Locations.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    Locations = Locations.Take(length.Value).ToList();
                }

                var Items_Modified = Locations.Select(x => new
                {
                    DT_RowAttr = new
                    {
                        locid = x.LocationMasterId
                    },
                    LocationID=x.LocationMasterId,
                    LocationCode = x.LocationMasterCode,
                    LocationName = x.LocationMasterName,
                    LocationAddress = x.LocationMasterAddress,
                    LocationActive = x.LocationMasterActive,
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = Items_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

      
    }
}