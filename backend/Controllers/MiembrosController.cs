using System.Data.SqlClient;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiembrosController : ControllerBase
    {
        private readonly string con;

        public MiembrosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<Miembro> Get()
        {
            List<Miembro> miembros = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ObtenerMiembros", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Miembro miembro = new()
                            {
                                Id = Convert.ToInt32(reader["id_miembro"]),
                                Dni = reader["dni"].ToString(),
                                Nombres = reader["nombres"].ToString(),
                                Apellidos = reader["apellidos"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"]),
                                Direccion = reader["direccion"].ToString(),
                                Email = reader["email"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                Universidad = reader["universidad"].ToString(),
                                Titulo = reader["titulo"].ToString(),
                                FechaGraduacion = reader["fecha_graduacion"] as DateTime?,
                                FotoUrl = reader["foto_url"].ToString(),
                                Estado = reader["estado"].ToString(),
                                FechaRegistro = reader["fecha_registro"] as DateTime?
                            };
                            miembros.Add(miembro);
                        }
                    }
                }
            }
            return miembros;
        }

        [HttpPost]
        public void Post([FromBody] Miembro miembro)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("InsertarMiembro", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dni", miembro.Dni);
                    cmd.Parameters.AddWithValue("@nombres", miembro.Nombres);
                    cmd.Parameters.AddWithValue("@apellidos", miembro.Apellidos);
                    cmd.Parameters.AddWithValue("@fecha_nacimiento", miembro.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@direccion", miembro.Direccion);
                    cmd.Parameters.AddWithValue("@email", miembro.Email);
                    cmd.Parameters.AddWithValue("@telefono", miembro.Telefono);
                    cmd.Parameters.AddWithValue("@universidad", miembro.Universidad);
                    cmd.Parameters.AddWithValue("@titulo", miembro.Titulo);
                    cmd.Parameters.AddWithValue("@fecha_graduacion", miembro.FechaGraduacion);
                    cmd.Parameters.AddWithValue("@foto_url", miembro.FotoUrl);
                    cmd.Parameters.AddWithValue("@estado", miembro.Estado);
                    cmd.Parameters.AddWithValue("@fecha_registro", miembro.FechaRegistro);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Miembro miembro, int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ActualizarMiembro", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_miembro", id);
                    cmd.Parameters.AddWithValue("@dni", miembro.Dni);
                    cmd.Parameters.AddWithValue("@nombres", miembro.Nombres);
                    cmd.Parameters.AddWithValue("@apellidos", miembro.Apellidos);
                    cmd.Parameters.AddWithValue("@fecha_nacimiento", miembro.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@direccion", miembro.Direccion);
                    cmd.Parameters.AddWithValue("@email", miembro.Email);
                    cmd.Parameters.AddWithValue("@telefono", miembro.Telefono);
                    cmd.Parameters.AddWithValue("@universidad", miembro.Universidad);
                    cmd.Parameters.AddWithValue("@titulo", miembro.Titulo);
                    cmd.Parameters.AddWithValue("@fecha_graduacion", miembro.FechaGraduacion);
                    cmd.Parameters.AddWithValue("@foto_url", miembro.FotoUrl);
                    cmd.Parameters.AddWithValue("@estado", miembro.Estado);
                    cmd.Parameters.AddWithValue("@fecha_registro", miembro.FechaRegistro);
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
                using (SqlCommand cmd = new SqlCommand("EliminarMiembro", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_miembro", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
