using System.Data.SqlClient;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly string con;

        public UsuariosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            List<Usuario> usuarios = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ObtenerUsuarios", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario usuario = new()
                            {
                                Id = Convert.ToInt32(reader["id_usuario"]),
                                IdMiembro = reader["id_miembro"] as int?,
                                Username = reader["username"].ToString(),
                                PasswordHash = reader["password_hash"].ToString(),
                                Rol = reader["rol"].ToString(),
                                FechaCreacion = reader["fecha_creacion"] as DateTime?,
                                UltimoAcceso = reader["ultimo_acceso"] as DateTime?
                            };
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }

        [HttpPost]
        public void Post([FromBody] Usuario usuario)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("InsertarUsuario", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_miembro", usuario.IdMiembro);
                    cmd.Parameters.AddWithValue("@username", usuario.Username);
                    cmd.Parameters.AddWithValue("@password_hash", usuario.PasswordHash);
                    cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@fecha_creacion", usuario.FechaCreacion);
                    cmd.Parameters.AddWithValue("@ultimo_acceso", usuario.UltimoAcceso);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Usuario usuario, int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ActualizarUsuario", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_usuario", id);
                    cmd.Parameters.AddWithValue("@id_miembro", usuario.IdMiembro);
                    cmd.Parameters.AddWithValue("@username", usuario.Username);
                    cmd.Parameters.AddWithValue("@password_hash", usuario.PasswordHash);
                    cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@fecha_creacion", usuario.FechaCreacion);
                    cmd.Parameters.AddWithValue("@ultimo_acceso", usuario.UltimoAcceso);
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
                using (SqlCommand cmd = new SqlCommand("EliminarUsuario", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_usuario", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
