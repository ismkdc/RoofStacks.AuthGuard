using Bogus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

var fakerDep = new Faker<Employee>()
    .RuleFor(e => e.Id, f => f.Random.Guid())
    .RuleFor(e => e.Name, f => f.Name.FullName())
    .RuleFor(e => e.Age, f => f.Random.Byte(18, 60))
    .RuleFor(e => e.Gender, f => f.PickRandom<Gender>());

builder.Services.AddSingleton(fakerDep);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = builder.Configuration["Authority"];
        options.Audience = builder.Configuration["Audience"];
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Read", policy => { policy.RequireClaim("scope", "employee.read"); })
    .AddPolicy("Create", policy => { policy.RequireClaim("scope", "employee.write"); })
    .AddPolicy("Update", policy => { policy.RequireClaim("scope", "employee.update"); })
    .AddPolicy("Delete", policy => { policy.RequireClaim("scope", "employee.delete"); });

var app = builder.Build();

app.MapGet("/employees", (Faker<Employee> faker) =>
    {
        var employees = faker.Generate(10);

        return Results.Ok(employees);
    })
    .RequireAuthorization("Read");

app.MapPost("/employees", (Employee employee) =>
    {
        // Save employee to database
        return Results.Ok(employee);
    }
).RequireAuthorization("Create");

app.MapPut("/employees/{id:guid}", (Employee employee, Guid id) =>
    {
        // Replace employee in database
        return Results.Ok(employee);
    }
).RequireAuthorization("Update");

app.MapPatch("/employees/{id:guid}", (Employee employee, Guid id) =>
    {
        // Update employee in database
        return Results.Ok(employee);
    }
).RequireAuthorization("Update");

app.MapDelete("/employees/{id:guid}", (Guid id) =>
    {
        // Delete employee from database
        return Results.NoContent();
    }
).RequireAuthorization("Delete");

app.Run();