using ProyectoPED.Models;
using System;
using System.Collections.Generic;

namespace ProyectoPED.DataStructures
{
    /// Pila (Stack) para implementar la funcionalidad de deshacer
    /// Guarda el estado completo de las tareas
    public class PilaDeshacer
    {
        private Stack<AccionHistorial> pila;

        public PilaDeshacer()
        {
            pila = new Stack<AccionHistorial>();
        }

        /// Agrega una acción al historial
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

        /// Obtiene la última acción sin eliminarla
        public AccionHistorial? ObtenerUltima()
        {
            if (pila.Count > 0)
            {
                return pila.Peek();
            }
            return null;
        }

        /// Extrae la última acción
        public AccionHistorial? Deshacer()
        {
            if (pila.Count > 0)
            {
                return pila.Pop();
            }
            return null;
        }

        /// Verifica si hay acciones para deshacer
        public bool TieneAcciones()
        {
            return pila.Count > 0;
        }

        /// Obtiene la cantidad de acciones en la pila
        public int ObtenerCantidadAcciones()
        {
            return pila.Count;
        }

        /// Limpia todas las acciones
        public void Limpiar()
        {
            pila.Clear();
        }

        /// Obtiene información de todas las acciones (para depuración)
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
