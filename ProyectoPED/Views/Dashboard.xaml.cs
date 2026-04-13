using System.Windows;
using System.Windows.Controls;

namespace ProyectoPED.Views
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            TxtFechaActual.Text = $"Lunes {DateTime.Now:dd} de {DateTime.Now:MMMM} de {DateTime.Now:yyyy}";
        }

        private void BtnNuevaTarea_Click(object sender, RoutedEventArgs e)
        {
            var modal = new NuevaTareaWindow();
            modal.Owner = Window.GetWindow(this);
            modal.ShowDialog();
        }

        private void BtnSimular_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simulando 1 día...", "Simulación");
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            var modal = new ReportesWindow();
            modal.Owner = Window.GetWindow(this);
            modal.ShowDialog();
        }

        private void BtnDeshacer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deshacer última acción", "Deshacer");
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Cerrar Sesión", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Cerrando sesión...", "Cerrar Sesión");
            }
        }

        private void BtnCompletar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                var result = MessageBox.Show($"¿Completar la tarea ID {btn.Tag}?", "Confirmar", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show($"Tarea {btn.Tag} completada", "Éxito");
                }
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                var modal = new EditarTareaWindow();
                modal.Owner = Window.GetWindow(this);
                modal.TareaId = (int)btn.Tag;
                modal.CargarTarea((int)btn.Tag, "Tarea de ejemplo", "Descripción de ejemplo", "Media", "Pendiente");
                modal.ShowDialog();
            }
        }

        private void BtnVerAVL_Click(object sender, RoutedEventArgs e)
        {
            AVLPanel.Visibility = Visibility.Visible;
            AVLColumn.Width = new GridLength(350);
        }

        private void BtnCerrarAVL_Click(object sender, RoutedEventArgs e)
        {
            AVLColumn.Width = new GridLength(0);
            AVLPanel.Visibility = Visibility.Collapsed;
        }

        private void BtnActualizarAVL_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Actualizando árbol AVL...", "Actualizar AVL");
        }
    }
}