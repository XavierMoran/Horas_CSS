namespace CDMYPE.CitasMedicas.Domain.Entities;

public class Profesional
{
    public int ProfesionalId { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string? NumeroJuntaVigilancia { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public ICollection<ProfesionalEspecialidad> ProfesionalEspecialidades { get; set; } = new List<ProfesionalEspecialidad>();

    public ICollection<HorarioProfesional> Horarios { get; set; } = new List<HorarioProfesional>();

    public ICollection<BloqueoAgenda> BloqueosAgenda { get; set; } = new List<BloqueoAgenda>();

    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}