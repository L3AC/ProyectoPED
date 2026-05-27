using System.Windows;
using ProyectoPED.Views;
using ProyectoPED.Models;

namespace ProyectoPED
{
    public partial class MainWindow : Window
    {
        private Usuario usuarioActual = null!;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Usuario usuario) : this()
        {
            usuarioActual = usuario;
            CargarDashboard(usuario);
        }

        private void CargarDashboard(Usuario usuario)
        {
            DashboardView.CargarUsuario(usuario.Id, usuario.Nombre, usuario.Carne);
        }
    }
}
