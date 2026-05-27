using ProyectoPED.Models;
using System;
using System.Collections.Generic;

namespace ProyectoPED.DataStructures
{
    /// <summary>
    /// Pila (Stack) para implementar la funcionalidad de deshacer
    /// Guarda el estado completo de las tareas
    /// </summary>
    public class PilaDeshacer
    {
        private Stack<AccionHistorial> pila;

        public PilaDeshacer()
        {
            pila = new Stack<AccionHistorial>();
        }

        /// <summary>
        /// Agrega una acción al historial
        /// </summary>
        public void Agregar(Tarea tarea, string tipoAccion)
        {
            var accion = new AccionHistorial
            {
                Tarea = new Tarea
                {
                    Id = tarea.Id,
                    UsuarioId = tarea.UsuarioId,
                    Titulo = tarea.Titulo,
                    Descripcion = tarea.Descripcion,
                    FechaLimite = tarea.FechaLimite,
                    Prioridad = tarea.Prioridad,
                    Estado = tarea.Estado,
                    CreatedAt = tarea.CreatedAt
                },
                TipoAccion = tipoAccion,
                FechaAccion = DateTime.Now
            };

            pila.Push(accion);
        }

        /// <summary>
        /// Obtiene la última acción sin eliminarla
        /// </summary>
        public AccionHistorial? ObtenerUltima()
        {
            if (pila.Count > 0)
            {
                return pila.Peek();
            }
            return null;
        }

        /// <summary>
        /// Extrae la última acción
        /// </summary>
        public AccionHistorial? Deshacer()
        {
            if (pila.Count > 0)
            {
                return pila.Pop();
            }
            return null;
        }

        /// <summary>
        /// Verifica si hay acciones para deshacer
        /// </summary>
        public bool TieneAcciones()
        {
            return pila.Count > 0;
        }

        /// <summary>
        /// Obtiene la cantidad de acciones en la pila
        /// </summary>
        public int ObtenerCantidadAcciones()
        {
            return pila.Count;
        }

        /// <summary>
        /// Limpia todas las acciones
        /// </summary>
        public void Limpiar()
        {
            pila.Clear();
        }

        /// <summary>
        /// Obtiene información de todas las acciones (para depuración)
        /// </summary>
        public List<string> ObtenerHistorial()
        {
            var resultado = new List<string>();
            var temp = new Stack<AccionHistorial>(pila);

            while (temp.Count > 0)
            {
                var accion = temp.Pop();
                resultado.Add($"[{accion.FechaAccion:HH:mm:ss}] {accion.TipoAccion}: {accion.Tarea.Titulo}");
            }

            return resultado;
        }
    }
}
