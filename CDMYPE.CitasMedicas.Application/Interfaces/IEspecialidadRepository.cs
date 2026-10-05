using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Interfaces;

public interface IEspecialidadRepository
{
    Task<List<Especialidad>> ObtenerActivasAsync();

    Task<List<Especialidad>> ObtenerTodasAsync();

    Task<Especialidad?> ObtenerPorIdAsync(
        int especialidadId);

    Task AgregarAsync(
        Especialidad especialidad);

    Task ActualizarAsync(
        Especialidad especialidad);

    Task GuardarCambiosAsync();
}