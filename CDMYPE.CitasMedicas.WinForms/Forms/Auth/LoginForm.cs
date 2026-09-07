using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.WinForms.Forms.Main;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Auth;

public class LoginForm : Form
{
    private readonly UsuarioService _usuarioService;

    private readonly TextBox txtUsuario;
    private readonly TextBox txtPassword;
    private readonly Button btnIngresar;
    private readonly Label lblError;
    private readonly PacienteService _pacienteService;

    public LoginForm(UsuarioService usuarioService, PacienteService pacienteService)
    {
        _usuarioService = usuarioService;
        _pacienteService = pacienteService;

        Text = "Sistema de Gestión de Citas Médicas";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = true;
        Width = 900;
        Height = 560;
        BackColor = Color.White;

        var panelIzquierdo = new Panel
        {
            Dock = DockStyle.Left,
            Width = 380,
            BackColor = Color.FromArgb(31, 41, 55)
        };

        var lblSistema = new Label
        {
            Text = "Sistema de Gestión\nde Citas Médicas",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(45, 170)
        };

        var lblDescripcion = new Label
        {
            Text = "Administración de pacientes,\nagendas y citas médicas.",
            ForeColor = Color.Gainsboro,
            Font = new Font("Segoe UI", 11, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(48, 270)
        };

        panelIzquierdo.Controls.Add(lblSistema);
        panelIzquierdo.Controls.Add(lblDescripcion);

        var lblTitulo = new Label
        {
            Text = "Iniciar sesión",
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(465, 110)
        };

        var lblSubtitulo = new Label
        {
            Text = "Ingrese sus credenciales para continuar",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(468, 155)
        };

        var lblUsuario = new Label
        {
            Text = "Usuario",
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(468, 215)
        };

        txtUsuario = new TextBox
        {
            Font = new Font("Segoe UI", 11),
            Width = 310,
            Height = 32,
            Location = new Point(470, 242)
        };

        var lblPassword = new Label
        {
            Text = "Contraseña",
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(468, 295)
        };

        txtPassword = new TextBox
        {
            Font = new Font("Segoe UI", 11),
            Width = 310,
            Height = 32,
            UseSystemPasswordChar = true,
            Location = new Point(470, 322)
        };

        lblError = new Label
        {
            ForeColor = Color.Firebrick,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Visible = false,
            Location = new Point(470, 365)
        };

        btnIngresar = new Button
        {
            Text = "Ingresar",
            Width = 310,
            Height = 42,
            Location = new Point(470, 405),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(31, 41, 55),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        btnIngresar.FlatAppearance.BorderSize = 0;
        btnIngresar.Click += BtnIngresar_Click;

        Controls.Add(panelIzquierdo);
        Controls.Add(lblTitulo);
        Controls.Add(lblSubtitulo);
        Controls.Add(lblUsuario);
        Controls.Add(txtUsuario);
        Controls.Add(lblPassword);
        Controls.Add(txtPassword);
        Controls.Add(lblError);
        Controls.Add(btnIngresar);

        AcceptButton = btnIngresar;
    }

    private async void BtnIngresar_Click(object? sender, EventArgs e)
    {
        lblError.Visible = false;

        var usuario = txtUsuario.Text.Trim();
        var password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(usuario) ||
            string.IsNullOrWhiteSpace(password))
        {
            MostrarError("Ingrese usuario y contraseña.");
            return;
        }

        btnIngresar.Enabled = false;
        btnIngresar.Text = "Ingresando...";

        try
        {
            var usuarioAutenticado =
                await _usuarioService.LoginAsync(usuario, password);

            if (usuarioAutenticado is null)
            {
                MostrarError("Usuario o contraseña incorrectos.");
                return;
            }

            var mainForm = new MainForm(usuarioAutenticado, _pacienteService);

            Hide();

            mainForm.ShowDialog();

            Show();

            txtPassword.Clear();
            txtPassword.Focus();        }
            
        catch (Exception)
        {
            MostrarError(
                "No fue posible iniciar sesión. Intente nuevamente.");
        }
        finally
        {
            btnIngresar.Enabled = true;
            btnIngresar.Text = "Ingresar";
        }
    }

    private void MostrarError(string mensaje)
    {
        lblError.Text = mensaje;
        lblError.Visible = true;
    }
}