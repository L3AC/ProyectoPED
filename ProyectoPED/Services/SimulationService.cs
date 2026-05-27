using ProyectoPED.Models;
using ProyectoPED.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoPED.Services
{
    public class SimulationService
    {
        private readonly TareaService tareaService;
        private DateTime fechaActual;

        public SimulationService(TareaService tareaService)
        {
            this.tareaService = tareaService;
            this.fechaActual = DateTime.Today;
        }

        /// <summary>
        /// Obtiene la fecha actual de la simulación
        /// </summary>
        public DateTime ObtenerFechaActual()
        {
            return fechaActual;
        }

        /// <summary>
        /// Establece la fecha actual de la simulación
        /// </summary>
        public void EstablecerFechaActual(DateTime fecha)
        {
            fechaActual = fecha.Date;
        }

        /// <summary>
        /// Reinicia la simulación a la fecha actual del sistema
        /// </summary>
        public void ReiniciarSimulacion()
        {
            fechaActual = DateTime.Today;
        }

        /// <summary>
        /// Simula el avance de un día
        /// </summary>
        public SimulationResult SimularUnDia()
        {
            fechaActual = fechaActual.AddDays(1);
            return ObtenerResultadoSimulacion();
        }

        /// <summary>
        /// Simula el avance de múltiples días
        /// </summary>
        public SimulationResult SimularDias(int cantidad)
        {
            if (cantidad < 1)
                throw new ArgumentException("La cantidad de días debe ser mayor a 0");

            fechaActual = fechaActual.AddDays(cantidad);
            return ObtenerResultadoSimulacion();
        }

        /// <summary>
        /// Simula hasta una fecha específica
        /// </summary>
        public SimulationResult SimularHastaFecha(DateTime fecha)
        {
            if (fecha <= fechaActual)
                throw new ArgumentException("La fecha debe ser posterior a la fecha actual");

            fechaActual = fecha.Date;
            return ObtenerResultadoSimulacion();
        }

        /// <summary>
        /// Obtiene el resultado de la simulación actual
        /// </summary>
        private SimulationResult ObtenerResultadoSimulacion()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var resultado = new SimulationResult
            {
                FechaSimulacion = fechaActual,
                DiasAvanzados = (int)(fechaActual - DateTime.Today).TotalDays,
                TareasVencidasAhora = new List<TareaAlerta>(),
                TareasProximasAVencerAhora = new List<TareaAlerta>(),
                TareasCompletadas = tareas.Count(t => t.Estado == EstadoTarea.Completada),
                TareasPendientes = tareas.Count(t => t.Estado == EstadoTarea.Pendiente)
            };

            // Calcular tareas vencidas y próximas a vencer en base a la fecha simulada
            foreach (var tarea in tareas.Where(t => t.Estado != EstadoTarea.Completada))
            {
                var diasRestantesEnSimulacion = (tarea.FechaLimite.Date - fechaActual).Days;

                // Tareas vencidas
                if (diasRestantesEnSimulacion < 0)
                {
                    resultado.TareasVencidasAhora.Add(new TareaAlerta
                    {
                        TareaId = tarea.Id,
                        Titulo = tarea.Titulo,
                        FechaLimite = tarea.FechaLimite,
                        DiasRestantes = diasRestantesEnSimulacion,
                        Prioridad = tarea.Prioridad,
                        Severidad = "Crítica"
                    });
                }
                // Próximas a vencer (próximos 7 días)
                else if (diasRestantesEnSimulacion <= 7)
                {
                    var severidad = diasRestantesEnSimulacion switch
                    {
                        0 => "Vence hoy",
                        1 => "Muy Alta",
                        <= 3 => "Alta",
                        _ => "Normal"
                    };

                    resultado.TareasProximasAVencerAhora.Add(new TareaAlerta
                    {
                        TareaId = tarea.Id,
                        Titulo = tarea.Titulo,
                        FechaLimite = tarea.FechaLimite,
                        DiasRestantes = diasRestantesEnSimulacion,
                        Prioridad = tarea.Prioridad,
                        Severidad = severidad
                    });
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene una proyección de tareas hasta una fecha específica
        /// </summary>
        public ProyeccionTareas ObtenerProyeccion(DateTime fechaHasta)
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var proyeccion = new ProyeccionTareas
            {
                FechaInicio = DateTime.Today,
                FechaFin = fechaHasta,
                DiasTotales = (int)(fechaHasta.Date - DateTime.Today).TotalDays
            };

            // Agrupar tareas por semana
            var semanas = new Dictionary<int, List<Tarea>>();
            var hoy = DateTime.Today;

            foreach (var tarea in tareas.Where(t => t.Estado != EstadoTarea.Completada))
            {
                if (tarea.FechaLimite <= fechaHasta)
                {
                    var semana = (int)((tarea.FechaLimite.Date - hoy).TotalDays / 7);
                    if (!semanas.ContainsKey(semana))
                    {
                        semanas[semana] = new List<Tarea>();
                    }
                    semanas[semana].Add(tarea);
                }
            }

            proyeccion.TareasPorSemana = semanas;
            proyeccion.TotalTareasEnProyeccion = tareas.Count(t =>
                t.FechaLimite <= fechaHasta && t.Estado != EstadoTarea.Completada);

            return proyeccion;
        }

        /// <summary>
        /// Identifica cuellos de botella (picos de carga)
        /// </summary>
        public List<CuellodeBottella> IdentificarCuellosdeBottella()
        {
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var cargaPorDia = new Dictionary<DateTime, List<Tarea>>();

            // Agrupar tareas por fecha límite
            foreach (var tarea in tareas.Where(t => t.Estado != EstadoTarea.Completada))
            {
                var fecha = tarea.FechaLimite.Date;
                if (!cargaPorDia.ContainsKey(fecha))
                {
                    cargaPorDia[fecha] = new List<Tarea>();
                }
                cargaPorDia[fecha].Add(tarea);
            }

            // Identificar días con carga alta (pico = promedio + 1 desviación estándar)
            var promediBo = cargaPorDia.Values.Average(x => x.Count);
            var desviacionEstandar = Math.Sqrt(
                cargaPorDia.Values.Average(x => Math.Pow(x.Count - promediBo, 2))
            );
            var umbralPico = promediBo + desviacionEstandar;

            var cuellos = new List<CuellodeBottella>();

            foreach (var dia in cargaPorDia.Where(x => x.Value.Count > umbralPico))
            {
                var tareasPorPrioridad = new Dictionary<string, int>
                {
                    { "Alta", dia.Value.Count(t => t.Prioridad == "Alta") },
                    { "Media", dia.Value.Count(t => t.Prioridad == "Media") },
                    { "Baja", dia.Value.Count(t => t.Prioridad == "Baja") }
                };

                cuellos.Add(new CuellodeBottella
                {
                    Fecha = dia.Key,
                    CantidadTareas = dia.Value.Count,
                    IntensidadCarga = (dia.Value.Count / umbralPico) * 100,
                    TareasPorPrioridad = tareasPorPrioridad,
                    Tareas = dia.Value
                });
            }

            return cuellos.OrderBy(x => x.Fecha).ToList();
        }

        /// <summary>
        /// Obtiene recomendaciones basadas en la simulación
        /// </summary>
        public List<string> ObtenerRecomendaciones()
        {
            var recomendaciones = new List<string>();
            var tareas = tareaService.ObtenerTareasOrdenadas();
            var vencidas = tareaService.ObtenerTareasVencidas();
            var proximasAVencer = tareaService.ObtenerTareasProximasAVencer();

            if (vencidas.Count > 0)
            {
                recomendaciones.Add($"⚠️ Tienes {vencidas.Count} tarea(s) vencida(s). Atiéndelas inmediatamente.");
            }

            if (proximasAVencer.Count > 5)
            {
                recomendaciones.Add($"⚠️ Hay {proximasAVencer.Count} tareas próximas a vencer en los próximos 7 días. Prioriza tu trabajo.");
            }

            var tareasAlta = tareas.Count(t => t.Prioridad == "Alta" && t.Estado != EstadoTarea.Completada);
            if (tareasAlta > 3)
            {
                recomendaciones.Add($"📊 Tienes {tareasAlta} tareas de alta prioridad. Considera redistribuir tu carga de trabajo.");
            }

            var cuellos = IdentificarCuellosdeBottella();
            if (cuellos.Count > 0)
            {
                var primerCuello = cuellos.First();
                recomendaciones.Add($"📌 Existe un cuello de botella el {primerCuello.Fecha:dd/MM/yyyy} con {primerCuello.CantidadTareas} tareas.");
            }

            if (recomendaciones.Count == 0)
            {
                recomendaciones.Add("✅ Todo está bajo control. Sigue así.");
            }

            return recomendaciones;
        }
    }
}
