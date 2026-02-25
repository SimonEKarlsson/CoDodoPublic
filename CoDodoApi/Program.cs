using CoDodoApi;
using CoDodoApi.Services;
using CoDodoApi.Services.EFService;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddConfiguredSerilog();

IServiceCollection services = builder.Services;
services.AddSwagger();
services.AddSingleton(TimeProvider.System);
services.AddSingleton<ProcessInMemoryStore>();
services.AddScoped<ExcelImporter>();
services.AddConfiguredAuthentication();
services.AddAuthorization();
string dbConnectionstring = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("ConnectionStrings:DefaultConnection is missing in appsettings.json");
services.AddCoDodoDbContext(dbConnectionstring);

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    CoDodoDbContext dbContext = scope.ServiceProvider.GetRequiredService<CoDodoDbContext>();
    if (!await dbContext.Database.CanConnectAsync())
    {
        throw new Exception("Cannot connect to the database. Please check the connection string and database server.");
    }
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapAllRoutes();


app.Run();