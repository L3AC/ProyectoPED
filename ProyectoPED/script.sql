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
    estado ENUM('Pendiente', 'Completada', 'Vencida') DEFAULT 'Pendiente',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);
-- ============================================
-- DATOS DE EJEMPLO / SEED DATA
-- ============================================

-- Insertar usuario de prueba (contraseña hasheada con SHA256: password123)
INSERT INTO usuarios (carne, nombre, password) VALUES 
('ML250903', 'María López', '75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8='),
('JP250773', 'Juan Pérez', '75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=');

-- Insertar tareas de ejemplo para usuario 1 (María López)
INSERT INTO tareas (usuario_id, titulo, descripcion, fecha_limite, prioridad, estado) VALUES
(1, 'Proyecto Final de Base de Datos', 'Desarrollar el sistema de gestión de tareas con AVL', '2026-05-28', 'Alta', 'Pendiente'),
(1, 'Ensayo de Investigación', 'Escribir ensayo sobre estructuras de datos', '2026-05-27', 'Media', 'Pendiente'),
(1, 'Presentación de Matemáticas', 'Preparar slides para exposición del tema 5', '2026-05-26', 'Alta', 'Pendiente'),
(1, 'Laboratorio de Física', 'Realizar informe del experimento de óptica', '2026-05-29', 'Baja', 'Pendiente'),
(1, 'Tarea de Programación', 'Ejercicios de recursion en C#', '2026-05-20', 'Media', 'Completada'),
(1, 'Resumen de Historia', 'Resumen del capítulo 4 de historia universal', '2026-05-26', 'Baja', 'Pendiente'),
(1, 'Examen de Inglés', 'Estudiar vocabulario y gramática avanzada', '2026-05-28', 'Media', 'Pendiente'),
(1, 'Práctica de Química', 'Experimento de reacciones ácido-base', '2026-05-30', 'Baja', 'Pendiente');

