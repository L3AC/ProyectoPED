using System.Windows;
using System.Windows.Controls;

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
            remove { RemoveHandler(ActualizarRequestedEvent, value); }
        }

        public AVLTreeView()
        {
            InitializeComponent();
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