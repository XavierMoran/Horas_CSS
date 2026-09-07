namespace CDMYPE.CitasMedicas.Domain.Entities;

public class Configuracion
{
    public int ConfiguracionId { get; set; }

    public string Clave { get; set; } = string.Empty;

    public string Valor { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
}