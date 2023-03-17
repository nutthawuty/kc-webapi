namespace kc_webapi.Models
{
    public class BaseUserRepresentation
    {
        public string username { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool emailVerified { get; set; }
        public bool enabled { get; set; }
        public Dictionary<string, IList<string>> attributes { get; set; }
        public long? createdTimestamp { get; set; }
    }
    public class GetUserRepresentation : BaseUserRepresentation
    {
        public string id { get; set; }
    }
    public class PostUserRepresentation : BaseUserRepresentation
    {
        public List<CredentialRepresentation> credentials { get; set; }
    }
    public class PutUserRepresentation
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

        public class CredentialRepresentation
    {
        public string type { get; set; }
        public string value { get; set; }
        public bool temporary {get;set;}
    }
}
