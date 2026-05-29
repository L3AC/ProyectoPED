using System.Windows;
using System.Windows.Controls;
using ProyectoPED.Repositories;

namespace ProyectoPED.Views
{
    public partial class LoginWindow : Window
    {
        public bool LoginExitoso { get; private set; } = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string carne = txtCarneLogin.Text.Trim();
            string password = txtPasswordLogin.Password;

            if (string.IsNullOrWhiteSpace(carne) || string.IsNullOrWhiteSpace(password))
            {
                ErrorPanel.Visibility = Visibility.Visible;
                ((TextBlock)ErrorPanel.FindName("ErrorTitle")).Text = "Campos requeridos";
                ((TextBlock)ErrorPanel.FindName("ErrorDetail")).Text = "Por favor ingresa tu carné y contraseña";
                return;
            }

            LoadingOverlay.Visibility = Visibility.Visible;
            btnLogin.IsEnabled = false;
            txtCarneLogin.IsEnabled = false;
            txtPasswordLogin.IsEnabled = false;

            var usuario = await Task.Run(() => UsuarioRepository.AutenticarUsuario(carne, password));

            LoadingOverlay.Visibility = Visibility.Collapsed;
            btnLogin.IsEnabled = true;
            txtCarneLogin.IsEnabled = true;
            txtPasswordLogin.IsEnabled = true;

            if (usuario != null)
            {
                ErrorPanel.Visibility = Visibility.Collapsed;
                LoginExitoso = true;

                var mainWindow = new MainWindow(usuario);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorPanel.Visibility = Visibility.Visible;
                ((TextBlock)ErrorPanel.FindName("ErrorTitle")).Text = "Carné o contraseña incorrectos";
                ((TextBlock)ErrorPanel.FindName("ErrorDetail")).Text = "Verifica tus credenciales e intenta nuevamente";
            }
        }

        private void BtnIrRegistro_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
            ErrorPanel.Visibility = Visibility.Collapsed;

            txtCarneLogin.Clear();
            txtPasswordLogin.Clear();
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            string carne = txtCarneReg.Text.Trim();
            string nombre = txtNombreReg.Text.Trim();
            string password = txtPasswordReg.Password;
            string confirmPassword = txtConfirmPasswordReg.Password;

            if (string.IsNullOrWhiteSpace(carne) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor verifique.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool registrado = await Task.Run(() => UsuarioRepository.RegistrarUsuario(carne, nombre, password));

            if (registrado)
            {
                MessageBox.Show("Cuenta creada exitosamente. Puede iniciar sesión.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                VolverALogin();
            }
            else
            {
                bool existe = await Task.Run(() => UsuarioRepository.ExisteUsuario(carne));
                if (existe)
                {
                    MessageBox.Show("El carné ingresado ya está registrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Ocurrió un error al crear la cuenta. Intente nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            VolverALogin();
        }

        private void VolverALogin()
        {
            RegisterPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;

            txtCarneReg.Clear();
            txtNombreReg.Clear();
            txtPasswordReg.Clear();
            txtConfirmPasswordReg.Clear();
        }
    }
}
