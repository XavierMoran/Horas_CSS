using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;
using CDMYPE.CitasMedicas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CDMYPE.CitasMedicas.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly ApplicationDbContext _context;

    public PacienteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Paciente>> ObtenerTodosAsync()
    {
        return await _context.Pacientes
            .Where(x => x.Activo)
            .OrderBy(x => x.Apellidos)
            .ThenBy(x => x.Nombres)
            .ToListAsync();
    }

    public async Task<Paciente?> ObtenerPorIdAsync(int pacienteId)
    {
        return await _context.Pacientes
            .FirstOrDefaultAsync(x =>
                x.PacienteId == pacienteId);
    }

    public async Task<Paciente?> ObtenerPorDocumentoAsync(
        string numeroDocumento)
    {
        return await _context.Pacientes
            .FirstOrDefaultAsync(x =>
                x.NumeroDocumento == numeroDocumento);
    }

    public async Task<List<Paciente>> BuscarAsync(string texto)
    {
        return await _context.Pacientes
            .Where(x =>
                x.Nombres.Contains(texto) ||
                x.Apellidos.Contains(texto) ||
                x.NumeroDocumento.Contains(texto))            
            .OrderBy(x => x.Apellidos)
            .ThenBy(x => x.Nombres)
            .ToListAsync();
    }

    public async Task AgregarAsync(Paciente paciente)
    {
        await _context.Pacientes.AddAsync(paciente);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Paciente>> ObtenerInactivosAsync()
    {
        return await _context.Pacientes
            .Where(x => !x.Activo)
            .OrderBy(x => x.Apellidos)
            .ThenBy(x => x.Nombres)
            .ToListAsync();
    }

    public async Task<List<Paciente>> ObtenerTodosIncluyendoInactivosAsync()
    {
        return await _context.Pacientes
            .OrderByDescending(x => x.Activo)
            .ThenBy(x => x.Apellidos)
            .ThenBy(x => x.Nombres)
            .ToListAsync();
    }

}