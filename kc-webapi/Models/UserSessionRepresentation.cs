using Swashbuckle.AspNetCore.SwaggerGen;

namespace kc_webapi.Models
{
    public class UserSessionRepresentation
    {
        public string id { get; set; }
        public string ipAddress { get; set; }
        public Int64 lastAccess { get; set; }
        public Int64 start { get; set; }
        public string userId { get; set; }
        public string username { get; set; }

        public Dictionary<string, string> clients { get; set; }
    }
}
