using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Profesionales;

public class HorarioProfesionalForm : Form
{
    private readonly HorarioProfesionalService _horarioService;
    private readonly Profesional _profesional;
    private readonly HorarioProfesional? _horarioEditar;

    private readonly ComboBox cmbDia;
    private readonly DateTimePicker dtpHoraInicio;
    private readonly DateTimePicker dtpHoraFin;
    private readonly NumericUpDown nudDuracion;
    private readonly Button btnGuardar;
    private readonly Button btnCancelar;

    public HorarioProfesionalForm(
        HorarioProfesionalService horarioService,
        Profesional profesional,
        HorarioProfesional? horarioEditar = null)
    {
        _horarioService = horarioService;
        _profesional = profesional;
        _horarioEditar = horarioEditar;

        Text = horarioEditar is null
            ? "Nuevo bloque de horario"
            : "Editar bloque de horario";

        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;

        ClientSize = new Size(520, 470);
        BackColor = Color.White;
        Font = new Font("Segoe UI", 10F);

        // =====================================
        // TÍTULO
        // =====================================

        var lblTitulo = new Label
        {
            Text = horarioEditar is null
                ? "Nuevo bloque de atención"
                : "Editar bloque de atención",

            Font = new Font("Segoe UI", 17F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(35, 25)
        };

        var lblProfesional = new Label
        {
            Text = $"{_profesional.Nombres} {_profesional.Apellidos}",
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(38, 65)
        };

        // =====================================
        // DÍA
        // =====================================

        var lblDia = CrearLabel(
            "Día de atención *",
            38,
            115);

        cmbDia = new ComboBox
        {
            Location = new Point(38, 140),
            Width = 440,
            Height = 35,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        cmbDia.DataSource = ObtenerDiasSemana();
        cmbDia.DisplayMember = nameof(DiaSemanaOpcion.Nombre);
        cmbDia.ValueMember = nameof(DiaSemanaOpcion.Dia);

        // =====================================
        // HORA INICIO
        // =====================================

        var lblInicio = CrearLabel(
            "Hora de inicio *",
            38,
            195);

        dtpHoraInicio = new DateTimePicker
        {
            Location = new Point(38, 220),
            Width = 200,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "hh:mm tt",
            ShowUpDown = true
        };

        // =====================================
        // HORA FIN
        // =====================================

        var lblFin = CrearLabel(
            "Hora de finalización *",
            278,
            195);

        dtpHoraFin = new DateTimePicker
        {
            Location = new Point(278, 220),
            Width = 200,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "hh:mm tt",
            ShowUpDown = true
        };

        // =====================================
        // DURACIÓN
        // =====================================

        var lblDuracion = CrearLabel(
            "Duración de cada cita *",
            38,
            275);

        nudDuracion = new NumericUpDown
        {
            Location = new Point(38, 300),
            Width = 200,
            Minimum = 5,
            Maximum = 480,
            Increment = 5,
            Value = 30
        };

        var lblMinutos = new Label
        {
            Text = "minutos",
            AutoSize = true,
            ForeColor = Color.DimGray,
            Location = new Point(250, 304)
        };

        // =====================================
        // BOTONES
        // =====================================

        btnGuardar = new Button
        {
            Text = "Guardar",
            Location = new Point(278, 380),
            Size = new Size(95, 40),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(31, 41, 55),
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };

        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.Click += BtnGuardar_Click;

        btnCancelar = new Button
        {
            Text = "Cancelar",
            Location = new Point(383, 380),
            Size = new Size(95, 40),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(31, 41, 55),
            Cursor = Cursors.Hand,
            DialogResult = DialogResult.Cancel
        };

        btnCancelar.FlatAppearance.BorderColor =
            Color.FromArgb(209, 213, 219);

        Controls.AddRange(
        [
            lblTitulo,
            lblProfesional,

            lblDia,
            cmbDia,

            lblInicio,
            dtpHoraInicio,

            lblFin,
            dtpHoraFin,

            lblDuracion,
            nudDuracion,
            lblMinutos,

            btnGuardar,
            btnCancelar
        ]);

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;

        ConfigurarValoresIniciales();
    }

    // =========================================
    // VALORES INICIALES
    // =========================================

    private void ConfigurarValoresIniciales()
    {
        if (_horarioEditar is null)
        {
            cmbDia.SelectedValue = DayOfWeek.Monday;

            dtpHoraInicio.Value =
                DateTime.Today.AddHours(8);

            dtpHoraFin.Value =
                DateTime.Today.AddHours(12);

            nudDuracion.Value = 30;

            return;
        }

        cmbDia.SelectedValue =
            _horarioEditar.DiaSemana;

        dtpHoraInicio.Value =
            DateTime.Today.Add(
                _horarioEditar.HoraInicio);

        dtpHoraFin.Value =
            DateTime.Today.Add(
                _horarioEditar.HoraFin);

        var duracion =
            _horarioEditar.DuracionCitaMinutos;

        if (duracion >= nudDuracion.Minimum &&
            duracion <= nudDuracion.Maximum)
        {
            nudDuracion.Value = duracion;
        }
    }

    // =========================================
    // GUARDAR
    // =========================================

    private async void BtnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            if (cmbDia.SelectedValue is not DayOfWeek diaSemana)
            {
                MessageBox.Show(
                    "Seleccione un día de atención.",
                    "Horario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var horaInicio =
                dtpHoraInicio.Value.TimeOfDay;

            var horaFin =
                dtpHoraFin.Value.TimeOfDay;

            var duracion =
                Convert.ToInt32(
                    nudDuracion.Value);

            btnGuardar.Enabled = false;

            if (_horarioEditar is null)
            {
                var nuevoHorario =
                    new HorarioProfesional
                    {
                        ProfesionalId =
                            _profesional.ProfesionalId,

                        DiaSemana = diaSemana,

                        HoraInicio = horaInicio,

                        HoraFin = horaFin,

                        DuracionCitaMinutos =
                            duracion,

                        Activo = true
                    };

                await _horarioService
                    .CrearHorarioAsync(
                        nuevoHorario);
            }
            else
            {
                _horarioEditar.DiaSemana =
                    diaSemana;

                _horarioEditar.HoraInicio =
                    horaInicio;

                _horarioEditar.HoraFin =
                    horaFin;

                _horarioEditar
                    .DuracionCitaMinutos =
                    duracion;

                await _horarioService
                    .ActualizarHorarioAsync(
                        _horarioEditar);
            }

            DialogResult = DialogResult.OK;
            Close();
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
                $"No fue posible guardar el horario.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnGuardar.Enabled = true;
        }
    }

    // =========================================
    // UTILIDADES
    // =========================================

    private static Label CrearLabel(
        string texto,
        int x,
        int y)
    {
        return new Label
        {
            Text = texto,
            AutoSize = true,
            Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Bold),

            ForeColor =
                Color.FromArgb(55, 65, 81),

            Location = new Point(x, y)
        };
    }

    private static List<DiaSemanaOpcion>
        ObtenerDiasSemana()
    {
        return
        [
            new()
            {
                Dia = DayOfWeek.Monday,
                Nombre = "Lunes"
            },
            new()
            {
                Dia = DayOfWeek.Tuesday,
                Nombre = "Martes"
            },
            new()
            {
                Dia = DayOfWeek.Wednesday,
                Nombre = "Miércoles"
            },
            new()
            {
                Dia = DayOfWeek.Thursday,
                Nombre = "Jueves"
            },
            new()
            {
                Dia = DayOfWeek.Friday,
                Nombre = "Viernes"
            },
            new()
            {
                Dia = DayOfWeek.Saturday,
                Nombre = "Sábado"
            },
            new()
            {
                Dia = DayOfWeek.Sunday,
                Nombre = "Domingo"
            }
        ];
    }

    private class DiaSemanaOpcion
    {
        public DayOfWeek Dia { get; set; }

        public string Nombre { get; set; } =
            string.Empty;
    }
}