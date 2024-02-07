using System.Net;

namespace Swallow.Services
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static ServiceResponse<T> Success(T data)
        {
            return new ServiceResponse<T> { Data = data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }

        public static ServiceResponse<T> Failure(string message, HttpStatusCode statusCode)
        {
            return new ServiceResponse<T> { IsSuccess = false, Message = message, StatusCode = statusCode };
        }
    }

}
