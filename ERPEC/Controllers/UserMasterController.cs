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
    public class UserMasterController : Controller
    {
        UserMasterBLL _UserMasterBLL = new UserMasterBLL();
        // GET: UserMaster
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveUser(UserMasterDOM User)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(User.USER_name))
                    throw new Exception("Please Enter Name Of The User");
                if (string.IsNullOrWhiteSpace(User.USER_name))
                    throw new Exception("Please Enter Username");
                if (string.IsNullOrWhiteSpace(User.USER_password))
                    throw new Exception("Please Enter Username");
                if (string.IsNullOrWhiteSpace(User.USER_email))
                    throw new Exception("Please Enter Email");
                if (string.IsNullOrWhiteSpace(User.USER_contact))
                    throw new Exception("Please Enter Contact Number");
                _UserMasterBLL.saveUser(User);

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", SvrMsgBody = "User saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }

        public ActionResult GetUsers(int? start, int? length, int? User_Id)
        {
            try
            {
                var Users = _UserMasterBLL.getUsers(User_Id);
                
                int totRecords = Users.Count;

                if (start != null)
                {
                    Users = Users.Skip(start.Value).ToList();
                }
                if (length != null)
                {
                    Users = Users.Take(length.Value).ToList();
                }

                var Users_Modified = Users.Select(x => new
                {
                   
                    UserID = x.USER_id,
                    UsersName = x.USER_userName,
                    UserName = x.USER_userName,
                    Email = x.USER_email,
                    ContactNo = x.USER_contact
                }).ToList();

                return Json(new { IsSuccess = true, SvrMsgTitle = "Successful", data = Users_Modified, recordsTotal = totRecords, recordsFiltered = totRecords });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, SvrMsgTitle = "Unsuccessful", SvrMsgBody = ex.Message });
            }
        }
    }
}