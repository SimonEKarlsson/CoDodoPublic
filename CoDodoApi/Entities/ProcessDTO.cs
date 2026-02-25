namespace CoDodoApi.Entities;

public sealed
class ProcessDTO(string name,
    string uriForAssignment,
    string company,
    string capability,
    string status,
    string nameOfSalesLead,
    int hourlyRateInSEK,
    DateTimeOffset updatedDate,
    DateTimeOffset createdDate,
    int daysSinceUpdate,
    int daysSinceCreation)
{
    public string Name { get; set; } = name;
    public string UriForAssignment { get; set; } = uriForAssignment;
    public string Company { get; set; } = company;
    public string Capability { get; set; } = capability;
    public string Status { get; set; } = status;
    public string NameOfSalesLead { get; set; } = nameOfSalesLead;
    public int HourlyRateInSEK { get; set; } = hourlyRateInSEK;
    public DateTimeOffset UpdatedDate { get; set; } = updatedDate;
    public DateTimeOffset CreatedDate { get; set; } = createdDate;
    public int DaysSinceUpdate { get; set; } = daysSinceUpdate; //not in use
    public int DaysSinceCreation { get; set; } = daysSinceCreation; //not in use
}

public static class ProcessDtoExtensions
{
    public static Process ToProcess(this ProcessDTO dto, TimeProvider provider)
    {
        Opportunity opportunity = new(dto.UriForAssignment,
            dto.Company,
            dto.Capability,
            dto.NameOfSalesLead,
            dto.HourlyRateInSEK);

        return new Process(dto.Name,
            opportunity,
            dto.Status,
            dto.CreatedDate,
            dto.UpdatedDate,
            provider);
    }
}