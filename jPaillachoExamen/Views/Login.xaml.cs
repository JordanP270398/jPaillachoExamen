namespace jPaillachoExamen.Views;

public partial class Login : ContentPage
{

    private readonly string[,] usuarios = new string[4, 2]
    {
        { "recepcion",  "gym2026"  },
        { "entrenador", "fit_01"   },
        { "uisrael",    "2026"     },
        { "gerente",    "admin123" }
    };

    public Login()
    {
        InitializeComponent();
    }

    private async void OnIniciarSesionClicked(
        object sender, EventArgs e)
    {
        string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
        string contrasena = txtContrasena.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(usuario) ||
            string.IsNullOrWhiteSpace(contrasena))
        {
            await DisplayAlertAsync(
                "Datos incompletos",
                "Ingrese el usuario y la contraseña.",
                "Aceptar");

            return;
        }

        bool accesoCorrecto = false;

        for (int fila = 0; fila < usuarios.GetLength(0); fila++)
        {
    
            if (usuario == usuarios[fila, 0] &&
                contrasena == usuarios[fila, 1])
            {
                accesoCorrecto = true;
                break;
            }
        }

        if (!accesoCorrecto)
        {
            await DisplayAlertAsync(
                "Acceso denegado",
                "Usuario o contraseña incorrectos.",
                "Aceptar");

            txtContrasena.Text = string.Empty;
            txtContrasena.Focus();

            return;
        }

       
        txtContrasena.Text = string.Empty;

        await Navigation.PushAsync(new Registro(usuario));
    }

    private async void OnAcercaDeClicked(
    object sender, EventArgs e)
    {
        await DisplayAlertAsync(
            "Acerca de",
            "Nombre: Jordan Paillacho\n" +
            "Carrera: Sistemas de Información\n" +
            "Paralelo: 8vo A",
            "Aceptar");
    }


}