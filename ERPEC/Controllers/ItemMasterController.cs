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
    public class ItemMasterController : Controller
    {
        ItemMasterBLL _ItemMasterBLL = new ItemMasterBLL();
        CategoryMasterBLL _CategoryMasterBLL = new CategoryMasterBLL();
        // GET: ItemMaster
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveItem(ItemMasterDOM Item)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Item.ITEM_code))
                    throw new Exception("Please Enter Code");
                if (string.IsNullOrWhiteSpace(Item.ITEM_name))
                    throw new Exception("Please Enter Name");
                if (Item.ITEM_cat==null)
                    throw new Exception("Please Select Catagory");
                if (Item.ITEM_price==null)
                    throw new Exception("Please Enter Price");
                if (Item.ITEM_cost == null)
                    throw new Exception("Please Enter Cost Of The Item");
                if (Item.ITEM_cost == null)
                    throw new Exception("Please Enter Min Qty");
                _ItemMasterBLL.saveItem(Item);

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "Item saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult GetAllItems(int? start, int? length,bool? active,long? Item_Id,string Item_code)
        {
            try
            {
                var Items = _ItemMasterBLL.getAll(active, Item_Id,Item_code).OrderBy(x => x.ITEM_code).ToList();
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
                        itemid = x.ITEM_id
                    },
                    ItemID = x.ITEM_id,
                    ItemCode = x.ITEM_code,
                    ItemName = x.ITEM_name,
                    ItemCat = x.ITEM_cat,
                    ItemCatname = _CategoryMasterBLL.getCatagoryName(x.ITEM_cat),
                    ItemCost = x.ITEM_cost,
                    ItemPrice = x.ITEM_price,
                    ItemMinQty = x.ITEM_minqty,
                    ItemActive=x.ITEM_active
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = Items_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult loadItems(int Itm_Loc, long? item_id, string item_code)
        {
            try
            {
                var items = _ItemMasterBLL.loadAvalibleItems(Itm_Loc, item_id, item_code);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "", data = items });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult loadReorderItems(int Itm_Loc, long? item_id, string item_code)
        {
            try
            {
                var items = _ItemMasterBLL.loadAvalibleItems(Itm_Loc, item_id, item_code);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "", data = items });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

       
    }
}