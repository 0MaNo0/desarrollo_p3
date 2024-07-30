namespace backend.Models
{
    public class Renovacion
    {
        public int Id { get; set; }
        public int IdMiembro { get; set; }
        public int IdPago { get; set; }
        public int IdDocumento { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string Estado { get; set; }
        public Miembro Miembro { get; set; }
        public Pago Pago { get; set; }
        public Documento Documento { get; set; }
    }
}