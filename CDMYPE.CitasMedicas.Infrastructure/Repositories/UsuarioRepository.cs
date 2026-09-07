using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;
using CDMYPE.CitasMedicas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CDMYPE.CitasMedicas.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(
        string nombreUsuario)
    {
        return await _context.Usuarios
            .Include(x => x.Rol)
            .FirstOrDefaultAsync(x =>
                x.NombreUsuario == nombreUsuario);
    }

    public async Task<bool> ExisteNombreUsuarioAsync(
        string nombreUsuario)
    {
        return await _context.Usuarios
            .AnyAsync(x =>
                x.NombreUsuario == nombreUsuario);
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}