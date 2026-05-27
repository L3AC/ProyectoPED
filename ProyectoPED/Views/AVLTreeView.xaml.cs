using ProyectoPED.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProyectoPED.Views
{
    public partial class AVLTreeView : UserControl
    {
        public static readonly RoutedEvent CerrarRequestedEvent = EventManager.RegisterRoutedEvent(
            "CerrarRequested", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AVLTreeView));

        public static readonly RoutedEvent ActualizarRequestedEvent = EventManager.RegisterRoutedEvent(
            "ActualizarRequested", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AVLTreeView));

        public event RoutedEventHandler CerrarRequested
        {
            add { AddHandler(CerrarRequestedEvent, value); }
            remove { RemoveHandler(CerrarRequestedEvent, value); }
        }

        public event RoutedEventHandler ActualizarRequested
        {
            add { AddHandler(ActualizarRequestedEvent, value); }
            remove { RemoveHandler(CerrarRequestedEvent, value); }
        }

        private List<NodoInfoArbol>? datosArbol;

        public AVLTreeView()
        {
            InitializeComponent();
        }

        public void CargarDatos(List<NodoInfoArbol> nodos, int totalNodos, int altura, int factorBalance)
        {
            datosArbol = nodos;
            TxtInfoNodos.Text = $"Nodos: {totalNodos}";
            TxtInfoAltura.Text = $"Altura: {altura}";
            TxtInfoBalance.Text = $"Balance: {(altura <= Math.Ceiling(Math.Log2(totalNodos + 1)) * 2 ? "OK" : "Pendiente")}";
            DibujarArbol();
        }

        private void DibujarArbol()
        {
            AVLCanvas.Children.Clear();

            if (datosArbol == null || datosArbol.Count == 0)
            {
                MostrarMensajeVacio();
                return;
            }

            var nodosPorId = datosArbol.ToDictionary(n => n.TareaId);
            var raiz = datosArbol.FirstOrDefault(n => n.PadreId == null);

            if (raiz == null)
            {
                MostrarMensajeVacio();
                return;
            }

            // Calcular profundidad del árbol
            int profundidad = CalcularProfundidad(raiz, nodosPorId);
            int nodeWidth = 120;
            int nodeHeight = 65;
            int spacingX = (int)Math.Pow(2, profundidad) * 30;
            if (spacingX < 120) spacingX = 120;

            int canvasWidth = Math.Max(spacingX * 3, 600);
            int canvasHeight = (profundidad + 1) * 120 + 50;

            AVLCanvas.Width = canvasWidth;
            AVLCanvas.Height = canvasHeight;

            int centerX = canvasWidth / 2;
            int startY = 30;

            // Dibujar recursivamente
            DibujarNodoRecursivo(raiz, nodosPorId, centerX, startY, spacingX, 0);
        }

        private void DibujarNodoRecursivo(NodoInfoArbol nodo, Dictionary<int, NodoInfoArbol> nodosPorId,
                                           int x, int y, int spacingX, int nivel)
        {
            int nodeWidth = 120;
            int nodeHeight = 65;
            int verticalSpacing = 100;

            // Dibujar líneas a los hijos
            var hijosIzq = datosArbol!.Where(n => n.PadreId == nodo.TareaId && n.EsIzquierdo).ToList();
            var hijosDer = datosArbol!.Where(n => n.PadreId == nodo.TareaId && !n.EsIzquierdo).ToList();

            int childSpacing = Math.Max(spacingX / 2, 60);

            if (hijosIzq.Count > 0)
            {
                int childX = x - childSpacing;
                int childY = y + verticalSpacing;
                DibujarLinea(x, y + nodeHeight / 2, childX, childY);
                DibujarNodoRecursivo(hijosIzq.First(), nodosPorId, childX, childY, childSpacing, nivel + 1);
            }

            if (hijosDer.Count > 0)
            {
                int childX = x + childSpacing;
                int childY = y + verticalSpacing;
                DibujarLinea(x + nodeWidth / 2, y + nodeHeight / 2, childX + nodeWidth / 2, childY);
                DibujarNodoRecursivo(hijosDer.First(), nodosPorId, childX, childY, childSpacing, nivel + 1);
            }

            // Dibujar el nodo actual
            DibujarNodo(x, y, nodeWidth, nodeHeight, nodo);
        }

        private void DibujarNodo(int x, int y, int width, int height, NodoInfoArbol nodo)
        {
            // Determinar color según urgencia
            string bgColor, borderColor, textColor;
            if (nodo.DiasRestantes < 0)
            {
                bgColor = "#FEE2E2";
                borderColor = "#EF4444";
                textColor = "#991B1B";
            }
            else if (nodo.DiasRestantes <= 3)
            {
                bgColor = "#FEE2E2";
                borderColor = "#F87171";
                textColor = "#991B1B";
            }
            else if (nodo.DiasRestantes <= 7)
            {
                bgColor = "#FFEDD5";
                borderColor = "#FB923C";
                textColor = "#9A3412";
            }
            else
            {
                bgColor = "#DCFCE7";
                borderColor = "#4ADE80";
                textColor = "#166534";
            }

            var converter = new System.Windows.Media.BrushConverter();
            var border = new Border
            {
                Width = width,
                MinHeight = height,
                Background = (Brush)converter.ConvertFromString(bgColor)!,
                BorderBrush = (Brush)converter.ConvertFromString(borderColor)!,
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(12),
                Tag = nodo.TareaId
            };

            var stack = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };

            var titulo = new TextBlock
            {
                Text = nodo.Titulo,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = (Brush)converter.ConvertFromString(textColor)!,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = width - 10
            };

            string estadoTexto = nodo.DiasRestantes < 0
                ? $"Vencido ({Math.Abs(nodo.DiasRestantes)} días)"
                : $"{nodo.DiasRestantes} días | fb: {nodo.FactorBalance}";

            var info = new TextBlock
            {
                Text = estadoTexto,
                FontSize = 9,
                Foreground = (Brush)converter.ConvertFromString(textColor)!,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 3, 0, 0)
            };

            stack.Children.Add(titulo);
            stack.Children.Add(info);

            if (nodo.Prioridad == "Alta")
            {
                var prioridadTag = new Border
                {
                    Background = new SolidColorBrush(Color.FromArgb(80, 239, 68, 68)),
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(6, 2, 6, 2),
                    Margin = new Thickness(0, 4, 0, 0)
                };
                var prioridadText = new TextBlock
                {
                    Text = "ALTA",
                    FontSize = 8,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    TextAlignment = TextAlignment.Center
                };
                prioridadTag.Child = prioridadText;
                stack.Children.Add(prioridadTag);
            }

            border.Child = stack;
            Canvas.SetLeft(border, x);
            Canvas.SetTop(border, y);
            AVLCanvas.Children.Add(border);
        }

        private void DibujarLinea(int x1, int y1, int x2, int y2)
        {
            var line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                StrokeThickness = 2,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round
            };
            AVLCanvas.Children.Add(line);
        }

        private int CalcularProfundidad(NodoInfoArbol nodo, Dictionary<int, NodoInfoArbol> nodosPorId)
        {
            var hijos = datosArbol!.Where(n => n.PadreId == nodo.TareaId).ToList();
            if (hijos.Count == 0)
                return 1;
            return 1 + hijos.Max(h => CalcularProfundidad(h, nodosPorId));
        }

        private void MostrarMensajeVacio()
        {
            var textBlock = new TextBlock
            {
                Text = "No hay tareas para mostrar.\nCrea una tarea para ver el árbol AVL.",
                FontSize = 16,
                Foreground = new SolidColorBrush(Color.FromRgb(156, 163, 175)),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Canvas.SetLeft(textBlock, 100);
            Canvas.SetTop(textBlock, 150);
            AVLCanvas.Children.Add(textBlock);
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ActualizarRequestedEvent, this));
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CerrarRequestedEvent, this));
        }
    }
}
