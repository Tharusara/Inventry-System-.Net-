using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class ROBLL
    {
        ReOrderDAL _RODAL = new ReOrderDAL();

        public ReOrderDOM saveRO(ReOrderDOM RO)
        {
            return _RODAL.Save(null, null, RO);
        }

        public List<ReOrderDOM> getAllROs()
        {
            return _RODAL.GetAllROS(null, null);
        }

        public ReOrderDOM selectRO(long RO_ID)
        {
            return _RODAL.SelectRO(null, null, RO_ID);
        }
    }
}