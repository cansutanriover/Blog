using MyBlog.Panel.Models;
using MyBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Panel.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            if (Session["loginuser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            using (UserService service = new UserService())
            {
                var user = service.Authenticate(model.UserName, model.PassWord);

                if (user == null)
                {
                    ViewBag.LoginResult = "Hatalı kullanıcı adı veya şifre";
                    return View();
                }
                else
                {
                    Session["loginuser"] = user;
                    Session["username"]=user.FullName;
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("loginuser");

            return RedirectToAction("Index", "Account");
        }
    }
}