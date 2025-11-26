using EMSFrontend.Frontend.Clients;
using EMSFrontend.Frontend.Components;
using EMSFrontend.Frontend.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Razor Components
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Load backend URL
var emsApiUrl = builder.Configuration["EMSApiUrl"]
    ?? "http://localhost:5222";

// Shared CookieContainer
var cookieContainer = new CookieContainer();

// Default HttpClient (used for login/register/auth)
builder.Services.AddHttpClient("EMSApi", c =>
{
    c.BaseAddress = new Uri(emsApiUrl);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseCookies = true,
    CookieContainer = cookieContainer,
    AllowAutoRedirect = false     // ⬅️ IMPORTANT (avoid 302 → 405)
});

// Make Http available via DI
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("EMSApi"));

// Typed clients
builder.Services.AddHttpClient<EmployeesClient>(c => c.BaseAddress = new Uri(emsApiUrl))
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseCookies = true,
    CookieContainer = cookieContainer,
    AllowAutoRedirect = false
});

builder.Services.AddHttpClient<DepartmentsClient>(c => c.BaseAddress = new Uri(emsApiUrl))
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseCookies = true,
    CookieContainer = cookieContainer,
    AllowAutoRedirect = false
});

builder.Services.AddScoped<UserContext>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
