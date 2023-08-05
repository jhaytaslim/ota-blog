using API.Extensions;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Application.Mapper;
using Application.Helpers;
using API.Middlewares;
using Infrastructure.Data.Seeders.Org;
using System.Text.Json.Serialization;
using Hangfire;
using Grpc.Core;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureKestrelForGrpc();

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureServices();
builder.Services.AddInfrastructureServices();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureIisIntegration();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureApiVersioning(builder.Configuration);
builder.Services.ConfigureMvc();
builder.Services.ConfigureHangfire(builder.Configuration);
builder.Services.ConfigureAzureStorageServices();
builder.Services.ConfigureControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHangfireDashboard();
}
else
{
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangFireAuthorizationFilter(builder.Configuration) }
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseErrorHandler();

app.MapControllers();

WebHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.Run();

public partial class Program
{

}
