using System.Net;

namespace WebsiteStatisticsAPI.Models
{
    public class ApiResultModel<T>
    {
        public T Result { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; }
    }
}