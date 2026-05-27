using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProyectoPED.Repositories;

namespace ProyectoPED.Views
{
    public partial class CambiarPasswordWindow : Window
    {
        private readonly int usuarioId;

        public CambiarPasswordWindow(int usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
        }

        private void PwbNueva_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ActualizarIndicadorSeguridad();
            ValidarFormulario();
        }

        private void PwbConfirmar_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidarConfirmacion();
            ValidarFormulario();
        }

        private void ActualizarIndicadorSeguridad()
        {
            string password = PwbNueva.Password;
            int nivel = CalcularNivelSeguridad(password);

            Color[] colores = { Colors.Transparent, Colors.Red, Colors.OrangeRed, Colors.Orange, Colors.DodgerBlue, Colors.Green };
            string[] etiquetas = { "", "Muy débil", "Débil", "Media", "Fuerte", "Muy fuerte" };
            string[] info = {
                "",
                "Usa al menos 8 caracteres",
                "Agrega números y mayúsculas",
                "Agrega símbolos especiales",
                "Buena combinación, casi perfecta",
                "Contraseña muy segura"
            };
            Color[] barColores = { Colors.Transparent, Color.FromRgb(220, 38, 38), Color.FromRgb(234, 88, 12), Color.FromRgb(217, 119, 6), Color.FromRgb(37, 99, 235), Color.FromRgb(5, 150, 105) };

            var barras = new[] { Bar1, Bar2, Bar3, Bar4, Bar5 };

            for (int i = 0; i < 5; i++)
            {
                if (string.IsNullOrEmpty(password))
                {
                    barras[i].Background = new SolidColorBrush(Color.FromRgb(229, 231, 235));
                }
                else if (i < nivel)
                {
                    barras[i].Background = new SolidColorBrush(barColores[nivel]);
                }
                else
                {
                    barras[i].Background = new SolidColorBrush(Color.FromRgb(229, 231, 235));
                }
            }

            if (string.IsNullOrEmpty(password))
            {
                TxtNivelSeguridad.Text = "Muy débil";
                TxtNivelSeguridad.Foreground = new SolidColorBrush(Color.FromRgb(156, 163, 175));
                TxtInfoSeguridad.Text = "Ingresa una contraseña";
            }
            else
            {
                TxtNivelSeguridad.Text = etiquetas[nivel];
                TxtNivelSeguridad.Foreground = new SolidColorBrush(barColores[nivel]);
                TxtInfoSeguridad.Text = info[nivel];
            }
        }

        private int CalcularNivelSeguridad(string password)
        {
            if (string.IsNullOrEmpty(password))
                return 0;

            int puntaje = 0;

            if (password.Length >= 8) puntaje++;
            if (password.Length >= 12) puntaje++;
            if (password.Any(char.IsUpper)) puntaje++;
            if (password.Any(char.IsLower)) puntaje++;
            if (password.Any(char.IsDigit)) puntaje++;
            if (password.Any(c => !char.IsLetterOrDigit(c))) puntaje++;

            if (puntaje <= 1) return 1;
            if (puntaje == 2) return 2;
            if (puntaje <= 4) return 3;
            if (puntaje == 5) return 4;
            return 5;
        }

        private void ValidarConfirmacion()
        {
            string nueva = PwbNueva.Password;
            string confirmar = PwbConfirmar.Password;

            if (string.IsNullOrEmpty(confirmar))
            {
                TxtConfirmacion.Visibility = Visibility.Collapsed;
                return;
            }

            if (nueva == confirmar)
            {
                TxtConfirmacion.Text = "✓ Las contraseñas coinciden";
                TxtConfirmacion.Foreground = new SolidColorBrush(Color.FromRgb(5, 150, 105));
                TxtConfirmacion.Visibility = Visibility.Visible;
            }
            else
            {
                TxtConfirmacion.Text = "✗ Las contraseñas no coinciden";
                TxtConfirmacion.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                TxtConfirmacion.Visibility = Visibility.Visible;
            }
        }

        private void ValidarFormulario()
        {
            string actual = PwbActual.Password;
            string nueva = PwbNueva.Password;
            string confirmar = PwbConfirmar.Password;

            BtnGuardar.IsEnabled = !string.IsNullOrEmpty(actual)
                && !string.IsNullOrEmpty(nueva)
                && nueva.Length >= 8
                && nueva == confirmar;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string actual = PwbActual.Password;
            string nueva = PwbNueva.Password;

            var usuario = UsuarioRepository.ObtenerUsuarioPorId(usuarioId);
            if (usuario == null)
            {
                MessageBox.Show("Error al obtener los datos del usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string hashActual = UsuarioRepository.HashearContraseña(actual);
            if (hashActual != usuario.Password)
            {
                MessageBox.Show("La contraseña actual no es correcta", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PwbActual.Focus();
                return;
            }

            if (UsuarioRepository.ActualizarPassword(usuarioId, nueva))
            {
                MessageBox.Show("Contraseña actualizada exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Error al actualizar la contraseña", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
