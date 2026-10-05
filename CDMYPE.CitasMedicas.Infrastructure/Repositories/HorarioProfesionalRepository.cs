using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;
using CDMYPE.CitasMedicas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CDMYPE.CitasMedicas.Infrastructure.Repositories;

public class HorarioProfesionalRepository
    : IHorarioProfesionalRepository
{
    private readonly IDbContextFactory<ApplicationDbContext>
        _contextFactory;

    public HorarioProfesionalRepository(
        IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<HorarioProfesional>>
        ObtenerPorProfesionalAsync(
            int profesionalId)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.HorariosProfesional
            .AsNoTracking()
            .Where(h =>
                h.ProfesionalId == profesionalId &&
                h.Activo)
            .OrderBy(h => h.DiaSemana)
            .ThenBy(h => h.HoraInicio)
            .ToListAsync();
    }

    public async Task<List<HorarioProfesional>>
        ObtenerTodosPorProfesionalAsync(
            int profesionalId)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.HorariosProfesional
            .AsNoTracking()
            .Where(h =>
                h.ProfesionalId == profesionalId)
            .OrderBy(h => h.DiaSemana)
            .ThenBy(h => h.HoraInicio)
            .ToListAsync();
    }

    public async Task<HorarioProfesional?>
        ObtenerPorIdAsync(
            int horarioProfesionalId)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.HorariosProfesional
            .AsNoTracking()
            .FirstOrDefaultAsync(h =>
                h.HorarioProfesionalId ==
                horarioProfesionalId);
    }

    public async Task AgregarAsync(
        HorarioProfesional horario)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        await context.HorariosProfesional
            .AddAsync(horario);

        await context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(
        HorarioProfesional horario)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        context.HorariosProfesional.Update(horario);

        await context.SaveChangesAsync();
    }

    public Task GuardarCambiosAsync()
    {
        // Con DbContextFactory cada operación utiliza
        // su propio contexto. Las actualizaciones deben
        // realizarse mediante ActualizarAsync.
        return Task.CompletedTask;
    }
}