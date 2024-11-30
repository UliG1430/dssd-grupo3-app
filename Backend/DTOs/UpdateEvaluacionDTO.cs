namespace Backend.Dto;

public class UpdateEvaluacionDTO
{
    public required int caseId { get; set; }
    public required string state { get; set; }
    public required int cantOrdenesOk { get; set; }
    public required int cantOrdenesMal { get; set; }
    public required int cantOrdenes { get; set; }
    public required string observaciones { get; set; }
}