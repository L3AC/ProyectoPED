using ProyectoPED.DataStructures;
using ProyectoPED.Models;
using ProyectoPED.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProyectoPED.Services
{
    public class TareaService
    {
        private readonly int usuarioId;
        private readonly ArbolAVL arbolAVL;
        private readonly PilaDeshacer pilaDeshacer;
        private ObservableCollection<Tarea> tareas;

        public TareaService(int usuarioId)
        {
            this.usuarioId = usuarioId;
            this.arbolAVL = new ArbolAVL();
            this.pilaDeshacer = new PilaDeshacer();
            this.tareas = new ObservableCollection<Tarea>();
            CargarTareasDelUsuario();
        }

        public void CargarTareasDelUsuario()
        {
            tareas.Clear();
            arbolAVL.Limpiar();

            var tareasDelBD = TareaRepository.GetTareasPorUsuario(usuarioId);

            foreach (var tarea in tareasDelBD)
            {
                if (tarea.Estado == EstadoTarea.Pendiente && tarea.DiasRestantes < 0)
                {
                    tarea.Estado = EstadoTarea.Vencida;
                    TareaRepository.ActualizarEstado(tarea.Id, "Vencida");
                }

                tareas.Add(tarea);
                arbolAVL.Insertar(tarea);
            }
        }

        public List<Tarea> ObtenerTareasOrdenadas()
        {
            return arbolAVL.ObtenerRecorridoInOrder();
        }

        public ObservableCollection<Tarea> ObtenerTareasEnColeccion()
        {
            var tareasOrdenadas = ObtenerTareasOrdenadas();
            var resultado = new ObservableCollection<Tarea>(tareasOrdenadas);
            return resultado;
        }

        public bool CrearTarea(string titulo, string? descripcion, DateTime fechaLimite, string prioridad)
        {
            var tarea = new Tarea
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Descripcion = descripcion,
                FechaLimite = fechaLimite,
                Prioridad = prioridad,
                Estado = EstadoTarea.Pendiente,
                CreatedAt = DateTime.Now
            };

            var idGenerado = TareaRepository.InsertarTareaYObtenerId(tarea);
            if (idGenerado.HasValue)
            {
                tarea.Id = idGenerado.Value;
                tareas.Add(tarea);
                arbolAVL.Insertar(tarea);
                pilaDeshacer.Agregar(tarea, "Crear");

                return true;
            }

            return false;
        }

        public bool ActualizarTarea(Tarea tarea, bool guardarHistorial = true)
        {
            try
            {
                if (guardarHistorial)
                {
                    var tareaAnterior = arbolAVL.Buscar(tarea.Id);
                    if (tareaAnterior != null)
                    {
                        pilaDeshacer.Agregar(tareaAnterior, "Editar");
                    }
                }

                if (TareaRepository.ActualizarTarea(tarea))
                {
                    arbolAVL.Eliminar(tarea.Id);
                    arbolAVL.Insertar(tarea);

                    var indice = tareas.ToList().FindIndex(t => t.Id == tarea.Id);
                    if (indice >= 0)
                    {
                        tareas[indice] = tarea;
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool CompletarTarea(int tareaId)
        {
            try
            {
                var tarea = arbolAVL.Buscar(tareaId);
                if (tarea == null)
                    return false;

                pilaDeshacer.Agregar(tarea, "Completar");

                if (TareaRepository.CompletarTarea(tareaId))
                {
                    tarea.Estado = EstadoTarea.Completada;
                    arbolAVL.Eliminar(tareaId);
                    arbolAVL.Insertar(tarea);

                    var indice = tareas.ToList().FindIndex(t => t.Id == tareaId);
                    if (indice >= 0)
                    {
                        tareas[indice] = tarea;
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Deshacer()
        {
            try
            {
                var accion = pilaDeshacer.Deshacer();
                if (accion == null)
                    return false;

                switch (accion.TipoAccion)
                {
                    case "Crear":
                        return EliminarTarea(accion.Tarea.Id);

                    case "Completar":
                    case "Editar":
                        return ActualizarTarea(accion.Tarea, guardarHistorial: false);

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarTarea(int tareaId)
        {
            try
            {
                if (TareaRepository.EliminarTarea(tareaId))
                {
                    arbolAVL.Eliminar(tareaId);
                    var tarea = tareas.FirstOrDefault(t => t.Id == tareaId);
                    if (tarea != null)
                    {
                        tareas.Remove(tarea);
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public Tarea? ObtenerTareaPorId(int tareaId)
        {
            return arbolAVL.Buscar(tareaId);
        }

        public List<Tarea> ObtenerTareasPorEstado(EstadoTarea estado)
        {
            return ObtenerTareasOrdenadas().Where(t => t.Estado == estado).ToList();
        }

        public List<Tarea> ObtenerTareasPorPrioridad(string prioridad)
        {
            return ObtenerTareasOrdenadas().Where(t => t.Prioridad == prioridad).ToList();
        }

        public List<Tarea> ObtenerTareasVencidas()
        {
            return ObtenerTareasOrdenadas()
                .Where(t => t.DiasRestantes < 0 && t.Estado != EstadoTarea.Completada)
                .ToList();
        }

        public List<Tarea> ObtenerTareasProximasAVencer()
        {
            return ObtenerTareasOrdenadas()
                .Where(t => t.DiasRestantes >= 0 && t.DiasRestantes <= 7 && t.Estado != EstadoTarea.Completada)
                .ToList();
        }

        public bool TieneHistorialParaDeshacer()
        {
            return pilaDeshacer.TieneAcciones();
        }

        public ArbolAVL ObtenerArbolAVL()
        {
            return arbolAVL;
        }

        public List<NodoInfoArbol> ObtenerInfoArbolVisual()
        {
            return arbolAVL.ObtenerInfoArbolVisual();
        }

        public Dictionary<string, object> ObtenerEstadisticas()
        {
            var tareasOrdenadas = ObtenerTareasOrdenadas();

            return new Dictionary<string, object>
            {
                { "TotalTareas", tareasOrdenadas.Count },
                { "TareasCompletadas", tareasOrdenadas.Count(t => t.Estado == EstadoTarea.Completada) },
                { "TareasPendientes", tareasOrdenadas.Count(t => t.Estado == EstadoTarea.Pendiente) },
                { "TareasAlta", tareasOrdenadas.Count(t => t.Prioridad == "Alta") },
                { "TareasMedia", tareasOrdenadas.Count(t => t.Prioridad == "Media") },
                { "TareasBaja", tareasOrdenadas.Count(t => t.Prioridad == "Baja") },
                { "TareasVencidas", ObtenerTareasVencidas().Count },
                { "TareasProximasAVencer", ObtenerTareasProximasAVencer().Count },
                { "AlturaarbolAVL", arbolAVL.ObtenerAltura() },
                { "CantidadNodosArbol", arbolAVL.ObtenerCantidadNodos() }
            };
        }
    }
}
