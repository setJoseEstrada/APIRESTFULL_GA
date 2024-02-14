using ConexionBD.DTO;
using ConexionBD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CapaNegocio
{

	public enum eTipoOperacion
	{
		Error,
		Procesados
	}
    public class APIREST
    {

		evaluacion_jestradaContext _context = new evaluacion_jestradaContext();

        public async Task<List<string>> InsertarDatos(List<SYS_DTO_Ticket> oTicket)
        {
			List<string> sLista = new List<string>();
			string sNombre = "";
			using (var Transaction = _context.Database.BeginTransaction())
			{

				try
				{

						foreach (var item in oTicket)
						{
						sNombre = item.sNombreArchivo;
							await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertarTicket {item.sTienda}, {item.sRegistradora},{item.sFecha},{item.sTicket},{item.dImporteImpuesto},{item.dTotal},{DateTime.Now}");
							sLista.Add(item.sNombreArchivo);
							
							GuardarLog(item.sNombreArchivo, eTipoOperacion.Procesados);


                        }

                    Transaction.Commit();


                }
				catch (Exception ex)
				{
                    Transaction.Rollback();
                    GuardarLog(sNombre, eTipoOperacion.Error);
                    
                    throw;
				}
            }

            return sLista;
        }



        public void GuardarLog(string sNombreArchivo, eTipoOperacion eTipo)
        {
            string sRutaLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            string sNombreArchivoLog = "";

            switch (eTipo)
            {
                case eTipoOperacion.Error:
                    sNombreArchivoLog = $"Errores_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                    break;
                case eTipoOperacion.Procesados:
                    sNombreArchivoLog = $"Procesados_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                    break;
                default:
                    break;
            }

            if (!Directory.Exists(sRutaLog))
            {
                Directory.CreateDirectory(sRutaLog);
            }

            string sArchivo = Path.Combine(sRutaLog, sNombreArchivoLog);

            using (StreamWriter swriter = new StreamWriter(sArchivo, true))
            {
                if (eTipo == eTipoOperacion.Error)
                {
                    swriter.WriteLine($"{sNombreArchivo},{DateTime.Now}_error");
                }
                else
                {
                    swriter.WriteLine($"{sNombreArchivo},{DateTime.Now} procesado con éxito");
                }
                swriter.Flush(); // Forzar la escritura de los datos en el archivo
            }
        }



        public bool IntegracionDatos(List<SYS_DTO_Ticket> oTicket, ref string sError, ref int iCode)
		{

			bool result = false;
			try
			{

                foreach (var item in oTicket)
                {

                    var propiedades = item.GetType().GetProperties();

                    foreach (var unicos in propiedades)
                    {
                        var valor = unicos.GetValue(item);
                        if (string.IsNullOrEmpty(valor.ToString()))
                        {
                            sError = "Elemento vacio";
                            iCode = 400;

                            result = false;
                            break;
                        }
                    }
                }


				result = true;

				
            }
            catch (Exception)
			{

				throw;
			}

			return result;
		}





    }
}
