namespace CoDodoApi;

public static class MiddlewareExtensions
{
    public static
    WebApplication MapAllRoutes(this WebApplication app)
    {
        RouteGroupBuilder api = app.MapGroup("/api");

        api.MapPost("/ImportExcel", Endpoints.ImportExcelAsync)
            .DisableAntiforgery()
            .RequireAuthorization();

        RouteGroupBuilder processes = api.MapGroup("/processes");

        processes.MapGet("", Endpoints.AllProcesses)
            .RequireAuthorization();

        processes.MapPost("", Endpoints.CreateProcess)
            .RequireAuthorization();

        processes.MapDelete("", Endpoints.DeleteProcess)
            .RequireAuthorization();

        processes.MapPut("", Endpoints.UpdateProcess)
            .RequireAuthorization();

        return app;
    }
}