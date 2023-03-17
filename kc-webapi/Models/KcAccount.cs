using Newtonsoft.Json;

namespace kc_webapi.Models
{
    public class BaseUserRepresentation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? username { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? email { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? firstName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? lastName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? emailVerified { get; set; } = false;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? enabled { get; set; } = true;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, IList<string>> attributes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? createdTimestamp { get; set; }
    }
    public class GetUserRepresentation : BaseUserRepresentation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }
    }
    public class PostUserRepresentation : BaseUserRepresentation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CredentialRepresentation> credentials { get; set; }
    }
    public class PutUserRepresentation : BaseUserRepresentation
    {

    }
    public class CredentialRepresentation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool temporary { get; set; }
    }
}
