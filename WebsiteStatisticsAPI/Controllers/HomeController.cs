using Newtonsoft.Json;
using System.Web.Mvc;
using System.Linq;
using WebsiteStatisticsAPI.Models;
using WebsiteStatisticsAPI.Helpers;

namespace WebsiteStatisticsAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";            
            return View("~/Views/Home/Images.cshtml");
        }

        [HttpPost]
        public JsonResult GetImages(string WebUrl)
        {
            string JsonData = string.Empty;
            var WebPageData = new WebsiteStats();
            var UrlModel = new UrlModel { urlWeb = WebUrl };
            var ApiUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));            
            var Status = false;
            
            WebPageData = ApiHelper.HttpPostResult<WebsiteStats>(ApiUrl + "api/websiteinfo/loadurl", UrlModel).Result.Result;            
            if (WebPageData != null)
            {
                Status = true;
                WebPageData.WordFrequency = (from item in WebPageData.WordFrequency orderby item.Value descending select item)
                        .Take(10)
                        .ToDictionary(x => x.Key, x => x.Value);
                JsonData = JsonConvert.SerializeObject(WebPageData);                
            }
            return Json(new { success = Status, ImageData = JsonData });
        }
    }
}
