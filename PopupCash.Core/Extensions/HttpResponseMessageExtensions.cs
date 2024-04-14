using System.Net;
using System.Net.Http;

namespace PopupCash.Core.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Http Response 에 대한 기본 응답 처리
        /// throw Expception Message를 통해 App 단에서 handling
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public static bool HandleResponseCode(this HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.NotFound:
                    throw new HttpRequestException("Not found");
                case HttpStatusCode.BadRequest:
                    throw new HttpRequestException("Bad request");
                case HttpStatusCode.Unauthorized:
                    throw new HttpRequestException("Unauthorized");
                case HttpStatusCode.Forbidden:
                    throw new HttpRequestException("Forbidden");
                case HttpStatusCode.InternalServerError:
                    throw new HttpRequestException("Internal server error");
                case HttpStatusCode.ServiceUnavailable:
                    throw new HttpRequestException("Service unavailable");
                // 다른 응답 코드에 대한 처리 추가
                default:
                    throw new HttpRequestException($"{response.StatusCode}");
            }
        }
    }
}
