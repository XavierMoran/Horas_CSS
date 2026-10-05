using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Profesionales;

public class ProfesionalesForm : Form
{
    private readonly ProfesionalService _profesionalService;
    private readonly EspecialidadService _especialidadService;

    private readonly TextBox txtBuscar;
    private readonly ComboBox cmbEstado;

    private readonly Button btnNuevo;
    private readonly Button btnEditar;
    private readonly Button btnDesactivar;

    private readonly Label lblTotal;
    private readonly DataGridView dgvProfesionales;

    public ProfesionalesForm(
        ProfesionalService profesionalService, EspecialidadService especialidadService)
    {
        _profesionalService = profesionalService;
        _especialidadService = especialidadService;

        FormBorderStyle = FormBorderStyle.None;
        TopLevel = false;
        Dock = DockStyle.Fill;

        BackColor =
            Color.FromArgb(245, 246, 248);

        // ==========================================
        // TÍTULO
        // ==========================================

        var lblTitulo = new Label
        {
            Text = "Profesionales",
            Font = new Font(
                "Segoe UI",
                24,
                FontStyle.Bold),

            ForeColor =
                Color.FromArgb(31, 41, 55),

            AutoSize = true,
            Location = new Point(40, 30)
        };

        var lblDescripcion = new Label
        {
            Text =
                "Administración de profesionales de salud.",

            Font = new Font("Segoe UI", 10),
            ForeColor = Color.DimGray,

            AutoSize = true,
            Location = new Point(43, 78)
        };

        // ==========================================
        // BUSCADOR
        // ==========================================

        txtBuscar = new TextBox
        {
            PlaceholderText =
                "Buscar por nombre, apellido o Junta de Vigilancia...",

            Font = new Font("Segoe UI", 10),

            Location = new Point(43, 125),

            Width = 420,
            Height = 34
        };

        txtBuscar.TextChanged +=
            TxtBuscar_TextChanged;

        // ==========================================
        // BOTÓN NUEVO
        // ==========================================

        btnNuevo = new Button
        {
            Text = "Nuevo profesional",

            Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold),

            Width = 150,
            Height = 36,

            Location = new Point(480, 123),

            FlatStyle = FlatStyle.Flat,

            BackColor =
                Color.FromArgb(31, 41, 55),

            ForeColor = Color.White,

            Cursor = Cursors.Hand
        };

        btnNuevo.FlatAppearance.BorderSize = 0;

        btnNuevo.Click += BtnNuevo_Click;

        // ==========================================
        // BOTÓN EDITAR
        // ==========================================

        btnEditar = new Button
        {
            Text = "Editar profesional",

            Font = new Font("Segoe UI", 10),

            Width = 150,
            Height = 36,

            Location = new Point(645, 123),

            FlatStyle = FlatStyle.Flat,

            BackColor = Color.White,

            ForeColor =
                Color.FromArgb(31, 41, 55),

            Cursor = Cursors.Hand
        };

        btnEditar.FlatAppearance.BorderColor =
            Color.FromArgb(156, 163, 175);

        btnEditar.Click += BtnEditar_Click;

        // ==========================================
        // BOTÓN DESACTIVAR
        // ==========================================

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

        // ==========================================
        // FILTRO DE ESTADO
        // ==========================================

        cmbEstado = new ComboBox
        {
            Location = new Point(43, 165),

            Width = 150,
            Height = 34,

            Font = new Font("Segoe UI", 10),

            DropDownStyle =
                ComboBoxStyle.DropDownList
        };

        cmbEstado.Items.AddRange(
            new object[]
            {
                "Activos",
                "Inactivos",
                "Todos"
            });

        cmbEstado.SelectedIndex = 0;

        cmbEstado.SelectedIndexChanged +=
            async (_, _) =>
            {
                await CargarProfesionalesAsync(
                    txtBuscar.Text);
            };

        // ==========================================
        // CONTADOR
        // ==========================================

        lblTotal = new Label
        {
            Text = "0 profesionales",

            Font = new Font("Segoe UI", 9),

            ForeColor = Color.DimGray,

            AutoSize = true,

            Location = new Point(210, 170)
        };

        // ==========================================
        // TABLA
        // ==========================================

        dgvProfesionales = new DataGridView
        {
            Location = new Point(43, 210),

            Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right,

            Width = ClientSize.Width - 86,
            Height = ClientSize.Height - 250,

            BackgroundColor = Color.White,

            BorderStyle =
                BorderStyle.FixedSingle,

            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,

            ReadOnly = true,
            MultiSelect = false,

            SelectionMode =
                DataGridViewSelectionMode.FullRowSelect,

            AutoGenerateColumns = false,

            RowHeadersVisible = false,

            AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None,

            ScrollBars = ScrollBars.Both
        };

        dgvProfesionales
            .ColumnHeadersDefaultCellStyle.Font =
            new Font(
                "Segoe UI",
                9,
                FontStyle.Bold);

        dgvProfesionales
            .DefaultCellStyle.Font =
            new Font("Segoe UI", 9);

        dgvProfesionales.RowTemplate.Height = 34;

        ConfigurarColumnas();

        dgvProfesionales.SelectionChanged +=
            (_, _) =>
            {
                ActualizarBotonEstado();
            };

        // ==========================================
        // CONTROLES
        // ==========================================

        Controls.Add(lblTitulo);
        Controls.Add(lblDescripcion);

        Controls.Add(txtBuscar);

        Controls.Add(btnNuevo);
        Controls.Add(btnEditar);
        Controls.Add(btnDesactivar);

        Controls.Add(cmbEstado);
        Controls.Add(lblTotal);

        Controls.Add(dgvProfesionales);

        // ==========================================
        // EVENTOS DEL FORMULARIO
        // ==========================================

        Load += ProfesionalesForm_Load;

        Resize += (_, _) =>
        {
            if (ClientSize.Width > 100 &&
                ClientSize.Height > 300)
            {
                dgvProfesionales.Width =
                    ClientSize.Width - 86;

                dgvProfesionales.Height =
                    ClientSize.Height - 250;
            }
        };
    }

    private void ConfigurarColumnas()
    {
        dgvProfesionales.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "ProfesionalId",
                DataPropertyName = "ProfesionalId",
                Visible = false
            });

        dgvProfesionales.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Activo",
                DataPropertyName = "Activo",
                Visible = false
            });

        dgvProfesionales.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "NombreCompleto",

                HeaderText =
                    "Nombre completo",

                DataPropertyName =
                    "NombreCompleto",

                Width = 260,
                MinimumWidth = 220
            });

        dgvProfesionales.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText =
                    "Junta de Vigilancia",

                DataPropertyName =
                    "NumeroJuntaVigilancia",

                Width = 170,
                MinimumWidth = 140
            });

        dgvProfesionales.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Especialidades",

                DataPropertyName =
                    "Especialidades",

                Width = 230,
                MinimumWidth = 180
            });

        dgvProfesionales.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Teléfono",

                DataPropertyName =
                    "Telefono",

                Width = 140,
                MinimumWidth = 110
            });

        dgvProfesionales.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Correo",

                DataPropertyName =
                    "Correo",

                Width = 220,
                MinimumWidth = 170
            });

        dgvProfesionales
            .Columns["NombreCompleto"]!
            .AutoSizeMode =
            DataGridViewAutoSizeColumnMode.Fill;
    }

    private async void ProfesionalesForm_Load(
        object? sender,
        EventArgs e)
    {
        await CargarProfesionalesAsync();
    }

    private async void TxtBuscar_TextChanged(
        object? sender,
        EventArgs e)
    {
        await CargarProfesionalesAsync(
            txtBuscar.Text);
    }

    private async Task CargarProfesionalesAsync(
        string? busqueda = null)
    {
        try
        {
            List<Profesional> profesionales;

            var estadoSeleccionado =
                cmbEstado.SelectedItem?.ToString()
                ?? "Activos";

            if (string.IsNullOrWhiteSpace(busqueda))
            {
                profesionales =
                    estadoSeleccionado switch
                    {
                        "Inactivos" =>
                            await _profesionalService
                                .ObtenerInactivosAsync(),

                        "Todos" =>
                            await _profesionalService
                                .ObtenerTodosIncluyendoInactivosAsync(),

                        _ =>
                            await _profesionalService
                                .ObtenerTodosAsync()
                    };
            }
            else
            {
                profesionales =
                    await _profesionalService
                        .BuscarAsync(busqueda);

                profesionales =
                    estadoSeleccionado switch
                    {
                        "Inactivos" =>
                            profesionales
                                .Where(p => !p.Activo)
                                .ToList(),

                        "Todos" =>
                            profesionales,

                        _ =>
                            profesionales
                                .Where(p => p.Activo)
                                .ToList()
                    };
            }

            var datos =
                profesionales
                    .Select(p => new
                    {
                        p.ProfesionalId,

                        p.Activo,

                        NombreCompleto =
                            $"{p.Nombres} {p.Apellidos}",

                        p.NumeroJuntaVigilancia,

                        Especialidades =
                            string.Join(
                                ", ",
                                p.ProfesionalEspecialidades
                                    .Where(pe =>
                                        pe.Activo &&
                                        pe.Especialidad.Activo)
                                    .Select(pe =>
                                        pe.Especialidad.Nombre)),

                        p.Telefono,

                        p.Correo
                    })
                    .ToList();

            dgvProfesionales.DataSource = datos;

            lblTotal.Text =
                $"{profesionales.Count} " +
                (profesionales.Count == 1
                    ? "profesional"
                    : "profesionales");

            ActualizarBotonEstado();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible cargar los profesionales.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void BtnNuevo_Click(
        object? sender,
        EventArgs e)
    {
        using var formulario = new ProfesionalForm(
            _profesionalService,
            _especialidadService);

        var resultado =
            formulario.ShowDialog(this);

        if (resultado == DialogResult.OK)
        {
            await CargarProfesionalesAsync(
                txtBuscar.Text);
        }
    }

    private async void BtnEditar_Click(
        object? sender,
        EventArgs e)
    {
        if (dgvProfesionales.SelectedRows.Count == 0)
        {
            MessageBox.Show(
                "Seleccione un profesional para editar.",
                "Editar profesional",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        try
        {
            var valorId =
                dgvProfesionales
                    .SelectedRows[0]
                    .Cells["ProfesionalId"]
                    .Value;

            if (valorId is null)
                return;

            var profesionalId =
                Convert.ToInt32(valorId);

            var profesional =
                await _profesionalService
                    .ObtenerPorIdAsync(profesionalId);

            if (profesional is null)
            {
                MessageBox.Show(
                    "No fue posible encontrar el profesional seleccionado.",
                    "Profesional no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using var formulario =
                new ProfesionalForm(
                    _profesionalService,
                    _especialidadService,
                    profesional);

            var resultado =
                formulario.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                await CargarProfesionalesAsync(
                    txtBuscar.Text);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible editar el profesional.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void BtnDesactivar_Click(
        object? sender,
        EventArgs e)
    {
        if (dgvProfesionales.SelectedRows.Count == 0)
        {
            MessageBox.Show(
                "Seleccione un profesional.",
                "Profesionales",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        try
        {
            var fila =
                dgvProfesionales.SelectedRows[0];

            var valorId =
                fila.Cells["ProfesionalId"].Value;

            var valorActivo =
                fila.Cells["Activo"].Value;

            if (valorId is null ||
                valorActivo is null)
            {
                return;
            }

            var profesionalId =
                Convert.ToInt32(valorId);

            var activo =
                Convert.ToBoolean(valorActivo);

            var profesional =
                await _profesionalService
                    .ObtenerPorIdAsync(profesionalId);

            if (profesional is null)
            {
                MessageBox.Show(
                    "No fue posible encontrar el profesional seleccionado.",
                    "Profesional no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var nombreCompleto =
                $"{profesional.Nombres} {profesional.Apellidos}";

            if (activo)
            {
                var confirmacion =
                    MessageBox.Show(
                        $"¿Desea desactivar al profesional \"{nombreCompleto}\"?\n\n" +
                        "El profesional dejará de aparecer entre los profesionales activos, " +
                        "pero su información permanecerá almacenada.",
                        "Desactivar profesional",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (confirmacion != DialogResult.Yes)
                    return;

                await _profesionalService
                    .DesactivarProfesionalAsync(
                        profesional);
            }
            else
            {
                var confirmacion =
                    MessageBox.Show(
                        $"¿Desea reactivar al profesional \"{nombreCompleto}\"?",
                        "Reactivar profesional",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (confirmacion != DialogResult.Yes)
                    return;

                await _profesionalService
                    .ReactivarProfesionalAsync(
                        profesional);
            }

            await CargarProfesionalesAsync(
                txtBuscar.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible actualizar el estado del profesional.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }    

    private void ActualizarBotonEstado()
    {
        var haySeleccion =
            dgvProfesionales.SelectedRows.Count > 0;

        btnEditar.Enabled = haySeleccion;
        btnDesactivar.Enabled = haySeleccion;

        if (!haySeleccion)
        {
            btnDesactivar.Text =
                "Desactivar";

            btnDesactivar.ForeColor =
                Color.Gray;

            return;
        }

        var valorActivo =
            dgvProfesionales
                .SelectedRows[0]
                .Cells["Activo"]
                .Value;

        if (valorActivo is null)
        {
            btnEditar.Enabled = false;
            btnDesactivar.Enabled = false;

            return;
        }

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

}