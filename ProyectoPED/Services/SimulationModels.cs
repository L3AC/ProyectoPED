using ProyectoPED.Models;
using System;
using System.Collections.Generic;

namespace ProyectoPED.Services
{
    /// Resultado de la simulación de un día/período
    public class SimulationResult
    {
        public DateTime FechaSimulacion { get; set; }
        public int DiasAvanzados { get; set; }
        public List<TareaAlerta> TareasVencidasAhora { get; set; } = new();
        public List<TareaAlerta> TareasProximasAVencerAhora { get; set; } = new();
        public int TareasCompletadas { get; set; }
        public int TareasPendientes { get; set; }
    }

    /// Alerta sobre una tarea
    public class TareaAlerta
    {
        public int TareaId { get; set; }
        public string Titulo { get; set; } = "";
        public DateTime FechaLimite { get; set; }
        public int DiasRestantes { get; set; }
        public string Prioridad { get; set; } = "";
        public string Severidad { get; set; } = "";
    }

    /// Proyección de tareas
    public class ProyeccionTareas
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasTotales { get; set; }
        public int TotalTareasEnProyeccion { get; set; }
        public Dictionary<int, List<Tarea>> TareasPorSemana { get; set; } = new();
    }

    /// Información sobre cuello de botella
    public class CuellodeBottella
    {
        public DateTime Fecha { get; set; }
        public int CantidadTareas { get; set; }
        public double IntensidadCarga { get; set; }
        public Dictionary<string, int> TareasPorPrioridad { get; set; } = new();
        public List<Tarea> Tareas { get; set; } = new();
    }
}
