using System.Data.SqlClient;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly string con;

        public DocumentosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<Documento> Get()
        {
            List<Documento> documentos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ObtenerDocumentos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Documento documento = new()
                            {
                                Id = Convert.ToInt32(reader["id_documento"]),
                                IdMiembro = Convert.ToInt32(reader["id_miembro"]),
                                TipoDocumento = reader["tipo_documento"].ToString(),
                                DocumentoUrl = reader["documento_url"].ToString(),
                                FechaCarga = Convert.ToDateTime(reader["fecha_carga"]),
                                Estado = reader["estado"].ToString()
                            };
                            documentos.Add(documento);
                        }
                    }
                }
            }
            return documentos;
        }

        [HttpPost]
        public void Post([FromBody] Documento documento)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("InsertarDocumento", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_miembro", documento.IdMiembro);
                    cmd.Parameters.AddWithValue("@tipo_documento", documento.TipoDocumento);
                    cmd.Parameters.AddWithValue("@documento_url", documento.DocumentoUrl);
                    cmd.Parameters.AddWithValue("@fecha_carga", documento.FechaCarga);
                    cmd.Parameters.AddWithValue("@estado", documento.Estado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Documento documento, int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ActualizarDocumento", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_documento", id);
                    cmd.Parameters.AddWithValue("@id_miembro", documento.IdMiembro);
                    cmd.Parameters.AddWithValue("@tipo_documento", documento.TipoDocumento);
                    cmd.Parameters.AddWithValue("@documento_url", documento.DocumentoUrl);
                    cmd.Parameters.AddWithValue("@fecha_carga", documento.FechaCarga);
                    cmd.Parameters.AddWithValue("@estado", documento.Estado);
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
                using (SqlCommand cmd = new SqlCommand("EliminarDocumento", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_documento", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
