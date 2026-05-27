using System;
using System.Linq;
using System.Windows;
using ProyectoPED.Services;

namespace ProyectoPED.Views
{
    public partial class SimulationResultWindow : Window
    {
        public SimulationResultWindow(SimulationResult resultado, SimulationService? simulationService)
        {
            InitializeComponent();

            TxtFechaSimulada.Text = $"Fecha simulada: {resultado.FechaSimulacion:dd/MM/yyyy}";
            TxtDiasAvanzados.Text = $"Días avanzados: {resultado.DiasAvanzados}";
            TxtCompletadas.Text = resultado.TareasCompletadas.ToString();
            TxtPendientes.Text = resultado.TareasPendientes.ToString();

            if (resultado.TareasVencidasAhora.Count > 0)
            {
                TxtVencidasTitulo.Text = $"Tareas Vencidas ({resultado.TareasVencidasAhora.Count})";
                ListaVencidas.ItemsSource = resultado.TareasVencidasAhora;
            }
            else
            {
                CardVencidas.Visibility = Visibility.Collapsed;
            }

            if (resultado.TareasProximasAVencerAhora.Count > 0)
            {
                TxtProximasTitulo.Text = $"Próximas a Vencer ({resultado.TareasProximasAVencerAhora.Count})";
                ListaProximas.ItemsSource = resultado.TareasProximasAVencerAhora;
            }
            else
            {
                CardProximas.Visibility = Visibility.Collapsed;
            }

            if (simulationService != null)
            {
                var recomendaciones = simulationService.ObtenerRecomendaciones();
                if (recomendaciones.Count > 0)
                {
                    CardRecomendaciones.Visibility = Visibility.Visible;
                    ListaRecomendaciones.ItemsSource = recomendaciones;
                }
            }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
