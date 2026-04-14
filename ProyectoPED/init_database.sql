DROP DATABASE IF EXISTS taskuni_db;
CREATE DATABASE IF NOT EXISTS taskuni_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_spanish_ci;
USE taskuni_db;

-- 1. Usuarios (login)
CREATE TABLE IF NOT EXISTS usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    carne VARCHAR(20) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    password VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 2. Tareas
CREATE TABLE IF NOT EXISTS tareas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT NOT NULL,
    titulo VARCHAR(150) NOT NULL,
    descripcion TEXT, 
    fecha_limite DATE NOT NULL,
    prioridad ENUM('Alta', 'Media', 'Baja') DEFAULT 'Media',
    estado ENUM('Pendiente', 'Completada') DEFAULT 'Pendiente',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);

-- 3. Subtareas
CREATE TABLE IF NOT EXISTS subtareas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tarea_id INT NOT NULL,
    descripcion VARCHAR(200) NOT NULL,
    completada BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (tarea_id) REFERENCES tareas(id) ON DELETE CASCADE
);

-- 4. Dependencias
CREATE TABLE IF NOT EXISTS dependencias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tarea_id INT NOT NULL,
    depende_de INT NOT NULL,
    FOREIGN KEY (tarea_id) REFERENCES tareas(id),
    FOREIGN KEY (depende_de) REFERENCES tareas(id)
);

-- ============================================
-- DATOS DE EJEMPLO / SEED DATA
-- ============================================

-- Insertar usuario de prueba
INSERT INTO usuarios (carne, nombre, password) VALUES 
('ML250903', 'María López', 'password123'),
('JP250773', 'Juan Pérez', 'password123');

-- Insertar tareas de ejemplo para usuario 1 (María López)
INSERT INTO tareas (usuario_id, titulo, descripcion, fecha_limite, prioridad, estado) VALUES
(1, 'Proyecto Final de Base de Datos', 'Desarrollar el sistema de gestión de tareas con AVL', '2026-04-20', 'Alta', 'Pendiente'),
(1, 'Ensayo de Investigación', 'Escribir ensayo sobre estructuras de datos', '2026-04-25', 'Media', 'Pendiente'),
(1, 'Presentación de Matemáticas', 'Preparar slides para exposición del tema 5', '2026-04-18', 'Alta', 'Pendiente'),
(1, 'Laboratorio de Física', 'Realizar informe del experimento de óptica', '2026-05-01', 'Baja', 'Pendiente'),
(1, 'Tarea de Programación', 'Ejercicios de recursion en C#', '2026-04-15', 'Media', 'Completada'),
(1, 'Resumen de Historia', 'Resumen del capítulo 4 de historia universal', '2026-04-22', 'Baja', 'Pendiente'),
(1, 'Examen de Inglés', 'Estudiar vocabulario y gramática avanzada', '2026-04-28', 'Media', 'Pendiente'),
(1, 'Práctica de Química', 'Experimento de reacciones ácido-base', '2026-05-05', 'Baja', 'Pendiente');

-- Insertar subtareas para algunas tareas
INSERT INTO subtareas (tarea_id, descripcion, completada) VALUES
(1, 'Diseñar la base de datos', TRUE),
(1, 'Implementar el árbol AVL', FALSE),
(1, 'Crear la interfaz de usuario', FALSE),
(2, 'Investigación de fuentes', TRUE),
(2, 'Redactar introducción', FALSE),
(3, 'Crear presentación en PowerPoint', FALSE),
(3, 'Preparar discurso', FALSE);

-- Insertar dependencias entre tareas
INSERT INTO dependencias (tarea_id, depende_de) VALUES
(2, 1),
(3, 1);
