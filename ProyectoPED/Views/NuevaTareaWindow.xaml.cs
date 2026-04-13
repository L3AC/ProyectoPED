using System.Windows;
using System.Windows.Controls;

namespace ProyectoPED.Views
{
    public partial class NuevaTareaWindow : Window
    {
        public NuevaTareaWindow()
        {
            InitializeComponent();
        }

        private void BtnAgregarSubtarea_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtSubtarea.Text))
            {
                LbSubtareas.Items.Add(TxtSubtarea.Text);
                TxtSubtarea.Text = "";
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTitulo.Text))
            {
                MessageBox.Show("El título es obligatorio", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Aquí iría la lógica para guardar la tarea
            MessageBox.Show("Tarea creada correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}