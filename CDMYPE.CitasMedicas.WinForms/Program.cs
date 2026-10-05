using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Application.Security;
using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Infrastructure.Data;
using CDMYPE.CitasMedicas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CDMYPE.CitasMedicas.WinForms.Forms.Auth;
using CDMYPE.CitasMedicas.WinForms.Forms.Main;
using CDMYPE.CitasMedicas.WinForms.Forms.Pacientes;

namespace CDMYPE.CitasMedicas.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                @"Server=localhost\SQLEXPRESS;Database=CDMYPE_CitasMedicas;Trusted_Connection=True;TrustServerCertificate=True;"));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<IProfesionalRepository, ProfesionalRepository>();
        services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();

        services.AddScoped<PasswordService>();

        services.AddScoped<UsuarioService>();
        services.AddScoped<PacienteService>();
        services.AddScoped<ProfesionalService>();
        services.AddScoped<EspecialidadService>();

        services.AddTransient<LoginForm>();
        services.AddTransient<MainForm>();

        using var serviceProvider = services.BuildServiceProvider();

        CrearUsuarioInicial(serviceProvider);

        System.Windows.Forms.Application.Run(
            serviceProvider.GetRequiredService<LoginForm>());    }

    private static void CrearUsuarioInicial(
        ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var usuarioService =
            scope.ServiceProvider.GetRequiredService<UsuarioService>();

        var existe = usuarioService
            .ExisteUsuarioAsync("admin")
            .GetAwaiter()
            .GetResult();

        if (existe)
            return;

        usuarioService
            .CrearUsuarioAsync(
                "Administrador del Sistema",
                "admin",
                "Admin123*",
                2)
            .GetAwaiter()
            .GetResult();
    }
}