using CoDodoApi.Entities;
using CoDodoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoDodoApi;

static class Endpoints
{
    public static async Task<IResult> DeleteProcess([FromBody] DeleteProcessDTO dto,
        ProcessInMemoryStore store,
        TimeProvider provider,
        ILoggerFactory logger)
    {
        try
        {
            Process process = dto.ToProcess(provider);

            Process result = await store.Delete(process).ConfigureAwait(false);

            return OkProcessDto(result);
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
        ILoggerFactory logger)
    {
        try
        {
            Process process = dto.ToProcess(provider);

            Process result = await store.Add(process);

            return OkProcessDto(result);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(CreateProcess)}: {ex.Message}");
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async Task AllProcesses(ProcessInMemoryStore store,
        ILoggerFactory logger,
        HttpContext context)
    {
        try
        {
            Process[] result = await store.GetAll().ConfigureAwait(false);

            context.Response.StatusCode = 200;

            await context.Response.WriteAsJsonAsync(result);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(AllProcesses)}: {ex.Message}");

            context.Response.StatusCode = 500;
        }
    }

    private static IResult OkProcessDto(Process process)
    {
        ProcessDTO dto = process.ToDto();

        return TypedResults.Ok(dto);
    }

    private static IResult OkProcessesDto(Process[] processes)
    {
        ProcessDTO[] dtos = processes
            .Select(p => p.ToDto())
            .ToArray();

        return TypedResults.Ok(dtos);
    }

    public static IResult ImportExcel(IFormFile file, ExcelImporter importer)
    {
        try
        {
            importer.Import(file);

            return Results.Ok();
        }
        catch
        {
            return Results.Problem("Failed to import excel file.");
        }
    }
}