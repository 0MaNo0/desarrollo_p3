using System.Data.SqlClient;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly string con;

        public PagosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<Pago> Get()
        {
            List<Pago> pagos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ObtenerPagos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pago pago = new()
                            {
                                Id = Convert.ToInt32(reader["id_pago"]),
                                IdMiembro = Convert.ToInt32(reader["id_miembro"]),
                                Monto = Convert.ToDecimal(reader["monto"]),
                                FechaPago = Convert.ToDateTime(reader["fecha_pago"]),
                                TipoPago = reader["tipo_pago"].ToString(),
                                ComprobanteUrl = reader["comprobante_url"].ToString(),
                                Estado = reader["estado"].ToString()
                            };
                            pagos.Add(pago);
                        }
                    }
                }
            }
            return pagos;
        }

        [HttpPost]
        public void Post([FromBody] Pago pago)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("InsertarPago", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_miembro", pago.IdMiembro);
                    cmd.Parameters.AddWithValue("@monto", pago.Monto);
                    cmd.Parameters.AddWithValue("@fecha_pago", pago.FechaPago);
                    cmd.Parameters.AddWithValue("@tipo_pago", pago.TipoPago);
                    cmd.Parameters.AddWithValue("@comprobante_url", pago.ComprobanteUrl);
                    cmd.Parameters.AddWithValue("@estado", pago.Estado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Pago pago, int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ActualizarPago", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pago", id);
                    cmd.Parameters.AddWithValue("@id_miembro", pago.IdMiembro);
                    cmd.Parameters.AddWithValue("@monto", pago.Monto);
                    cmd.Parameters.AddWithValue("@fecha_pago", pago.FechaPago);
                    cmd.Parameters.AddWithValue("@tipo_pago", pago.TipoPago);
                    cmd.Parameters.AddWithValue("@comprobante_url", pago.ComprobanteUrl);
                    cmd.Parameters.AddWithValue("@estado", pago.Estado);
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
                using (SqlCommand cmd = new SqlCommand("EliminarPago", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pago", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
