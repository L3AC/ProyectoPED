using System.Windows;
using System.Windows.Controls;

namespace ProyectoPED.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string carne = txtCarneLogin.Text;
            string password = txtPasswordLogin.Password;

            LoadingOverlay.Visibility = Visibility.Visible;
            btnLogin.IsEnabled = false;
            txtCarneLogin.IsEnabled = false;
            txtPasswordLogin.IsEnabled = false;

            await Task.Delay(1500);

            bool isSuccess = true;

            if (isSuccess)
            {
                ErrorPanel.Visibility = Visibility.Collapsed;
                
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorPanel.Visibility = Visibility.Visible;
            }

            LoadingOverlay.Visibility = Visibility.Collapsed;
            btnLogin.IsEnabled = true;
            txtCarneLogin.IsEnabled = true;
            txtPasswordLogin.IsEnabled = true;
        }

        private void BtnIrRegistro_Click(object sender, RoutedEventArgs e)
        {
            // Ocultar panel de login y mostrar el de registro
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
            ErrorPanel.Visibility = Visibility.Collapsed;
            
            // Limpiar campos de login
            txtCarneLogin.Clear();
            txtPasswordLogin.Clear();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            string carne = txtCarneReg.Text;
            string nombre = txtNombreReg.Text;
            string password = txtPasswordReg.Password;
            string confirmPassword = txtConfirmPasswordReg.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor verifique.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(carne) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Agregar lógica de registro en base de datos MySQL aquí
            // 1. Conectar con la base de datos
            // 2. Verificar que el carné no exista ya
            // 3. Hashear la contraseña
            // 4. Insertar nuevo usuario en la tabla
            
            MessageBox.Show("Cuenta creada exitosamente. Puede iniciar sesión.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            
            // Regresar al inicio de sesión
            VolverALogin();
        }

        private void BtnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            VolverALogin();
        }

        private void VolverALogin()
        {
            RegisterPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            
            // Limpiar campos de registro
            txtCarneReg.Clear();
            txtNombreReg.Clear();
            txtPasswordReg.Clear();
            txtConfirmPasswordReg.Clear();
        }
    }
}
