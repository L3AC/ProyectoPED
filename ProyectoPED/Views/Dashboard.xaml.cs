using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProyectoPED.Repositories;
using ProyectoPED.Models;
using System.Collections.ObjectModel;

namespace ProyectoPED.Views
{
    public partial class Dashboard : UserControl
    {
        private ObservableCollection<Tarea> listaTareas = null!;
        private int usuarioId = 1; //SE COLOCA 1 COMO USUARIO DEFAULT 

        public Dashboard()
        {
            InitializeComponent();
            TxtFechaActual.Text = $"{DateTime.Now:dd} de {DateTime.Now:MMMM} de {DateTime.Now:yyyy}";
            CargarTareas();
        }

        private void CargarTareas()
        {
            listaTareas = TareaRepository.GetTareasPorUsuario(usuarioId);
            DgTareas.ItemsSource = listaTareas;
        }

        private void BtnNuevaTarea_Click(object sender, RoutedEventArgs e)
        {
            var modal = new NuevaTareaWindow();
            modal.Owner = Window.GetWindow(this);
            if (modal.ShowDialog() == true)
            {
                CargarTareas();
            }
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
            if (sender is Button btn && btn.Tag is int tareaId)
            {
                var result = MessageBox.Show($"¿Deshacer la tarea ID {tareaId}? Volverá a estado Pendiente.", "Confirmar", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (TareaRepository.DeshacerTarea(tareaId))
                    {
                        MessageBox.Show("Tarea marcada como Pendiente", "Éxito");
                        CargarTareas();
                    }
                }
            }
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
            if (sender is Button btn && btn.Tag is int tareaId)
            {
                var result = MessageBox.Show($"¿Completar la tarea ID {tareaId}?", "Confirmar", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (TareaRepository.CompletarTarea(tareaId))
                    {
                        MessageBox.Show("Tarea completada", "Éxito");
                        CargarTareas();
                    }
                }
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int tareaId)
            {
                var tarea = listaTareas.FirstOrDefault(t => t.Id == tareaId);
                if (tarea != null)
                {
                    var modal = new EditarTareaWindow();
                    modal.Owner = Window.GetWindow(this);
                    modal.CargarTarea(tarea.Id, tarea.Titulo, tarea.Descripcion ?? "", tarea.Prioridad, tarea.Estado);
                    if (modal.ShowDialog() == true)
                    {
                        CargarTareas();
                    }
                }
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