using AutoMapper;
using FoodApp.Api.Extensions;
using FoodApp.Api.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectManagementSystem.Data.Context;
using ProjectManagementSystem.Helper;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicationService(builder.Configuration);

        var app = builder.Build();
        {
            #region Update-Database

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbcontext = services.GetRequiredService<ApplicationDBContext>();
                await dbcontext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An error occured during updating database");
            }

            #endregion

            MapperHandler.mapper = app.Services.GetService<IMapper>();
            TokenGenerator.options = app.Services.GetService<IOptions<JwtOptions>>()!.Value;

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}