using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Pacientes;

public class PacientesForm : Form
{
    private readonly PacienteService _pacienteService;
    private readonly ComboBox cmbEstado;

    private readonly DataGridView dgvPacientes;
    private readonly TextBox txtBuscar;
    private readonly Button btnNuevo;
    private readonly Button btnEditar;
    private readonly Label lblTotal;
    private readonly Button btnDesactivar;

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

        cmbEstado = new ComboBox
        {
            Location = new Point(480, 125),
            Width = 120,
            Height = 34,
            Font = new Font("Segoe UI", 10),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        cmbEstado.Items.AddRange(new object[]
        {
            "Activos",
            "Inactivos",
            "Todos"
        });

        cmbEstado.SelectedIndex = 0;

        cmbEstado.SelectedIndexChanged += async (_, _) =>
        {
            await CargarPacientesAsync(txtBuscar.Text);
        };

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

        btnDesactivar = new Button
        {
            Text = "Desactivar",
            Font = new Font("Segoe UI", 10),
            Width = 130,
            Height = 36,
            Location = new Point(810, 123),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.Firebrick,
            Cursor = Cursors.Hand
        };

        btnDesactivar.FlatAppearance.BorderColor =
            Color.FromArgb(180, 80, 80);

        btnDesactivar.Click += BtnDesactivar_Click;

        btnEditar = new Button
        {
            Text = "Editar paciente",
            Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Regular),

            Width = 150,
            Height = 36,

            Location = new Point(645, 123),

            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(31, 41, 55),

            Cursor = Cursors.Hand
        };

        btnEditar.FlatAppearance.BorderColor =
            Color.FromArgb(156, 163, 175);

        btnEditar.Click += BtnEditar_Click;

        btnNuevo.FlatAppearance.BorderSize = 0;
        btnNuevo.Click += BtnNuevo_Click;

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

        cmbEstado.Location = new Point(43, 165);
        cmbEstado.Width = 150;

        lblTotal.Location = new Point(210, 170);

        dgvPacientes.Location = new Point(43, 210);

        dgvPacientes.SelectionChanged += (_, _) =>
        {
            ActualizarBotonEstado();
        };

        Controls.Add(lblTitulo);
        Controls.Add(lblDescripcion);
        Controls.Add(txtBuscar);
        Controls.Add(btnNuevo);
        Controls.Add(lblTotal);
        Controls.Add(dgvPacientes);
        Controls.Add(btnEditar);
        Controls.Add(btnDesactivar);
        Controls.Add(cmbEstado);

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
        
        dgvPacientes.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "PacienteId",
                DataPropertyName = "PacienteId",
                Visible = false
            });

        dgvPacientes.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Activo",
                DataPropertyName = "Activo",
                Visible = false
            });

        dgvPacientes.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "NombreCompleto",
                HeaderText = "Nombre completo",
                DataPropertyName = "NombreCompleto",
                Width = 280,
                MinimumWidth = 250
            });

        dgvPacientes.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Documento",
                DataPropertyName = "DocumentoCompleto",
                Width = 190,
                MinimumWidth = 150
            });

        dgvPacientes.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Sexo",
                DataPropertyName = "Sexo",
                Width = 100,
                MinimumWidth = 80
            });

        dgvPacientes.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Edad",
                DataPropertyName = "Edad",
                Width = 80,
                MinimumWidth = 70
            });

        dgvPacientes.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Nacionalidad",
                DataPropertyName = "Nacionalidad",
                Width = 170,
                MinimumWidth = 130
            });

        dgvPacientes.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                Width = 150,
                MinimumWidth = 120
            });

        dgvPacientes.Columns["NombreCompleto"]!
            .AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;
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

            var estadoSeleccionado =
                cmbEstado.SelectedItem?.ToString() ?? "Activos";

            if (string.IsNullOrWhiteSpace(busqueda))
            {
                pacientes = estadoSeleccionado switch
                {
                    "Inactivos" =>
                        await _pacienteService.ObtenerInactivosAsync(),

                    "Todos" =>
                        await _pacienteService.ObtenerTodosIncluyendoInactivosAsync(),

                    _ =>
                        await _pacienteService.ObtenerTodosAsync()
                };
            }
            else
            {
                pacientes =
                    await _pacienteService.BuscarAsync(busqueda);

                pacientes = estadoSeleccionado switch
                {
                    "Inactivos" =>
                        pacientes.Where(x => !x.Activo).ToList(),

                    "Todos" =>
                        pacientes,

                    _ =>
                        pacientes.Where(x => x.Activo).ToList()
                };
            }

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

                p.Telefono,

                p.Activo
            }).ToList();

            dgvPacientes.DataSource = datos;

            lblTotal.Text =
                $"{pacientes.Count} " +
                (pacientes.Count == 1
                    ? "paciente"
                    : "pacientes");

            ActualizarBotonEstado();
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

    private void ActualizarBotonEstado()
    {
        if (dgvPacientes.SelectedRows.Count == 0)
        {
            btnDesactivar.Text = "Desactivar";
            return;
        }

        var valorActivo =
            dgvPacientes
                .SelectedRows[0]
                .Cells["Activo"]
                .Value;

        if (valorActivo is null)
            return;

        var activo =
            Convert.ToBoolean(valorActivo);

        btnDesactivar.Text =
            activo
                ? "Desactivar"
                : "Reactivar";

        btnDesactivar.ForeColor =
            activo
                ? Color.Firebrick
                : Color.FromArgb(31, 41, 55);
    }

    private async void BtnNuevo_Click(
    object? sender,
    EventArgs e)
    {
        using var pacienteForm =
            new PacienteForm(_pacienteService);

        var resultado =
            pacienteForm.ShowDialog(this);

        if (resultado == DialogResult.OK)
        {
            txtBuscar.Clear();
            await CargarPacientesAsync();
        }
    }

    private async void BtnEditar_Click(
        object? sender,
        EventArgs e)
    {
        if (dgvPacientes.SelectedRows.Count == 0)
        {
            MessageBox.Show(
                "Seleccione un paciente para editar.",
                "Pacientes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        var pacienteId =
            Convert.ToInt32(
                dgvPacientes
                    .SelectedRows[0]
                    .Cells["PacienteId"]
                    .Value);

        var paciente =
            await _pacienteService
                .ObtenerPorIdAsync(pacienteId);

        if (paciente is null)
        {
            MessageBox.Show(
                "No fue posible encontrar el paciente.",
                "Pacientes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        using var pacienteForm =
            new PacienteForm(
                _pacienteService,
                paciente);

        if (pacienteForm.ShowDialog(this)
            == DialogResult.OK)
        {
            await CargarPacientesAsync();
        }
    }

    private async void BtnDesactivar_Click(
        object? sender,
        EventArgs e)
    {
        if (dgvPacientes.SelectedRows.Count == 0)
        {
            MessageBox.Show(
                "Seleccione un paciente.",
                "Pacientes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        var pacienteId =
            Convert.ToInt32(
                dgvPacientes
                    .SelectedRows[0]
                    .Cells["PacienteId"]
                    .Value);

        var paciente =
            await _pacienteService
                .ObtenerPorIdAsync(pacienteId);

        if (paciente is null)
        {
            MessageBox.Show(
                "No fue posible encontrar el paciente.",
                "Pacientes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        var accion =
            paciente.Activo
                ? "desactivar"
                : "reactivar";

        var mensaje = paciente.Activo
            ? $"¿Desea desactivar a {paciente.Nombres} {paciente.Apellidos}?\n\n" +
            "El paciente dejará de estar disponible para nuevas citas, " +
            "pero se conservará su historial."
            : $"¿Desea reactivar a {paciente.Nombres} {paciente.Apellidos}?\n\n" +
            "El paciente volverá a estar disponible para nuevas citas.";

        var respuesta = MessageBox.Show(
            mensaje,
            paciente.Activo
                ? "Desactivar paciente"
                : "Reactivar paciente",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (respuesta != DialogResult.Yes)
            return;

        try
        {
            if (paciente.Activo)
            {
                await _pacienteService
                    .DesactivarPacienteAsync(paciente);
            }
            else
            {
                await _pacienteService
                    .ReactivarPacienteAsync(paciente);
            }

            await CargarPacientesAsync(txtBuscar.Text);

            MessageBox.Show(
                paciente.Activo
                    ? "El paciente fue reactivado correctamente."
                    : "El paciente fue desactivado correctamente.",
                "Pacientes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible {accion} el paciente.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}