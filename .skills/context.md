# TaskUni - Contexto del Sistema

## Descripción General del Proyecto

**TaskUni** es un sistema de gestión de tareas académicas diseñado para estudiantes universitarios. Su objetivo principal es ayudar a organizar múltiples tareas (trabajos, proyectos, exámenes, entregas) de forma inteligente, utilizando un **Árbol AVL** para ordenar automáticamente las tareas según su urgencia real (días restantes).

El sistema permite registrar tareas, simular el paso del tiempo, visualizar la estructura del árbol AVL, generar reportes y deshacer acciones.

---

## Situación Problemática

Los estudiantes universitarios enfrentan diariamente el desafío de gestionar múltiples tareas académicas con fechas límite diferentes y niveles de urgencia variables. Esta situación suele manejarse de forma desorganizada (papel, mensajería o memoria).

**TaskUni** resuelve esta brecha ofreciendo una herramienta que no solo almacena tareas, sino que las organiza automáticamente por urgencia real mediante un Árbol AVL y permite simular el avance del tiempo para anticipar problemas.

---

## Flujo Completo del Sistema

### Flujo de Usuario (Frontend)

1. **Login** → Pantalla de inicio de sesión
2. **Dashboard Principal** (pantalla central):
   - Crear nueva tarea (modal)
   - Simular 1 Día (acción directa)
   - Ver Reportes (modal)
   - Ver Estructura AVL (modal)
   - Completar tarea (desde la tabla)
   - Deshacer última acción
3. **Cerrar Sesión** → Volver al Login

### Flujo Lógico (Backend)

- Al iniciar: Conexión MySQL + Login
- Después del login: Cargar tareas del usuario → Insertar en Árbol AVL (en memoria) → Mostrar tabla ordenada
- Cada acción (crear, completar, simular, deshacer) actualiza:
  - Árbol AVL (en memoria)
  - Base de datos MySQL
  - Interfaz en tiempo real

---

## Diseños de Pantallas

### Pantalla 1: Login
- Header con logo "TaskUni"
- Card central:
  - Título: "Iniciar Sesión"
  - Campo: Carné
  - Campo: Contraseña
  - Botón: "Iniciar Sesión"
  - Enlace: "¿No tienes cuenta? Registrarse"
  - Mensaje de error (visible solo en fallo)

### Pantalla 2: Dashboard Principal
**Barra superior:**
- Logo TaskUni (izquierda)
- "Bienvenido [Nombre] ([Carné])" (centro)
- Botón "Cerrar Sesión" (derecha)

**Menú lateral izquierdo:**
- Dashboard
- Nueva Tarea
- Simular 1 Día
- Ver Estructura AVL
- Reportes
- Deshacer acción

**Área central:**
- Título: "Panel de Tareas"
- Fecha actual
- Tabla: ID | Título | Fecha Límite | Días Restantes | Prioridad | Estado | Acciones
- Estados: Pendiente, Completada, Vencida
- Colores de "Días Restantes":
  - Verde: ≥ 8 días
  - Naranja: 4 a 7 días
  - Rojo: ≤ 3 días
  - Morado: vencido

### Pantalla 3: Nueva Tarea (Modal)
- Título: "Nueva Tarea Académica"
- Campos:
  - Título
  - Descripción (textarea)
  - Fecha Límite (date picker)
  - Prioridad (Alta / Media / Baja)
- Botones: Guardar Tarea | Cancelar

### Pantalla 4: Reportes (Modal)
- Estadísticas generales
- Recomendaciones automáticas
- Cuellos de botella
- Análisis de carga de trabajo
- Botón: Cerrar

---

## Estructuras de Datos Principales

### 1. Árbol AVL (Estructura Central)
- **Propósito**: Mantener las tareas ordenadas por urgencia real.
- **Clave de ordenamiento**: `- (días restantes)` (las más urgentes primero), con `Id` como desempate.
- **Ventajas**: Operaciones O(log n) garantizadas, autobalanceo.
- **Métodos**: Insertar, Eliminar, RecorridoInOrder, VisualizarEstructura.

### 2. Pila (Stack)
- **Propósito**: Implementar funcionalidad **Deshacer**.
- Guarda el estado completo de la tarea antes de cada acción.
- LIFO: Permite revertir la última acción.

### 3. EstadoTarea (Enum)
- **Valores**: Pendiente, Completada, Vencida
- Las tareas Pendientes con fecha pasada se marcan automáticamente como Vencidas al cargar.

---

## Tecnologías y Herramientas

- **Lenguaje**: C# (.NET)
- **IDE**: Visual Studio 2022
- **Interfaz**: Windows Presentation Foundation (WPF)
- **Base de Datos**: MySQL + MySqlConnector
- **Framework UI**: MaterialDesignThemes

---

## Resultados y Beneficios para el Usuario

- Tabla siempre ordenada por urgencia real (gracias al AVL).
- Colores visuales claros según criticidad.
- Simulación del tiempo para anticipar sobrecargas.
- Reportes estadísticos y recomendaciones.
- Función Deshacer segura.
- Persistencia de datos por estudiante (vinculado al Carné).

---

**Proyecto creado para resolver una necesidad real del estudiante universitario:** organización inteligente y visualización de prioridades dinámicas.
