using ProyectoPED.Models;
using System;
using System.Collections.Generic;

namespace ProyectoPED.DataStructures
{
    /// Nodo del Árbol AVL
    public class NodoAVL
    {
        public Tarea Tarea { get; set; } = null!;
        public NodoAVL? Izquierda { get; set; }
        public NodoAVL? Derecha { get; set; }
        public int Altura { get; set; } = 1;

        public NodoAVL(Tarea tarea)
        {
            Tarea = tarea;
        }

        public int ObtenerAltura()
        {
            return Altura;
        }

        public int ObtenerFactorBalance()
        {
            int alturaIzq = Izquierda?.Altura ?? 0;
            int alturaDer = Derecha?.Altura ?? 0;
            return alturaIzq - alturaDer;
        }

        public void ActualizarAltura()
        {
            int alturaIzq = Izquierda?.Altura ?? 0;
            int alturaDer = Derecha?.Altura ?? 0;
            Altura = 1 + Math.Max(alturaIzq, alturaDer);
        }
    }

    /// Árbol AVL para mantener tareas ordenadas por urgencia (días restantes)
    public class ArbolAVL
    {
        private NodoAVL? raiz;

        public ArbolAVL()
        {
            raiz = null;
        }

        /// Inserta una tarea en el árbol
        /// Ordenamiento: por días restantes (negativo, para que los más urgentes estén al inicio)
        public void Insertar(Tarea tarea)
        {
            raiz = InsertarRecursivo(raiz, tarea);
        }

        private NodoAVL InsertarRecursivo(NodoAVL? nodo, Tarea tarea)
        {
            if (nodo == null)
            {
                return new NodoAVL(tarea);
            }

            // Comparar por días restantes negados para ordenar de más urgente a menos urgente
            int clave = -tarea.DiasRestantes;
            int claveNodo = -nodo.Tarea.DiasRestantes;

            if (clave < claveNodo)
            {
                nodo.Izquierda = InsertarRecursivo(nodo.Izquierda, tarea);
            }
            else if (clave > claveNodo)
            {
                nodo.Derecha = InsertarRecursivo(nodo.Derecha, tarea);
            }
            else
            {
                // Si tienen los mismos días restantes, usar el ID para evitar duplicados
                if (tarea.Id < nodo.Tarea.Id)
                {
                    nodo.Izquierda = InsertarRecursivo(nodo.Izquierda, tarea);
                }
                else if (tarea.Id > nodo.Tarea.Id)
                {
                    nodo.Derecha = InsertarRecursivo(nodo.Derecha, tarea);
                }
                else
                {
                    return nodo; // Elemento ya existe
                }
            }

            nodo.ActualizarAltura();
            return Balancear(nodo);
        }

        /// Elimina una tarea del árbol por su ID
        public void Eliminar(int tareaId)
        {
            raiz = EliminarRecursivoPorId(raiz, tareaId);
        }

        private NodoAVL? EliminarRecursivoPorId(NodoAVL? nodo, int tareaId)
        {
            if (nodo == null)
                return null;

            if (nodo.Tarea.Id == tareaId)
            {
                if (nodo.Izquierda == null && nodo.Derecha == null)
                    return null;

                if (nodo.Izquierda == null)
                    return nodo.Derecha;

                if (nodo.Derecha == null)
                    return nodo.Izquierda;

                var minimo = ObtenerNodoMinimo(nodo.Derecha);
                nodo.Tarea = minimo.Tarea;
                nodo.Derecha = EliminarRecursivoPorId(nodo.Derecha, minimo.Tarea.Id);

                nodo.ActualizarAltura();
                return Balancear(nodo);
            }

            nodo.Izquierda = EliminarRecursivoPorId(nodo.Izquierda, tareaId);
            nodo.Derecha = EliminarRecursivoPorId(nodo.Derecha, tareaId);

            nodo.ActualizarAltura();
            return Balancear(nodo);
        }

        /// Busca una tarea por ID (recorrido completo)
        public Tarea? Buscar(int tareaId)
        {
            return BuscarRecursivo(raiz, tareaId);
        }

        private Tarea? BuscarRecursivo(NodoAVL? nodo, int tareaId)
        {
            if (nodo == null)
                return null;

            if (nodo.Tarea.Id == tareaId)
                return nodo.Tarea;

            var izquierda = BuscarRecursivo(nodo.Izquierda, tareaId);
            if (izquierda != null)
                return izquierda;

            return BuscarRecursivo(nodo.Derecha, tareaId);
        }

        /// Obtiene el recorrido InOrder (tareas ordenadas por urgencia)
        public List<Tarea> ObtenerRecorridoInOrder()
        {
            var resultado = new List<Tarea>();
            RecorridoInOrder(raiz, resultado);
            return resultado;
        }

        private void RecorridoInOrder(NodoAVL? nodo, List<Tarea> resultado)
        {
            if (nodo == null)
                return;

            RecorridoInOrder(nodo.Izquierda, resultado);
            resultado.Add(nodo.Tarea);
            RecorridoInOrder(nodo.Derecha, resultado);
        }

        /// Obtiene el recorrido PreOrder (para visualización jerárquica)
        public List<Tarea> ObtenerRecorridoPreOrder()
        {
            var resultado = new List<Tarea>();
            RecorridoPreOrder(raiz, resultado);
            return resultado;
        }

        private void RecorridoPreOrder(NodoAVL? nodo, List<Tarea> resultado)
        {
            if (nodo == null)
                return;

            resultado.Add(nodo.Tarea);
            RecorridoPreOrder(nodo.Izquierda, resultado);
            RecorridoPreOrder(nodo.Derecha, resultado);
        }

        /// Obtiene información jerárquica del árbol para visualización
        public List<NodoInfoArbol> ObtenerInfoArbolVisual()
        {
            var resultado = new List<NodoInfoArbol>();
            ObtenerInfoArbolVisualRecursivo(raiz, resultado, null, false);
            return resultado;
        }

        private void ObtenerInfoArbolVisualRecursivo(NodoAVL? nodo, List<NodoInfoArbol> resultado, int? padreId, bool esIzquierdo)
        {
            if (nodo == null)
                return;

            var info = new NodoInfoArbol
            {
                TareaId = nodo.Tarea.Id,
                Titulo = nodo.Tarea.Titulo.Length > 35 ? nodo.Tarea.Titulo.Substring(0, 35) + "..." : nodo.Tarea.Titulo,
                DiasRestantes = nodo.Tarea.DiasRestantes,
                Prioridad = nodo.Tarea.Prioridad,
                PadreId = padreId,
                EsIzquierdo = esIzquierdo,
                Altura = nodo.Altura,
                FactorBalance = nodo.ObtenerFactorBalance()
            };

            resultado.Add(info);

            if (nodo.Izquierda != null)
            {
                ObtenerInfoArbolVisualRecursivo(nodo.Izquierda, resultado, nodo.Tarea.Id, true);
            }

            if (nodo.Derecha != null)
            {
                ObtenerInfoArbolVisualRecursivo(nodo.Derecha, resultado, nodo.Tarea.Id, false);
            }
        }

        /// Reconstruye el árbol (útil después de cambios masivos)
        public void Reconstruir(List<Tarea> tareas)
        {
            raiz = null;
            foreach (var tarea in tareas)
            {
                Insertar(tarea);
            }
        }

        /// Limpia el árbol
        public void Limpiar()
        {
            raiz = null;
        }

        /// Obtiene la cantidad de nodos
        public int ObtenerCantidadNodos()
        {
            return ContarNodos(raiz);
        }

        private int ContarNodos(NodoAVL? nodo)
        {
            if (nodo == null)
                return 0;
            return 1 + ContarNodos(nodo.Izquierda) + ContarNodos(nodo.Derecha);
        }

        /// Obtiene la altura del árbol
        public int ObtenerAltura()
        {
            return raiz?.Altura ?? 0;
        }

        /// Realiza rotación a la derecha
        private NodoAVL RotarDerecha(NodoAVL nodo)
        {
            var nuevoRaiz = nodo.Izquierda!;
            nodo.Izquierda = nuevoRaiz.Derecha;
            nuevoRaiz.Derecha = nodo;

            nodo.ActualizarAltura();
            nuevoRaiz.ActualizarAltura();

            return nuevoRaiz;
        }

        /// Realiza rotación a la izquierda
        private NodoAVL RotarIzquierda(NodoAVL nodo)
        {
            var nuevoRaiz = nodo.Derecha!;
            nodo.Derecha = nuevoRaiz.Izquierda;
            nuevoRaiz.Izquierda = nodo;

            nodo.ActualizarAltura();
            nuevoRaiz.ActualizarAltura();

            return nuevoRaiz;
        }

        /// Balancea el árbol después de inserciones o eliminaciones
        private NodoAVL Balancear(NodoAVL nodo)
        {
            int factorBalance = nodo.ObtenerFactorBalance();

            // Caso izquierda-izquierda
            if (factorBalance > 1 && nodo.Izquierda != null && nodo.Izquierda.ObtenerFactorBalance() >= 0)
            {
                return RotarDerecha(nodo);
            }

            // Caso derecha-derecha
            if (factorBalance < -1 && nodo.Derecha != null && nodo.Derecha.ObtenerFactorBalance() <= 0)
            {
                return RotarIzquierda(nodo);
            }

            // Caso izquierda-derecha
            if (factorBalance > 1 && nodo.Izquierda != null && nodo.Izquierda.ObtenerFactorBalance() < 0)
            {
                nodo.Izquierda = RotarIzquierda(nodo.Izquierda);
                return RotarDerecha(nodo);
            }

            // Caso derecha-izquierda
            if (factorBalance < -1 && nodo.Derecha != null && nodo.Derecha.ObtenerFactorBalance() > 0)
            {
                nodo.Derecha = RotarDerecha(nodo.Derecha);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        private NodoAVL ObtenerNodoMinimo(NodoAVL nodo)
        {
            while (nodo.Izquierda != null)
            {
                nodo = nodo.Izquierda;
            }
            return nodo;
        }
    }

    /// Información de un nodo para visualización
    public class NodoInfoArbol
    {
        public int TareaId { get; set; }
        public string Titulo { get; set; } = "";
        public int DiasRestantes { get; set; }
        public string Prioridad { get; set; } = "";
        public int? PadreId { get; set; }
        public bool EsIzquierdo { get; set; }
        public int Altura { get; set; }
        public int FactorBalance { get; set; }
    }
}
