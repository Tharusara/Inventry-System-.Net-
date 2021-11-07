using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class POBLL
    {
        PODDAL _PODDAL = new PODDAL();
        public PurchaseOrderDOM savePO(PurchaseOrderDOM PO)
        {
            return _PODDAL.Save(null, null, PO);
        }

        public List<PurchaseOrderDOM> getAllPOs(int? IsComplete)
        {
            return _PODDAL.GetAllPOS(null, null, IsComplete);
        }


        public PurchaseOrderDOM selectPO(long PO_ID)
        {
            return _PODDAL.SelectPO(null, null, PO_ID);
        }
    }
}