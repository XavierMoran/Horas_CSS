using CDMYPE.CitasMedicas.Domain.Entities;
using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.WinForms.Forms.Pacientes;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Main;

public class MainForm : Form
{
    private readonly Usuario _usuario;

    private readonly Panel panelMenu;
    private readonly Panel panelContenido;
    private readonly Label lblUsuario;
    private readonly Label lblRol;
    private readonly PacienteService _pacienteService;


    public MainForm(Usuario usuario, PacienteService pacienteService)
    {
        _usuario = usuario;
        _pacienteService = pacienteService;

        Text = "Sistema de Gestión de Citas Médicas";
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1100, 700);
        BackColor = Color.White;

        panelMenu = new Panel
        {
            Dock = DockStyle.Left,
            Width = 240,
            BackColor = Color.FromArgb(31, 41, 55)
        };

        panelContenido = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(245, 246, 248)
        };

        var lblSistema = new Label
        {
            Text = "Gestión Médica",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(25, 30)
        };

        var lblSubtitulo = new Label
        {
            Text = "CDMYPE",
            ForeColor = Color.LightGray,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Location = new Point(27, 62)
        };

        panelMenu.Controls.Add(lblSistema);
        panelMenu.Controls.Add(lblSubtitulo);

        var btnInicio = CrearBotonMenu("Inicio", 110);
        var btnPacientes = CrearBotonMenu("Pacientes", 160);
        var btnAgenda = CrearBotonMenu("Agenda", 210);
        var btnCitas = CrearBotonMenu("Citas", 260);

        panelMenu.Controls.Add(btnInicio);
        panelMenu.Controls.Add(btnPacientes);
        panelMenu.Controls.Add(btnAgenda);
        panelMenu.Controls.Add(btnCitas);

        var posicionActual = 310;

        if (_usuario.Rol.Nombre == "SuperUsuario")
        {
            var btnProfesionales = CrearBotonMenu(
                "Profesionales",
                posicionActual);

            posicionActual += 50;

            var btnUsuarios = CrearBotonMenu(
                "Usuarios",
                posicionActual);

            posicionActual += 50;

            var btnConfiguracion = CrearBotonMenu(
                "Configuración",
                posicionActual);

            panelMenu.Controls.Add(btnProfesionales);
            panelMenu.Controls.Add(btnUsuarios);
            panelMenu.Controls.Add(btnConfiguracion);
        }

        var panelUsuario = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 110,
            BackColor = Color.FromArgb(24, 33, 46)
        };

        lblUsuario = new Label
        {
            Text = _usuario.NombreCompleto,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 20)
        };

        lblRol = new Label
        {
            Text = _usuario.Rol.Nombre,
            ForeColor = Color.LightGray,
            Font = new Font("Segoe UI", 8),
            AutoSize = true,
            Location = new Point(20, 45)
        };

        var btnCerrarSesion = new Button
        {
            Text = "Cerrar sesión",
            Width = 190,
            Height = 32,
            Location = new Point(20, 68),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(24, 33, 46),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9),
            Cursor = Cursors.Hand
        };

        btnCerrarSesion.FlatAppearance.BorderColor =
            Color.FromArgb(75, 85, 99);

        btnCerrarSesion.Click += (_, _) =>
        {
            DialogResult = DialogResult.OK;
            Close();
        };

        btnPacientes.Click += (_, _) => MostrarPacientes();
        btnInicio.Click += (_, _) => MostrarInicio();

        panelUsuario.Controls.Add(lblUsuario);
        panelUsuario.Controls.Add(lblRol);
        panelUsuario.Controls.Add(btnCerrarSesion);        panelMenu.Controls.Add(panelUsuario);

        Controls.Add(panelContenido);
        Controls.Add(panelMenu);

        MostrarInicio();

    }

    private void MostrarPacientes()
        {
            panelContenido.Controls.Clear();

            var pacientesForm =
                new PacientesForm(_pacienteService);

            panelContenido.Controls.Add(pacientesForm);

            pacientesForm.Show();
        }

    private Button CrearBotonMenu(string texto, int top)
    {
        var boton = new Button
        {
            Text = texto,
            Width = 240,
            Height = 45,
            Location = new Point(0, top),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(31, 41, 55),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(25, 0, 0, 0),
            Cursor = Cursors.Hand
        };

        boton.FlatAppearance.BorderSize = 0;

        boton.MouseEnter += (_, _) =>
            boton.BackColor = Color.FromArgb(55, 65, 81);

        boton.MouseLeave += (_, _) =>
            boton.BackColor = Color.FromArgb(31, 41, 55);

        return boton;
    }

    private void MostrarInicio()
    {
        panelContenido.Controls.Clear();

        var lblTitulo = new Label
        {
            Text = "Inicio",
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(40, 35)
        };

        var lblBienvenida = new Label
        {
            Text = $"Bienvenido, {_usuario.NombreCompleto}",
            Font = new Font("Segoe UI", 12),
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(43, 85)
        };

        var lblDescripcion = new Label
        {
            Text = "Seleccione una opción del menú para comenzar.",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.Gray,
            AutoSize = true,
            Location = new Point(43, 120)
        };

        panelContenido.Controls.Add(lblTitulo);
        panelContenido.Controls.Add(lblBienvenida);
        panelContenido.Controls.Add(lblDescripcion);
    }
}