using System.Data.SqlClient;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RenovacionesController : ControllerBase
    {
        private readonly string con;

        public RenovacionesController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<Renovacion> Get()
        {
            List<Renovacion> renovaciones = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ObtenerRenovaciones", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Renovacion renovacion = new()
                            {
                                Id = Convert.ToInt32(reader["id_renovacion"]),
                                IdMiembro = Convert.ToInt32(reader["id_miembro"]),
                                IdPago = Convert.ToInt32(reader["id_pago"]),
                                IdDocumento = Convert.ToInt32(reader["id_documento"]),
                                FechaSolicitud = Convert.ToDateTime(reader["fecha_solicitud"]),
                                FechaAprobacion = reader["fecha_aprobacion"] as DateTime?,
                                Estado = reader["estado"].ToString()
                            };
                            renovaciones.Add(renovacion);
                        }
                    }
                }
            }
            return renovaciones;
        }

        [HttpPost]
        public void Post([FromBody] Renovacion renovacion)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("InsertarRenovacion", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_miembro", renovacion.IdMiembro);
                    cmd.Parameters.AddWithValue("@id_pago", renovacion.IdPago);
                    cmd.Parameters.AddWithValue("@id_documento", renovacion.IdDocumento);
                    cmd.Parameters.AddWithValue("@fecha_solicitud", renovacion.FechaSolicitud);
                    cmd.Parameters.AddWithValue("@fecha_aprobacion", renovacion.FechaAprobacion);
                    cmd.Parameters.AddWithValue("@estado", renovacion.Estado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Renovacion renovacion, int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ActualizarRenovacion", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_renovacion", id);
                    cmd.Parameters.AddWithValue("@id_miembro", renovacion.IdMiembro);
                    cmd.Parameters.AddWithValue("@id_pago", renovacion.IdPago);
                    cmd.Parameters.AddWithValue("@id_documento", renovacion.IdDocumento);
                    cmd.Parameters.AddWithValue("@fecha_solicitud", renovacion.FechaSolicitud);
                    cmd.Parameters.AddWithValue("@fecha_aprobacion", renovacion.FechaAprobacion);
                    cmd.Parameters.AddWithValue("@estado", renovacion.Estado);
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
                using (SqlCommand cmd = new SqlCommand("EliminarRenovacion", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_renovacion", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
