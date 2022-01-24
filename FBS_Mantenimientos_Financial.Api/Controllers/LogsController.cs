using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FBS_Mantenimientos_Financial.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController: ControllerBase
    {

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ConsultaLogs([FromBody] ConsultaAuditoriaOperacionRequest request, CancellationToken cancellationToken)
        {
            LoginResponse loginResponse;

            using (var client = new HttpClient())
            {
                var loginRequest = new LoginRequest()
                {
                    Usuario = "AAMARO",
                    Password = "123456",
                    UsaHuellaDigital = false,
                    Direcciones = new[] { string.Empty },
                    FechaSistemaCliente = DateTime.Now,
                    Maquina = string.Empty,
                    NumeroDeIntento = 1,
                    IPMaquinaIngreso = "10.100.0.25"
                };
                var responseLogin = await client.PostAsJsonAsync(Constantes.URL_AUTH, loginRequest);
                Console.WriteLine(responseLogin.StatusCode);
                loginResponse = await responseLogin.Content.ReadFromJsonAsync<LoginResponse>();
                Console.WriteLine(loginResponse.AccessToken);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFBTUFSTyIsInJvbGUiOiJJbnRlcm5vIiwiRW1wcmVzYSI6IjEiLCJFcXVpcG9DbGllbnRlIjoiIiwiRmVjaGFTaXN0ZW1hIjoiMy8xLzIwMjIgMDowMDowMCIsIklQTWFxdWluYUluZ3Jlc28iOiIxMC4xMDAuMC4yNSIsIm5iZiI6MTY0MzA2MjI2MCwiZXhwIjoxNjQzMDY0MDYwLCJpYXQiOjE2NDMwNjIyNjAsImlzcyI6IlNpZml6U29mdF9Bc3BpcmUiLCJhdWQiOiJTaWZpelNvZnRfQXNwaXJlIn0.PWDDGtD5Hl2DFCvsnoI7eGQitvUoLYRJ-fX70q5ek8g");
                var response = await client.PostAsJsonAsync(Constantes.URL_LOGS, request);
                System.Console.WriteLine(response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    var auditLogsList = await response.Content.ReadFromJsonAsync<List<AuditLog>>();
                    return Ok(auditLogsList);
                }
                return BadRequest();

            }

          

            }

    }


    public class ConsultaAuditoriaOperacionRequest
    {
        public string Filtro { get; set; }
        public int Cantidad { get; set; }
        public int Omitir { get; set; }
    }
}
