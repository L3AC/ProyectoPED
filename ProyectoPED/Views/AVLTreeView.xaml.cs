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
        private const int NODE_MIN_WIDTH = 160;
        private const int NODE_HEIGHT = 75;
        private const int VERTICAL_SPACING = 110;

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

            int profundidad = CalcularProfundidad(raiz, nodosPorId);
            int spacingX = (int)(Math.Pow(2, profundidad) * 50);
            if (spacingX < 180) spacingX = 180;

            int canvasWidth = Math.Max(spacingX * 2 + NODE_MIN_WIDTH, (int)this.ActualWidth);
            int canvasHeight = (profundidad + 1) * VERTICAL_SPACING + 50;

            AVLCanvas.Width = canvasWidth;
            AVLCanvas.Height = canvasHeight;

            int centerX = canvasWidth / 2;
            int startY = 20;

            DibujarNodoRecursivo(raiz, nodosPorId, centerX, startY, spacingX, 0);
        }

        private void DibujarNodoRecursivo(NodoInfoArbol nodo, Dictionary<int, NodoInfoArbol> nodosPorId,
                                           int x, int y, int spacingX, int nivel)
        {
            var hijosIzq = datosArbol!.Where(n => n.PadreId == nodo.TareaId && n.EsIzquierdo).ToList();
            var hijosDer = datosArbol!.Where(n => n.PadreId == nodo.TareaId && !n.EsIzquierdo).ToList();

            int childSpacing = Math.Max(spacingX / 2, 80);

            int nodeWidth = CalcularAnchoNodo(nodo);

            if (hijosIzq.Count > 0)
            {
                int childX = x - childSpacing;
                int childY = y + VERTICAL_SPACING;
                int childW = CalcularAnchoNodo(hijosIzq.First());
                DibujarLinea(x + nodeWidth / 2, y + NODE_HEIGHT, childX + childW / 2, childY);
                DibujarNodoRecursivo(hijosIzq.First(), nodosPorId, childX, childY, childSpacing, nivel + 1);
            }

            if (hijosDer.Count > 0)
            {
                int childX = x + childSpacing;
                int childY = y + VERTICAL_SPACING;
                int childW = CalcularAnchoNodo(hijosDer.First());
                DibujarLinea(x + nodeWidth / 2, y + NODE_HEIGHT, childX + childW / 2, childY);
                DibujarNodoRecursivo(hijosDer.First(), nodosPorId, childX, childY, childSpacing, nivel + 1);
            }

            DibujarNodo(x, y, nodeWidth, nodo);
        }

        private int CalcularAnchoNodo(NodoInfoArbol nodo)
        {
            int baseWidth = 140;
            int extraPorCaracter = nodo.Titulo.Length > 15 ? (nodo.Titulo.Length - 15) * 6 : 0;
            return Math.Max(NODE_MIN_WIDTH, baseWidth + extraPorCaracter);
        }

        private void DibujarNodo(int x, int y, int width, NodoInfoArbol nodo)
        {
            var converter = new BrushConverter();

            string bgColor, borderColor, badgeColor, badgeText;
            if (nodo.DiasRestantes < 0)
            {
                bgColor = "#FEF2F2";
                borderColor = "#DC2626";
                badgeColor = "#DC2626";
                badgeText = "Vencida";
            }
            else if (nodo.DiasRestantes <= 3)
            {
                bgColor = "#FFF5F5";
                borderColor = "#F87171";
                badgeColor = "#EF4444";
                badgeText = $"Urgente";
            }
            else if (nodo.DiasRestantes <= 7)
            {
                bgColor = "#FFF7ED";
                borderColor = "#FB923C";
                badgeColor = "#F97316";
                badgeText = $"Próximo";
            }
            else
            {
                bgColor = "#F0FDF4";
                borderColor = "#4ADE80";
                badgeColor = "#22C55E";
                badgeText = $"Normal";
            }

            var border = new Border
            {
                Width = width,
                MinHeight = NODE_HEIGHT,
                Background = (Brush)converter.ConvertFromString(bgColor)!,
                BorderBrush = (Brush)converter.ConvertFromString(borderColor)!,
                BorderThickness = new Thickness(2.5),
                CornerRadius = new CornerRadius(14),
                Tag = nodo.TareaId,
                ToolTip = $"{nodo.Titulo}\nDías restantes: {nodo.DiasRestantes}\nPrioridad: {nodo.Prioridad}\nAltura: {nodo.Altura} | FB: {nodo.FactorBalance}"
            };

            var shadow = new System.Windows.Media.Effects.DropShadowEffect
            {
                ShadowDepth = 2,
                BlurRadius = 8,
                Opacity = 0.12,
                Color = Colors.Black
            };
            border.Effect = shadow;

            var stack = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 8, 10, 8)
            };

            var titulo = new TextBlock
            {
                Text = nodo.Titulo,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Foreground = (Brush)converter.ConvertFromString("#1F2937")!,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = width - 20
            };
            stack.Children.Add(titulo);

            var infoRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 6, 0, 0)
            };

            var diasBadge = new Border
            {
                Background = (Brush)converter.ConvertFromString(badgeColor)!,
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                Margin = new Thickness(0, 0, 4, 0)
            };
            diasBadge.Child = new TextBlock
            {
                Text = nodo.DiasRestantes < 0
                    ? $"{Math.Abs(nodo.DiasRestantes)} días vencido"
                    : $"{nodo.DiasRestantes} días",
                FontSize = 9,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                TextAlignment = TextAlignment.Center
            };
            infoRow.Children.Add(diasBadge);

            string prioridadBg = nodo.Prioridad switch
            {
                "Alta" => "#DC2626",
                "Media" => "#F97316",
                "Baja" => "#22C55E",
                _ => "#6B7280"
            };

            var prioridadBadge = new Border
            {
                Background = (Brush)converter.ConvertFromString(prioridadBg)!,
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                Margin = new Thickness(4, 0, 0, 0)
            };
            prioridadBadge.Child = new TextBlock
            {
                Text = nodo.Prioridad.ToUpper(),
                FontSize = 9,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                TextAlignment = TextAlignment.Center
            };
            infoRow.Children.Add(prioridadBadge);

            stack.Children.Add(infoRow);

            var fbRow = new TextBlock
            {
                Text = $"FB: {nodo.FactorBalance} | h: {nodo.Altura}",
                FontSize = 8,
                Foreground = (Brush)converter.ConvertFromString("#9CA3AF")!,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 4, 0, 0)
            };
            stack.Children.Add(fbRow);

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
                Stroke = new SolidColorBrush(Color.FromRgb(156, 163, 175)),
                StrokeThickness = 2.5,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round
            };

            var dashEffect = new System.Windows.Media.Effects.DropShadowEffect
            {
                ShadowDepth = 0,
                BlurRadius = 2,
                Opacity = 0.3,
                Color = Colors.Black
            };
            line.Effect = dashEffect;

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
