using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class UserMasterBLL
    {
        UserMasterDAL _UserMasterDAL=new UserMasterDAL();

        public UserMasterDOM saveUser(UserMasterDOM user)
        {
            return _UserMasterDAL.Save(null, null, user);
        }

        public List<UserMasterDOM> getUsers(int? userID)
        {
            return _UserMasterDAL.GetUsers(null, null, userID);
        }

        public UserMasterDOM checkUser(UserMasterDOM user)
        {
            return _UserMasterDAL.ChekUser(null, null, user);
        }
    }
}