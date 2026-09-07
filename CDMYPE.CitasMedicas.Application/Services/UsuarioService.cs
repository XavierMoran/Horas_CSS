using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Application.Security;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Services;

public class UsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly PasswordService _passwordService;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        PasswordService passwordService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordService = passwordService;
    }

    public async Task<Usuario?> LoginAsync(
        string nombreUsuario,
        string password)
    {
        var usuario = await _usuarioRepository
            .ObtenerPorNombreUsuarioAsync(nombreUsuario);

        if (usuario is null || !usuario.Activo)
            return null;

        var passwordValido = _passwordService.VerifyPassword(
            usuario,
            usuario.PasswordHash,
            password);

        if (!passwordValido)
            return null;

        usuario.UltimoAcceso = DateTime.Now;

        await _usuarioRepository.GuardarCambiosAsync();

        return usuario;
    }

    public async Task<Usuario> CrearUsuarioAsync(
        string nombreCompleto,
        string nombreUsuario,
        string password,
        int rolId)
    {
        var existe = await _usuarioRepository
            .ExisteNombreUsuarioAsync(nombreUsuario);

        if (existe)
        {
            throw new InvalidOperationException(
                "El nombre de usuario ya existe.");
        }

        var usuario = new Usuario
        {
            NombreCompleto = nombreCompleto.Trim(),
            NombreUsuario = nombreUsuario.Trim(),
            RolId = rolId,
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        usuario.PasswordHash =
            _passwordService.HashPassword(usuario, password);

        await _usuarioRepository.AgregarAsync(usuario);
        await _usuarioRepository.GuardarCambiosAsync();

        return usuario;
    }

    public async Task<bool> ExisteUsuarioAsync(string nombreUsuario)
    {
        return await _usuarioRepository
            .ExisteNombreUsuarioAsync(nombreUsuario);
    }
}