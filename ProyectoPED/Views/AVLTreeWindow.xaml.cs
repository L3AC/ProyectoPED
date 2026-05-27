using System;
using System.Windows;
using ProyectoPED.DataStructures;

namespace ProyectoPED.Views
{
    public partial class AVLTreeWindow : Window
    {
        private Func<(List<NodoInfoArbol> nodos, int total, int altura)>? obtenerDatos;

        public AVLTreeWindow()
        {
            InitializeComponent();
            AVLTreeControl.CerrarRequested += (s, e) => Close();
            AVLTreeControl.ActualizarRequested += (s, e) => Actualizar();
        }

        public void CargarDatos(List<NodoInfoArbol> nodos, int totalNodos, int altura, int factorBalance)
        {
            AVLTreeControl.CargarDatos(nodos, totalNodos, altura, factorBalance);
        }

        public void SetObtenerDatos(Func<(List<NodoInfoArbol> nodos, int total, int altura)> callback)
        {
            obtenerDatos = callback;
        }

        private void Actualizar()
        {
            if (obtenerDatos != null)
            {
                var datos = obtenerDatos();
                AVLTreeControl.CargarDatos(datos.nodos, datos.total, datos.altura, 0);
            }
        }
    }
}