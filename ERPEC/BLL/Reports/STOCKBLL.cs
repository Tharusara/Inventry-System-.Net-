using ERPEC.DAL.Reports;
using ERPEC.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL.Reports
{
    public class STOCKBLL
    {
        STOCKDAL _STOCKDAL = new STOCKDAL();

        public List<STOCKDOM> getStockItems(int? Location)
        {
            return _STOCKDAL.getStockData(null, null, Location);
        }

    }
}