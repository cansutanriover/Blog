using MyBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Panel.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            using (CategoryService service=new CategoryService())
            {
                if (Session["loginuser"] == null)
                {
                    return RedirectToAction("Index", "Account");
                }
                var categories = service.GetCategories();
                return View(categories); 
            }
        }

        public ActionResult Get(int id)
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}