namespace CDMYPE.CitasMedicas.Domain.Entities;

public class ProfesionalEspecialidad
{
    public int ProfesionalEspecialidadId { get; set; }

    public int ProfesionalId { get; set; }

    public int EspecialidadId { get; set; }

    public bool Activo { get; set; } = true;

    public Profesional Profesional { get; set; } = null!;

    public Especialidad Especialidad { get; set; } = null!;
}