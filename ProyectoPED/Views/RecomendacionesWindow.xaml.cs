using System;
using System.Windows;
using System.Windows.Controls;
using ProyectoPED.Services;

namespace ProyectoPED.Views
{
    public partial class RecomendacionesWindow : Window
    {
        private TareaService? tareaService;
        private ReportService? reportService;
        private SimulationService? simulationService;

        public RecomendacionesWindow(TareaService tareaService, ReportService reportService, SimulationService simulationService)
        {
            InitializeComponent();
            this.tareaService = tareaService;
            this.reportService = reportService;
            this.simulationService = simulationService;
            CargarDatos();
        }

        private void CargarDatos()
        {
            CargarRecomendaciones();
            CargarEstadisticas();
            CargarCuellos();
            CargarCargaTrabajo();
        }

        private void CargarRecomendaciones()
        {
            if (simulationService == null)
                return;

            PnlRecomendaciones.Children.Clear();

            var recomendaciones = simulationService.ObtenerRecomendaciones();

            foreach (var rec in recomendaciones)
            {
                var border = new Border
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 224, 242, 254)),
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 14, 165, 233)),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var textBlock = new TextBlock
                {
                    Text = rec,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 14,
                    Foreground = System.Windows.Media.Brushes.Black
                };

                border.Child = textBlock;
                PnlRecomendaciones.Children.Add(border);
            }

            if (recomendaciones.Count == 0)
            {
                var textBlock = new TextBlock
                {
                    Text = "No hay recomendaciones disponibles en este momento.",
                    FontSize = 14,
                    Foreground = System.Windows.Media.Brushes.Gray
                };
                PnlRecomendaciones.Children.Add(textBlock);
            }
        }

        private void CargarEstadisticas()
        {
            if (reportService == null || tareaService == null)
                return;

            PnlEstadisticas.Children.Clear();

            var resumen = reportService.ObtenerResumenGeneral();

            AgregarEtiquetaEstadistica("Total de Tareas", resumen.TotalTareas.ToString(), "#E8F4F8");
            AgregarEtiquetaEstadistica("Tareas Completadas", resumen.TareasCompletadas.ToString(), "#D1FAE5");
            AgregarEtiquetaEstadistica("Tareas Pendientes", resumen.TareasPendientes.ToString(), "#FEF3C7");
            AgregarEtiquetaEstadistica("% Completación", $"{resumen.PorcentajeCompletacion:F1}%", "#FEE2E2");
            AgregarEtiquetaEstadistica("Tareas Vencidas", resumen.TareasVencidas.ToString(), "#FECACA");
            AgregarEtiquetaEstadistica("Próximas a Vencer", resumen.TareasProximasAVencer.ToString(), "#FCA5A5");

            // Estadísticas por Prioridad
            var prioridades = reportService.ObtenerEstadisticasPorPrioridad();
            var converter = new System.Windows.Media.BrushConverter();
            var borderBrush = (System.Windows.Media.Brush)converter.ConvertFromString("#F3F4F6");
            var borderBorder = (System.Windows.Media.Brush)converter.ConvertFromString("#D1D5DB");

            var border = new Border
            {
                Background = borderBrush,
                BorderBrush = borderBorder,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 10, 0, 0)
            };

            var stack = new StackPanel();
            var titulo = new TextBlock { Text = "Por Prioridad", FontSize = 14, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 10) };
            stack.Children.Add(titulo);

            foreach (var p in prioridades)
            {
                var text = new TextBlock
                {
                    Text = $"  • {p.Key}: {p.Value}",
                    FontSize = 12,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                stack.Children.Add(text);
            }

            border.Child = stack;
            PnlEstadisticas.Children.Add(border);
        }

        private void CargarCuellos()
        {
            if (simulationService == null)
                return;

            PnlCuellos.Children.Clear();

            var cuellos = simulationService.IdentificarCuellosdeBottella();

            if (cuellos.Count == 0)
            {
                var textBlock = new TextBlock
                {
                    Text = "No hay cuellos de botella detectados.",
                    FontSize = 14,
                    Foreground = System.Windows.Media.Brushes.Green
                };
                PnlCuellos.Children.Add(textBlock);
                return;
            }

            foreach (var cuello in cuellos.Take(5))
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brushBackground = (System.Windows.Media.Brush)converter.ConvertFromString("#FEF3C7");
                var brushBorder = (System.Windows.Media.Brush)converter.ConvertFromString("#FBBF24");

                var border = new Border
                {
                    Background = brushBackground,
                    BorderBrush = brushBorder,
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var stack = new StackPanel();
                var titulo = new TextBlock
                {
                    Text = $"📌 {cuello.Fecha:dd/MM/yyyy} - {cuello.CantidadTareas} tareas",
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                stack.Children.Add(titulo);

                var intensidad = new TextBlock
                {
                    Text = $"Intensidad: {cuello.IntensidadCarga:F1}%",
                    FontSize = 12,
                    Margin = new Thickness(0, 5, 0, 5)
                };
                stack.Children.Add(intensidad);

                foreach (var prior in cuello.TareasPorPrioridad)
                {
                    var text = new TextBlock
                    {
                        Text = $"  • {prior.Key}: {prior.Value}",
                        FontSize = 11,
                        Margin = new Thickness(0, 3, 0, 0)
                    };
                    stack.Children.Add(text);
                }

                border.Child = stack;
                PnlCuellos.Children.Add(border);
            }
        }

        private void CargarCargaTrabajo()
        {
            if (reportService == null)
                return;

            PnlCargaTrabajo.Children.Clear();

            var analisis = reportService.ObtenerAnálisisCargaTrabajo();

            AgregarEtiquetaEstadistica("Carga Promedio Diaria", $"{analisis.CargaPromedioDiaria:F1} tareas", "#E8F4F8");
            AgregarEtiquetaEstadistica("Máximo Carga Diaria", analisis.MaximoCargaDiaria.ToString(), "#FEE2E2");
            AgregarEtiquetaEstadistica("Día con Mayor Carga", analisis.DiaConMayorCarga.ToString("dd/MM/yyyy"), "#FEF3C7");

            // Top 5 días con más carga
            if (analisis.DiasConCargaDiaria.Count > 0)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var borderBrush = (System.Windows.Media.Brush)converter.ConvertFromString("#F3F4F6");
                var borderBorder = (System.Windows.Media.Brush)converter.ConvertFromString("#D1D5DB");

                var border = new Border
                {
                    Background = borderBrush,
                    BorderBrush = borderBorder,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 10, 0, 0)
                };

                var stack = new StackPanel();
                var titulo = new TextBlock { Text = "Top 5 Días con Mayor Carga", FontSize = 14, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 10) };
                stack.Children.Add(titulo);

                foreach (var dia in analisis.DiasConCargaDiaria.Take(5))
                {
                    var text = new TextBlock
                    {
                        Text = $"  • {dia.Key:dd/MM/yyyy}: {dia.Value} tareas",
                        FontSize = 12,
                        Margin = new Thickness(0, 5, 0, 0)
                    };
                    stack.Children.Add(text);
                }

                border.Child = stack;
                PnlCargaTrabajo.Children.Add(border);
            }
        }

        private void AgregarEtiquetaEstadistica(string etiqueta, string valor, string colorFondoHex)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (System.Windows.Media.Brush)converter.ConvertFromString(colorFondoHex);
            
            var border = new Border
            {
                Background = brush,
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 229, 231, 235)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var stack = new StackPanel();
            var label = new TextBlock { Text = etiqueta, FontSize = 12, Foreground = System.Windows.Media.Brushes.Gray };
            var value = new TextBlock { Text = valor, FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 0) };

            stack.Children.Add(label);
            stack.Children.Add(value);
            border.Child = stack;
            PnlEstadisticas.Children.Add(border);
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
