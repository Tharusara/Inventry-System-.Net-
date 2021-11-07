using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class InvoiceBLL
    {
        InvoiceDAL _InvoiceDAL = new InvoiceDAL();

        public InvoiceDOM saveInvoice(InvoiceDOM Invoice)
        {
            return _InvoiceDAL.Save(null, null, Invoice);
        }

        public List<InvoiceDOM> getAllInvoices(DateTime? FromDate, DateTime? ToDate)
        {
            return _InvoiceDAL.GetAllInvoices(null, null,FromDate,ToDate);
        }

        public InvoiceDOM selectInvoice(long Invoice_ID)
        {
            return _InvoiceDAL.SelectInvoice(null, null, Invoice_ID);
        }

        public InvoiceDOM getFastMoving(DateTime? FromDate, DateTime? ToDate)
        {
            return _InvoiceDAL.getFastomoving(null, null, FromDate, ToDate);
        }

    }
}