using HealthyTodo.API.Settings;
using HealthyTodo.BLL;
using HealthyTodo.BLL.Exceptions;
using HealthyTodo.DAL;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder
    .Host
    .ConfigureServices((context, collection) =>
    {
        GlobalSettings globalSettings = context.Configuration.Get<GlobalSettings>();

        collection
            .AddDataAccessLayer(globalSettings.Database)
            .AddBusinessLayer();
    });

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseExceptionHandler(app =>
{
    app.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ForbiddenException => StatusCodes.Status403Forbidden,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new
        {
            error = exception?.Message
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

using (var scope = app.Services.CreateScope())
{
    var mongoInitializer = scope.ServiceProvider.GetRequiredService<MongoInitializer>();
    await mongoInitializer.InitializeAsync();
    
    var seeder = scope.ServiceProvider.GetRequiredService<SampleDataSeeder>();
    await seeder.SeedAsync();
}

app.Run();