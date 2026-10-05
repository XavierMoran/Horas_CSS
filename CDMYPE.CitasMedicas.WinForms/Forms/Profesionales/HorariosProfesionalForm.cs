using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Profesionales;

public class HorariosProfesionalForm : Form
{
    private readonly HorarioProfesionalService _horarioService;
    private readonly Profesional _profesional;

    private readonly ComboBox cmbEstado;
    private readonly Button btnNuevo;
    private readonly Button btnEditar;
    private readonly Button btnDesactivar;
    private readonly Label lblTotal;
    private readonly DataGridView dgvHorarios;

    public HorariosProfesionalForm(
        HorarioProfesionalService horarioService,
        Profesional profesional)
    {
        _horarioService = horarioService;
        _profesional = profesional;

        Text = "Horarios del profesional";

        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(850, 550);
        Size = new Size(950, 620);

        BackColor = Color.White;
        Font = new Font("Segoe UI", 10F);

        // =========================================
        // TÍTULO
        // =========================================

        var lblTitulo = new Label
        {
            Text = "Horario de atención",
            Font = new Font(
                "Segoe UI",
                20F,
                FontStyle.Bold),

            ForeColor =
                Color.FromArgb(31, 41, 55),

            AutoSize = true,
            Location = new Point(35, 25)
        };

        var lblProfesional = new Label
        {
            Text =
                $"{_profesional.Nombres} {_profesional.Apellidos}",

            Font = new Font(
                "Segoe UI",
                11F,
                FontStyle.Regular),

            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(38, 70)
        };

        var lblDescripcion = new Label
        {
            Text =
                "Configure los días, horas y duración de las citas del profesional.",

            Font = new Font(
                "Segoe UI",
                9.5F),

            ForeColor = Color.Gray,
            AutoSize = true,
            Location = new Point(38, 98)
        };

        // =========================================
        // FILTRO
        // =========================================

        var lblEstado = new Label
        {
            Text = "Estado",
            Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Bold),

            ForeColor =
                Color.FromArgb(55, 65, 81),

            AutoSize = true,
            Location = new Point(38, 145)
        };

        cmbEstado = new ComboBox
        {
            Location = new Point(38, 170),
            Width = 145,
            DropDownStyle =
                ComboBoxStyle.DropDownList
        };

        cmbEstado.Items.AddRange(
        [
            "Activos",
            "Inactivos",
            "Todos"
        ]);

        cmbEstado.SelectedIndex = 0;

        cmbEstado.SelectedIndexChanged +=
            CmbEstado_SelectedIndexChanged;

        lblTotal = new Label
        {
            Text = "0 bloques",
            AutoSize = true,
            ForeColor = Color.DimGray,
            Location = new Point(205, 175)
        };

        // =========================================
        // BOTONES
        // =========================================

        btnNuevo = new Button
        {
            Text = "Nuevo bloque",
            Size = new Size(130, 40),
            FlatStyle = FlatStyle.Flat,

            BackColor =
                Color.FromArgb(31, 41, 55),

            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right
        };

        btnNuevo.FlatAppearance.BorderSize = 0;

        btnEditar = new Button
        {
            Text = "Editar",
            Size = new Size(100, 40),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,

            ForeColor =
                Color.FromArgb(31, 41, 55),

            Cursor = Cursors.Hand,
            Enabled = false,

            Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right
        };

        btnEditar.FlatAppearance.BorderColor =
            Color.FromArgb(209, 213, 219);

        btnDesactivar = new Button
        {
            Text = "Desactivar",
            Size = new Size(115, 40),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.Firebrick,
            Cursor = Cursors.Hand,
            Enabled = false,

            Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right
        };

        btnDesactivar.FlatAppearance.BorderColor =
            Color.FromArgb(209, 213, 219);

        btnNuevo.Click += BtnNuevo_Click;
        btnEditar.Click += BtnEditar_Click;
        btnDesactivar.Click +=
            BtnDesactivar_Click;

        // =========================================
        // GRID
        // =========================================

        dgvHorarios = new DataGridView
        {
            Location = new Point(38, 230),
            Size = new Size(850, 300),

            Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right,

            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,

            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,

            ReadOnly = true,

            SelectionMode =
                DataGridViewSelectionMode.FullRowSelect,

            MultiSelect = false,
            AutoGenerateColumns = false,

            RowHeadersVisible = false,
            AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None,

            RowTemplate =
            {
                Height = 38
            }
        };

        dgvHorarios.ColumnHeadersHeight = 42;
        dgvHorarios.ColumnHeadersHeightSizeMode =
            DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        dgvHorarios.EnableHeadersVisualStyles = false;

        dgvHorarios.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(243, 244, 246);

        dgvHorarios.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.FromArgb(55, 65, 81);

        dgvHorarios.ColumnHeadersDefaultCellStyle.Font =
            new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Bold);

        dgvHorarios.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(229, 231, 235);

        dgvHorarios.DefaultCellStyle.SelectionForeColor =
            Color.FromArgb(17, 24, 39);

        dgvHorarios.DefaultCellStyle.ForeColor =
            Color.FromArgb(55, 65, 81);

        dgvHorarios.GridColor =
            Color.FromArgb(229, 231, 235);

        ConfigurarColumnas();

        dgvHorarios.SelectionChanged +=
            DgvHorarios_SelectionChanged;

        dgvHorarios.CellDoubleClick +=
            DgvHorarios_CellDoubleClick;

        // =========================================
        // AGREGAR CONTROLES
        // =========================================

        Controls.AddRange(
        [
            lblTitulo,
            lblProfesional,
            lblDescripcion,

            lblEstado,
            cmbEstado,
            lblTotal,

            btnNuevo,
            btnEditar,
            btnDesactivar,

            dgvHorarios
        ]);

        // =========================================
        // POSICIONAR BOTONES
        // =========================================

        ReposicionarBotones();

        Resize += (_, _) =>
        {
            ReposicionarBotones();
        };

        // =========================================
        // CARGA
        // =========================================

        Shown += async (_, _) =>
        {
            await CargarHorariosAsync();
        };
    }

    // =========================================
    // COLUMNAS
    // =========================================

    private void ConfigurarColumnas()
    {
        dgvHorarios.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "HorarioProfesionalId",
                DataPropertyName =
                    "HorarioProfesionalId",
                Visible = false
            });

        dgvHorarios.Columns.Add(
            new DataGridViewCheckBoxColumn
            {
                Name = "Activo",
                DataPropertyName = "Activo",
                Visible = false
            });

        dgvHorarios.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Dia",
                HeaderText = "Día",
                DataPropertyName = "Dia",
                FillWeight = 25
            });

        dgvHorarios.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "HoraInicio",
                HeaderText = "Desde",
                DataPropertyName = "HoraInicio",
                FillWeight = 20
            });

        dgvHorarios.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "HoraFin",
                HeaderText = "Hasta",
                DataPropertyName = "HoraFin",
                FillWeight = 20
            });

        dgvHorarios.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Duracion",
                HeaderText = "Duración",
                DataPropertyName = "Duracion",
                FillWeight = 20
            });

        dgvHorarios.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                HeaderText = "Estado",
                DataPropertyName = "Estado",
                FillWeight = 20
            });

        dgvHorarios.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;
    }

    // =========================================
    // CARGAR HORARIOS
    // =========================================

    private async Task CargarHorariosAsync()
    {
        try
        {
            List<HorarioProfesional> horarios;

            switch (cmbEstado.SelectedItem?.ToString())
            {
                case "Inactivos":

                    var todosInactivos =
                        await _horarioService
                            .ObtenerTodosPorProfesionalAsync(
                                _profesional.ProfesionalId);

                    horarios = todosInactivos
                        .Where(h => !h.Activo)
                        .ToList();

                    break;

                case "Todos":

                    horarios =
                        await _horarioService
                            .ObtenerTodosPorProfesionalAsync(
                                _profesional.ProfesionalId);

                    break;

                default:

                    horarios =
                        await _horarioService
                            .ObtenerPorProfesionalAsync(
                                _profesional.ProfesionalId);

                    break;
            }

            var datos = horarios
                .OrderBy(h =>
                    ObtenerOrdenDia(
                        h.DiaSemana))
                .ThenBy(h => h.HoraInicio)
                .Select(h => new
                {
                    h.HorarioProfesionalId,
                    h.Activo,

                    Dia =
                        ObtenerNombreDia(
                            h.DiaSemana),

                    HoraInicio =
                        FormatearHora(
                            h.HoraInicio),

                    HoraFin =
                        FormatearHora(
                            h.HoraFin),

                    Duracion =
                        $"{h.DuracionCitaMinutos} min",

                    Estado =
                        h.Activo
                            ? "Activo"
                            : "Inactivo"
                })
                .ToList();

            dgvHorarios.DataSource = null;
            dgvHorarios.DataSource = datos;

            lblTotal.Text =
                datos.Count == 1
                    ? "1 bloque"
                    : $"{datos.Count} bloques";

            if (dgvHorarios.Rows.Count > 0)
            {
                dgvHorarios.ClearSelection();
            }

            ActualizarBotones();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible cargar los horarios.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    // =========================================
    // NUEVO
    // =========================================

    private async void BtnNuevo_Click(
        object? sender,
        EventArgs e)
    {
        using var formulario =
            new HorarioProfesionalForm(
                _horarioService,
                _profesional);

        var resultado =
            formulario.ShowDialog(this);

        if (resultado == DialogResult.OK)
        {
            await CargarHorariosAsync();
        }
    }

    // =========================================
    // EDITAR
    // =========================================

    private async void BtnEditar_Click(
        object? sender,
        EventArgs e)
    {
        await EditarHorarioSeleccionadoAsync();
    }

    private async Task
        EditarHorarioSeleccionadoAsync()
    {
        if (dgvHorarios.SelectedRows.Count == 0)
        {
            MessageBox.Show(
                "Seleccione un bloque de horario para editar.",
                "Horarios",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        try
        {
            var valorId =
                dgvHorarios.SelectedRows[0]
                    .Cells["HorarioProfesionalId"]
                    .Value;

            if (valorId is null)
            {
                return;
            }

            var horarioId =
                Convert.ToInt32(valorId);

            var horario =
                await _horarioService
                    .ObtenerPorIdAsync(
                        horarioId);

            if (horario is null)
            {
                MessageBox.Show(
                    "No fue posible encontrar el horario seleccionado.",
                    "Horario no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using var formulario =
                new HorarioProfesionalForm(
                    _horarioService,
                    _profesional,
                    horario);

            var resultado =
                formulario.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                await CargarHorariosAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible editar el horario.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    // =========================================
    // DESACTIVAR / REACTIVAR
    // =========================================

    private async void BtnDesactivar_Click(
        object? sender,
        EventArgs e)
    {
        if (dgvHorarios.SelectedRows.Count == 0)
        {
            MessageBox.Show(
                "Seleccione un bloque de horario.",
                "Horarios",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        try
        {
            var fila =
                dgvHorarios.SelectedRows[0];

            var valorId =
                fila.Cells[
                    "HorarioProfesionalId"]
                    .Value;

            var valorActivo =
                fila.Cells["Activo"].Value;

            if (valorId is null ||
                valorActivo is null)
            {
                return;
            }

            var horarioId =
                Convert.ToInt32(valorId);

            var activo =
                Convert.ToBoolean(valorActivo);

            var horario =
                await _horarioService
                    .ObtenerPorIdAsync(
                        horarioId);

            if (horario is null)
            {
                MessageBox.Show(
                    "No fue posible encontrar el horario seleccionado.",
                    "Horario no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (activo)
            {
                var confirmacion =
                    MessageBox.Show(
                        "¿Desea desactivar este bloque de horario?\n\n" +
                        "Dejará de estar disponible para nuevas citas, " +
                        "pero permanecerá almacenado en el sistema.",
                        "Desactivar horario",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (confirmacion !=
                    DialogResult.Yes)
                {
                    return;
                }

                await _horarioService
                    .DesactivarHorarioAsync(
                        horario);
            }
            else
            {
                var confirmacion =
                    MessageBox.Show(
                        "¿Desea reactivar este bloque de horario?",
                        "Reactivar horario",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (confirmacion !=
                    DialogResult.Yes)
                {
                    return;
                }

                await _horarioService
                    .ReactivarHorarioAsync(
                        horario);
            }

            await CargarHorariosAsync();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Validación de horario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible actualizar el horario.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    // =========================================
    // SELECCIÓN
    // =========================================

    private void DgvHorarios_SelectionChanged(
        object? sender,
        EventArgs e)
    {
        ActualizarBotones();
    }

    private async void DgvHorarios_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        await EditarHorarioSeleccionadoAsync();
    }

    private void ActualizarBotones()
    {
        var haySeleccion =
            dgvHorarios.SelectedRows.Count > 0;

        btnEditar.Enabled =
            haySeleccion;

        btnDesactivar.Enabled =
            haySeleccion;

        if (!haySeleccion)
        {
            btnDesactivar.Text =
                "Desactivar";

            btnDesactivar.ForeColor =
                Color.Gray;

            return;
        }

        var valorActivo =
            dgvHorarios.SelectedRows[0]
                .Cells["Activo"]
                .Value;

        if (valorActivo is null)
        {
            btnEditar.Enabled = false;
            btnDesactivar.Enabled = false;

            return;
        }

        var activo =
            Convert.ToBoolean(
                valorActivo);

        btnDesactivar.Text =
            activo
                ? "Desactivar"
                : "Reactivar";

        btnDesactivar.ForeColor =
            activo
                ? Color.Firebrick
                : Color.FromArgb(
                    31,
                    41,
                    55);
    }

    // =========================================
    // FILTRO
    // =========================================

    private async void CmbEstado_SelectedIndexChanged(
        object? sender,
        EventArgs e)
    {
        if (!IsHandleCreated)
        {
            return;
        }

        await CargarHorariosAsync();
    }

    // =========================================
    // RESPONSIVE
    // =========================================

    private void ReposicionarBotones()
    {
        var margenDerecho = 45;

        btnDesactivar.Location =
            new Point(
                ClientSize.Width -
                    margenDerecho -
                    btnDesactivar.Width,
                165);

        btnEditar.Location =
            new Point(
                btnDesactivar.Left -
                    10 -
                    btnEditar.Width,
                165);

        btnNuevo.Location =
            new Point(
                btnEditar.Left -
                    10 -
                    btnNuevo.Width,
                165);
    }

    // =========================================
    // UTILIDADES
    // =========================================

    private static string ObtenerNombreDia(
        DayOfWeek dia)
    {
        return dia switch
        {
            DayOfWeek.Monday =>
                "Lunes",

            DayOfWeek.Tuesday =>
                "Martes",

            DayOfWeek.Wednesday =>
                "Miércoles",

            DayOfWeek.Thursday =>
                "Jueves",

            DayOfWeek.Friday =>
                "Viernes",

            DayOfWeek.Saturday =>
                "Sábado",

            DayOfWeek.Sunday =>
                "Domingo",

            _ => dia.ToString()
        };
    }

    private static int ObtenerOrdenDia(
        DayOfWeek dia)
    {
        return dia switch
        {
            DayOfWeek.Monday => 1,
            DayOfWeek.Tuesday => 2,
            DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,
            DayOfWeek.Saturday => 6,
            DayOfWeek.Sunday => 7,
            _ => 8
        };
    }

    private static string FormatearHora(
        TimeSpan hora)
    {
        return DateTime.Today
            .Add(hora)
            .ToString("hh:mm tt");
    }
}