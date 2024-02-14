using Microsoft.AspNetCore.Mvc;
using ConexionBD.Models;
using ConexionBD.DTO;
using CapaNegocio;

namespace APIREST.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TiendaController : Controller
    {


        CapaNegocio.APIREST oApiRest = new CapaNegocio.APIREST();

        private readonly ILogger<TiendaController> _logger;

        public TiendaController(ILogger<TiendaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost("InsertarDatos")]
        [ProducesResponseType(typeof(List<SYS_DTO_Ticket>), 200)]
        public async Task<IActionResult> InsertarDatos(List<SYS_DTO_Ticket> oTicket)
        {
            List<string> oArchivosIngresados = null;
            string sError = "";
            int iCode = 500;
            try
            {
                if(!oApiRest.IntegracionDatos(oTicket, ref sError, ref iCode))
                {
                    if(sError == "")
                    {
                        sError = "Error Interno del Servidor";
                        iCode = 500;
                        return StatusCode(iCode, sError);

                    }
                }

                oArchivosIngresados = await oApiRest.InsertarDatos(oTicket);

                if(oArchivosIngresados != null)
                {
                    return Ok(oArchivosIngresados);
                }


            }
            catch (Exception)
            {
                return StatusCode(iCode,"Error Interno del server"); 

                throw;
            }


            return Ok(oArchivosIngresados);
        }


    }



    
}
