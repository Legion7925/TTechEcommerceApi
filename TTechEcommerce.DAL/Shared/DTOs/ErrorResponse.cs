using Newtonsoft.Json;

namespace TTechEcommerceApi.Shared.Model
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
