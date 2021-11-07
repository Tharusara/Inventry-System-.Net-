using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class GRNBLL
    {

        GRNDAL _GRNDAL = new GRNDAL();
        public GRNDOM saveGRN(GRNDOM GRN)
        {
            return _GRNDAL.Save(null,null,GRN);
        }

        public List<GRNDOM> getAllGRNs()
        {
            return _GRNDAL.GetAllGRNS(null, null);
        }

        public GRNDOM selectGRN(long GRN_ID)
        {
            return _GRNDAL.SelectGRN(null, null, GRN_ID);
        }
     
    }
}