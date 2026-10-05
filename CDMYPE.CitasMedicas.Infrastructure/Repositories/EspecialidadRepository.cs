using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;
using CDMYPE.CitasMedicas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CDMYPE.CitasMedicas.Infrastructure.Repositories;

public class EspecialidadRepository : IEspecialidadRepository
{
    private readonly ApplicationDbContext _context;

    public EspecialidadRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Especialidad>> ObtenerActivasAsync()
    {
        return await _context.Especialidades
            .Where(e => e.Activo)
            .OrderBy(e => e.Nombre)
            .ToListAsync();
    }

    public async Task<List<Especialidad>> ObtenerTodasAsync()
    {
        return await _context.Especialidades
            .OrderByDescending(e => e.Activo)
            .ThenBy(e => e.Nombre)
            .ToListAsync();
    }

    public async Task<Especialidad?> ObtenerPorIdAsync(
        int especialidadId)
    {
        return await _context.Especialidades
            .FirstOrDefaultAsync(
                e => e.EspecialidadId == especialidadId);
    }

    public async Task AgregarAsync(
        Especialidad especialidad)
    {
        await _context.Especialidades
            .AddAsync(especialidad);

        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(
        Especialidad especialidad)
    {
        _context.Especialidades
            .Update(especialidad);

        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}