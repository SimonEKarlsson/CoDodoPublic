using ClosedXML.Excel;
using CoDodoApi.Entities;
using CoDodoApi.Services.EFService;

namespace CoDodoApi.Services;

public record ExcelImporter(ProcessInMemoryStore Store,
    TimeProvider Provider,
    ILogger<ExcelImporter> Logger,
        CoDodoDbContext dbContext)
{
    readonly ProcessInMemoryStore store = Store;
    readonly TimeProvider timeProvider = Provider;
    readonly ILogger logger = Logger;

    public void Import(IFormFile file)
    {
        try
        {
            Stream readStream = file.OpenReadStream();

            using XLWorkbook wb = new(readStream);

            IXLWorksheet ws = wb.Worksheet(1);

            IXLRows rows = ws.Rows();

            IEnumerable<Process> processes = rows
                .Skip(1)
                .TakeWhile(x => !x.Cell(1).IsEmpty())
                .Select(RowToProcess);

            _ = processes.Select(store.Add).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception in {nameof(ExcelImporter)}");
            throw ex;
        }
    }

    Process RowToProcess(IXLRow row)
    {
        IXLCell NAME = row.Cell(1);
        IXLCell CAPABILITY = row.Cell(2);
        IXLCell OPPORTUNITY = row.Cell(3);
        IXLCell STATUS = row.Cell(4);
        IXLCell SALESLEAD = row.Cell(5);
        IXLCell HOURLYRATE = row.Cell(6);
        IXLCell LASTUPDATE = row.Cell(7);
        IXLCell GENERATIONDATE = row.Cell(8);

        string name = NAME.GetValue<string>();
        string capability = CAPABILITY.GetValue<string>();
        string company = OPPORTUNITY.GetValue<string>();
        string status = STATUS.GetValue<string>();
        string salesLead = SALESLEAD.GetValue<string>();
        HOURLYRATE.TryGetValue(out int hourlyRate);
        string lu = LASTUPDATE.GetValue<string>();
        string gd = GENERATIONDATE.GetValue<string>();

        DateTime LastUpdate = DateTime.Parse(lu);
        DateTime generationDate = DateTime.Parse(gd);

        string uri = Guid.NewGuid().ToString();

        Opportunity opportunity = new(
            uri,
            company,
            capability,
            salesLead,
            hourlyRate);

        return new(
            name,
            opportunity,
            status,
            generationDate,
            LastUpdate,
            timeProvider);
    }
}
