using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Profesionales;

public class ProfesionalForm : Form
{
    private readonly ProfesionalService _profesionalService;
    private readonly EspecialidadService _especialidadService;
    private readonly Profesional? _profesional;

    private readonly TextBox txtNombres;
    private readonly TextBox txtApellidos;
    private readonly TextBox txtJuntaVigilancia;
    private readonly TextBox txtTelefono;
    private readonly TextBox txtCorreo;

    private readonly CheckedListBox clbEspecialidades;

    private readonly Button btnGuardar;
    private readonly Button btnCancelar;

    private List<Especialidad> _especialidades = new();

    public ProfesionalForm(
        ProfesionalService profesionalService,
        EspecialidadService especialidadService,
        Profesional? profesional = null)
    {
        _profesionalService = profesionalService;
        _especialidadService = especialidadService;
        _profesional = profesional;

        Text = _profesional is null
            ? "Nuevo profesional"
            : "Editar profesional";

        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;

        MaximizeBox = false;
        MinimizeBox = false;

        Width = 720;
        Height = 680;

        BackColor = Color.FromArgb(245, 247, 250);

        // =========================
        // TÍTULO
        // =========================

        var lblTitulo = new Label
        {
            Text = _profesional is null
                ? "Nuevo profesional"
                : "Editar profesional",

            Font = new Font(
                "Segoe UI",
                22,
                FontStyle.Bold),

            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(40, 30)
        };

        var lblSubtitulo = new Label
        {
            Text = _profesional is null
                ? "Registre la información del profesional de salud."
                : "Modifique la información del profesional de salud.",

            Font = new Font(
                "Segoe UI",
                10),

            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(43, 75)
        };

        // =========================
        // NOMBRES
        // =========================

        var lblNombres = CrearLabel(
            "Nombres *",
            40,
            125);

        txtNombres = CrearTextBox(
            40,
            150,
            290);

        // =========================
        // APELLIDOS
        // =========================

        var lblApellidos = CrearLabel(
            "Apellidos *",
            360,
            125);

        txtApellidos = CrearTextBox(
            360,
            150,
            290);

        // =========================
        // JUNTA DE VIGILANCIA
        // =========================

        var lblJunta = CrearLabel(
            "Número de Junta de Vigilancia",
            40,
            210);

        txtJuntaVigilancia = CrearTextBox(
            40,
            235,
            290);

        // =========================
        // TELÉFONO
        // =========================

        var lblTelefono = CrearLabel(
            "Teléfono",
            360,
            210);

        txtTelefono = CrearTextBox(
            360,
            235,
            290);

        // =========================
        // CORREO
        // =========================

        var lblCorreo = CrearLabel(
            "Correo electrónico",
            40,
            295);

        txtCorreo = CrearTextBox(
            40,
            320,
            610);

        // =========================
        // ESPECIALIDADES
        // =========================

        var lblEspecialidades = CrearLabel(
            "Especialidades *",
            40,
            380);

        var lblAyudaEspecialidades = new Label
        {
            Text = "Seleccione una o varias especialidades.",
            Font = new Font(
                "Segoe UI",
                9),

            ForeColor = Color.Gray,
            AutoSize = true,
            Location = new Point(165, 383)
        };

        clbEspecialidades = new CheckedListBox
        {
            Location = new Point(40, 410),
            Width = 610,
            Height = 115,

            Font = new Font(
                "Segoe UI",
                10),

            CheckOnClick = true,
            BorderStyle = BorderStyle.FixedSingle
        };

        // =========================
        // BOTONES
        // =========================

        btnGuardar = new Button
        {
            Text = "Guardar",
            Width = 135,
            Height = 42,
            Location = new Point(365, 560),

            FlatStyle = FlatStyle.Flat,

            BackColor = Color.FromArgb(
                31,
                41,
                55),

            ForeColor = Color.White,

            Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold),

            Cursor = Cursors.Hand
        };

        btnGuardar.FlatAppearance.BorderSize = 0;

        btnCancelar = new Button
        {
            Text = "Cancelar",
            Width = 135,
            Height = 42,
            Location = new Point(515, 560),

            FlatStyle = FlatStyle.Flat,

            BackColor = Color.White,
            ForeColor = Color.FromArgb(31, 41, 55),

            Font = new Font(
                "Segoe UI",
                10),

            Cursor = Cursors.Hand
        };

        btnCancelar.FlatAppearance.BorderColor =
            Color.FromArgb(209, 213, 219);

        // =========================
        // EVENTOS
        // =========================

        Load += ProfesionalForm_Load;

        btnGuardar.Click +=
            BtnGuardar_Click;

        btnCancelar.Click += (_, _) =>
            Close();

        // =========================
        // CONTROLES
        // =========================

        Controls.Add(lblTitulo);
        Controls.Add(lblSubtitulo);

        Controls.Add(lblNombres);
        Controls.Add(txtNombres);

        Controls.Add(lblApellidos);
        Controls.Add(txtApellidos);

        Controls.Add(lblJunta);
        Controls.Add(txtJuntaVigilancia);

        Controls.Add(lblTelefono);
        Controls.Add(txtTelefono);

        Controls.Add(lblCorreo);
        Controls.Add(txtCorreo);

        Controls.Add(lblEspecialidades);
        Controls.Add(lblAyudaEspecialidades);
        Controls.Add(clbEspecialidades);

        Controls.Add(btnGuardar);
        Controls.Add(btnCancelar);

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;
    }

    // =========================================
    // CARGA DEL FORMULARIO
    // =========================================

    private async void ProfesionalForm_Load(
        object? sender,
        EventArgs e)
    {
        try
        {
            await CargarEspecialidadesAsync();

            if (_profesional is not null)
            {
                CargarDatosProfesional();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible cargar la información.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            Close();
        }
    }

    // =========================================
    // CARGAR ESPECIALIDADES
    // =========================================

    private async Task CargarEspecialidadesAsync()
    {
        _especialidades =
            await _especialidadService
                .ObtenerActivasAsync();

        clbEspecialidades.Items.Clear();

        foreach (var especialidad in _especialidades)
        {
            clbEspecialidades.Items.Add(
                especialidad.Nombre);
        }
    }

    // =========================================
    // CARGAR DATOS AL EDITAR
    // =========================================

    private void CargarDatosProfesional()
    {
        if (_profesional is null)
            return;

        txtNombres.Text =
            _profesional.Nombres;

        txtApellidos.Text =
            _profesional.Apellidos;

        txtJuntaVigilancia.Text =
            _profesional.NumeroJuntaVigilancia ?? string.Empty;

        txtTelefono.Text =
            _profesional.Telefono ?? string.Empty;

        txtCorreo.Text =
            _profesional.Correo ?? string.Empty;

        var especialidadesProfesional =
            _profesional
                .ProfesionalEspecialidades
                .Where(pe => pe.Activo)
                .Select(pe => pe.EspecialidadId)
                .ToHashSet();

        for (var i = 0;
             i < _especialidades.Count;
             i++)
        {
            if (especialidadesProfesional.Contains(
                _especialidades[i].EspecialidadId))
            {
                clbEspecialidades.SetItemChecked(
                    i,
                    true);
            }
        }
    }

    // =========================================
    // GUARDAR
    // =========================================

    private async void BtnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        btnGuardar.Enabled = false;
        btnCancelar.Enabled = false;

        try
        {
            if (!ValidarFormulario())
                return;

            var especialidadesSeleccionadas =
                ObtenerEspecialidadesSeleccionadas();

            if (_profesional is null)
            {
                await CrearProfesionalAsync(
                    especialidadesSeleccionadas);
            }
            else
            {
                await EditarProfesionalAsync(
                    especialidadesSeleccionadas);
            }

            DialogResult = DialogResult.OK;

            Close();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible guardar el profesional.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
        }
    }

    // =========================================
    // CREAR PROFESIONAL
    // =========================================

    private async Task CrearProfesionalAsync(
        List<Especialidad> especialidadesSeleccionadas)
    {
        var profesional = new Profesional
        {
            Nombres = txtNombres.Text,
            Apellidos = txtApellidos.Text,

            NumeroJuntaVigilancia =
                txtJuntaVigilancia.Text,

            Telefono =
                txtTelefono.Text,

            Correo =
                txtCorreo.Text
        };

        foreach (var especialidad
                 in especialidadesSeleccionadas)
        {
            profesional
                .ProfesionalEspecialidades
                .Add(
                    new ProfesionalEspecialidad
                    {
                        EspecialidadId =
                            especialidad.EspecialidadId,

                        Activo = true
                    });
        }

        await _profesionalService
            .CrearProfesionalAsync(profesional);
    }

    // =========================================
    // EDITAR PROFESIONAL
    // =========================================

    private async Task EditarProfesionalAsync(
        List<Especialidad> especialidadesSeleccionadas)
    {
        if (_profesional is null)
            return;

        _profesional.Nombres =
            txtNombres.Text;

        _profesional.Apellidos =
            txtApellidos.Text;

        _profesional.NumeroJuntaVigilancia =
            txtJuntaVigilancia.Text;

        _profesional.Telefono =
            txtTelefono.Text;

        _profesional.Correo =
            txtCorreo.Text;

        var idsSeleccionados =
            especialidadesSeleccionadas
                .Select(e => e.EspecialidadId)
                .ToHashSet();

        // Desactivar relaciones que ya no
        // están seleccionadas.

        foreach (var relacion in
                 _profesional.ProfesionalEspecialidades)
        {
            relacion.Activo =
                idsSeleccionados.Contains(
                    relacion.EspecialidadId);
        }

        // Agregar especialidades nuevas.

        var idsExistentes =
            _profesional
                .ProfesionalEspecialidades
                .Select(pe => pe.EspecialidadId)
                .ToHashSet();

        foreach (var especialidad
                 in especialidadesSeleccionadas)
        {
            if (idsExistentes.Contains(
                especialidad.EspecialidadId))
            {
                continue;
            }

            _profesional
                .ProfesionalEspecialidades
                .Add(
                    new ProfesionalEspecialidad
                    {
                        ProfesionalId =
                            _profesional.ProfesionalId,

                        EspecialidadId =
                            especialidad.EspecialidadId,

                        Activo = true
                    });
        }

        await _profesionalService
            .ActualizarProfesionalAsync(
                _profesional);
    }

    // =========================================
    // ESPECIALIDADES SELECCIONADAS
    // =========================================

    private List<Especialidad>
        ObtenerEspecialidadesSeleccionadas()
    {
        var seleccionadas =
            new List<Especialidad>();

        for (var i = 0;
             i < clbEspecialidades.Items.Count;
             i++)
        {
            if (clbEspecialidades.GetItemChecked(i))
            {
                seleccionadas.Add(
                    _especialidades[i]);
            }
        }

        return seleccionadas;
    }

    // =========================================
    // VALIDACIONES
    // =========================================

    private bool ValidarFormulario()
    {
        if (string.IsNullOrWhiteSpace(
            txtNombres.Text))
        {
            MostrarValidacion(
                "Ingrese los nombres del profesional.",
                txtNombres);

            return false;
        }

        if (string.IsNullOrWhiteSpace(
            txtApellidos.Text))
        {
            MostrarValidacion(
                "Ingrese los apellidos del profesional.",
                txtApellidos);

            return false;
        }

        if (!string.IsNullOrWhiteSpace(
                txtCorreo.Text) &&
            !txtCorreo.Text.Contains('@'))
        {
            MostrarValidacion(
                "Ingrese un correo electrónico válido.",
                txtCorreo);

            return false;
        }

        if (clbEspecialidades.CheckedItems.Count == 0)
        {
            MessageBox.Show(
                "Seleccione al menos una especialidad.",
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            clbEspecialidades.Focus();

            return false;
        }

        return true;
    }

    private static void MostrarValidacion(
        string mensaje,
        Control control)
    {
        MessageBox.Show(
            mensaje,
            "Validación",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        control.Focus();
    }

    // =========================================
    // CONTROLES AUXILIARES
    // =========================================

    private static Label CrearLabel(
        string texto,
        int x,
        int y)
    {
        return new Label
        {
            Text = texto,

            Font = new Font(
                "Segoe UI",
                10),

            ForeColor = Color.FromArgb(
                55,
                65,
                81),

            AutoSize = true,

            Location = new Point(
                x,
                y)
        };
    }

    private static TextBox CrearTextBox(
        int x,
        int y,
        int ancho)
    {
        return new TextBox
        {
            Font = new Font(
                "Segoe UI",
                10),

            Location = new Point(
                x,
                y),

            Width = ancho,

            Height = 32
        };
    }
}