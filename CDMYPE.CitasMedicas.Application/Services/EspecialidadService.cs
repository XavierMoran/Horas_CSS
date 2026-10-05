using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Services;

public class EspecialidadService
{
    private readonly IEspecialidadRepository _especialidadRepository;

    public EspecialidadService(
        IEspecialidadRepository especialidadRepository)
    {
        _especialidadRepository = especialidadRepository;
    }

    public async Task<List<Especialidad>> ObtenerActivasAsync()
    {
        return await _especialidadRepository
            .ObtenerActivasAsync();
    }

    public async Task<List<Especialidad>> ObtenerTodasAsync()
    {
        return await _especialidadRepository
            .ObtenerTodasAsync();
    }

    public async Task<Especialidad?> ObtenerPorIdAsync(
        int especialidadId)
    {
        return await _especialidadRepository
            .ObtenerPorIdAsync(especialidadId);
    }

    public async Task CrearEspecialidadAsync(
        Especialidad especialidad)
    {
        ValidarEspecialidad(especialidad);

        especialidad.Nombre =
            especialidad.Nombre.Trim();

        especialidad.Descripcion =
            LimpiarTextoOpcional(
                especialidad.Descripcion);

        especialidad.Activo = true;

        await _especialidadRepository
            .AgregarAsync(especialidad);
    }

    public async Task ActualizarEspecialidadAsync(
        Especialidad especialidad)
    {
        ValidarEspecialidad(especialidad);

        especialidad.Nombre =
            especialidad.Nombre.Trim();

        especialidad.Descripcion =
            LimpiarTextoOpcional(
                especialidad.Descripcion);

        await _especialidadRepository
            .ActualizarAsync(especialidad);
    }

    public async Task DesactivarEspecialidadAsync(
        Especialidad especialidad)
    {
        especialidad.Activo = false;

        await _especialidadRepository
            .GuardarCambiosAsync();
    }

    public async Task ReactivarEspecialidadAsync(
        Especialidad especialidad)
    {
        especialidad.Activo = true;

        await _especialidadRepository
            .GuardarCambiosAsync();
    }

    private static void ValidarEspecialidad(
        Especialidad especialidad)
    {
        if (string.IsNullOrWhiteSpace(
            especialidad.Nombre))
        {
            throw new InvalidOperationException(
                "El nombre de la especialidad es obligatorio.");
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