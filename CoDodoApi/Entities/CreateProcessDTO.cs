namespace CoDodoApi.Entities;

public sealed class CreateProcessDTO
{
    public string Name { get; set; } = string.Empty;
    public string UriForAssignment { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Capability { get; set; } = string.Empty;
    public string NameOfSalesLead { get; set; } = string.Empty;
    public int HourlyRateInSEK { get; set; }
}

public static class CreateProcessDtoExtensions
{
    public static Process ToProcess(this CreateProcessDTO dto, TimeProvider provider)
    {
        Opportunity opportunity = new(dto.UriForAssignment,
            dto.Company,
            dto.Capability,
            dto.NameOfSalesLead,
            dto.HourlyRateInSEK);
        string startStatus = "OFFERED";
        return new Process(dto.Name,
            opportunity,
            startStatus,
            provider.GetUtcNow(),
            provider.GetUtcNow(),
            provider);
    }
}