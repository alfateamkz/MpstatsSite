using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MpstatsSite.Helpers;
using MpstatsSite.ViewModels;

namespace MpstatsSite.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Index(string category = "Ершик для унитаза")
        {
            return View(new IndexViewModel()
            {
                Category = category,
                Info = MpstatsParsedHelper.GetCategoryInfo(category),
                OurTotalVolume = MpstatsParsedHelper.GetCategoryInfo(category).OurPercent 
            });
        }
        [HttpGet]
        public ActionResult IndexJson(string category = "Ершик для унитаза")
        {
            return Json(new IndexViewModel()
            {
                Category = category,
                Info = MpstatsParsedHelper.GetCategoryInfo(category),
                OurTotalVolume = MpstatsParsedHelper.GetCategoryInfo(category).OurPercent
            }, JsonRequestBehavior.AllowGet);
        }
    }
}