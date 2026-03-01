using CoDodoApi.Entities;
using CoDodoApi.Services;
using CoDodoApi.Services.EFService;
using Microsoft.AspNetCore.Mvc;

namespace CoDodoApi;

public static class Endpoints
{
    public static async Task<IResult> DeleteProcess([FromBody] DeleteProcessDTO dto,
        TimeProvider provider,
        ILoggerFactory logger,
        CoDodoDbContext dbContext)
    {
        try
        {
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.UriForAssignment))
            {
                ErrorDTO errorDto = new("Name and UriForAssignment are required.", 400);
                return TypedResults.BadRequest(errorDto);
            }
            Process process = dto.ToProcess(provider);

            EFProcess? dbProcess = await dbContext.GetProcessByKey(dto.Name, dto.UriForAssignment);

            if (dbProcess is null)
            {
                ErrorDTO errorDto = new($"Process with name {process.Name} and UriForAssignment {process.Opportunity.UriForAssignment} not found.", 404);
                return TypedResults.NotFound(errorDto);
            }

            dbContext.Processes.Remove(dbProcess);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok(dbProcess.ToProcess(provider).ToDto());
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(DeleteProcess)}: {ex.Message}");
            ProblemDetails problemDetails = new()
            {
                Title = "An error occurred while deleting the process.",
                Detail = ex.Message,
                Status = 500
            };
            return TypedResults.Problem(problemDetails);
        }
    }

    public static async Task<IResult> CreateProcess(CreateProcessDTO dto,
        TimeProvider provider,
        ILoggerFactory logger,
        CoDodoDbContext dbContext)
    {
        try
        {
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.UriForAssignment))
            {
                ErrorDTO errorDto = new("Name and UriForAssignment are required.", 400);
                return TypedResults.BadRequest(errorDto);
            }
            Process process = dto.ToProcess(provider);

            await dbContext.Processes.AddAsync(process.ToEFProcess());
            await dbContext.SaveChangesAsync();

            return TypedResults.Created(string.Empty, process.ToDto());
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(CreateProcess)}: {ex.Message}");
            ProblemDetails problemDetails = new()
            {
                Title = "An error occurred while creating the process.",
                Detail = ex.Message,
                Status = 500
            };
            return TypedResults.Problem(problemDetails);
        }
    }

    public static async Task<IResult> AllProcesses(
        ILoggerFactory logger,
        TimeProvider provider,
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
            ProblemDetails problemDetails = new()
            {
                Title = "An error occurred while retrieving all processes.",
                Detail = ex.Message,
                Status = 500
            };
            return TypedResults.Problem(problemDetails);
        }
    }

    public static async Task<IResult> UpdateProcess(UpdateProcessDTO dto,
        TimeProvider provider,
        ILoggerFactory logger,
        CoDodoDbContext dbContext)
    {
        try
        {
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.UriForAssignment))
            {
                ErrorDTO errorDto = new("Name and UriForAssignment are required.", 400);
                return TypedResults.BadRequest(errorDto);
            }
            if (!IsValidStatus(dto.Status))
            {
                ErrorDTO errorDto = new($"Invalid status: {dto.Status}. Valid statuses are: OFFERED, INTERVIEW, ASSIGNED, LOST.", 400);
                return TypedResults.BadRequest(errorDto);
            }

            EFProcess? dbProcess = await dbContext.Processes.FindAsync(dto.Name, dto.UriForAssignment);
            if (dbProcess is null)
            {
                ErrorDTO errorDto = new($"Process with name {dto.Name} and UriForAssignment {dto.UriForAssignment} not found.", 404);
                return TypedResults.NotFound(errorDto);
            }

            dbProcess.UpdateProcess(dto, provider);
            dbContext.Processes.Update(dbProcess);
            await dbContext.SaveChangesAsync();

            if (IsWon(dto.Status))
            {
                using HttpClient httpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7069/")
                };
                string endpoint = "/WonOpportunities";
                WonOpportunityDTO wonOpportunityDto = new(dto.Name, dto.UriForAssignment);

                HttpResponseMessage response = await httpClient.PostAsJsonAsync(endpoint, wonOpportunityDto);

                if (!response.IsSuccessStatusCode)
                {
                    ProblemDetails problemDetails = new()
                    {
                        Title = "An error occurred while retrieving all processes.",
                        Detail = "Failed to notify WonOpportunities service.",
                        Status = (int)response.StatusCode
                    };
                    return TypedResults.Problem(problemDetails);
                }
            }

            return TypedResults.Ok(dbProcess.ToProcess(provider).ToDto());
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(UpdateProcess)}: {ex.Message}");
            ProblemDetails problemDetails = new()
            {
                Title = "An error occurred while updating the process.",
                Detail = ex.Message,
                Status = 500
            };
            return TypedResults.Problem(problemDetails);
        }
    }

    public static async Task<IResult> ImportExcelAsync(IFormFile file, ExcelImporter importer)
    {
        try
        {
            await importer.Import(file);

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "An error occurred while importing the Excel file.",
                Detail = ex.Message,
                Status = 500
            };
            return Results.Problem(problemDetails);
        }
    }

    private static bool IsValidStatus(string status)
    {
        string[] validStatuses = ["OFFERED", "INTERVIEW", "ASSIGNED", "LOST"];
        return validStatuses.Contains(status);
    }

    private static bool IsWon(string status)
    {
        return status == "ASSIGNED";
    }
}