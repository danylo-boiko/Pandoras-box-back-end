using System.Reflection;
using Auth.API.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Description = "API for user authentication",
        Contact = new OpenApiContact
        {
            Email = "kostyabek@gmail.com",
            Name = "Kostiantyn Biektin"
        },
        Title = "TaskMan Authentication API",
        Version = "1.0.0",
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services
    .AddCookieAuthentication()
    .AddDataAccess(builder.Configuration)
    .AddConfigurations(builder.Configuration)
    .AddIdentityConfiguration()
    .AddMediatr()
    .AddCustomServices()
    .AddFluentValidation(o => o.RegisterValidatorsFromAssemblyContaining(typeof(Program)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        o.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
