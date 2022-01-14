using Blazored.Modal;
using FBS_ComponentesDinamicos.Entidades.Autenticacion;
using FBS_ComponentesDinamicos.Sevices;
using FBS_ComponentesDinamicos.Utiles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Sotsera.Blazor.Toaster.Core.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace FBS_Manteniemientos_Financial.Cliente
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://192.168.7.224:5301/") });
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["apiUrl"]) }/*.EnableIntercept(sp)*/);
            builder.Services.AddHotKeys();
            builder.Services.AddAntDesign();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<Radzen.NotificationService>();
            builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>();
            builder.Services.AddScoped<RefrescarTokenService>();
            builder.Services.AddBlazoredModal();

            builder.Services.AddToaster(config =>
            {
                config.PositionClass = Defaults.Classes.Position.TopRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = false;
            });

            //builder.Services.AddHttpClientInterceptor();
            //builder.Services.AddScoped<HttpInterceptorService>();
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddOptions();
            builder.Services.AddScoped<IObtenerNombresServices, ObtenerNombresService>();
            builder.Services.AddScoped<IValidadorLlavePrimariaService, ValidadorLlavePrimariaService>();

            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>()
                .AddScoped<IConsultarAsociadoService, ConsultarAsociadoService>();

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
        }
    }
}