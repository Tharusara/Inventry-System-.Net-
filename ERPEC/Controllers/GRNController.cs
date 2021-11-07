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
    public class GRNController : Controller
    {
        // GET: GRN
        GRNBLL _GRNBLL = new GRNBLL();

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveGRN(GRNDOM grn)
        {
            try
            {
                if(!string.IsNullOrEmpty(grn.GRN_H_Code))
                    throw new Exception("Sorry You Can't Update GRN");
                if (grn.GRN_H_Supplier==0)
                    throw new Exception("Please Select A Supplier");
                if (grn.GRN_H_Location == 0)
                    throw new Exception("Please Select A Location");
                if (grn.Items.Count == 0)
                    throw new Exception("Please Add Items to GRN");
               grn= _GRNBLL.saveGRN(grn);
               return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "GRN saved successfully.", code = grn.GRN_H_Code, id = grn.GRN_H_Id });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult getAllGRN(int? start, int? length)
        {
            try
            {
                var GRN = _GRNBLL.getAllGRNs().OrderBy(x => x.GRN_H_Code).ToList();
                int totRecords = GRN.Count;

                if (start != null)
                {
                    GRN = GRN.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    GRN = GRN.Take(length.Value).ToList();
                }

                var GRNs_Modified = GRN.Select(x => new
                {
                    GRN_ID = x.GRN_H_Id,
                    GRN_Code = x.GRN_H_Code,
                    GRN_Date = x.GRN_H_Date.ToString("dd/MM/yyyy"),
                    GRN_Amount = x.GRN_H_Amount,
                    GRN_Location = x.GRN_H_Location,
                    GRN_Supplier = x.GRN_H_Supplier,
                    GRN_User = x.GRN_H_User,
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = GRNs_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult selectGRN(long GRN_ID)
        {
            try
            {
                var GRN = _GRNBLL.selectGRN(GRN_ID);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "", data = GRN });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

  



        
    }
}