using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;
using CDMYPE.CitasMedicas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CDMYPE.CitasMedicas.Infrastructure.Repositories;

public class ProfesionalRepository : IProfesionalRepository
{
    private readonly ApplicationDbContext _context;

    public ProfesionalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Profesional>> ObtenerTodosAsync()
    {
        return await _context.Profesionales
            .Where(p => p.Activo)
            .Include(p => p.ProfesionalEspecialidades)
                .ThenInclude(pe => pe.Especialidad)
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }

    public async Task<List<Profesional>> ObtenerInactivosAsync()
    {
        return await _context.Profesionales
            .Where(p => !p.Activo)
            .Include(p => p.ProfesionalEspecialidades)
                .ThenInclude(pe => pe.Especialidad)
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }

    public async Task<List<Profesional>> ObtenerTodosIncluyendoInactivosAsync()
    {
        return await _context.Profesionales
            .Include(p => p.ProfesionalEspecialidades)
                .ThenInclude(pe => pe.Especialidad)
            .OrderByDescending(p => p.Activo)
            .ThenBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }

    public async Task<List<Profesional>> BuscarAsync(string texto)
    {
        texto = texto.Trim();

        return await _context.Profesionales
            .Include(p => p.ProfesionalEspecialidades)
                .ThenInclude(pe => pe.Especialidad)
            .Where(p =>
                p.Nombres.Contains(texto) ||
                p.Apellidos.Contains(texto) ||
                (p.NumeroJuntaVigilancia != null &&
                 p.NumeroJuntaVigilancia.Contains(texto)))
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }

    public async Task<Profesional?> ObtenerPorIdAsync(
        int profesionalId)
    {
        return await _context.Profesionales
            .Include(p => p.ProfesionalEspecialidades)
                .ThenInclude(pe => pe.Especialidad)
            .FirstOrDefaultAsync(
                p => p.ProfesionalId == profesionalId);
    }

    public async Task AgregarAsync(
        Profesional profesional)
    {
        await _context.Profesionales
            .AddAsync(profesional);

        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(
        Profesional profesional)
    {
        _context.Profesionales.Update(profesional);

        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}