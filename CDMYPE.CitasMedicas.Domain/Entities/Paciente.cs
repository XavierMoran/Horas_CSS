namespace CDMYPE.CitasMedicas.Domain.Entities;

public class Paciente
{
    public int PacienteId { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Sexo { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    public string Nacionalidad { get; set; } = string.Empty;

    public string TipoDocumento { get; set; } = string.Empty;

    public string NumeroDocumento { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Correo { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public bool Activo { get; set; } = true;

    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}