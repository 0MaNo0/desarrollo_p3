USE master;
GO

DROP DATABASE IF EXISTS prueba_2;
GO

CREATE DATABASE prueba_2;
GO

USE prueba_2;
GO

CREATE TABLE Alumno(
	codigo INT PRIMARY KEY,
	nombres NVARCHAR(100),
	carrera NVARCHAR(100),
	domicilio NVARCHAR(200)
);
GO
--INSERTAR UN ALUMNO
--INSERT INTO Alumno (codigo, nombres, carrera, domicilio) VALUES (1, 'Juan Quispe Ochoa','Ingenieria Informática','Perayoq')

--select * from Alumno

CREATE PROCEDURE InsertarAlumno
	@codigo INT,
	@nombres NVARCHAR(100),
	@carrera NVARCHAR(100),
	@domicilio NVARCHAR(200)
AS
BEGIN 
	INSERT INTO Alumno (codigo, nombres, carrera, domicilio) VALUES (@codigo, @nombres, @carrera, @domicilio);
END;
GO

--EXEC InsertarAlumno @codigo=1,@nombres='Jesus Ochoa Ramirez',@carrera='Medicina',@domicilio='AV. LA CULTURA';
--GO

CREATE PROCEDURE ObtenerAlumnos
AS 
BEGIN
	SELECT * FROM Alumno;
END;
GO

--EXEC ObtenerAlumnos
--GO

CREATE PROCEDURE ActualizarAlumno 
	@codigo INT,
	@nombres NVARCHAR(100),
	@carrera NVARCHAR(100),
	@domicilio NVARCHAR(200)
AS
BEGIN
	UPDATE Alumno 
	SET nombres = @nombres, carrera =@carrera, domicilio = @domicilio 
	WHERE codigo =@codigo;
END;
GO

--EXEC ActualizarAlumno @codigo=1, @nombres='Rosa',@carrera='Saldivar', @domicilio='Guerra';
--GO
--EXEC ObtenerAlumnos
--GO

CREATE PROCEDURE EliminarAlumno
	@codigo INT
AS
BEGIN
	DELETE FROM Alumno
	WHERE codigo = @codigo;
END;
GO

--EXEC EliminarAlumno @codigo=1;
--GO
--EXEC ObtenerAlumnos
--GO
-----------------------------------------------------------------
-- Tabla Miembros
CREATE TABLE Miembros (
    id_miembro INT IDENTITY(1,1) PRIMARY KEY,
    dni VARCHAR(8) NOT NULL,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    direccion VARCHAR(255),
    email VARCHAR(100),
    telefono VARCHAR(15),
    universidad VARCHAR(100),
    titulo VARCHAR(100),
    fecha_graduacion DATE,
    foto_url VARCHAR(255),
    estado VARCHAR(20) NOT NULL,
    fecha_registro DATE
);
GO

-- Tabla Documentos
CREATE TABLE Documentos (
    id_documento INT IDENTITY(1,1) PRIMARY KEY,
    id_miembro INT NOT NULL,
    tipo_documento VARCHAR(50) NOT NULL,
    documento_url VARCHAR(255) NOT NULL,
    fecha_carga TIMESTAMP,
    estado VARCHAR(20) NOT NULL,
    FOREIGN KEY (id_miembro) REFERENCES Miembros(id_miembro)
);
GO

-- Tabla Certificaciones
CREATE TABLE Certificaciones (
    id_certificacion INT IDENTITY(1,1) PRIMARY KEY,
    id_documento INT NOT NULL,
    tipo_certificacion VARCHAR(50) NOT NULL,
    fecha_emision DATE NOT NULL,
    fecha_expiracion DATE,
    certificado_url VARCHAR(255),
    estado VARCHAR(20) NOT NULL,
    FOREIGN KEY (id_documento) REFERENCES Documentos(id_documento)
);
GO

-- Tabla Pagos
CREATE TABLE Pagos (
    id_pago INT IDENTITY(1,1) PRIMARY KEY,
    id_miembro INT NOT NULL,
    monto DECIMAL(10, 2) NOT NULL,
    fecha_pago DATE NOT NULL,
    tipo_pago VARCHAR(50) NOT NULL,
    comprobante_url VARCHAR(255),
    estado VARCHAR(20) NOT NULL,
    FOREIGN KEY (id_miembro) REFERENCES Miembros(id_miembro)
);
GO

-- Tabla Renovaciones
CREATE TABLE Renovaciones (
    id_renovacion INT IDENTITY(1,1) PRIMARY KEY,
    id_miembro INT NOT NULL,
    id_pago INT NOT NULL,
    id_documento INT NOT NULL,
    fecha_solicitud DATE NOT NULL,
    fecha_aprobacion DATE,
    estado VARCHAR(20) NOT NULL,
    FOREIGN KEY (id_miembro) REFERENCES Miembros(id_miembro),
    FOREIGN KEY (id_pago) REFERENCES Pagos(id_pago),
    FOREIGN KEY (id_documento) REFERENCES Documentos(id_documento)
);
GO

-- Tabla Usuarios
CREATE TABLE Usuarios (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    id_miembro INT,
    username VARCHAR(50) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    rol VARCHAR(20) NOT NULL,
    fecha_creacion DATE,
    ultimo_acceso TIMESTAMP,
    FOREIGN KEY (id_miembro) REFERENCES Miembros(id_miembro)
);
GO

-- Procedimiento para obtener todos los miembros
CREATE PROCEDURE ObtenerMiembros
AS
BEGIN
    SELECT * FROM Miembros
END
GO

-- Procedimiento para insertar un nuevo miembro
CREATE PROCEDURE InsertarMiembro
    @dni VARCHAR(8),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @fecha_nacimiento DATE,
    @direccion VARCHAR(255),
    @email VARCHAR(100),
    @telefono VARCHAR(15),
    @universidad VARCHAR(100),
    @titulo VARCHAR(100),
    @fecha_graduacion DATE,
    @foto_url VARCHAR(255),
    @estado VARCHAR(20),
    @fecha_registro DATE
AS
BEGIN
    INSERT INTO Miembros (dni, nombres, apellidos, fecha_nacimiento, direccion, email, telefono, universidad, titulo, fecha_graduacion, foto_url, estado, fecha_registro)
    VALUES (@dni, @nombres, @apellidos, @fecha_nacimiento, @direccion, @email, @telefono, @universidad, @titulo, @fecha_graduacion, @foto_url, @estado, @fecha_registro)
END
GO

-- Procedimiento para actualizar un miembro
CREATE PROCEDURE ActualizarMiembro
    @id_miembro INT,
    @dni VARCHAR(8),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @fecha_nacimiento DATE,
    @direccion VARCHAR(255),
    @email VARCHAR(100),
    @telefono VARCHAR(15),
    @universidad VARCHAR(100),
    @titulo VARCHAR(100),
    @fecha_graduacion DATE,
    @foto_url VARCHAR(255),
    @estado VARCHAR(20),
    @fecha_registro DATE
AS
BEGIN
    UPDATE Miembros
    SET dni = @dni,
        nombres = @nombres,
        apellidos = @apellidos,
        fecha_nacimiento = @fecha_nacimiento,
        direccion = @direccion,
        email = @email,
        telefono = @telefono,
        universidad = @universidad,
        titulo = @titulo,
        fecha_graduacion = @fecha_graduacion,
        foto_url = @foto_url,
        estado = @estado,
        fecha_registro = @fecha_registro
    WHERE id_miembro = @id_miembro
END
GO

-- Procedimiento para eliminar un miembro
CREATE PROCEDURE EliminarMiembro
    @id_miembro INT
AS
BEGIN
    DELETE FROM Miembros
    WHERE id_miembro = @id_miembro
END
GO
-- Procedimiento para obtener todos los documentos
CREATE PROCEDURE ObtenerDocumentos
AS
BEGIN
    SELECT * FROM Documentos
END
GO

-- Procedimiento para insertar un nuevo documento
CREATE PROCEDURE InsertarDocumento
    @id_miembro INT,
    @tipo_documento VARCHAR(50),
    @documento_url VARCHAR(255),
    @fecha_carga TIMESTAMP,
    @estado VARCHAR(20)
AS
BEGIN
    INSERT INTO Documentos (id_miembro, tipo_documento, documento_url, fecha_carga, estado)
    VALUES (@id_miembro, @tipo_documento, @documento_url, @fecha_carga, @estado)
END
GO

-- Procedimiento para actualizar un documento
CREATE PROCEDURE ActualizarDocumento
    @id_documento INT,
    @id_miembro INT,
    @tipo_documento VARCHAR(50),
    @documento_url VARCHAR(255),
    @fecha_carga TIMESTAMP,
    @estado VARCHAR(20)
AS
BEGIN
    UPDATE Documentos
    SET id_miembro = @id_miembro,
        tipo_documento = @tipo_documento,
        documento_url = @documento_url,
        fecha_carga = @fecha_carga,
        estado = @estado
    WHERE id_documento = @id_documento
END
GO

-- Procedimiento para eliminar un documento
CREATE PROCEDURE EliminarDocumento
    @id_documento INT
AS
BEGIN
    DELETE FROM Documentos
    WHERE id_documento = @id_documento
END
GO
-- Procedimiento para obtener todas las certificaciones
CREATE PROCEDURE ObtenerCertificaciones
AS
BEGIN
    SELECT * FROM Certificaciones
END
GO

-- Procedimiento para insertar una nueva certificación
CREATE PROCEDURE InsertarCertificacion
    @id_documento INT,
    @tipo_certificacion VARCHAR(50),
    @fecha_emision DATE,
    @fecha_expiracion DATE,
    @certificado_url VARCHAR(255),
    @estado VARCHAR(20)
AS
BEGIN
    INSERT INTO Certificaciones (id_documento, tipo_certificacion, fecha_emision, fecha_expiracion, certificado_url, estado)
    VALUES (@id_documento, @tipo_certificacion, @fecha_emision, @fecha_expiracion, @certificado_url, @estado)
END
GO

-- Procedimiento para actualizar una certificación
CREATE PROCEDURE ActualizarCertificacion
    @id_certificacion INT,
    @id_documento INT,
    @tipo_certificacion VARCHAR(50),
    @fecha_emision DATE,
    @fecha_expiracion DATE,
    @certificado_url VARCHAR(255),
    @estado VARCHAR(20)
AS
BEGIN
    UPDATE Certificaciones
    SET id_documento = @id_documento,
        tipo_certificacion = @tipo_certificacion,
        fecha_emision = @fecha_emision,
        fecha_expiracion = @fecha_expiracion,
        certificado_url = @certificado_url,
        estado = @estado
    WHERE id_certificacion = @id_certificacion
END
GO

-- Procedimiento para eliminar una certificación
CREATE PROCEDURE EliminarCertificacion
    @id_certificacion INT
AS
BEGIN
    DELETE FROM Certificaciones
    WHERE id_certificacion = @id_certificacion
END
GO
-- Procedimiento para obtener todos los pagos
CREATE PROCEDURE ObtenerPagos
AS
BEGIN
    SELECT * FROM Pagos
END
GO

-- Procedimiento para insertar un nuevo pago
CREATE PROCEDURE InsertarPago
    @id_miembro INT,
    @monto DECIMAL(10, 2),
    @fecha_pago DATE,
    @tipo_pago VARCHAR(50),
    @comprobante_url VARCHAR(255),
    @estado VARCHAR(20)
AS
BEGIN
    INSERT INTO Pagos (id_miembro, monto, fecha_pago, tipo_pago, comprobante_url, estado)
    VALUES (@id_miembro, @monto, @fecha_pago, @tipo_pago, @comprobante_url, @estado)
END
GO

-- Procedimiento para actualizar un pago
CREATE PROCEDURE ActualizarPago
    @id_pago INT,
    @id_miembro INT,
    @monto DECIMAL(10, 2),
    @fecha_pago DATE,
    @tipo_pago VARCHAR(50),
    @comprobante_url VARCHAR(255),
    @estado VARCHAR(20)
AS
BEGIN
    UPDATE Pagos
    SET id_miembro = @id_miembro,
        monto = @monto,
        fecha_pago = @fecha_pago,
        tipo_pago = @tipo_pago,
        comprobante_url = @comprobante_url,
        estado = @estado
    WHERE id_pago = @id_pago
END
GO

-- Procedimiento para eliminar un pago
CREATE PROCEDURE EliminarPago
    @id_pago INT
AS
BEGIN
    DELETE FROM Pagos
    WHERE id_pago = @id_pago
END
GO
-- Procedimiento para obtener todas las renovaciones
CREATE PROCEDURE ObtenerRenovaciones
AS
BEGIN
    SELECT * FROM Renovaciones
END
GO

-- Procedimiento para insertar una nueva renovación
CREATE PROCEDURE InsertarRenovacion
    @id_miembro INT,
    @id_pago INT,
    @id_documento INT,
    @fecha_solicitud DATE,
    @fecha_aprobacion DATE,
    @estado VARCHAR(20)
AS
BEGIN
    INSERT INTO Renovaciones (id_miembro, id_pago, id_documento, fecha_solicitud, fecha_aprobacion, estado)
    VALUES (@id_miembro, @id_pago, @id_documento, @fecha_solicitud, @fecha_aprobacion, @estado)
END
GO

-- Procedimiento para actualizar una renovación
CREATE PROCEDURE ActualizarRenovacion
    @id_renovacion INT,
    @id_miembro INT,
    @id_pago INT,
    @id_documento INT,
    @fecha_solicitud DATE,
    @fecha_aprobacion DATE,
    @estado VARCHAR(20)
AS
BEGIN
    UPDATE Renovaciones
    SET id_miembro = @id_miembro,
        id_pago = @id_pago,
        id_documento = @id_documento,
        fecha_solicitud = @fecha_solicitud,
        fecha_aprobacion = @fecha_aprobacion,
        estado = @estado
    WHERE id_renovacion = @id_renovacion
END
GO

-- Procedimiento para eliminar una renovación
CREATE PROCEDURE EliminarRenovacion
    @id_renovacion INT
AS
BEGIN
    DELETE FROM Renovaciones
    WHERE id_renovacion = @id_renovacion
END
GO
-- Procedimiento para obtener todos los usuarios
CREATE PROCEDURE ObtenerUsuarios
AS
BEGIN
    SELECT * FROM Usuarios
END
GO

-- Procedimiento para insertar un nuevo usuario
CREATE PROCEDURE InsertarUsuario
    @id_miembro INT,
    @username VARCHAR(50),
    @password_hash VARCHAR(255),
    @rol VARCHAR(20),
    @fecha_creacion DATE,
    @ultimo_acceso TIMESTAMP
AS
BEGIN
    INSERT INTO Usuarios (id_miembro, username, password_hash, rol, fecha_creacion, ultimo_acceso)
    VALUES (@id_miembro, @username, @password_hash, @rol, @fecha_creacion, @ultimo_acceso)
END
GO

-- Procedimiento para actualizar un usuario
CREATE PROCEDURE ActualizarUsuario
    @id_usuario INT,
    @id_miembro INT,
    @username VARCHAR(50),
    @password_hash VARCHAR(255),
    @rol VARCHAR(20),
    @fecha_creacion DATE,
    @ultimo_acceso TIMESTAMP
AS
BEGIN
    UPDATE Usuarios
    SET id_miembro = @id_miembro,
        username = @username,
        password_hash = @password_hash,
        rol = @rol,
        fecha_creacion = @fecha_creacion,
        ultimo_acceso = @ultimo_acceso
    WHERE id_usuario = @id_usuario
END
GO

-- Procedimiento para eliminar un usuario
CREATE PROCEDURE EliminarUsuario
    @id_usuario INT
AS
BEGIN
    DELETE FROM Usuarios
    WHERE id_usuario = @id_usuario
END
GO
