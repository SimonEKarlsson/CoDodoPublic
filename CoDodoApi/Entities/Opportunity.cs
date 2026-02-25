namespace CoDodoApi.Entities;

public sealed class Opportunity(string uriForAssignment,
    string company,
    string capability,
    string nameOfSalesLead,
    int hourlyRateInSEK)
{
    public string UriForAssignment { get; set; } = uriForAssignment;
    public string Company { get; set; } = company;
    public string Capability { get; set; } = capability;
    public string NameOfSalesLead { get; set; } = nameOfSalesLead;
    public int HourlyRateInSEK { get; set; } = hourlyRateInSEK;
}