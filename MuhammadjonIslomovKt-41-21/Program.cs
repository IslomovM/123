using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Project.DataBase;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<StudentDbCotext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionHandlerMiddleware>();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stoped program because of exception");
}
finally
{
    LogManager.Shutdown();
}