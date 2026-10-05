using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Services;

public class HorarioProfesionalService
{
    private readonly IHorarioProfesionalRepository
        _horarioProfesionalRepository;

    public HorarioProfesionalService(
        IHorarioProfesionalRepository horarioProfesionalRepository)
    {
        _horarioProfesionalRepository =
            horarioProfesionalRepository;
    }

    // =========================================
    // CONSULTAS
    // =========================================

    public async Task<List<HorarioProfesional>>
        ObtenerPorProfesionalAsync(
            int profesionalId)
    {
        return await _horarioProfesionalRepository
            .ObtenerPorProfesionalAsync(
                profesionalId);
    }

    public async Task<List<HorarioProfesional>>
        ObtenerTodosPorProfesionalAsync(
            int profesionalId)
    {
        return await _horarioProfesionalRepository
            .ObtenerTodosPorProfesionalAsync(
                profesionalId);
    }

    public async Task<HorarioProfesional?>
        ObtenerPorIdAsync(
            int horarioProfesionalId)
    {
        return await _horarioProfesionalRepository
            .ObtenerPorIdAsync(
                horarioProfesionalId);
    }

    // =========================================
    // CREAR
    // =========================================

    public async Task CrearHorarioAsync(
        HorarioProfesional horario)
    {
        ValidarDatosBasicos(horario);

        await ValidarSuperposicionAsync(
            horario);

        horario.Activo = true;

        await _horarioProfesionalRepository
            .AgregarAsync(horario);
    }

    // =========================================
    // ACTUALIZAR
    // =========================================

    public async Task ActualizarHorarioAsync(
        HorarioProfesional horario)
    {
        ValidarDatosBasicos(horario);

        await ValidarSuperposicionAsync(
            horario);

        await _horarioProfesionalRepository
            .ActualizarAsync(horario);
    }

    // =========================================
    // DESACTIVAR
    // =========================================

    public async Task DesactivarHorarioAsync(
        HorarioProfesional horario)
    {
        horario.Activo = false;

        await _horarioProfesionalRepository
            .ActualizarAsync(horario);
    }

    // =========================================
    // REACTIVAR
    // =========================================

    public async Task ReactivarHorarioAsync(
        HorarioProfesional horario)
    {
        ValidarDatosBasicos(horario);

        await ValidarSuperposicionAsync(
            horario);

        horario.Activo = true;

        await _horarioProfesionalRepository
            .ActualizarAsync(horario);
    }

    // =========================================
    // VALIDACIONES BÁSICAS
    // =========================================

    private static void ValidarDatosBasicos(
        HorarioProfesional horario)
    {
        if (horario.ProfesionalId <= 0)
        {
            throw new InvalidOperationException(
                "Debe seleccionar un profesional.");
        }

        if (horario.HoraInicio >= horario.HoraFin)
        {
            throw new InvalidOperationException(
                "La hora de inicio debe ser anterior a la hora de finalización.");
        }

        if (horario.DuracionCitaMinutos <= 0)
        {
            throw new InvalidOperationException(
                "La duración de la cita debe ser mayor a cero minutos.");
        }

        var duracionBloque =
            horario.HoraFin - horario.HoraInicio;

        if (horario.DuracionCitaMinutos >
            duracionBloque.TotalMinutes)
        {
            throw new InvalidOperationException(
                "La duración de la cita no puede ser mayor al bloque de atención.");
        }
    }

    // =========================================
    // VALIDAR SUPERPOSICIÓN
    // =========================================

    private async Task ValidarSuperposicionAsync(
        HorarioProfesional horario)
    {
        var horariosExistentes =
            await _horarioProfesionalRepository
                .ObtenerPorProfesionalAsync(
                    horario.ProfesionalId);

        var existeSuperposicion =
            horariosExistentes.Any(h =>
                h.HorarioProfesionalId !=
                    horario.HorarioProfesionalId &&

                h.DiaSemana ==
                    horario.DiaSemana &&

                horario.HoraInicio < h.HoraFin &&

                horario.HoraFin > h.HoraInicio);

        if (existeSuperposicion)
        {
            throw new InvalidOperationException(
                "El horario se superpone con otro bloque de atención del profesional para el mismo día.");
        }
    }
}