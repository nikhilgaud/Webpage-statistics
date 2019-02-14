using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebsiteStatisticsAPI.Models;

namespace WebsiteStatisticsAPI.Helpers
{
    //Summary
    //Helper class to make http calls to Web API
    public static class ApiHelper
    {
        public enum ContentType
        {
            Json
        }

        private static readonly HttpClient HttpClient = new HttpClient();
        public static async Task<ApiResultModel<T>> HttpPostResult<T>(string url, object content) where T : new()
        {
            HttpContent postContent = null;
            try
            {
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                var json = JsonConvert.SerializeObject(content);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                postContent = byteContent;

                var response = HttpClient.PostAsync(url, postContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var payload = await response.Content.ReadAsStringAsync();
                    ApiResultModel<T> result = JsonConvert.DeserializeObject<ApiResultModel<T>>(payload);
                    return result;
                }

                return new ApiResultModel<T>()
                {
                    Message = response.ReasonPhrase,
                    HttpStatusCode = response.StatusCode
                };
            }
            catch (Exception e)
            {
                // TODO; log error
                return new ApiResultModel<T>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Message = e.ToString()
                };
            }
        }
    }
}