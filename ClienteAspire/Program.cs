using ClienteAspire;
using FBS_ComponentesDinamicos.Entidades.Autenticacion;
using FBS_ComponentesDinamicos.Sevices;
using FBS_ComponentesDinamicos.Utiles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using Sotsera.Blazor.Toaster.Core.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["apiUrl"]) }/*.EnableIntercept(sp)*/);

builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<MudBlazor.DialogService>();
builder.Services.AddScoped<Radzen.NotificationService>();
builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>();
builder.Services.AddScoped<RefrescarTokenService>();
builder.Services.AddToaster(config =>
{
    config.PositionClass = Defaults.Classes.Position.TopRight;
    config.PreventDuplicates = true;
    config.NewestOnTop = false;
});

builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddOptions();
builder.Services.AddScoped<IObtenerNombresServices, ObtenerNombresService>();
builder.Services.AddScoped<IValidadorLlavePrimariaService, ValidadorLlavePrimariaService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();


var httpClient = builder.Build().Services.GetRequiredService<HttpClient>();
var navigationManager = builder.Build().Services.GetRequiredService<NavigationManager>();
var authenticationStateProvider = builder.Build().Services.GetRequiredService<AuthenticationStateProvider>();
var localStorage = builder.Build().Services.GetRequiredService<ILocalStorageService>();
var js = builder.Build().Services.GetRequiredService<IJSRuntime>();
var httpService = builder.Build().Services.GetRequiredService<IHttpService>();
var usuarioLogin = await localStorage.GetItem<UsuarioLogin>("usuario");
builder.Services.AddScoped<IAuthenticationService>(sp =>
    new AutenticacionService(navigationManager, localStorage, httpClient, authenticationStateProvider, js, httpService) { usuarioLogin = usuarioLogin });

await builder.Build().RunAsync();
