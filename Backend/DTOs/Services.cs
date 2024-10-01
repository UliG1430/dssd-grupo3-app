public class ServiceResult<T>
{
    public T? Data { get; set; } // La respuesta exitosa
    public string? ErrorMessage { get; set; } // Mensaje de error, si existe
    public bool Success { get; set; } // Indica si la operación fue exitosa
}