using System.Net;
using System.Web;
using System.Web.Http;
using WebsiteStatisticsAPI.Data.Interfaces;
using WebsiteStatisticsAPI.Models;

namespace WebsiteStatisticsAPI.Controllers
{
    [RoutePrefix("api/websiteinfo")]
    public class WebsiteInfoController : ApiController
    {
        private readonly IWebsiteInfoService _iService;
        //Using DI to inject iService into the Website Info Controller
        public WebsiteInfoController(IWebsiteInfoService iService)
        {
            _iService = iService;
        }
        
        //Summary
        //Retrieve webpage statistics
        [Route("loadUrl")]
        [HttpPost]        
        public ApiResultModel<WebsiteStats> LoadUrl([FromBody]UrlModel inputUrl)
        {
            var websitestats = _iService.GetAllData(HttpUtility.UrlDecode(inputUrl.urlWeb));
            if (websitestats == null)
            {
                return new ApiResultModel<WebsiteStats>
                {
                    Result = null,
                    HttpStatusCode = HttpStatusCode.NoContent
                };
            }

            return new ApiResultModel<WebsiteStats>
            {
                Result = websitestats,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
