using SEOAnalyser.Context;
using SEOAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SEOAnalyser.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Result result = new Result();           
            return View(result);
        }              

        public ActionResult processText(SearchModel sModel)
        {
            try
            {
                List<string> stopWords = SEOProcess.LoadJson();
                Result viewResult = SEOProcess.processText(sModel, stopWords);
                return PartialView("_ResultView", viewResult);
            }catch(Exception ex)
            {               
                return Json(ex.Message);
            }
        }
    }
}