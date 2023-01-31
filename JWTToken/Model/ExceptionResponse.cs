using System.Text.Json;

namespace JWTToken.Models
{
    public class ExceptionResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        //serialize this object as a json string
        public override string ToString()
        {
            return JsonSerializer.Serialize(this); 
        }
    }
}
