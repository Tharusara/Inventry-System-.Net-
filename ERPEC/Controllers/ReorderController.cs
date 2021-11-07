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
    public class ReorderController : Controller
    {
        // GET: Reorder
        ItemMasterBLL _ItemMasterBLL = new ItemMasterBLL();
        ROBLL _ROBLL = new ROBLL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveRO(ReOrderDOM RO)
        {
            try
            {
                if(RO.RO_ID!=0)
                    throw new Exception("You Can't Update Reorder");
                if (RO.RO_Location == 0)
                    throw new Exception("Please Select A Location");
                if (RO.Items.Count == 0)
                    throw new Exception("Please Add Items to Reorder");
                RO = _ROBLL.saveRO(RO);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "Reorder saved successfully.", code = RO.RO_Code, id = RO.RO_ID });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult getAllReordes(int? start, int? length)
        {
            try
            {
                var RO = _ROBLL.getAllROs().OrderBy(x => x.RO_Code).ToList();
                int totRecords = RO.Count;

                if (start != null)
                {
                    RO = RO.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    RO = RO.Take(length.Value).ToList();
                }

                var Ros_Modified = RO.Select(x => new
                {
                    RO_ID = x.RO_ID,
                    RO_Code = x.RO_Code,
                    RO_Date = x.RO_Date.ToString("dd/MM/yyyy"),
                    RO_Amount = x.RO_Amount,
                    RO_Location = x.RO_Location,
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = Ros_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }
        public ActionResult selectRO(long RO_ID)
        {
            try
            {
                var RO = _ROBLL.selectRO(RO_ID);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "", data = RO });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult loadReorderItems(int? start, int? length,int? Loc_ID)
        {
            try
            {
                var Items = _ItemMasterBLL.loadReOrderItems(Loc_ID).OrderBy(x => x.Item_Code).ToList();
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
                        itemid = x.Item_ID
                    },
                    ItemID = x.Item_ID,
                    ItemCode = x.Item_Code,
                    ItemName = x.Item_Name,
                    ItemPrice = x.Item_Price
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