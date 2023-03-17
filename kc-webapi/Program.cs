using kc_webapi.Interfaces;
using kc_webapi.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
// add service to container
builder.Services.AddHttpClient("keycloak-rest-api", httpclient => 
{
    httpclient.BaseAddress = new Uri("http://localhost:8080/admin/realms/education/");
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
