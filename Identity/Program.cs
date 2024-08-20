using Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(IdentityServerConfig.GetResources())
    .AddInMemoryApiScopes(IdentityServerConfig.GetScopes())
    .AddInMemoryClients(IdentityServerConfig.GetClients())
    .AddDeveloperSigningCredential();

var app = builder.Build();

app.UseIdentityServer();

app.Run();