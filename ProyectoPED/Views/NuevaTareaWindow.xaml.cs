using System;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoPED.Views
{
    public partial class NuevaTareaWindow : Window
    {
        public string TituloTarea => TxtTitulo.Text.Trim();
        public string DescripcionTarea => TxtDescripcion.Text.Trim();
        public DateTime FechaLimiteTarea => DpFechaLimite.SelectedDate ?? DateTime.Today.AddDays(7);
        public string PrioridadTarea => (CmbPrioridad.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Media";

        public NuevaTareaWindow()
        {
            InitializeComponent();
            DpFechaLimite.SelectedDate = DateTime.Today.AddDays(7);
            CmbPrioridad.SelectedIndex = 1;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTitulo.Text))
            {
                MessageBox.Show("El título es obligatorio", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DpFechaLimite.SelectedDate == null)
            {
                MessageBox.Show("La fecha límite es obligatoria", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
