using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.Models
{
    public class UserMasterDOM
    {
        public int USER_id { get; set; }
        public string USER_name { get; set; }
        public string USER_userName { get; set; }
        public string USER_password { get; set; }
        public string USER_email { get; set; }
        public string USER_contact { get; set; }
        public bool USER_Admin { get; set; }
        public bool isValidUser { get; set; }
    }
}