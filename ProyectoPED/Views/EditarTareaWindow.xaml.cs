using System.Windows;

namespace ProyectoPED.Views
{
    public partial class EditarTareaWindow : Window
    {
        public int TareaId { get; set; }

        public EditarTareaWindow()
        {
            InitializeComponent();
        }

        public void CargarTarea(int id, string titulo, string descripcion, string prioridad, string estado)
        {
            TxtId.Text = id.ToString();
            TxtTitulo.Text = titulo;
            TxtDescripcion.Text = descripcion;
            
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

            MessageBox.Show("Tarea actualizada correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
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