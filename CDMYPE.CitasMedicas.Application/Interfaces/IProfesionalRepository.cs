using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Interfaces;

public interface IProfesionalRepository
{
    Task<List<Profesional>> ObtenerTodosAsync();

    Task<List<Profesional>> ObtenerInactivosAsync();

    Task<List<Profesional>> ObtenerTodosIncluyendoInactivosAsync();

    Task<List<Profesional>> BuscarAsync(string texto);

    Task<Profesional?> ObtenerPorIdAsync(int profesionalId);

    Task AgregarAsync(Profesional profesional);

    Task ActualizarAsync(Profesional profesional);

    Task GuardarCambiosAsync();
}