using CDMYPE.CitasMedicas.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CDMYPE.CitasMedicas.Application.Security;

public class PasswordService
{
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public string HashPassword(
        Usuario usuario,
        string password)
    {
        return _passwordHasher.HashPassword(
            usuario,
            password);
    }

    public bool VerifyPassword(
        Usuario usuario,
        string hashedPassword,
        string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            usuario,
            hashedPassword,
            providedPassword);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}