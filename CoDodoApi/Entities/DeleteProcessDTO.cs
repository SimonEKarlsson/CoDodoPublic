namespace CoDodoApi.Entities;

public sealed class DeleteProcessDTO
{
    public string Name { get; set; } = string.Empty;
    public string UriForAssignment { get; set; } = string.Empty;
}

public static class DeleteProcessDtoExtensions
{
    public static Process ToProcess(this DeleteProcessDTO dto, TimeProvider provider)
    {
        string uriForAssignment = string.Empty;
        string company = string.Empty;
        string capability = string.Empty;
        string nameOfSalesLead = string.Empty;
        int hourlyRateInSEK = 0;
        Opportunity details = new(uriForAssignment, company, capability, nameOfSalesLead, hourlyRateInSEK);

        return new Process(dto.Name,
            details,
            "",
            provider.GetUtcNow(),
            provider.GetUtcNow(),
            provider);
    }
}