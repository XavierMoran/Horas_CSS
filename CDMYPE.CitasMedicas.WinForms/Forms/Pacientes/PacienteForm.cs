using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Domain.Entities;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Pacientes;

public class PacienteForm : Form
{
    private readonly PacienteService _pacienteService;
    private readonly Paciente? _paciente;

    private readonly TextBox txtNombres;
    private readonly TextBox txtApellidos;
    private readonly ComboBox cmbSexo;
    private readonly DateTimePicker dtpFechaNacimiento;
    private readonly TextBox txtNacionalidad;
    private readonly ComboBox cmbTipoDocumento;
    private readonly TextBox txtNumeroDocumento;
    private readonly TextBox txtDireccion;
    private readonly TextBox txtTelefono;
    private readonly TextBox txtCorreo;
    private readonly Button btnGuardar;
    private readonly Button btnCancelar;
    private readonly Label lblError;

    public PacienteForm(
        PacienteService pacienteService,
        Paciente? paciente = null)
    {
        _pacienteService = pacienteService;
        _paciente = paciente;

        Text = "Nuevo paciente";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        Width = 760;
        Height = 720;

        BackColor = Color.White;

        var lblTitulo = new Label
        {
            Text = "Nuevo paciente",
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(35, 25)
        };

        var lblDescripcion = new Label
        {
            Text = "Ingrese la información básica del paciente.",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(38, 68)
        };

        var lblNombres = CrearLabel("Nombres", 35, 120);

        txtNombres = CrearTextBox(35, 148, 310);

        var lblApellidos = CrearLabel("Apellidos", 380, 120);

        txtApellidos = CrearTextBox(380, 148, 310);

        var lblSexo = CrearLabel("Sexo", 35, 205);

        cmbSexo = new ComboBox
        {
            Location = new Point(35, 233),
            Width = 310,
            Height = 32,
            Font = new Font("Segoe UI", 10),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        cmbSexo.Items.AddRange(new object[]
        {
            "Masculino",
            "Femenino"
        });

        var lblFechaNacimiento =
            CrearLabel("Fecha de nacimiento", 380, 205);

        dtpFechaNacimiento = new DateTimePicker
        {
            Location = new Point(380, 233),
            Width = 310,
            Font = new Font("Segoe UI", 10),
            Format = DateTimePickerFormat.Short,
            MaxDate = DateTime.Today
        };

        var lblNacionalidad =
            CrearLabel("Nacionalidad", 35, 290);

        txtNacionalidad = CrearTextBox(35, 318, 310);

        var lblTipoDocumento =
            CrearLabel("Tipo de documento", 380, 290);

        cmbTipoDocumento = new ComboBox
        {
            Location = new Point(380, 318),
            Width = 310,
            Height = 32,
            Font = new Font("Segoe UI", 10),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        cmbTipoDocumento.Items.AddRange(new object[]
        {
            "DUI",
            "Pasaporte",
            "Otro"
        });

        var lblNumeroDocumento =
            CrearLabel("Número de documento", 35, 375);

        txtNumeroDocumento = CrearTextBox(
            35,
            403,
            310);

        var lblTelefono =
            CrearLabel("Teléfono", 380, 375);

        txtTelefono = CrearTextBox(
            380,
            403,
            310);

        var lblDireccion =
            CrearLabel("Dirección", 35, 460);

        txtDireccion = CrearTextBox(
            35,
            488,
            655);

        var lblCorreo =
            CrearLabel("Correo electrónico", 35, 545);

        txtCorreo = CrearTextBox(
            35,
            573,
            310);

        lblError = new Label
        {
            ForeColor = Color.Firebrick,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Visible = false,
            Location = new Point(380, 578)
        };

        btnGuardar = new Button
        {
            Text = "Guardar",
            Width = 140,
            Height = 38,
            Location = new Point(550, 600),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(31, 41, 55),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        btnGuardar.FlatAppearance.BorderSize = 0;

        btnCancelar = new Button
        {
            Text = "Cancelar",
            Width = 140,
            Height = 38,
            Location = new Point(395, 600),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(31, 41, 55),
            Font = new Font("Segoe UI", 10),
            Cursor = Cursors.Hand
        };

        btnCancelar.FlatAppearance.BorderColor =
            Color.FromArgb(156, 163, 175);

        btnGuardar.Click += BtnGuardar_Click;

        btnCancelar.Click += (_, _) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        Controls.Add(lblTitulo);
        Controls.Add(lblDescripcion);

        Controls.Add(lblNombres);
        Controls.Add(txtNombres);

        Controls.Add(lblApellidos);
        Controls.Add(txtApellidos);

        Controls.Add(lblSexo);
        Controls.Add(cmbSexo);

        Controls.Add(lblFechaNacimiento);
        Controls.Add(dtpFechaNacimiento);

        Controls.Add(lblNacionalidad);
        Controls.Add(txtNacionalidad);

        Controls.Add(lblTipoDocumento);
        Controls.Add(cmbTipoDocumento);

        Controls.Add(lblNumeroDocumento);
        Controls.Add(txtNumeroDocumento);

        Controls.Add(lblTelefono);
        Controls.Add(txtTelefono);

        Controls.Add(lblDireccion);
        Controls.Add(txtDireccion);

        Controls.Add(lblCorreo);
        Controls.Add(txtCorreo);

        Controls.Add(lblError);

        Controls.Add(btnCancelar);
        Controls.Add(btnGuardar);

        if (_paciente is not null)
        {
            CargarPaciente();
        }

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;
    }

    private async void BtnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        lblError.Visible = false;

        if (!ValidarFormulario())
            return;

        btnGuardar.Enabled = false;
        btnGuardar.Text = "Guardando...";

        try
        {
        if (_paciente is null)
        {
            await _pacienteService.CrearPacienteAsync(
                txtNombres.Text,
                txtApellidos.Text,
                cmbSexo.SelectedItem!.ToString()!,
                dtpFechaNacimiento.Value,
                txtNacionalidad.Text,
                cmbTipoDocumento.SelectedItem!.ToString()!,
                txtNumeroDocumento.Text,
                txtDireccion.Text,
                txtTelefono.Text,
                txtCorreo.Text);
        }
        else
        {
            _paciente.Nombres =
                txtNombres.Text;

            _paciente.Apellidos =
                txtApellidos.Text;

            _paciente.Sexo =
                cmbSexo.SelectedItem!.ToString()!;

            _paciente.FechaNacimiento =
                dtpFechaNacimiento.Value;

            _paciente.Nacionalidad =
                txtNacionalidad.Text;

            _paciente.TipoDocumento =
                cmbTipoDocumento.SelectedItem!.ToString()!;

            _paciente.NumeroDocumento =
                txtNumeroDocumento.Text;

            _paciente.Direccion =
                txtDireccion.Text;

            _paciente.Telefono =
                txtTelefono.Text;

            _paciente.Correo =
                txtCorreo.Text;

            await _pacienteService
                .ActualizarPacienteAsync(_paciente);
        }

            DialogResult = DialogResult.OK;
            Close(); 

        }
        catch (InvalidOperationException ex)
        {
            MostrarError(ex.Message);
        }
        catch (Exception)
        {
            MostrarError(
                "No fue posible registrar el paciente.");
        }
        finally
        {
            btnGuardar.Enabled = true;
            btnGuardar.Text = "Guardar";
        }
    }

    private bool ValidarFormulario()
    {
        if (string.IsNullOrWhiteSpace(txtNombres.Text))
        {
            MostrarError("Ingrese los nombres.");
            txtNombres.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtApellidos.Text))
        {
            MostrarError("Ingrese los apellidos.");
            txtApellidos.Focus();
            return false;
        }

        if (cmbSexo.SelectedIndex < 0)
        {
            MostrarError("Seleccione el sexo.");
            cmbSexo.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtNacionalidad.Text))
        {
            MostrarError("Ingrese la nacionalidad.");
            txtNacionalidad.Focus();
            return false;
        }

        if (cmbTipoDocumento.SelectedIndex < 0)
        {
            MostrarError("Seleccione el tipo de documento.");
            cmbTipoDocumento.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(
            txtNumeroDocumento.Text))
        {
            MostrarError(
                "Ingrese el número de documento.");

            txtNumeroDocumento.Focus();

            return false;
        }

        if (string.IsNullOrWhiteSpace(txtDireccion.Text))
        {
            MostrarError("Ingrese la dirección.");
            txtDireccion.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtTelefono.Text))
        {
            MostrarError("Ingrese el teléfono.");
            txtTelefono.Focus();
            return false;
        }

        return true;
    }

    private void MostrarError(string mensaje)
    {
        lblError.Text = mensaje;
        lblError.Visible = true;
    }

    private static Label CrearLabel(
        string texto,
        int x,
        int y)
    {
        return new Label
        {
            Text = texto,
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(x, y)
        };
    }

    private static TextBox CrearTextBox(
        int x,
        int y,
        int width)
    {
        return new TextBox
        {
            Location = new Point(x, y),
            Width = width,
            Height = 32,
            Font = new Font("Segoe UI", 10)
        };
    }

    private void CargarPaciente()
    {
        if (_paciente is null)
            return;

        Text = "Editar paciente";

        txtNombres.Text = _paciente.Nombres;
        txtApellidos.Text = _paciente.Apellidos;

        cmbSexo.SelectedItem = _paciente.Sexo;

        dtpFechaNacimiento.Value =
            _paciente.FechaNacimiento;

        txtNacionalidad.Text =
            _paciente.Nacionalidad;

        cmbTipoDocumento.SelectedItem =
            _paciente.TipoDocumento;

        txtNumeroDocumento.Text =
            _paciente.NumeroDocumento;

        txtDireccion.Text =
            _paciente.Direccion;

        txtTelefono.Text =
            _paciente.Telefono;

        txtCorreo.Text =
            _paciente.Correo ?? "";
    }   

}