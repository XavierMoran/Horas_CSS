using CDMYPE.CitasMedicas.Application.Services;
using CDMYPE.CitasMedicas.Domain.Entities;
using CDMYPE.CitasMedicas.WinForms.Forms.Pacientes;
using CDMYPE.CitasMedicas.WinForms.Forms.Profesionales;

namespace CDMYPE.CitasMedicas.WinForms.Forms.Main;

public class MainForm : Form
{
    private readonly Usuario _usuario;

    private readonly PacienteService _pacienteService;
    private readonly ProfesionalService _profesionalService;
    private readonly EspecialidadService _especialidadService;

    private readonly Panel panelMenu;
    private readonly Panel panelContenido;
    private readonly Label lblUsuario;
    private readonly Label lblRol;

    public MainForm(
        Usuario usuario,
        PacienteService pacienteService,
        ProfesionalService profesionalService,
        EspecialidadService especialidadService)
    {
        _usuario = usuario;
        _pacienteService = pacienteService;
        _profesionalService = profesionalService;
        _especialidadService = especialidadService;

        Text = "Sistema de Gestión de Citas Médicas";
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(245, 247, 250);

        // =========================
        // MENÚ LATERAL
        // =========================

        panelMenu = new Panel
        {
            Dock = DockStyle.Left,
            Width = 240,
            BackColor = Color.FromArgb(31, 41, 55)
        };

        // =========================
        // CONTENIDO PRINCIPAL
        // =========================

        panelContenido = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(245, 247, 250)
        };

        // =========================
        // ENCABEZADO DEL MENÚ
        // =========================

        var lblSistema = new Label
        {
            Text = "CITAS MÉDICAS",
            ForeColor = Color.White,
            Font = new Font(
                "Segoe UI",
                16,
                FontStyle.Bold),
            AutoSize = true,
            Location = new Point(25, 30)
        };

        var lblSubtitulo = new Label
        {
            Text = "Sistema de gestión",
            ForeColor = Color.FromArgb(156, 163, 175),
            Font = new Font(
                "Segoe UI",
                9),
            AutoSize = true,
            Location = new Point(27, 65)
        };

        panelMenu.Controls.Add(lblSistema);
        panelMenu.Controls.Add(lblSubtitulo);

        // =========================
        // BOTONES DEL MENÚ
        // =========================

        var posicionActual = 115;

        var btnInicio = CrearBotonMenu(
            "Inicio",
            posicionActual);

        btnInicio.Click += (_, _) =>
            MostrarInicio();

        posicionActual += 50;

        var btnPacientes = CrearBotonMenu(
            "Pacientes",
            posicionActual);

        btnPacientes.Click += (_, _) =>
            MostrarPacientes();

        posicionActual += 50;

        var btnAgenda = CrearBotonMenu(
            "Agenda",
            posicionActual);

        posicionActual += 50;

        var btnCitas = CrearBotonMenu(
            "Citas",
            posicionActual);

        posicionActual += 50;

        panelMenu.Controls.Add(btnInicio);
        panelMenu.Controls.Add(btnPacientes);
        panelMenu.Controls.Add(btnAgenda);
        panelMenu.Controls.Add(btnCitas);

        // =========================
        // MENÚ SUPERUSUARIO
        // =========================

        if (_usuario.Rol.Nombre == "SuperUsuario")
        {
            var btnProfesionales = CrearBotonMenu(
                "Profesionales",
                posicionActual);

            btnProfesionales.Click += (_, _) =>
                MostrarProfesionales();

            posicionActual += 50;

            var btnUsuarios = CrearBotonMenu(
                "Usuarios",
                posicionActual);

            posicionActual += 50;

            var btnConfiguracion = CrearBotonMenu(
                "Configuración",
                posicionActual);

            posicionActual += 50;

            panelMenu.Controls.Add(
                btnProfesionales);

            panelMenu.Controls.Add(
                btnUsuarios);

            panelMenu.Controls.Add(
                btnConfiguracion);
        }

        // =========================
        // INFORMACIÓN DEL USUARIO
        // =========================

        var panelUsuario = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 110,
            BackColor = Color.FromArgb(17, 24, 39)
        };

        lblUsuario = new Label
        {
            Text = _usuario.NombreCompleto,
            ForeColor = Color.White,
            Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold),
            AutoSize = true,
            Location = new Point(25, 25)
        };

        lblRol = new Label
        {
            Text = _usuario.Rol.Nombre,
            ForeColor = Color.FromArgb(156, 163, 175),
            Font = new Font(
                "Segoe UI",
                9),
            AutoSize = true,
            Location = new Point(25, 50)
        };

        panelUsuario.Controls.Add(lblUsuario);
        panelUsuario.Controls.Add(lblRol);

        panelMenu.Controls.Add(panelUsuario);

        // =========================
        // AGREGAR CONTROLES
        // =========================

        Controls.Add(panelContenido);
        Controls.Add(panelMenu);

        // Vista inicial
        MostrarInicio();
    }

    private Button CrearBotonMenu(
        string texto,
        int posicionY)
    {
        var boton = new Button
        {
            Text = texto,
            Width = 240,
            Height = 45,
            Location = new Point(0, posicionY),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(31, 41, 55),
            ForeColor = Color.White,
            Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Regular),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(25, 0, 0, 0),
            Cursor = Cursors.Hand
        };

        boton.FlatAppearance.BorderSize = 0;

        boton.FlatAppearance.MouseOverBackColor =
            Color.FromArgb(55, 65, 81);

        boton.FlatAppearance.MouseDownBackColor =
            Color.FromArgb(75, 85, 99);

        return boton;
    }

    private void MostrarInicio()
    {
        panelContenido.Controls.Clear();

        var lblTitulo = new Label
        {
            Text = "Inicio",
            Font = new Font(
                "Segoe UI",
                26,
                FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(45, 40)
        };

        var lblBienvenida = new Label
        {
            Text =
                $"Bienvenido, {_usuario.NombreCompleto}.",
            Font = new Font(
                "Segoe UI",
                12),
            ForeColor = Color.DimGray,
            AutoSize = true,
            Location = new Point(48, 95)
        };

        var lblDescripcion = new Label
        {
            Text =
                "Seleccione una opción del menú para comenzar.",
            Font = new Font(
                "Segoe UI",
                10),
            ForeColor = Color.Gray,
            AutoSize = true,
            Location = new Point(48, 130)
        };

        panelContenido.Controls.Add(lblTitulo);
        panelContenido.Controls.Add(lblBienvenida);
        panelContenido.Controls.Add(lblDescripcion);
    }

    private void MostrarPacientes()
    {
        panelContenido.Controls.Clear();

        var pacientesForm =
            new PacientesForm(
                _pacienteService);

        panelContenido.Controls.Add(
            pacientesForm);

        pacientesForm.Show();
    }

    private void MostrarProfesionales()
    {
        panelContenido.Controls.Clear();

        var profesionalesForm =
            new ProfesionalesForm(
                _profesionalService,
                _especialidadService);

        panelContenido.Controls.Add(
            profesionalesForm);

        profesionalesForm.Show();
    }    
}