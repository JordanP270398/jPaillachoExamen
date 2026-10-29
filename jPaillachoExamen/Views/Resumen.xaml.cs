using jPaillachoExamen.Models;
using System.Globalization;

namespace jPaillachoExamen.Views;

public partial class Resumen : ContentPage
{
    private readonly string usuarioConectado;

    public Resumen(RegistroSocio socio)
    {
        InitializeComponent();

        usuarioConectado = socio.Usuario;

        CultureInfo cultura = CultureInfo.CurrentCulture;

        lblUsuarioConectado.Text =
            $"Usuario conectado: {usuarioConectado}";

        lblNombre.Text = socio.Nombre;

        lblApellido.Text = socio.Apellido;

        lblEdad.Text = socio.Edad.ToString(cultura);

        lblFechaInicio.Text = socio.FechaInicio.ToString(
            "dd/MM/yyyy",
            cultura);

        lblPlan.Text = socio.Plan;

        lblCasillero.Text = socio.Casillero ? "Sí" : "No";

        lblMontoInicial.Text = socio.MontoInicial.ToString(
            "F2",
            cultura);

        lblPagoMensual.Text = socio.PagoMensual.ToString(
            "F2",
            cultura);

        lblPagoTotal.Text = socio.PagoTotal.ToString(
            "F2",
            cultura);
    }

    private void OnNuevoRegistroClicked(
        object? sender, EventArgs e)
    {
        if (Window is null)
        {
            return;
        }

        // Abrir un formulario limpio con el mismo usuario.
        Window.Page = new NavigationPage(
            new Registro(usuarioConectado));
    }

    private void OnCerrarSesionClicked(
        object? sender, EventArgs e)
    {
        if (Window is null)
        {
            return;
        }

        // Abrir Login y reiniciar el historial de navegación.
        Window.Page = new NavigationPage(
            new Login());
    }
}