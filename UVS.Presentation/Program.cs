using UVS.Application;
using UVS.Infrastructure;
using UVS.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAntiforgery();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.DocumentTitle = "UVS API";
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "UVS API");
    });
    app.ApplyMigration();

}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.Run();