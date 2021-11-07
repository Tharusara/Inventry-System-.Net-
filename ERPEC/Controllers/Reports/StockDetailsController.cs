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
    public class StockDetailsController : Controller
    {
        STOCKBLL _STOCKBLL = new STOCKBLL();
        // GET: StockDetails
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStockItems(int? start, int? length, bool? IsActive, int? Loc_ID)
        {
            try
            {
                var Items = _STOCKBLL.getStockItems(Loc_ID);
                int totRecords = Items.Count;

                if (start != null)
                {
                    Items = Items.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    Items = Items.Take(length.Value).ToList();
                }

                var Items_Modified = Items.Select(x => new
                {
                    DT_RowAttr = new
                    {
                        itemID = x.Item_ID
                    },
                    ItemID = x.Item_ID,
                    ItemCode = x.Item_Code,
                    ItemName = x.Item_Name,
                    Qty = x.Qty,
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