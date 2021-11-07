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
    public class LoginController : Controller
    {
        UserMasterBLL _UserMasterBLL = new UserMasterBLL();
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.UserName = TempData["username"];
            ViewBag.Svrmsg = TempData["Svrmsg"];
            return View();
        }

        [AllowAnonymous]
        public ActionResult SubmitLogin(UserMasterDOM user)
        {
            //try
            //{
            //    user=_UserMasterBLL.checkUser(user);
            //    if (user.isValidUser)
            //    {
                    user = new UserMasterDOM
                    {
                        USER_id = 1,
                        USER_name = user.USER_userName,
                        USER_userName = user.USER_userName,
                        USER_password = user.USER_userName,
                        USER_email = user.USER_userName+"@saassaf",
                        USER_contact = "0775456489",
                        USER_Admin = true,
                        isValidUser = true,
                    };

                    Session["LoggedInUser"] = user.USER_userName;
                    Session["UsersName"] = user.USER_name;
                    return RedirectToRoute("Invoice");
            //    }
            //    else
            //        throw new Exception("Incorrect User Name Or Password");
            //}
            //catch (Exception ex)
            //{
            //    TempData["Svrmsg"] = ex.Message;
            //    TempData["username"] = user.USER_userName;
            //    return RedirectToRoute("Logins");
            //}
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToRoute("Logins");
        }
    }
}