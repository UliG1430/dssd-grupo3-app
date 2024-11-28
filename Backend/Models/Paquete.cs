namespace Backend.Model
{
    public class Paquete
    {
        public int Id { get; set; } // Auto-incremental ID
        public int CaseId { get; set; } // Case identifier
        public string State { get; set; } // State of the package
    }
}