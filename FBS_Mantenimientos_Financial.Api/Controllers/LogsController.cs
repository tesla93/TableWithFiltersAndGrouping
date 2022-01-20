using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
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

            using (var client = new HttpClient())
            {
                var consRequest = new ConsultaAuditoriaOperacionRequest() { Cantidad = 10, Omitir = 0, Filtro = "" };

                var response = await client.PostAsJsonAsync("https://4ym24tt90g-vpce-0d7e145c5e45557ac.execute-api.us-west-2.amazonaws.com/PROD/Logs/ConsultaAuditoriaOperacion/ConsultaOperaciones", consRequest);
                return Ok(await response.Content.ReadFromJsonAsync<List<AuditLog>>());
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
