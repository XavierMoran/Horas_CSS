using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);

    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);

    Task AgregarAsync(Usuario usuario);

    Task GuardarCambiosAsync();
}