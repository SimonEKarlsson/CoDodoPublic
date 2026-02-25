namespace CoDodoApi.Entities;

public sealed class DeleteProcessDTO
{
    public string Name { get; set; } = string.Empty;
    public string UriForAssignment { get; set; } = string.Empty; //not in use
}

public static class DeleteProcessDtoExtensions
{
    public static Process ToProcess(this DeleteProcessDTO dto, TimeProvider provider)
    {
        string company = string.Empty;
        string capability = string.Empty;
        string nameOfSalesLead = string.Empty;
        int hourlyRateInSEK = 0;
        Opportunity details = new(dto.UriForAssignment, company, capability, nameOfSalesLead, hourlyRateInSEK);
        string status = string.Empty;
        return new Process(dto.Name,
            details,
            status,
            provider.GetUtcNow(),
            provider.GetUtcNow(),
            provider);
    }
}