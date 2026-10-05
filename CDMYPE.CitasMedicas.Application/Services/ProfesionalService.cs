using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Services;

public class ProfesionalService
{
    private readonly IProfesionalRepository _profesionalRepository;

    public ProfesionalService(
        IProfesionalRepository profesionalRepository)
    {
        _profesionalRepository = profesionalRepository;
    }

    public async Task<List<Profesional>> ObtenerTodosAsync()
    {
        return await _profesionalRepository
            .ObtenerTodosAsync();
    }

    public async Task<List<Profesional>> ObtenerInactivosAsync()
    {
        return await _profesionalRepository
            .ObtenerInactivosAsync();
    }

    public async Task<List<Profesional>>
        ObtenerTodosIncluyendoInactivosAsync()
    {
        return await _profesionalRepository
            .ObtenerTodosIncluyendoInactivosAsync();
    }

    public async Task<List<Profesional>> BuscarAsync(
        string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return await ObtenerTodosAsync();

        return await _profesionalRepository
            .BuscarAsync(texto.Trim());
    }

    public async Task<Profesional?> ObtenerPorIdAsync(
        int profesionalId)
    {
        return await _profesionalRepository
            .ObtenerPorIdAsync(profesionalId);
    }

    public async Task CrearProfesionalAsync(
        Profesional profesional)
    {
        ValidarProfesional(profesional);

        profesional.Nombres =
            profesional.Nombres.Trim();

        profesional.Apellidos =
            profesional.Apellidos.Trim();

        profesional.NumeroJuntaVigilancia =
            LimpiarTextoOpcional(
                profesional.NumeroJuntaVigilancia);

        profesional.Telefono =
            LimpiarTextoOpcional(
                profesional.Telefono);

        profesional.Correo =
            LimpiarTextoOpcional(
                profesional.Correo);

        profesional.Activo = true;
        profesional.FechaRegistro = DateTime.Now;

        await _profesionalRepository
            .AgregarAsync(profesional);
    }

    public async Task ActualizarProfesionalAsync(
        Profesional profesional)
    {
        ValidarProfesional(profesional);

        profesional.Nombres =
            profesional.Nombres.Trim();

        profesional.Apellidos =
            profesional.Apellidos.Trim();

        profesional.NumeroJuntaVigilancia =
            LimpiarTextoOpcional(
                profesional.NumeroJuntaVigilancia);

        profesional.Telefono =
            LimpiarTextoOpcional(
                profesional.Telefono);

        profesional.Correo =
            LimpiarTextoOpcional(
                profesional.Correo);

        await _profesionalRepository
            .ActualizarAsync(profesional);
    }

    public async Task DesactivarProfesionalAsync(
        Profesional profesional)
    {
        profesional.Activo = false;

        await _profesionalRepository
            .GuardarCambiosAsync();
    }

    public async Task ReactivarProfesionalAsync(
        Profesional profesional)
    {
        profesional.Activo = true;

        await _profesionalRepository
            .GuardarCambiosAsync();
    }

    private static void ValidarProfesional(
        Profesional profesional)
    {
        if (string.IsNullOrWhiteSpace(
            profesional.Nombres))
        {
            throw new InvalidOperationException(
                "Los nombres del profesional son obligatorios.");
        }

        if (string.IsNullOrWhiteSpace(
            profesional.Apellidos))
        {
            throw new InvalidOperationException(
                "Los apellidos del profesional son obligatorios.");
        }

        if (!string.IsNullOrWhiteSpace(
                profesional.Correo) &&
            !profesional.Correo.Contains('@'))
        {
            throw new InvalidOperationException(
                "El correo electrónico no es válido.");
        }
    }

    private static string? LimpiarTextoOpcional(
        string? valor)
    {
        return string.IsNullOrWhiteSpace(valor)
            ? null
            : valor.Trim();
    }
}