using System;

namespace ProyectoPED.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Titulo { get; set; } = "";
        public string? Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Prioridad { get; set; } = "Media";
        public string Estado { get; set; } = "Pendiente";
        public DateTime CreatedAt { get; set; }
        
        public int DiasRestantes => (FechaLimite.Date - DateTime.Today).Days;
        public string DiasRestantesTexto => DiasRestantes >= 0 ? $"{DiasRestantes} días" : $"{Math.Abs(DiasRestantes)} días";
        
        public string ColorDiasRestantes => DiasRestantes switch
        {
            >= 8 => "#22C55E",
            >= 4 => "#F97316",
            _ => "#EF4444"
        };

        public string ColorPrioridad => Prioridad switch
        {
            "Alta" => "#FEE2E2",
            "Media" => "#FEF3C7",
            "Baja" => "#D1FAE5",
            _ => "#FEF3C7"
        };

        public string ColorTextoPrioridad => Prioridad switch
        {
            "Alta" => "#DC2626",
            "Media" => "#D97706",
            "Baja" => "#059669",
            _ => "#D97706"
        };

        public string ColorEstado => Estado switch
        {
            "Completada" => "#10B981",
            "Pendiente" => "#6366F1",
            "Vencida" => "#EF4444",
            _ => "#6366F1"
        };
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Carne { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}