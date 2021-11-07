using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class LocationMasterDOM
    {
        public int LocationMasterId { get; set; }
        public string LocationMasterName { get; set; }
        public string LocationMasterCode { get; set; }
        public string LocationMasterAddress { get; set; }
        public bool LocationMasterActive { get; set; }
        public string LocationMasterChq { get; set; }

        public string userId { get; set; }
    }
}