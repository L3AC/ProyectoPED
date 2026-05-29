using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProyectoPED.Repositories;
using ProyectoPED.Models;
using ProyectoPED.Services;
using System.Collections.ObjectModel;

namespace ProyectoPED.Views
{
    public partial class Dashboard : UserControl
    {
        private ObservableCollection<Tarea> listaTareas = null!;
        private List<Tarea>? listaTareasCompleta;
        private int usuarioId = 1;
        private bool modoOffline = false;
        private TareaService? tareaService;
        private ReportService? reportService;
        private SimulationService? simulationService;

        public Dashboard()
        {
            InitializeComponent();
        }

        public void CargarUsuario(int id, string nombre, string carne)
        {
            this.usuarioId = id;
            TxtNombreUsuario.Text = $"Bienvenido/a {nombre}";
            TxtCarnet.Text = $"Carné: {carne}";
            ActualizarEtiquetaFecha();
            InicializarServicios();
            CargarTareas();
        }

        private void InicializarServicios()
        {
            tareaService = new TareaService(usuarioId);
            reportService = new ReportService(tareaService);
            simulationService = new SimulationService(tareaService);
        }

        private void CargarTareas()
        {
            if (tareaService != null)
            {
                var tareasOrdenadas = tareaService.ObtenerTareasEnColeccion();
                listaTareas = tareasOrdenadas;
            }
            else
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
            }

            listaTareasCompleta = listaTareas.ToList();
            AplicarFiltroBusqueda();
            ActualizarEtiquetaFecha();
        }

        private void AplicarFiltroBusqueda()
        {
            if (listaTareasCompleta == null) return;

            var filtro = TxtBuscar?.Text?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(filtro))
            {
                listaTareas = new ObservableCollection<Tarea>(listaTareasCompleta);
                BtnLimpiarBusqueda.Visibility = Visibility.Collapsed;
            }
            else
            {
                var filtradas = listaTareasCompleta
                    .Where(t =>
                        (t.Titulo?.ToLower().Contains(filtro) ?? false) ||
                        (t.Descripcion?.ToLower().Contains(filtro) ?? false) ||
                        (t.Prioridad?.ToLower().Contains(filtro) ?? false) ||
                        t.Estado.ToString().ToLower().Contains(filtro)
                    )
                    .ToList();
                listaTareas = new ObservableCollection<Tarea>(filtradas);
                BtnLimpiarBusqueda.Visibility = Visibility.Visible;
            }

            DgTareas.ItemsSource = listaTareas;
        }

        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltroBusqueda();
        }

        private void BtnLimpiarBusqueda_Click(object sender, RoutedEventArgs e)
        {
            TxtBuscar.Text = "";
            TxtBuscar.Focus();
        }

        private void ActualizarEtiquetaFecha()
        {
            if (simulationService != null)
            {
                var fecha = simulationService.ObtenerFechaActual();
                TxtFechaActual.Text = $"{fecha:dd} de {fecha:MMMM} de {fecha:yyyy}";
            }
        }

        private ObservableCollection<Tarea> CargarDatosHardcodeados()
        {
            var tareas = new ObservableCollection<Tarea>();
            var fechaBase = DateTime.Now;

            tareas.Add(new Tarea { Id = 1, UsuarioId = 1, Titulo = "Proyecto Final de Estructura de Datos", Descripcion = "Implementar un árbol AVL con todas las operaciones", FechaLimite = fechaBase.AddDays(3), Prioridad = "Alta", Estado = EstadoTarea.Pendiente, CreatedAt = fechaBase });
            tareas.Add(new Tarea { Id = 2, UsuarioId = 1, Titulo = "Ensayo de Literatura", Descripcion = "Analisis de la obra 'Cien años de soledad'", FechaLimite = fechaBase.AddDays(16), Prioridad = "Media", Estado = EstadoTarea.Pendiente, CreatedAt = fechaBase });
            tareas.Add(new Tarea { Id = 3, UsuarioId = 1, Titulo = "Laboratorio de Programación", Descripcion = "Completar ejercicios de herencia en Java", FechaLimite = fechaBase.AddDays(-2), Prioridad = "Baja", Estado = EstadoTarea.Completada, CreatedAt = fechaBase.AddDays(-10) });
            tareas.Add(new Tarea { Id = 4, UsuarioId = 1, Titulo = "Informe de Física", Descripcion = "Reporte sobre termodinámica", FechaLimite = fechaBase.AddDays(5), Prioridad = "Media", Estado = EstadoTarea.Pendiente, CreatedAt = fechaBase });
            tareas.Add(new Tarea { Id = 5, UsuarioId = 1, Titulo = "Examen de Cálculo II", Descripcion = "Estudiar integrales múltiples", FechaLimite = fechaBase.AddDays(7), Prioridad = "Alta", Estado = EstadoTarea.Pendiente, CreatedAt = fechaBase });

            return tareas;
        }

        private void BtnNuevaTarea_Click(object sender, RoutedEventArgs e)
        {
            if (tareaService == null)
            {
                MessageBox.Show("El servicio de tareas no está disponible", "Error");
                return;
            }

            var modal = new NuevaTareaWindow();
            modal.Owner = Window.GetWindow(this);
            if (modal.ShowDialog() == true)
            {
                if (tareaService.CrearTarea(
                    titulo: modal.TituloTarea,
                    descripcion: modal.DescripcionTarea,
                    fechaLimite: modal.FechaLimiteTarea,
                    prioridad: modal.PrioridadTarea
                ))
                {
                    CargarTareas();
                }
                else
                {
                    MessageBox.Show("Error al crear la tarea", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnSimular_Click(object sender, RoutedEventArgs e)
        {
            if (simulationService == null)
            {
                MessageBox.Show("El servicio de simulación no está disponible", "Error");
                return;
            }

            try
            {
                var modal = new SimularDiasWindow(simulationService);
                modal.Owner = Window.GetWindow(this);
                if (modal.ShowDialog() == true)
                {
                    CargarTareas();
                    ActualizarEtiquetaFecha();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la simulación: {ex.Message}", "Error");
            }
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            if (reportService == null || simulationService == null || tareaService == null)
            {
                MessageBox.Show("Los servicios no están disponibles", "Error");
                return;
            }

            try
            {
                var modal = new RecomendacionesWindow(tareaService, reportService, simulationService);
                modal.Owner = Window.GetWindow(this);
                modal.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir reportes: {ex.Message}", "Error");
            }
        }

        private void BtnDeshacer_Click(object sender, RoutedEventArgs e)
        {
            if (tareaService == null)
            {
                MessageBox.Show("El servicio de tareas no está disponible", "Error");
                return;
            }

            if (!tareaService.TieneHistorialParaDeshacer())
            {
                MessageBox.Show("No hay acciones para deshacer", "Información");
                return;
            }

            var result = MessageBox.Show("¿Deshacer la última acción?", "Confirmar", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                if (tareaService.Deshacer())
                {
                    MessageBox.Show("Acción deshecha", "Éxito");
                    CargarTareas();
                }
                else
                {
                    MessageBox.Show("No se pudo deshacer la acción", "Error");
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
                    mainWindow.Hide();
                    loginWindow.Show();
                    loginWindow.Closed += (s, args) =>
                    {
                        if (!loginWindow.LoginExitoso)
                            Application.Current.Shutdown();
                    };
                }
            }
        }

        private void BtnCompletar_Click(object sender, RoutedEventArgs e)
        {
            if (tareaService == null)
            {
                MessageBox.Show("El servicio de tareas no está disponible", "Error");
                return;
            }

            if (sender is Button btn && btn.Tag is int tareaId)
            {
                var result = MessageBox.Show($"¿Completar la tarea ID {tareaId}?", "Confirmar", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    if (tareaService.CompletarTarea(tareaId))
                    {
                        MessageBox.Show("Tarea completada", "Éxito");
                        CargarTareas();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo completar la tarea", "Error");
                    }
                }
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (tareaService == null)
            {
                MessageBox.Show("El servicio de tareas no está disponible", "Error");
                return;
            }

            if (sender is Button btn && btn.Tag is int tareaId)
            {
                var tarea = listaTareas.FirstOrDefault(t => t.Id == tareaId);
                if (tarea != null)
                {
                    var modal = new EditarTareaWindow();
                    modal.Owner = Window.GetWindow(this);
                    modal.CargarTarea(tarea.Id, tarea.Titulo, tarea.Descripcion ?? "", tarea.FechaLimite, tarea.Prioridad, tarea.Estado.ToString());
                    if (modal.ShowDialog() == true)
                    {
                        var estadoParseado = Enum.TryParse<EstadoTarea>(modal.EstadoEditado, true, out var estado)
                            ? estado : EstadoTarea.Pendiente;

                        var tareaActualizada = new Tarea
                        {
                            Id = tarea.Id,
                            UsuarioId = tarea.UsuarioId,
                            Titulo = modal.TituloEditado,
                            Descripcion = modal.DescripcionEditada,
                            FechaLimite = modal.FechaLimiteEditada,
                            Prioridad = modal.PrioridadEditada,
                            Estado = estadoParseado,
                            CreatedAt = tarea.CreatedAt
                        };

                        if (tareaService.ActualizarTarea(tareaActualizada))
                        {
                            CargarTareas();
                        }
                        else
                        {
                            MessageBox.Show("Error al actualizar la tarea", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (tareaService == null)
            {
                MessageBox.Show("El servicio de tareas no está disponible", "Error");
                return;
            }

            if (sender is Button btn && btn.Tag is int tareaId)
            {
                var tarea = listaTareas.FirstOrDefault(t => t.Id == tareaId);
                var titulo = tarea?.Titulo ?? "ID " + tareaId;
                var result = MessageBox.Show($"¿Eliminar la tarea \"{titulo}\"?\nEsta acción no se puede deshacer.", 
                    "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (tareaService.EliminarTarea(tareaId))
                    {
                        CargarTareas();
                    }
                    else
                    {
                        var tareaError = listaTareas.FirstOrDefault(t => t.Id == tareaId);
                        var tituloError = tareaError?.Titulo ?? "(desconocida)";
                        MessageBox.Show($"No se pudo eliminar: ID {tareaId} - \"{tituloError}\"\n" +
                            "Posible causa: el ID no existe en la base de datos.\n" +
                            "Reinicia la aplicación para recargar los IDs correctos.",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnVerAVL_Click(object sender, RoutedEventArgs e)
        {
            if (tareaService == null)
            {
                MessageBox.Show("El servicio de tareas no está disponible", "Error");
                return;
            }

            try
            {
                var infoArbol = tareaService.ObtenerInfoArbolVisual();
                var estadisticas = tareaService.ObtenerEstadisticas();

                int totalNodos = (int)estadisticas["CantidadNodosArbol"];
                int altura = (int)estadisticas["AlturaarbolAVL"];

                var ventana = new AVLTreeWindow();
                ventana.Owner = Window.GetWindow(this);
                ventana.CargarDatos(infoArbol, totalNodos, altura, 0);
                ventana.SetObtenerDatos(() =>
                {
                    if (tareaService == null)
                        return (new System.Collections.Generic.List<DataStructures.NodoInfoArbol>(), 0, 0);

                    var nuevosNodos = tareaService.ObtenerInfoArbolVisual();
                    var nuevasStats = tareaService.ObtenerEstadisticas();
                    CargarTareas();
                    return (nuevosNodos, (int)nuevasStats["CantidadNodosArbol"], (int)nuevasStats["AlturaarbolAVL"]);
                });
                ventana.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el árbol: {ex.Message}", "Error");
            }
        }

        private void BtnCambiarPassword_Click(object sender, RoutedEventArgs e)
        {
            var modal = new CambiarPasswordWindow(usuarioId);
            modal.Owner = Window.GetWindow(this);
            modal.ShowDialog();
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