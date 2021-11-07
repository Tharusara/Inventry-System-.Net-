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
    public class LocationTransferController : Controller
    {
        // GET: LocationTransfer
        LocationTransferBLL _LTBLL = new LocationTransferBLL();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveLT(LocationTransferDOM lt)
        {
            try
            {
                if (!string.IsNullOrEmpty(lt.TRANS_H_Code))
                    throw new Exception("Sorry You Can't Update Location Transfer");
                if (lt.TRANS_H_From == 0)
                    throw new Exception("Please Select From Location");
                if (lt.TRANS_H_To == 0)
                    throw new Exception("Please Select To Location");
                if (lt.Items==null)
                    throw new Exception("Please Add Items to Location Transfer");
                lt = _LTBLL.saveLT(lt);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "Location transfer saved successfully.", code = lt.TRANS_H_Code, id = lt.TRANS_H_Id });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult getAllLT(int? start, int? length)
        {
            try
            {
                var LT = _LTBLL.getAllLTs().OrderBy(x => x.TRANS_H_Code).ToList();
                int totRecords = LT.Count;

                if (start != null)
                {
                    LT = LT.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    LT = LT.Take(length.Value).ToList();
                }

                var LTs_Modified = LT.Select(x => new
                {
                    LT_ID = x.TRANS_H_Id,
                    LT_Code = x.TRANS_H_Code,
                    LT_Date = x.TRANS_H_Date.ToString("dd/MM/yyyy"),
                    LT_Amount = x.TRANS_H_Value,
                    LT_To_Location = x.TRANS_H_To,
                    LT_From_Location = x.TRANS_H_From,
                    LT_User = x.TRANS_H_User,
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = LTs_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult selectLT(long LT_ID)
        {
            try
            {
                var LT = _LTBLL.selectGRN(LT_ID);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "", data = LT });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }
    }
}