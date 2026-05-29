using System;
using System.Collections.Generic;

namespace ProyectoPED.Models
{
    public enum EstadoTarea
    {
        Pendiente,
        Completada,
        Vencida
    }

    public class Tarea
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Titulo { get; set; } = "";
        public string? Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Prioridad { get; set; } = "Media";
        public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;
        public DateTime CreatedAt { get; set; }

        public int DiasRestantes => (FechaLimite.Date - DateTime.Today).Days;
        
        public string DiasRestantesTexto => DiasRestantes switch
        {
            > 0 => $"{DiasRestantes} días",
            0 => "Hoy",
            _ => "---"
        };

        public string ColorDiasRestantes => DiasRestantes switch
        {
            >= 8 => "#22C55E",
            >= 4 => "#F97316",
            >= 0 => "#EF4444",
            _ => "#7C3AED"
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
            EstadoTarea.Completada => "#10B981",
            EstadoTarea.Pendiente => "#6366F1",
            EstadoTarea.Vencida => "#EF4444",
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

    public class AccionHistorial
    {
        public Tarea Tarea { get; set; } = null!;
        public string TipoAccion { get; set; } = "";
        public DateTime FechaAccion { get; set; } = DateTime.Now;
    }
}
