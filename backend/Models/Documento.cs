namespace backend.Models
{    
    public class Documento
    {
        public int Id { get; set; }
        public int IdMiembro { get; set; }
        public string TipoDocumento { get; set; }
        public string DocumentoUrl { get; set; }
        public DateTime FechaCarga { get; set; }
        public string Estado { get; set; }
        public Miembro Miembro { get; set; }
    }
}