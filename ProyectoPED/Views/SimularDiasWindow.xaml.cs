using System;
using System.Windows;
using ProyectoPED.Services;

namespace ProyectoPED.Views
{
    public partial class SimularDiasWindow : Window
    {
        private SimulationService? simulationService;

        public SimularDiasWindow(SimulationService simulationService)
        {
            InitializeComponent();
            this.simulationService = simulationService;
            DtpFechaSimulacion.SelectedDate = DateTime.Today.AddDays(7);
            TxtFechaActualInfo.Text = $"Fecha actual del sistema: {simulationService.ObtenerFechaActual():dd/MM/yyyy}";
        }

        private void BtnSimularDias_Click(object sender, RoutedEventArgs e)
        {
            if (simulationService == null)
            {
                MessageBox.Show("El servicio de simulación no está disponible", "Error");
                return;
            }

            try
            {
                if (!int.TryParse(TxtCantidadDias.Text, out int cantidad) || cantidad < 1)
                {
                    MessageBox.Show("Ingresa una cantidad válida de días (mayor a 0)", "Error de Validación");
                    return;
                }

                var resultado = simulationService.SimularDias(cantidad);
                MostrarResultado(resultado);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la simulación: {ex.Message}", "Error");
            }
        }

        private void BtnSimularFecha_Click(object sender, RoutedEventArgs e)
        {
            if (simulationService == null)
            {
                MessageBox.Show("El servicio de simulación no está disponible", "Error");
                return;
            }

            try
            {
                if (DtpFechaSimulacion.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona una fecha", "Error de Validación");
                    return;
                }

                var fechaSeleccionada = DtpFechaSimulacion.SelectedDate.Value;
                
                if (fechaSeleccionada <= DateTime.Today)
                {
                    MessageBox.Show("La fecha debe ser posterior a hoy", "Error de Validación");
                    return;
                }

                var resultado = simulationService.SimularHastaFecha(fechaSeleccionada);
                MostrarResultado(resultado);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la simulación: {ex.Message}", "Error");
            }
        }

        private void MostrarResultado(SimulationResult resultado)
        {
            var resultWindow = new SimulationResultWindow(resultado, simulationService);
            resultWindow.Owner = this;
            resultWindow.ShowDialog();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
