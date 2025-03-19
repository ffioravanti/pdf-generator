using Microsoft.OpenApi.Models;
using pdf_generator.Endpoints;
using pdf_generator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

// Adicione esses serviços
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PDF Generator API", Version = "v1" });
});


// Registrar serviços
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Registrar endpoints
app.MapPdfEndpoints();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PDF Generator v1");
});

app.Run();