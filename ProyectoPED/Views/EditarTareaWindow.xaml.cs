using System.Windows;
using System.Windows.Controls;
using ProyectoPED.Models;

namespace ProyectoPED.Views
{
    public partial class EditarTareaWindow : Window
    {
        public int TareaId { get; set; }
        public string TituloEditado => TxtTitulo.Text.Trim();
        public string DescripcionEditada => TxtDescripcion.Text.Trim();
        public DateTime FechaLimiteEditada => DpFechaLimite.SelectedDate ?? DateTime.Today;
        public string PrioridadEditada => (CmbPrioridad.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Media";
        public string EstadoEditado => (CmbEstado.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Pendiente";

        public EditarTareaWindow()
        {
            InitializeComponent();
        }

        public void CargarTarea(int id, string titulo, string descripcion, DateTime fechaLimite, string prioridad, string estado)
        {
            TareaId = id;
            TxtId.Text = id.ToString();
            TxtTitulo.Text = titulo;
            TxtDescripcion.Text = descripcion;
            DpFechaLimite.SelectedDate = fechaLimite;

            CmbPrioridad.SelectedIndex = prioridad switch
            {
                "Alta" => 0,
                "Media" => 1,
                "Baja" => 2,
                _ => 1
            };

            CmbEstado.SelectedIndex = estado switch
            {
                "Pendiente" => 0,
                "Completada" => 1,
                "Vencida" => 2,
                _ => 0
            };
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTitulo.Text))
            {
                MessageBox.Show("El título es obligatorio", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
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