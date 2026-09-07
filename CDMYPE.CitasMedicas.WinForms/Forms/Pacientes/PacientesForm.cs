using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Pacientes;

public class PacientesForm : Form
{
    private readonly PacienteService _pacienteService;

    private readonly DataGridView dgvPacientes;
    private readonly TextBox txtBuscar;
    private readonly Button btnNuevo;
    private readonly Label lblTotal;

    public PacientesForm(PacienteService pacienteService)
    {
        _pacienteService = pacienteService;

        FormBorderStyle = FormBorderStyle.None;
        TopLevel = false;
        Dock = DockStyle.Fill;
        BackColor = Color.FromArgb(245, 246, 248);

        var lblTitulo = new Label
        {
            Text = "Pacientes",
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(40, 30)
        };

        var lblDescripcion = new Label
        {
            Text = "Administración y consulta de pacientes registrados.",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(43, 78)
        };

        txtBuscar = new TextBox
        {
            PlaceholderText = "Buscar por nombre, apellido o documento...",
            Font = new Font("Segoe UI", 10),
            Location = new Point(43, 125),
            Width = 420,
            Height = 34
        };

        txtBuscar.TextChanged += TxtBuscar_TextChanged;

        btnNuevo = new Button
        {
            Text = "Nuevo paciente",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Width = 150,
            Height = 36,
            Location = new Point(480, 123),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(31, 41, 55),
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };

        btnNuevo.FlatAppearance.BorderSize = 0;

        lblTotal = new Label
        {
            Text = "0 pacientes",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(43, 180)
        };

        dgvPacientes = new DataGridView
        {
            Location = new Point(43, 210),

            Anchor = AnchorStyles.Top |
                    AnchorStyles.Bottom |
                    AnchorStyles.Left |
                    AnchorStyles.Right,

            Width = ClientSize.Width - 86,
            Height = ClientSize.Height - 250,

            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,

            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,

            ReadOnly = true,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,

            AutoGenerateColumns = false,
            RowHeadersVisible = false,

            // IMPORTANTE
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,

            ScrollBars = ScrollBars.Both
        };

        dgvPacientes.ColumnHeadersDefaultCellStyle.Font =
            new Font("Segoe UI", 9, FontStyle.Bold);

        dgvPacientes.DefaultCellStyle.Font =
            new Font("Segoe UI", 9);

        dgvPacientes.RowTemplate.Height = 34;

        ConfigurarColumnas();

        Controls.Add(lblTitulo);
        Controls.Add(lblDescripcion);
        Controls.Add(txtBuscar);
        Controls.Add(btnNuevo);
        Controls.Add(lblTotal);
        Controls.Add(dgvPacientes);

        Load += PacientesForm_Load;

        Resize += (_, _) =>
        {
            if (ClientSize.Width > 100 && ClientSize.Height > 300)
            {
                dgvPacientes.Width = ClientSize.Width - 86;
                dgvPacientes.Height = ClientSize.Height - 250;
            }
        };
    }

    private void ConfigurarColumnas()
    {
        dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Nombre completo",
            DataPropertyName = "NombreCompleto",
            Width = 280,
            MinimumWidth = 200
        });

        dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Documento",
            DataPropertyName = "DocumentoCompleto",
            Width = 190,
            MinimumWidth = 150
        });

        dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Sexo",
            DataPropertyName = "Sexo",
            Width = 100,
            MinimumWidth = 80
        });

        dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Edad",
            DataPropertyName = "Edad",
            Width = 80,
            MinimumWidth = 70
        });

        dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Nacionalidad",
            DataPropertyName = "Nacionalidad",
            Width = 170,
            MinimumWidth = 130
        });

        dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Teléfono",
            DataPropertyName = "Telefono",
            Width = 150,
            MinimumWidth = 120
        });

        dgvPacientes.Columns[0].AutoSizeMode =
            DataGridViewAutoSizeColumnMode.Fill;

        dgvPacientes.Columns[0].MinimumWidth = 250;
    }   

    private async void PacientesForm_Load(
        object? sender,
        EventArgs e)
    {
        await CargarPacientesAsync();
    }

    private async void TxtBuscar_TextChanged(
        object? sender,
        EventArgs e)
    {
        await CargarPacientesAsync(txtBuscar.Text);
    }

    private async Task CargarPacientesAsync(
        string? busqueda = null)
    {
        try
        {
            List<Paciente> pacientes;

            if (string.IsNullOrWhiteSpace(busqueda))
                pacientes =
                    await _pacienteService.ObtenerTodosAsync();
            else
                pacientes =
                    await _pacienteService.BuscarAsync(busqueda);

            var datos = pacientes.Select(p => new
            {
                p.PacienteId,

                NombreCompleto =
                    $"{p.Nombres} {p.Apellidos}",

                DocumentoCompleto =
                    $"{p.TipoDocumento} {p.NumeroDocumento}",

                p.Sexo,

                Edad = CalcularEdad(p.FechaNacimiento),

                p.Nacionalidad,

                p.Telefono
            }).ToList();

            dgvPacientes.DataSource = datos;

            lblTotal.Text =
                datos.Count == 1
                    ? "1 paciente"
                    : $"{datos.Count} pacientes";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible cargar los pacientes.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static int CalcularEdad(DateTime fechaNacimiento)
    {
        var hoy = DateTime.Today;

        var edad = hoy.Year - fechaNacimiento.Year;

        if (fechaNacimiento.Date > hoy.AddYears(-edad))
            edad--;

        return edad;
    }
}