using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi.Models;
using Serilog;
using UVS.Api.Extensions;
using UVS.Api.Middleware; 
using UVS.Common.Application;
using UVS.Common.Infrastructure;
using UVS.Modules.Authentication.Infrastructure;
using UVS.Modules.System.Infrastructure; 

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>configuration.ReadFrom.Configuration(context.Configuration));


builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token like this: Bearer {your token}"
    });

   
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


builder.Services.AddControllers()
    .PartManager.ApplicationParts.Add(new AssemblyPart(UVS.Common.Presentation.AssemblyReference.Assembly));
// builder.Services.AddControllers()
//     .PartManager
//     .ApplicationParts
//     .Add(new AssemblyPart(AssemblyReference.Assembly));

builder.Services.AddAutoMapper(UVS.Modules.System.Application.AssemblyReference.Assembly);


builder.Services.AddAntiforgery();

builder.Services.AddInfrastructure(
    builder.Configuration,
    [AuthModule.ConfigureConsumers]);


builder.Services.AddSystemModule(builder.Configuration);
builder.Services.AddAuthModule(builder.Configuration);

Assembly[] modulesAssemblies =
[
    UVS.Modules.System.Application.AssemblyReference.Assembly,
    UVS.Modules.Authentication.Application.AssemblyReference.Assembly,
];

builder.Services.AddApplication(modulesAssemblies);

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

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();