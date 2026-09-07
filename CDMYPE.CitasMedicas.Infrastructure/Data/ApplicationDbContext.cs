using CDMYPE.CitasMedicas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDMYPE.CitasMedicas.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Profesional> Profesionales => Set<Profesional>();
    public DbSet<Especialidad> Especialidades => Set<Especialidad>();
    public DbSet<ProfesionalEspecialidad> ProfesionalEspecialidades => Set<ProfesionalEspecialidad>();
    public DbSet<HorarioProfesional> HorariosProfesional => Set<HorarioProfesional>();
    public DbSet<BloqueoAgenda> BloqueosAgenda => Set<BloqueoAgenda>();
    public DbSet<EstadoCita> EstadosCita => Set<EstadoCita>();
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();
    public DbSet<Configuracion> Configuraciones => Set<Configuracion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================================================
        // ROLES
        // =========================================================

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.Property(x => x.Nombre)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Descripcion)
                .HasMaxLength(200);

            entity.HasIndex(x => x.Nombre)
                .IsUnique();

            entity.HasData(
                new Rol
                {
                    RolId = 1,
                    Nombre = "Secretaria",
                    Descripcion = "Usuario encargado de la gestión operativa de pacientes y citas.",
                    Activo = true
                },
                new Rol
                {
                    RolId = 2,
                    Nombre = "SuperUsuario",
                    Descripcion = "Usuario con acceso administrativo y operativo al sistema.",
                    Activo = true
                }
            );
        });

        // =========================================================
        // USUARIOS
        // =========================================================

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(x => x.NombreCompleto)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.NombreUsuario)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            entity.HasIndex(x => x.NombreUsuario)
                .IsUnique();

            entity.HasOne(x => x.Rol)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.RolId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================================================
        // PACIENTES
        // =========================================================

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.Property(x => x.Nombres)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Apellidos)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Sexo)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.Nacionalidad)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(x => x.TipoDocumento)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.NumeroDocumento)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Direccion)
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(x => x.Telefono)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Correo)
                .HasMaxLength(150);

            entity.HasIndex(x => x.NumeroDocumento)
                .IsUnique();
        });

        // =========================================================
        // PROFESIONALES
        // =========================================================

        modelBuilder.Entity<Profesional>(entity =>
        {
            entity.Property(x => x.Nombres)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Apellidos)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.NumeroJuntaVigilancia)
                .HasMaxLength(50);

            entity.Property(x => x.Telefono)
                .HasMaxLength(30);

            entity.Property(x => x.Correo)
                .HasMaxLength(150);
        });

        // =========================================================
        // ESPECIALIDADES
        // =========================================================

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Descripcion)
                .HasMaxLength(300);

            entity.HasIndex(x => x.Nombre)
                .IsUnique();
        });

        // =========================================================
        // PROFESIONAL - ESPECIALIDAD
        // =========================================================

        modelBuilder.Entity<ProfesionalEspecialidad>(entity =>
        {
            entity.HasIndex(x => new
            {
                x.ProfesionalId,
                x.EspecialidadId
            }).IsUnique();

            entity.HasOne(x => x.Profesional)
                .WithMany(x => x.ProfesionalEspecialidades)
                .HasForeignKey(x => x.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Especialidad)
                .WithMany(x => x.ProfesionalEspecialidades)
                .HasForeignKey(x => x.EspecialidadId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================================================
        // HORARIOS
        // =========================================================

        modelBuilder.Entity<HorarioProfesional>(entity =>
        {
            entity.HasOne(x => x.Profesional)
                .WithMany(x => x.Horarios)
                .HasForeignKey(x => x.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================================================
        // BLOQUEOS DE AGENDA
        // =========================================================

        modelBuilder.Entity<BloqueoAgenda>(entity =>
        {
            entity.Property(x => x.Motivo)
                .HasMaxLength(300);

            entity.HasOne(x => x.Profesional)
                .WithMany(x => x.BloqueosAgenda)
                .HasForeignKey(x => x.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================================================
        // ESTADOS DE CITA
        // =========================================================

        modelBuilder.Entity<EstadoCita>(entity =>
        {
            entity.Property(x => x.Nombre)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(x => x.Nombre)
                .IsUnique();

            entity.HasData(
                new EstadoCita
                {
                    EstadoCitaId = 1,
                    Nombre = "Programada",
                    Activo = true
                },
                new EstadoCita
                {
                    EstadoCitaId = 2,
                    Nombre = "Confirmada",
                    Activo = true
                },
                new EstadoCita
                {
                    EstadoCitaId = 3,
                    Nombre = "Atendida",
                    Activo = true
                },
                new EstadoCita
                {
                    EstadoCitaId = 4,
                    Nombre = "Cancelada",
                    Activo = true
                },
                new EstadoCita
                {
                    EstadoCitaId = 5,
                    Nombre = "NoAsistio",
                    Activo = true
                }
            );
        });

        // =========================================================
        // CITAS
        // =========================================================

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.Property(x => x.MotivoConsulta)
                .HasMaxLength(300);

            entity.Property(x => x.Observaciones)
                .HasMaxLength(1000);

            entity.HasOne(x => x.Paciente)
                .WithMany(x => x.Citas)
                .HasForeignKey(x => x.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Profesional)
                .WithMany(x => x.Citas)
                .HasForeignKey(x => x.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Especialidad)
                .WithMany(x => x.Citas)
                .HasForeignKey(x => x.EspecialidadId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.EstadoCita)
                .WithMany(x => x.Citas)
                .HasForeignKey(x => x.EstadoCitaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.UsuarioRegistro)
                .WithMany(x => x.CitasRegistradas)
                .HasForeignKey(x => x.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new
            {
                x.ProfesionalId,
                x.FechaHoraInicio
            });
        });

        // =========================================================
        // AUDITORÍA
        // =========================================================

        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.Property(x => x.Accion)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Entidad)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.EntidadId)
                .HasMaxLength(100);

            entity.Property(x => x.Detalle)
                .HasMaxLength(1000);

            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.Auditorias)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // =========================================================
        // CONFIGURACIÓN
        // =========================================================

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.Property(x => x.Clave)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Valor)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.Descripcion)
                .HasMaxLength(300);

            entity.HasIndex(x => x.Clave)
                .IsUnique();

            entity.HasData(
                new Configuracion
                {
                    ConfiguracionId = 1,
                    Clave = "NombreAplicacion",
                    Valor = "Sistema de Gestión de Citas Médicas",
                    Descripcion = "Nombre mostrado por el aplicativo."
                },
                new Configuracion
                {
                    ConfiguracionId = 2,
                    Clave = "DuracionCitaDefault",
                    Valor = "30",
                    Descripcion = "Duración predeterminada de una cita en minutos."
                }
            );
        });
    }
}