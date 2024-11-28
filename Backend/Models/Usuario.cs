namespace Backend.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UsuarioNombre { get; set; } // Changed to avoid conflict with 'Usuario' model name
        public string Password { get; set; }
        public bool comenzoRecorrido { get; set; }
        public int caseId { get; set; }
        public string rol { get; set; }
        public bool seleccionoPaquete { get; set; }
        public int paqueteId { get; set; }
    }
}
