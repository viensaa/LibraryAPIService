using System.Net;

namespace LibraryAPI.DomainObject
{
    public class ResponseBase
    {
        public ResponseBase() { }
        public string? Message { get; set; }
        public bool StatusCode { get; set; }
    }
}
