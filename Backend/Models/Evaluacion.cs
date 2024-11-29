namespace Backend.Model
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public int caseId { get; set; }
        public string state { get; set; }
        public string observaciones { get; set; }
        public int cantOrdenes { get; set; }
        public int cantOrdenesOk { get; set; }
        public int cantOrdenesMal { get; set; }
    }
}