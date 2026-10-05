using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Interfaces;

public interface IHorarioProfesionalRepository
{
    Task<List<HorarioProfesional>> ObtenerPorProfesionalAsync(
        int profesionalId);

    Task<List<HorarioProfesional>> ObtenerTodosPorProfesionalAsync(
        int profesionalId);

    Task<HorarioProfesional?> ObtenerPorIdAsync(
        int horarioProfesionalId);

    Task AgregarAsync(
        HorarioProfesional horario);

    Task ActualizarAsync(
        HorarioProfesional horario);

    Task GuardarCambiosAsync();
}