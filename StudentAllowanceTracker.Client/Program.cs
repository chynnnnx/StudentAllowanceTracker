using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using StudentAllowanceTracker.Client.Components;
using StudentAllowanceTracker.Client.Dependency_Injection;
using StudentAllowanceTracker.Client.Security;
using StudentAllowanceTracker.Client.Services;
using StudentAllowanceTracker.Client.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Framework services
builder.Services.AddMudServices();
builder.Services.AddRazorPages();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl");
builder.Services.AddClientServices(apiBaseUrl);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
