using CoDodoApi.Entities;
using CoDodoApi.Services;
using CoDodoApi.Services.EFService;
using Microsoft.AspNetCore.Mvc;

namespace CoDodoApi;

public static class Endpoints
{
    public static async Task<IResult> DeleteProcess([FromBody] DeleteProcessDTO dto,
        ProcessInMemoryStore store,
        TimeProvider provider,
        ILoggerFactory logger,
        CoDodoDbContext dbContext)
    {
        try
        {
            Process process = dto.ToProcess(provider);

            EFProcess? dbProcess = await dbContext.Processes.FindAsync(dto.Name, dto.UriForAssignment);

            if (dbProcess is null)
            {
                return TypedResults.NotFound($"Process with name {process.Name} and UriForAssignment {process.Opportunity.UriForAssignment} not found.");
            }

            dbContext.Processes.Remove(dbProcess);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok(dbProcess.ToProcess(provider).ToDto());
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(DeleteProcess)}: {ex.Message}");
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async Task<IResult> CreateProcess(CreateProcessDTO dto,
        ProcessInMemoryStore store,
        TimeProvider provider,
        ILoggerFactory logger,
        CoDodoDbContext dbContext)
    {
        try
        {
            Process process = dto.ToProcess(provider);

            await dbContext.Processes.AddAsync(process.ToEFProcess());
            await dbContext.SaveChangesAsync();

            return TypedResults.Created(string.Empty, process.ToDto());
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(CreateProcess)}: {ex.Message}");
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async Task<IResult> AllProcesses(ProcessInMemoryStore store,
        ILoggerFactory logger,
        TimeProvider provider,
        HttpContext context,
        CoDodoDbContext dbContext)
    {
        try
        {
            Process[] result = await dbContext.GetAllProcesses(provider);
            ProcessDTO[] dtos = [.. result.Select(p => p.ToDto())];
            return TypedResults.Ok(dtos);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(AllProcesses)}: {ex.Message}");
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async Task<IResult> ImportExcelAsync(IFormFile file, ExcelImporter importer)
    {
        try
        {
            await importer.Import(file);

            return Results.NoContent();
        }
        catch
        {
            return Results.Problem("Failed to import excel file.");
        }
    }
}