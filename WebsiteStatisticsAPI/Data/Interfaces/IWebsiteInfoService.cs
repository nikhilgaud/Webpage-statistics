using WebsiteStatisticsAPI.Models;

namespace WebsiteStatisticsAPI.Data.Interfaces
{
    public interface IWebsiteInfoService
    {
        WebsiteStats GetAllData(string url);
    }
}
