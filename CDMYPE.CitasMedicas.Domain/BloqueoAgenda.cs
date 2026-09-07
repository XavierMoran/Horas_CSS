namespace CDMYPE.CitasMedicas.Domain.Entities;

public class BloqueoAgenda
{
    public int BloqueoAgendaId { get; set; }

    public int ProfesionalId { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    public DateTime FechaHoraFin { get; set; }

    public string? Motivo { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public Profesional Profesional { get; set; } = null!;
}