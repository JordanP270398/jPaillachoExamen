using jPaillachoExamen.Models;
using System.Globalization;


namespace jPaillachoExamen.Views;



public partial class Registro : ContentPage
{
    private const decimal CostoMembresia = 1200m;
    private const int NumeroCuotas = 5;
    private RegistroSocio? registroCalculado;

    private readonly string usuarioConectado = string.Empty;

    public Registro()
    {
        InitializeComponent();

        dpFechaInicio.MinimumDate = DateTime.Today;
        dpFechaInicio.Date = DateTime.Today;

       
        txtNombre.TextChanged += (_, _) => InvalidarCalculo();
        txtApellido.TextChanged += (_, _) => InvalidarCalculo();
        txtEdad.TextChanged += (_, _) => InvalidarCalculo();
        dpFechaInicio.DateSelected += (_, _) => InvalidarCalculo();
        pkPlan.SelectedIndexChanged += (_, _) => InvalidarCalculo();
        swCasillero.Toggled += (_, _) => InvalidarCalculo();
        txtMontoInicial.TextChanged += (_, _) => InvalidarCalculo();
    }

    public Registro(string usuario) : this()
    {
        usuarioConectado = usuario;

        lblUsuarioConectado.Text =
            $"Usuario conectado: {usuarioConectado}";
    }

    private void InvalidarCalculo()
    {
        registroCalculado = null;
        txtPagoMensual.Text = string.Empty;
    }

    private async void OnCalcularPagoClicked(
        object sender, EventArgs e)
    {
        await ValidarYCalcularPagoAsync();
    }

    private async Task<bool> ValidarYCalcularPagoAsync()
    {
        InvalidarCalculo();

  
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text))
        {
            await DisplayAlertAsync(
                "Datos incompletos",
                "Ingrese el nombre y el apellido del socio.",
                "Aceptar");

            return false;
        }

     
        if (!int.TryParse(
                txtEdad.Text?.Trim(),
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out int edad) ||
            edad < 16)
        {
            await DisplayAlertAsync(
                "Edad incorrecta",
                "Ingrese una edad entera de 16 años o más.",
                "Aceptar");

            txtEdad.Focus();

            return false;
        }

    
        if (dpFechaInicio.Date is not DateTime fechaInicio ||
            fechaInicio.Date < DateTime.Today)
        {
            await DisplayAlertAsync(
                "Fecha incorrecta",
                "Seleccione hoy o una fecha futura.",
                "Aceptar");

            return false;
        }

      
        if (pkPlan.SelectedIndex == -1)
        {
            await DisplayAlertAsync(
                "Plan pendiente",
                "Seleccione un plan para el socio.",
                "Aceptar");

            pkPlan.Focus();

            return false;
        }


        string textoMonto = (txtMontoInicial.Text ?? string.Empty)
            .Trim()
            .Replace(',', '.');

  
        if (!decimal.TryParse(
                textoMonto,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out decimal montoInicial) ||
            montoInicial < 120m ||
            montoInicial > 600m)
        {
            await DisplayAlertAsync(
                "Monto incorrecto",
                "Ingrese un monto inicial entre 120 y 600.",
                "Aceptar");

            txtMontoInicial.Focus();

            return false;
        }


        decimal saldoPendiente = CostoMembresia - montoInicial;

        decimal cuotaBase = saldoPendiente / NumeroCuotas;

        decimal recargoPorCuota = CostoMembresia * 0.02m;

        decimal casilleroPorCuota =
            swCasillero.IsToggled ? 10m : 0m;

        decimal pagoMensual =
            cuotaBase + recargoPorCuota + casilleroPorCuota;


        registroCalculado = new RegistroSocio
        {
            Usuario = usuarioConectado,
            Nombre = txtNombre.Text?.Trim() ?? string.Empty,
            Apellido = txtApellido.Text?.Trim() ?? string.Empty,
            Edad = edad,
            FechaInicio = fechaInicio.Date,
            Plan = pkPlan.SelectedItem?.ToString() ?? string.Empty,
            Casillero = swCasillero.IsToggled,
            MontoInicial = montoInicial,
            PagoMensual = pagoMensual,
            PagoTotal = montoInicial + (pagoMensual * NumeroCuotas)
        };

        // Mostrar la cuota con dos decimales.
        txtPagoMensual.Text = pagoMensual.ToString(
            "F2",
            CultureInfo.CurrentCulture);

        return true;



    }


    private async void OnVerResumenClicked(
       object? sender, EventArgs e)
    {
        if (!await ValidarYCalcularPagoAsync())
        {
            return;
        }

        if (registroCalculado is null)
        {
            return;
        }

        await Navigation.PushAsync(
            new Resumen(registroCalculado));
    }
}