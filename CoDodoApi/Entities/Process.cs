using System.Text;

namespace CoDodoApi.Entities;

public sealed class Process(string name,
    Opportunity opportunity,
    string status,
    DateTimeOffset createdDate,
    DateTimeOffset updatedDate,
    TimeProvider provider)
{
    public string Name { get; set; } = name;
    public Opportunity Opportunity { get; set; } = opportunity;
    public string Status { get; set; } = status;
    public DateTimeOffset CreatedDate { get; set; } = createdDate;
    public DateTimeOffset UpdatedDate { get; set; } = updatedDate;
    public TimeProvider TimeProvider { get; set; } = provider;

    public int DaysSinceUpdate()
    {
        TimeSpan diff = TimeProvider.GetUtcNow() - UpdatedDate;

        return NumberOfWholeDays(diff);
    }

    public int DaysSinceCreation()
    {
        TimeSpan diff = TimeProvider.GetUtcNow() - CreatedDate;

        return NumberOfWholeDays(diff);
    }

    internal string Key()
    {
        string stringKey = Name + Opportunity.UriForAssignment;

        byte[] bytes = Encoding.UTF8.GetBytes(stringKey);

        return Convert.ToBase64String(bytes);
    }

    static int NumberOfWholeDays(TimeSpan diff)
    {
        double numberOfDays = diff.TotalDays;

        return (int)numberOfDays;
    }

    internal bool IsWon()
    {
        return Status == "WON";
    }
}

internal static class ProcessExtensions
{
    public static ProcessDTO ToDto(this Process process)
    {
        //Process p = process;
        Opportunity opportunity = process.Opportunity;

        return new ProcessDTO(process.Name,
            opportunity.UriForAssignment,
            opportunity.Company,
            opportunity.Capability,
            process.Status,
            opportunity.NameOfSalesLead,
            opportunity.HourlyRateInSEK,
            process.UpdatedDate,
            process.CreatedDate,
            process.DaysSinceUpdate(),
            process.DaysSinceCreation());
    }
}