using System.Data.SqlClient;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnosController : ControllerBase
    {
        public readonly string con;

        public AlumnosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("conexion");
        } 

        [HttpGet]
        public IEnumerable<Alumno> Get()
        {
            List<Alumno> alumnos = new();
            using (SqlConnection connection = new (con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ObtenerAlumnos",connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader() )
                    {
                        while (reader.Read())
                        {
                            Alumno a = new Alumno{
                                codigo = Convert.ToInt32(reader["codigo"]),
                                nombres = reader["nombres"].ToString(),
                                carrera = reader["carrera"].ToString(),
                                domicilio = reader["domicilio"].ToString()
                            };
                            alumnos.Add(a);
                        }
                    }
                }
            } 
            return alumnos;
        }

        [HttpPost]
        public void Post([FromBody] Alumno a)
        {
            using (SqlConnection connection = new (con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("InsertarAlumno",connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo",a.codigo);
                    cmd.Parameters.AddWithValue("@nombres",a.nombres);
                    cmd.Parameters.AddWithValue("@carrera",a.carrera);
                    cmd.Parameters.AddWithValue("@domicilio",a.domicilio);
                    cmd.ExecuteNonQuery();
                }
            } 
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Alumno a, int id)
        {
            using (SqlConnection connection = new (con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ActualizarAlumno",connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo",id);
                    cmd.Parameters.AddWithValue("@nombres",a.nombres);
                    cmd.Parameters.AddWithValue("@carrera",a.carrera);
                    cmd.Parameters.AddWithValue("@domicilio",a.domicilio);
                    cmd.ExecuteNonQuery();
                }
            } 
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection connection = new (con))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("EliminarAlumno",connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo",id);
                    cmd.ExecuteNonQuery();
                }
            } 
        }
    }
}