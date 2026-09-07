namespace CDMYPE.CitasMedicas.Domain.Entities;

public class Usuario
{
    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string NombreUsuario { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? UltimoAcceso { get; set; }

    public Rol Rol { get; set; } = null!;

    public ICollection<Cita> CitasRegistradas { get; set; } = new List<Cita>();

    public ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
}