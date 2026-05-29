using MySqlConnector;
using ProyectoPED.Data;
using ProyectoPED.Models;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoPED.Repositories
{
    public class UsuarioRepository
    {
        /// Autentica un usuario con su carné y contraseña
        public static Usuario? AutenticarUsuario(string carne, string password)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = "SELECT id, carne, nombre, password, created_at FROM usuarios WHERE carne = @carne LIMIT 1";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@carne", carne);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var usuarioEnBD = new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Carne = reader.GetString(1),
                        Nombre = reader.GetString(2),
                        Password = reader.GetString(3),
                        CreatedAt = reader.GetDateTime(4)
                    };

                    // Verificar contraseña con hash
                    if (VerificarContraseña(password, usuarioEnBD.Password))
                    {
                        return usuarioEnBD;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// Obtiene un usuario por su ID
        public static Usuario? ObtenerUsuarioPorId(int usuarioId)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = "SELECT id, carne, nombre, password, created_at FROM usuarios WHERE id = @id LIMIT 1";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", usuarioId);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Carne = reader.GetString(1),
                        Nombre = reader.GetString(2),
                        Password = reader.GetString(3),
                        CreatedAt = reader.GetDateTime(4)
                    };
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// Registra un nuevo usuario
        public static bool RegistrarUsuario(string carne, string nombre, string password)
        {
            try
            {
                // Verificar que el carné no exista
                if (ExisteUsuario(carne))
                    return false;

                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string passwordHash = HashearContraseña(password);

                string query = "INSERT INTO usuarios (carne, nombre, password) VALUES (@carne, @nombre, @password)";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@carne", carne);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@password", passwordHash);

                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        /// Verifica si un usuario con ese carné ya existe
        public static bool ExisteUsuario(string carne)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = "SELECT COUNT(*) FROM usuarios WHERE carne = @carne";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@carne", carne);

                var result = command.ExecuteScalar();
                return result != null && (long)result > 0;
            }
            catch
            {
                return false;
            }
        }

        /// Hashea una contraseña usando SHA256
        public static string HashearContraseña(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// Verifica si una contraseña coincide con su hash
        private static bool VerificarContraseña(string password, string hash)
        {
            var hashDelPassword = HashearContraseña(password);
            return hashDelPassword == hash;
        }

        /// Actualiza la contraseña de un usuario
        public static bool ActualizarPassword(int usuarioId, string nuevaPassword)
        {
            try
            {
                using var connection = DatabaseConnection.GetConnection();
                connection.Open();

                string passwordHash = HashearContraseña(nuevaPassword);
                string query = "UPDATE usuarios SET password = @password WHERE id = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", usuarioId);
                command.Parameters.AddWithValue("@password", passwordHash);

                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
