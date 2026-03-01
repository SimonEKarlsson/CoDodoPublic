using CoDodoApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoDodoApi;

public static class MiddlewareExtensions
{
    public static
    WebApplication MapAllRoutes(this WebApplication app)
    {
        RouteGroupBuilder api = app.MapGroup("/api");

        api.MapPost("/ImportExcel", Endpoints.ImportExcelAsync)
            .DisableAntiforgery()
            .RequireAuthorization()
            .WithName("ImportExcel")
            .WithSummary("Import data from an Excel file")
            .WithDescription("Imports and processes data from an uploaded Excel file")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(200)
            .Produces<ErrorDTO>(401)
            .Produces<ProblemDetails>(500);

        RouteGroupBuilder processes = api.MapGroup("/processes");

        processes.MapGet("", Endpoints.AllProcesses)
            .RequireAuthorization()
            .WithName("GetAllProcesses")
            .WithSummary("Get all processes")
            .WithDescription("Retrieves a list of all processes")
            .Produces<Process[]>(200)
            .Produces<ErrorDTO>(401)
            .Produces<ProblemDetails>(500);

        processes.MapPost("", Endpoints.CreateProcess)
            .RequireAuthorization()
            .WithName("CreateProcess")
            .WithSummary("Create a new process")
            .WithDescription("Creates a new process with the provided details")
            .Produces<ProcessDTO>(200)
            .Produces<ErrorDTO>(401)
            .Produces<ErrorDTO>(400)
            .Produces<ProblemDetails>(500);

        processes.MapDelete("", Endpoints.DeleteProcess)
            .RequireAuthorization()
            .WithName("DeleteProcess")
            .WithSummary("Delete a process")
            .WithDescription("Deletes an existing process")
            .Produces<ProcessDTO>(200)
            .Produces<ErrorDTO>(401)
            .Produces<ErrorDTO>(400)
            .Produces<ProblemDetails>(500);

        processes.MapPut("", Endpoints.UpdateProcess)
            .RequireAuthorization()
            .WithName("UpdateProcess")
            .WithSummary("Update a process")
            .WithDescription("Updates an existing process with new details")
            .Produces<ProcessDTO>(200)
            .Produces<ErrorDTO>(401)
            .Produces<ErrorDTO>(400)
            .Produces<ProblemDetails>(500);

        return app;
    }
}