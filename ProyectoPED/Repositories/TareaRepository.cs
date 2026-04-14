using MySqlConnector;
using ProyectoPED.Data;
using ProyectoPED.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace ProyectoPED.Repositories
{
    public class TareaRepository
    {
        public static ObservableCollection<Tarea> GetTareasPorUsuario(int usuarioId)
        {
            var tareas = new ObservableCollection<Tarea>();

            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = @"SELECT id, usuario_id, titulo, descripcion, fecha_limite, prioridad, estado, created_at 
                                 FROM tareas WHERE usuario_id = @usuarioId ORDER BY fecha_limite ASC";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@usuarioId", usuarioId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tarea = new Tarea
                    {
                        Id = reader.GetInt32(0),
                        UsuarioId = reader.GetInt32(1),
                        Titulo = reader.GetString(2),
                        Descripcion = reader.IsDBNull(3) ? null : reader.GetString(3),
                        FechaLimite = reader.GetDateTime(4),
                        Prioridad = reader.GetString(5),
                        Estado = reader.GetString(6),
                        CreatedAt = reader.GetDateTime(7)
                    };
                    tareas.Add(tarea);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar tareas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return tareas;
        }

        public static bool InsertarTarea(Tarea tarea)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = @"INSERT INTO tareas (usuario_id, titulo, descripcion, fecha_limite, prioridad, estado) 
                                 VALUES (@usuarioId, @titulo, @descripcion, @fechaLimite, @prioridad, @estado)";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@usuarioId", tarea.UsuarioId);
                command.Parameters.AddWithValue("@titulo", tarea.Titulo);
                command.Parameters.AddWithValue("@descripcion", tarea.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@fechaLimite", tarea.FechaLimite);
                command.Parameters.AddWithValue("@prioridad", tarea.Prioridad);
                command.Parameters.AddWithValue("@estado", tarea.Estado);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar tarea: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool ActualizarTarea(Tarea tarea)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = @"UPDATE tareas SET titulo = @titulo, descripcion = @descripcion, 
                                 fecha_limite = @fechaLimite, prioridad = @prioridad, estado = @estado 
                                 WHERE id = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", tarea.Id);
                command.Parameters.AddWithValue("@titulo", tarea.Titulo);
                command.Parameters.AddWithValue("@descripcion", tarea.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@fechaLimite", tarea.FechaLimite);
                command.Parameters.AddWithValue("@prioridad", tarea.Prioridad);
                command.Parameters.AddWithValue("@estado", tarea.Estado);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar tarea: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool CompletarTarea(int tareaId)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = "UPDATE tareas SET estado = 'Completada' WHERE id = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", tareaId);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al completar tarea: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool DeshacerTarea(int tareaId)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = "UPDATE tareas SET estado = 'Pendiente' WHERE id = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", tareaId);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al deshacer tarea: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool EliminarTarea(int tareaId)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = "DELETE FROM tareas WHERE id = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", tareaId);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar tarea: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}