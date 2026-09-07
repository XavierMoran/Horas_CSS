using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.Application.Interfaces;

public interface IPacienteRepository
{
    Task<List<Paciente>> ObtenerTodosAsync();

    Task<Paciente?> ObtenerPorIdAsync(int pacienteId);

    Task<Paciente?> ObtenerPorDocumentoAsync(string numeroDocumento);

    Task<List<Paciente>> BuscarAsync(string texto);

    Task AgregarAsync(Paciente paciente);

    Task GuardarCambiosAsync();

    Task<List<Paciente>> ObtenerInactivosAsync();

    Task<List<Paciente>> ObtenerTodosIncluyendoInactivosAsync();
}