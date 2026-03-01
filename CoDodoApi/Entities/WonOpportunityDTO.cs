namespace CoDodoApi.Entities
{
    public class WonOpportunityDTO(string name, string uri)
    {
        public string Name { get; } = name;
        public string Uri { get; } = uri;
    }
}
