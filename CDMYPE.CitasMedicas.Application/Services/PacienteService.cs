using CDMYPE.CitasMedicas.Application.Interfaces;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Services;

public class PacienteService
{
    private readonly IPacienteRepository _pacienteRepository;

    public PacienteService(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    public async Task<List<Paciente>> ObtenerTodosAsync()
    {
        return await _pacienteRepository.ObtenerTodosAsync();
    }

    public async Task<List<Paciente>> BuscarAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return await ObtenerTodosAsync();

        return await _pacienteRepository.BuscarAsync(texto.Trim());
    }

    public async Task<Paciente?> ObtenerPorIdAsync(
        int pacienteId)
    {
        return await _pacienteRepository
            .ObtenerPorIdAsync(pacienteId);
    }

    public async Task DesactivarPacienteAsync(Paciente paciente)
    {
        paciente.Activo = false;

        await _pacienteRepository.GuardarCambiosAsync();
    }

    public async Task<Paciente> CrearPacienteAsync(
        string nombres,
        string apellidos,
        string sexo,
        DateTime fechaNacimiento,
        string nacionalidad,
        string tipoDocumento,
        string numeroDocumento,
        string direccion,
        string telefono,
        string? correo)
    {
        if (string.IsNullOrWhiteSpace(nombres))
            throw new InvalidOperationException("Los nombres son obligatorios.");

        if (string.IsNullOrWhiteSpace(apellidos))
            throw new InvalidOperationException("Los apellidos son obligatorios.");

        if (string.IsNullOrWhiteSpace(numeroDocumento))
            throw new InvalidOperationException("El número de documento es obligatorio.");

        var existente =
            await _pacienteRepository.ObtenerPorDocumentoAsync(
                numeroDocumento.Trim());

        if (existente is not null)
            throw new InvalidOperationException(
                "Ya existe un paciente con ese número de documento.");

        if (fechaNacimiento.Date > DateTime.Today)
            throw new InvalidOperationException(
                "La fecha de nacimiento no puede ser futura.");

        var paciente = new Paciente
        {
            Nombres = nombres.Trim(),
            Apellidos = apellidos.Trim(),
            Sexo = sexo.Trim(),
            FechaNacimiento = fechaNacimiento.Date,
            Nacionalidad = nacionalidad.Trim(),
            TipoDocumento = tipoDocumento.Trim(),
            NumeroDocumento = numeroDocumento.Trim(),
            Direccion = direccion.Trim(),
            Telefono = telefono.Trim(),
            Correo = string.IsNullOrWhiteSpace(correo)
                ? null
                : correo.Trim(),
            FechaRegistro = DateTime.Now,
            Activo = true
        };

        await _pacienteRepository.AgregarAsync(paciente);
        await _pacienteRepository.GuardarCambiosAsync();

        return paciente;
    }

    public async Task ActualizarPacienteAsync(Paciente paciente)
    {
        if (paciente.FechaNacimiento.Date > DateTime.Today)
            throw new InvalidOperationException(
                "La fecha de nacimiento no puede ser futura.");

        var existente =
            await _pacienteRepository.ObtenerPorDocumentoAsync(
                paciente.NumeroDocumento.Trim());

        if (existente is not null &&
            existente.PacienteId != paciente.PacienteId)
        {
            throw new InvalidOperationException(
                "Ya existe otro paciente con ese número de documento.");
        }

        paciente.Nombres = paciente.Nombres.Trim();
        paciente.Apellidos = paciente.Apellidos.Trim();
        paciente.Sexo = paciente.Sexo.Trim();
        paciente.Nacionalidad = paciente.Nacionalidad.Trim();
        paciente.TipoDocumento = paciente.TipoDocumento.Trim();
        paciente.NumeroDocumento = paciente.NumeroDocumento.Trim();
        paciente.Direccion = paciente.Direccion.Trim();
        paciente.Telefono = paciente.Telefono.Trim();

        if (string.IsNullOrWhiteSpace(paciente.Correo))
            paciente.Correo = null;
        else
            paciente.Correo = paciente.Correo.Trim();

        await _pacienteRepository.GuardarCambiosAsync();
    }

    public async Task<List<Paciente>> ObtenerInactivosAsync()
    {
        return await _pacienteRepository.ObtenerInactivosAsync();
    }

    public async Task<List<Paciente>> ObtenerTodosIncluyendoInactivosAsync()
    {
        return await _pacienteRepository.ObtenerTodosIncluyendoInactivosAsync();
    }

    public async Task ReactivarPacienteAsync(Paciente paciente)
    {
        paciente.Activo = true;

        await _pacienteRepository.GuardarCambiosAsync();
    }

}