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
        private int usuarioId = 1;
        private bool modoOffline = false;

        public Dashboard()
        {
            InitializeComponent();
            TxtFechaActual.Text = $"{DateTime.Now:dd} de {DateTime.Now:MMMM} de {DateTime.Now:yyyy}";
            CargarTareas();
        }

        private void CargarTareas()
        {
            var tareasBD = TareaRepository.GetTareasPorUsuario(usuarioId);
            if (tareasBD != null && tareasBD.Count > 0)
            {
                listaTareas = tareasBD;
            }
            else
            {
                listaTareas = CargarDatosHardcodeados();
                modoOffline = true;
            }
            DgTareas.ItemsSource = listaTareas;
        }

        private ObservableCollection<Tarea> CargarDatosHardcodeados()
        {
            var tareas = new ObservableCollection<Tarea>();
            var fechaBase = DateTime.Now;

            tareas.Add(new Tarea { Id = 1, UsuarioId = 1, Titulo = "Proyecto Final de Estructura de Datos", Descripcion = "Implementar un árbol AVL con todas las operaciones", FechaLimite = fechaBase.AddDays(3), Prioridad = "Alta", Estado = "Pendiente", CreatedAt = fechaBase });
            tareas.Add(new Tarea { Id = 2, UsuarioId = 1, Titulo = "Ensayo de Literatura", Descripcion = "Analisis de la obra 'Cien años de soledad'", FechaLimite = fechaBase.AddDays(16), Prioridad = "Media", Estado = "Pendiente", CreatedAt = fechaBase });
            tareas.Add(new Tarea { Id = 3, UsuarioId = 1, Titulo = "Laboratorio de Programación", Descripcion = "Completar ejercicios de herencia en Java", FechaLimite = fechaBase.AddDays(-2), Prioridad = "Baja", Estado = "Completada", CreatedAt = fechaBase.AddDays(-10) });
            tareas.Add(new Tarea { Id = 4, UsuarioId = 1, Titulo = "Informe de Física", Descripcion = "Reporte sobre termodinámica", FechaLimite = fechaBase.AddDays(5), Prioridad = "Media", Estado = "Pendiente", CreatedAt = fechaBase });
            tareas.Add(new Tarea { Id = 5, UsuarioId = 1, Titulo = "Examen de Cálculo II", Descripcion = "Estudiar integrales múltiples", FechaLimite = fechaBase.AddDays(7), Prioridad = "Alta", Estado = "Pendiente", CreatedAt = fechaBase });

            return tareas;
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
                var loginWindow = new LoginWindow();
                var mainWindow = Window.GetWindow(this);
                
                if (mainWindow != null)
                {
                    var app = Application.Current;
                    mainWindow.Hide();
                    loginWindow.Show();
                    loginWindow.Closed += (s, args) => app.Shutdown();
                }
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
            if (SidebarColumn.Width.Value > 0)
            {
                SidebarColumn.Width = new GridLength(0);
                SidebarPanel.Visibility = Visibility.Collapsed;
            }
            AVLPanel.Visibility = Visibility.Visible;
            AVLColumn.Width = new GridLength(400);
        }

        private void CerrarPanelAVL()
        {
            AVLColumn.Width = new GridLength(0);
            AVLPanel.Visibility = Visibility.Collapsed;
        }

        private void ActualizarArbolAVL()
        {
            MessageBox.Show("Actualizando árbol AVL...", "Actualizar AVL");
        }

        private void AVLTreeControl_CerrarRequested(object sender, RoutedEventArgs e)
        {
            CerrarPanelAVL();
        }

        private void AVLTreeControl_ActualizarRequested(object sender, RoutedEventArgs e)
        {
            ActualizarArbolAVL();
        }

        private void BtnToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            if (SidebarColumn.Width.Value > 0)
            {
                SidebarColumn.Width = new GridLength(0);
                SidebarPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                SidebarColumn.Width = new GridLength(240);
                SidebarPanel.Visibility = Visibility.Visible;
            }
        }
    }
}