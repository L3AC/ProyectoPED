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

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string carne = txtCarneLogin.Text;
            string password = txtPasswordLogin.Password;

            // TODO: Agregar lógica de conexión a la base de datos MySQL aquí
            // 1. Conectar con la base de datos
            // 2. Consultar la tabla usuarios con el carné ingresado
            // 3. Comparar la contraseña (con hash) almacenada
            // 4. Si es correcto, guardar id de usuario en memoria y abrir Dashboard
            
            // Simulación de validación con usuario de prueba
            bool isSuccess = true;
            
            /*if (carne == "" && password == "")
            {
                isSuccess = true;
            }*/
            
            if (isSuccess)
            {
                ErrorPanel.Visibility = Visibility.Collapsed;
                
                // Abrir la ventana principal (Dashboard)
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                // 5. Mostrar mensaje de error en rojo debajo del formulario
                ErrorPanel.Visibility = Visibility.Visible;
            }
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
