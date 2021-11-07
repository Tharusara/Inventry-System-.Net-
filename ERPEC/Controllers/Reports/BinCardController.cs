using ERPEC.BLL.Reports;
using ERPEC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPEC.Controllers.Reports
{
    [LoginAuthorize]
    public class BinCardController : Controller
    {
        // GET: BinCard
        BINCARDBLL _BINCARDBLL = new BINCARDBLL();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBinData(int? start, int? length, long Item, int Location, DateTime fromdate, DateTime todate)
        {
            try
            {
                var Recs = _BINCARDBLL.getBinCardData(Item, Location, fromdate, todate);
                int totRecords = Recs.Count;

                if (start != null)
                {
                    Recs = Recs.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    Recs = Recs.Take(length.Value).ToList();
                }

                var Items_Modified = Recs.Select(x => new
                {
                    ItemName=x.ITM_Name,
                    Doc_Code=x.SMT_Doc,
                    Item_Qty=x.SMT_Qty,
                    Doc_Date=x.Trans_Date.ToString("dd/MM/yyyy")
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