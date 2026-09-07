namespace CDMYPE.CitasMedicas.Domain.Entities;

public class EstadoCita
{
    public int EstadoCitaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}