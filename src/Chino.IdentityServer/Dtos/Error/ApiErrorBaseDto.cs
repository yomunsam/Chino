using System.Text.Json.Serialization;

namespace Chino.IdentityServer.Dtos.Error
{
    public class ApiErrorBaseDto
    {

        [JsonPropertyName("error")]
        public ErrorInfo Error { get; set; }

        public class ErrorInfo
        {
            [JsonPropertyName("code")]
            public int ErrorCode { get; set; }

            [JsonPropertyName("msg")]
            public string Message { get; set; }

            [JsonPropertyName("details")]
            public string Details { get; set; }

            [JsonPropertyName("data")]
            public object ErrorData { get; set; }
        }


        public static ApiErrorBaseDto CreateError(int errorCode = 0, string mesage = "", string details = null, object errorData = null)
        {
            var dto = new ApiErrorBaseDto
            {
                Error = new ErrorInfo
                {
                    ErrorCode = errorCode,
                    Message = mesage,
                    Details = details,
                    ErrorData = errorData
                }
            };

            return dto;
        }

    }
}
