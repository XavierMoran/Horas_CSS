namespace CDMYPE.CitasMedicas.Domain.Entities;

public class Especialidad
{
    public int EspecialidadId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<ProfesionalEspecialidad> ProfesionalEspecialidades { get; set; } = new List<ProfesionalEspecialidad>();

    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}