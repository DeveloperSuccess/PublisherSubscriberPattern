using Microsoft.OpenApi.Models;
using PubSub.Application;
using PubSub.Domain.Interfaces;
using PubSub.Domain.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.CustomSchemaIds(type => type.ToString());
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "PubSub.Api", Version = "1.0" });

    var currentAssembly = Assembly.GetExecutingAssembly();
    var xmlDocs = currentAssembly.GetReferencedAssemblies()
    .Union(new[] { currentAssembly.GetName() })
    .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
    .Where(f => File.Exists(f)).ToList();

    xmlDocs.ForEach(xmlDoc => config.IncludeXmlComments(xmlDoc, true));
});

builder.Services.AddSingleton<IPublisherSubscriberManager, PublisherSubscriberManager>();

builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
