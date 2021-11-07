using ERPEC.DAL.Reports;
using ERPEC.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL.Reports
{
    public class BINCARDBLL
    {
        BINCARDDAL _BINCARDDAL = new BINCARDDAL();

        public List<BINCARDDOM> getBinCardData(long Item, int Location, DateTime fromdate, DateTime todate)
        {
            return _BINCARDDAL.getBinCardData(null, null, Item, Location,fromdate,todate);
        }
    }
}