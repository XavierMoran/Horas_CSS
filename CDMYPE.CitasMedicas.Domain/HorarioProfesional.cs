namespace CDMYPE.CitasMedicas.Domain.Entities;

public class HorarioProfesional
{
    public int HorarioProfesionalId { get; set; }

    public int ProfesionalId { get; set; }

    public DayOfWeek DiaSemana { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public int DuracionCitaMinutos { get; set; } = 30;

    public bool Activo { get; set; } = true;

    public Profesional Profesional { get; set; } = null!;
}