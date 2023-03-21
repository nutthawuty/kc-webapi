using kc_webapi.Interfaces;
using kc_webapi.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
// add service to container
builder.Services.AddHttpClient("keycloak-rest-api", httpclient => 
{
    httpclient.BaseAddress = new Uri("http://localhost:8080/admin/realms/education/");
});
builder.Services.AddHttpClient("keycloak-user", httpclient =>
{
    httpclient.BaseAddress = new Uri("http://localhost:8080/realms/education/protocol/openid-connect/");
});
// add MyDependency
builder.Services.AddTransient<IKcService, KcService>();
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
