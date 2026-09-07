namespace CDMYPE.CitasMedicas.Domain.Entities;

public class Cita
{
    public int CitaId { get; set; }

    public int PacienteId { get; set; }

    public int ProfesionalId { get; set; }

    public int EspecialidadId { get; set; }

    public int EstadoCitaId { get; set; }

    public int UsuarioRegistroId { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    public DateTime FechaHoraFin { get; set; }

    public string? MotivoConsulta { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public DateTime? FechaModificacion { get; set; }

    public Paciente Paciente { get; set; } = null!;

    public Profesional Profesional { get; set; } = null!;

    public Especialidad Especialidad { get; set; } = null!;

    public EstadoCita EstadoCita { get; set; } = null!;

    public Usuario UsuarioRegistro { get; set; } = null!;
}