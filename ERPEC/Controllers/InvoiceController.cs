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
    public class InvoiceController : Controller
    {
        // GET: Invoice
        InvoiceBLL _InvoiceBLL = new InvoiceBLL();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sales()
        {
            return View();
        }

        public ActionResult SaveInvoice(InvoiceDOM Invoice)
        {
            try
            {
                if (!string.IsNullOrEmpty(Invoice.INV_Code))
                    throw new Exception("Sorry You Update Can't Invoice");
                if (Invoice.INV_Location == 0)
                    throw new Exception("Please Select A Location");
                if (Invoice.Items == null || Invoice.Items.Count == 0)
                    throw new Exception("Please Add Items to GRN");
                Invoice = _InvoiceBLL.saveInvoice(Invoice);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "GRN saved successfully.", code = Invoice.INV_Code, id = Invoice.INV_HID });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult getAllInvoices(int? start, int? length, DateTime? fromdate, DateTime? todate)
        {
            try
            {
                var Invoice = _InvoiceBLL.getAllInvoices(fromdate,todate).OrderBy(x => x.INV_Code).ToList();
                int totRecords = Invoice.Count;

                if (start != null)
                {
                    Invoice = Invoice.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    Invoice = Invoice.Take(length.Value).ToList();
                }

                var Invs_Modified = Invoice.Select(x => new
                {
                    INV_ID = x.INV_HID,
                    INV_Code = x.INV_Code,
                    INV_Date = x.INV_Date.ToString("dd/MM/yyyy"),
                    INV_Amount = x.INV_Value,
                    INV_Location = x.INV_Location,
                    INV_User = x.INV_H_User,
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = Invs_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult selectInvoice(long INV_ID)
        {
            try
            {
                var invoice = _InvoiceBLL.selectInvoice(INV_ID);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "", data = invoice });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult getFastMoivingItems(DateTime? fromdate, DateTime? todate)
        {
            try
            {
                var invoice = _InvoiceBLL.getFastMoving(fromdate, todate);
                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "", data = invoice });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }
         
    }
}