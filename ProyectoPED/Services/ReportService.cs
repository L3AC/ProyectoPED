using ProyectoPED.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoPED.Services
{
    public class ReportService
    {
        private readonly TareaService tareaService;

        public ReportService(TareaService tareaService)
        {
            this.tareaService = tareaService;
        }

        /// <summary>
        /// Obtiene estadísticas de tareas por prioridad
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticasPorPrioridad()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            return new Dictionary<string, int>
            {
                { "Alta", tareas.Count(t => t.Prioridad == "Alta") },
                { "Media", tareas.Count(t => t.Prioridad == "Media") },
                { "Baja", tareas.Count(t => t.Prioridad == "Baja") }
            };
        }

        /// <summary>
        /// Obtiene estadísticas de tareas por estado
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticasPorEstado()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            return new Dictionary<string, int>
            {
                { "Completada", tareas.Count(t => t.Estado == EstadoTarea.Completada) },
                { "Pendiente", tareas.Count(t => t.Estado == EstadoTarea.Pendiente) }
            };
        }

        /// <summary>
        /// Obtiene el recuento de tareas por rango de urgencia
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticasPorUrgencia()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            return new Dictionary<string, int>
            {
                { "Crítica (≤ 3 días)", tareas.Count(t => t.DiasRestantes <= 3 && t.Estado != EstadoTarea.Completada) },
                { "Alta (4-7 días)", tareas.Count(t => t.DiasRestantes >= 4 && t.DiasRestantes <= 7 && t.Estado != EstadoTarea.Completada) },
                { "Normal (≥ 8 días)", tareas.Count(t => t.DiasRestantes >= 8 && t.Estado != EstadoTarea.Completada) },
                { "Vencidas", tareas.Count(t => t.DiasRestantes < 0 && t.Estado != EstadoTarea.Completada) }
            };
        }

        /// <summary>
        /// Obtiene el resumen general del sistema
        /// </summary>
        public ResumenGeneral ObtenerResumenGeneral()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var completadas = tareas.Count(t => t.Estado == EstadoTarea.Completada);
            var total = tareas.Count;
            var porcentajeCompletacion = total > 0 ? (completadas * 100.0) / total : 0;

            return new ResumenGeneral
            {
                TotalTareas = total,
                TareasCompletadas = completadas,
                TareasPendientes = total - completadas,
                PorcentajeCompletacion = porcentajeCompletacion,
                TareasVencidas = tareaService.ObtenerTareasVencidas().Count,
                TareasProximasAVencer = tareaService.ObtenerTareasProximasAVencer().Count,
                FechaActualizacion = DateTime.Now
            };
        }

        /// <summary>
        /// Obtiene las tareas vencidas con detalles
        /// </summary>
        public List<TareaVencida> ObtenerTareasVencidasDetallado()
        {
            var tareasVencidas = tareaService.ObtenerTareasVencidas();
            var resultado = new List<TareaVencida>();

            foreach (var tarea in tareasVencidas)
            {
                resultado.Add(new TareaVencida
                {
                    Id = tarea.Id,
                    Titulo = tarea.Titulo,
                    FechaLimite = tarea.FechaLimite,
                    DiasVencidos = Math.Abs(tarea.DiasRestantes),
                    Prioridad = tarea.Prioridad
                });
            }

            return resultado.OrderByDescending(t => t.DiasVencidos).ToList();
        }

        /// <summary>
        /// Obtiene tareas próximas a vencer con detalles
        /// </summary>
        public List<TareaProxima> ObtenerTareasProximasDetallado()
        {
            var tareasProximas = tareaService.ObtenerTareasProximasAVencer();
            var resultado = new List<TareaProxima>();

            foreach (var tarea in tareasProximas)
            {
                resultado.Add(new TareaProxima
                {
                    Id = tarea.Id,
                    Titulo = tarea.Titulo,
                    FechaLimite = tarea.FechaLimite,
                    DiasRestantes = tarea.DiasRestantes,
                    Prioridad = tarea.Prioridad,
                    Urgencia = CalcularUrgencia(tarea.DiasRestantes)
                });
            }

            return resultado.OrderBy(t => t.DiasRestantes).ToList();
        }

        /// <summary>
        /// Obtiene un resumen por semana
        /// </summary>
        public Dictionary<string, int> ObtenerResumenPorSemana()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var hoy = DateTime.Today;
            var semana = hoy.AddDays(-((int)hoy.DayOfWeek + 6) % 7);

            var resultado = new Dictionary<string, int>();

            for (int i = 0; i < 4; i++)
            {
                var inicio = semana.AddDays(i * 7);
                var fin = inicio.AddDays(6);
                var clave = $"Semana {i + 1}";

                var cantidadTareas = tareas.Count(t =>
                    t.FechaLimite >= inicio && t.FechaLimite <= fin && t.Estado != EstadoTarea.Completada);

                resultado[clave] = cantidadTareas;
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene promedio de tareas completadas por día
        /// </summary>
        public double ObtenerPromedioTareasCompletadasPorDia()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var completadas = tareas.Where(t => t.Estado == EstadoTarea.Completada).ToList();

            if (completadas.Count == 0)
                return 0;

            // Calcular días desde la tarea más antigua
            var tareasMasAntigua = tareas.OrderBy(t => t.CreatedAt).FirstOrDefault();
            if (tareasMasAntigua == null)
                return 0;

            var diasTranscurridos = (DateTime.Now - tareasMasAntigua.CreatedAt).TotalDays;
            if (diasTranscurridos <= 0)
                return 0;

            return completadas.Count / diasTranscurridos;
        }

        /// <summary>
        /// Genera reporte de comparativa por mes
        /// </summary>
        public Dictionary<string, Dictionary<string, int>> ObtenerReporteComparativaMeses()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var resultado = new Dictionary<string, Dictionary<string, int>>();

            var mesesUnicos = tareas
                .Select(t => new { t.FechaLimite.Year, t.FechaLimite.Month })
                .Distinct()
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            foreach (var mes in mesesUnicos)
            {
                var clave = $"{mes.Year}-{mes.Month:D2}";
                var tareasMes = tareas.Where(t =>
                    t.FechaLimite.Year == mes.Year && t.FechaLimite.Month == mes.Month).ToList();

                resultado[clave] = new Dictionary<string, int>
                {
                        { "Completadas", tareasMes.Count(t => t.Estado == EstadoTarea.Completada) },
                    { "Pendientes", tareasMes.Count(t => t.Estado == EstadoTarea.Pendiente) }
                };
            }

            return resultado;
        }

        /// <summary>
        /// Calcula la urgencia de una tarea basada en días restantes
        /// </summary>
        private string CalcularUrgencia(int diasRestantes)
        {
            return diasRestantes switch
            {
                <= 1 => "Crítica",
                <= 3 => "Muy Alta",
                <= 7 => "Alta",
                _ => "Normal"
            };
        }

        /// <summary>
        /// Obtiene el análisis de carga de trabajo
        /// </summary>
        public AnálisisCargaTrabajo ObtenerAnálisisCargaTrabajo()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var tareasPendientes = tareas.Where(t => t.Estado != EstadoTarea.Completada).ToList();

            var cargaPorDia = new Dictionary<DateTime, int>();
            foreach (var tarea in tareasPendientes)
            {
                var fecha = tarea.FechaLimite.Date;
                if (cargaPorDia.ContainsKey(fecha))
                    cargaPorDia[fecha]++;
                else
                    cargaPorDia[fecha] = 1;
            }

            var diasConMayorCarga = cargaPorDia
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            return new AnálisisCargaTrabajo
            {
                CargaPromedioDiaria = tareasPendientes.Count > 0 ? 
                    tareasPendientes.Count / (double)Math.Max(1, cargaPorDia.Count) : 0,
                DiaConMayorCarga = diasConMayorCarga.FirstOrDefault().Key,
                MaximoCargaDiaria = diasConMayorCarga.FirstOrDefault().Value,
                DiasConCargaDiaria = diasConMayorCarga
                    .Select(x => new KeyValuePair<DateTime, int>(x.Key, x.Value))
                    .ToList()
            };
        }
    }

    /// <summary>
    /// Resumen general de tareas
    /// </summary>
    public class ResumenGeneral
    {
        public int TotalTareas { get; set; }
        public int TareasCompletadas { get; set; }
        public int TareasPendientes { get; set; }
        public double PorcentajeCompletacion { get; set; }
        public int TareasVencidas { get; set; }
        public int TareasProximasAVencer { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }

    /// <summary>
    /// Información de tarea vencida
    /// </summary>
    public class TareaVencida
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public DateTime FechaLimite { get; set; }
        public int DiasVencidos { get; set; }
        public string Prioridad { get; set; } = "";
    }

    /// <summary>
    /// Información de tarea próxima a vencer
    /// </summary>
    public class TareaProxima
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public DateTime FechaLimite { get; set; }
        public int DiasRestantes { get; set; }
        public string Prioridad { get; set; } = "";
        public string Urgencia { get; set; } = "";
    }

    /// <summary>
    /// Análisis de carga de trabajo
    /// </summary>
    public class AnálisisCargaTrabajo
    {
        public double CargaPromedioDiaria { get; set; }
        public DateTime DiaConMayorCarga { get; set; }
        public int MaximoCargaDiaria { get; set; }
        public List<KeyValuePair<DateTime, int>> DiasConCargaDiaria { get; set; } = new();
    }
}
