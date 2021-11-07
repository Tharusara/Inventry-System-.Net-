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
    public class CategoryMasterController : Controller
    {
        CategoryMasterBLL _CategoryMasterBLL = new CategoryMasterBLL();
        // GET: CategoryMaster
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveCatagoty(CategoryMasterDOM Catagory)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Catagory.CAT_code))
                    throw new Exception("Please Enter Category Code");
                if (string.IsNullOrWhiteSpace(Catagory.CAT_Name))
                    throw new Exception("Please Enter Category Name");
                _CategoryMasterBLL.saveSupplier(Catagory);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "Catagory saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }
        public ActionResult GetCatagories(int? start, int? length,long? catID)
        {
            try
            {
                var Catagory = _CategoryMasterBLL.geAllCategories(catID).OrderBy(x => x.CAT_code).ToList();
                int totRecords = Catagory.Count;

                if (start != null)
                {
                    Catagory = Catagory.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    Catagory = Catagory.Take(length.Value).ToList();
                }

                var Items_Modified = Catagory.Select(x => new
                {
                    DT_RowAttr = new
                    {
                        catid = x.CAT_id
                    },
                    CATID = x.CAT_id,
                    CATCode = x.CAT_code,
                    CATName = x.CAT_Name,
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