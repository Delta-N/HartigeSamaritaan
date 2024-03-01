using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace RoosterPlanner.Api.Extensions
{
    /// <summary>
    /// HttpExtensions
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// Write response as json async
        /// </summary>
        /// <typeparam name="T">Entity T</typeparam>
        /// <param name="response">Httpresponse</param>
        /// <param name="value">Generic value</param>
        /// <param name="contentType">string contentType</param>
        /// <returns></returns>
        public static async Task WriteJsonAsync<T>(this HttpResponse response, T value, string? contentType = null)
        {
            response.ContentType = contentType ?? "application/json";

            await JsonSerializer.SerializeAsync(response.Body, value);
        }

        /// <summary>
        /// GetStatusCode for exception
        /// </summary>
        /// <param name="exception">BadRequestException</param>
        /// <returns>HttpStatusCode</returns>
        public static HttpStatusCode GetStatusCode(this BadHttpRequestException exception)
            => (HttpStatusCode)exception.StatusCode;
    }
}
