using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
            
            using (var client = new HttpClient())
            {
                var loginRequest = new LoginRequest()
                {
                    Usuario = "AAMARO",
                    Password = "123456",
                    UsaHuellaDigital = false,
                    Maquina = string.Empty,
                    NumeroDeIntento = 1,
                    IPMaquinaIngreso = "10.100.0.25"
                };
                var responseLogin = await client.PostAsJsonAsync(Constantes.URL_AUTH, loginRequest);
                
                var loginResponse = await responseLogin.Content.ReadFromJsonAsync<LoginResponse>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.AccessToken);
                
                var response = await client.PostAsJsonAsync(Constantes.URL_LOGS, request);
                if (response.IsSuccessStatusCode)
                {
                    var composeAuditLog = await response.Content.ReadFromJsonAsync<ComposeAuditLog>();
                    return Ok(composeAuditLog);
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
