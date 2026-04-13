using System.Windows;
using ProyectoPED.Views;

namespace ProyectoPED
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CargarDashboard();
        }

        private void CargarDashboard()
        {
            DashboardView.TxtNombreUsuario.Text = "Estudiante Demo";
            DashboardView.TxtCarnet.Text = "(20210001)";
            DashboardView.TxtFechaActual.Text = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
        }
    }
}
