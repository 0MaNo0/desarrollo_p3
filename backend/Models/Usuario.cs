namespace backend.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int? IdMiembro { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public Miembro Miembro { get; set; }
    }
}