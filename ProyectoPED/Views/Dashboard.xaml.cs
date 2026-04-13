using System.Windows;
using System.Windows.Controls;

namespace ProyectoPED.Views
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void BtnNuevaTarea_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Abrir ventana Nueva Tarea", "Navegación");
        }

        private void BtnSimular_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simulando 1 día...", "Simulación");
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Abrir ventana de Reportes", "Reportes");
        }

        private void BtnDeshacer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deshacer última acción", "Deshacer");
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cerrar sesión y volver al Login", "Cerrar Sesión");
        }

        private void BtnCompletar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                MessageBox.Show($"Completar tarea ID: {btn.Tag}", "Completar");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                MessageBox.Show($"Editar tarea ID: {btn.Tag}", "Editar");
            }
        }
    }
}
