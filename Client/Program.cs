using Client.Auth;
using Shared;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

const string apiClientName = "ApiClient";

var authSettings = builder.Configuration.GetSection("AuthSettings");
builder.Services.Configure<AuthSettings>(authSettings);

builder.Services.AddScoped<HttpClientAuthHandler>();
var apiUrl = builder.Configuration.GetSection("ApiUrl").Value;
builder.Services
    .AddHttpClient(apiClientName,
        client => { client.BaseAddress = new Uri(apiUrl); })
    .AddHttpMessageHandler<HttpClientAuthHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AuthMiddleware>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<AuthMiddleware>();

app.MapGet("/employees", async (IHttpClientFactory httpClientFactory) =>
    {
        var client = httpClientFactory.CreateClient(apiClientName);

        return await client.GetFromJsonAsync<IEnumerable<Employee>>("employees");
    })
    .WithName("GetEmployees")
    .WithOpenApi();

app.MapPost("/employees", async (Employee employee, IHttpClientFactory httpClientFactory) =>
        {
            var client = httpClientFactory.CreateClient(apiClientName);

            return await client.PostAsJsonAsync("employees", employee, SharedJsonContext.Default.Employee);
        }
    )
    .WithName("CreateEmployee")
    .WithOpenApi();

app.MapPut("/employees/{id:guid}", async (Employee employee, Guid id, IHttpClientFactory httpClientFactory) =>
        {
            var client = httpClientFactory.CreateClient(apiClientName);

            return await client.PutAsJsonAsync($"employees/{id}", employee, SharedJsonContext.Default.Employee);
        }
    )
    .WithName("ReplaceEmployee")
    .WithOpenApi();

app.MapPatch("/employees/{id:guid}", async (Employee employee, Guid id, IHttpClientFactory httpClientFactory) =>
        {
            var client = httpClientFactory.CreateClient(apiClientName);

            return await client.PatchAsJsonAsync($"employees/{id}", employee, SharedJsonContext.Default.Employee);
        }
    )
    .WithName("UpdateEmployee")
    .WithOpenApi();

app.MapDelete("/employees/{id:guid}", async (Guid id, IHttpClientFactory httpClientFactory) =>
        {
            var client = httpClientFactory.CreateClient(apiClientName);

            return await client.DeleteAsync($"employees/{id}");
        }
    )
    .WithName("DeleteEmployee")
    .WithOpenApi();

app.Run();