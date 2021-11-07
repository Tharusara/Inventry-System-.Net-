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
    public class SupplierMasterController : Controller
    {
        SupplierMasterBLL _SupplierMasterBLL = new SupplierMasterBLL();
        // GET: SupplierMaster
        public ActionResult Index()
        {
            string code = getSupCode();
            ViewData["Message"] = code; 
            return View();
        }

        public ActionResult SaveSupplier(SupplierMasterDOM Supplier)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Supplier.SUP_name))
                    throw new Exception("Please Enter Name");
                if (string.IsNullOrWhiteSpace(Supplier.SUP_address))
                    throw new Exception("Please Enter Address");
                if (string.IsNullOrWhiteSpace(Supplier.SUP_contact))
                    throw new Exception("Please Enter Contact Number");
                _SupplierMasterBLL.saveSupplier(Supplier);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "Supplier saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult GetSuppliers(int? start, int? length, bool? IsActive)
        {
            try
            {
                var Suppliers = _SupplierMasterBLL.getAllSuppliers(IsActive).OrderBy(x => x.SUP_code).ToList();
                int totRecords = Suppliers.Count;

                if (start != null)
                {
                    Suppliers = Suppliers.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    Suppliers = Suppliers.Take(length.Value).ToList();
                }

                var Items_Modified = Suppliers.Select(x => new
                {
                    DT_RowAttr = new
                    {
                        supid = x.SUP_id
                    },
                    SupplierID=x.SUP_id,
                    SupplierCode=x.SUP_code,
                    SupplierName=x.SUP_name,
                    SupplierAddress=x.SUP_address,
                    SupplierContact=x.SUP_contact,
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = Items_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public string getSupCode()
        {
            var code=_SupplierMasterBLL.getSupplierCode();
            return code.FirstOrDefault().SUP_code;
        }
    }
}