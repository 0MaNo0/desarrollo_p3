using System.Data.SqlClient;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificacionesController : ControllerBase
    {
        private readonly string con;

        public CertificacionesController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<Certificacion> Get()
        {
            List<Certificacion> certificaciones = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ObtenerCertificaciones", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Certificacion certificacion = new()
                            {
                                Id = Convert.ToInt32(reader["id_certificacion"]),
                                IdDocumento = Convert.ToInt32(reader["id_documento"]),
                                TipoCertificacion = reader["tipo_certificacion"].ToString(),
                                FechaEmision = Convert.ToDateTime(reader["fecha_emision"]),
                                FechaExpiracion = reader["fecha_expiracion"] as DateTime?,
                                CertificadoUrl = reader["certificado_url"].ToString(),
                                Estado = reader["estado"].ToString()
                            };
                            certificaciones.Add(certificacion);
                        }
                    }
                }
            }
            return certificaciones;
        }

        [HttpPost]
        public void Post([FromBody] Certificacion certificacion)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("InsertarCertificacion", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_documento", certificacion.IdDocumento);
                    cmd.Parameters.AddWithValue("@tipo_certificacion", certificacion.TipoCertificacion);
                    cmd.Parameters.AddWithValue("@fecha_emision", certificacion.FechaEmision);
                    cmd.Parameters.AddWithValue("@fecha_expiracion", certificacion.FechaExpiracion);
                    cmd.Parameters.AddWithValue("@certificado_url", certificacion.CertificadoUrl);
                    cmd.Parameters.AddWithValue("@estado", certificacion.Estado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Certificacion certificacion, int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ActualizarCertificacion", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_certificacion", id);
                    cmd.Parameters.AddWithValue("@id_documento", certificacion.IdDocumento);
                    cmd.Parameters.AddWithValue("@tipo_certificacion", certificacion.TipoCertificacion);
                    cmd.Parameters.AddWithValue("@fecha_emision", certificacion.FechaEmision);
                    cmd.Parameters.AddWithValue("@fecha_expiracion", certificacion.FechaExpiracion);
                    cmd.Parameters.AddWithValue("@certificado_url", certificacion.CertificadoUrl);
                    cmd.Parameters.AddWithValue("@estado", certificacion.Estado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("EliminarCertificacion", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_certificacion", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
