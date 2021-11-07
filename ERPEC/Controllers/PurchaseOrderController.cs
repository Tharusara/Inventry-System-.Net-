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
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        POBLL _POBLL = new POBLL();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SavePO(PurchaseOrderDOM PO)
        {
            try
            {
                if (PO.PO_H_Supplier == 0)
                    throw new Exception("Please Select A Supplier");
                if (PO.PO_H_Location == 0)
                    throw new Exception("Please Select A Location");
                if (PO.Items.Count == 0)
                    throw new Exception("Please Add Items to GRN");
                PO = _POBLL.savePO(PO);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "PO saved successfully.", code = PO.PO_H_Code, id = PO.PO_H_ID });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult getAllPO(int? start, int? length,int? IsComplete)
        {
            try
            {
                var POS = _POBLL.getAllPOs(IsComplete).OrderBy(x => x.PO_H_Code).ToList();
                int totRecords = POS.Count;

                if (start != null)
                {
                    POS = POS.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    POS = POS.Take(length.Value).ToList();
                }

                var POs_Modified = POS.Select(x => new
                {
                    PO_ID = x.PO_H_ID,
                    PO_Code = x.PO_H_Code,
                    PO_Date = x.PO_H_Date.ToString("dd/MM/yyyy"),
                    PO_Amount = x.PO_H_Amount,
                    PO_Location = x.PO_H_Location,
                    PO_Supplier = x.PO_H_Supplier,
                    PO_User = x.PO_H_User,
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = POs_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch(Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult selectPO(long PO_ID)
        {
            try
            {
                var PO = _POBLL.selectPO(PO_ID);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "",data=PO });
            }
            catch(Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

    }
}