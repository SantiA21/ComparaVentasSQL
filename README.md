# CinetCore

**Plataforma Integral de Gestión, Auditoría, Diagnóstico y Rescate de Ventas para Cinet / Mostaza ERP / GMG ERP**

---

## 📌 Descripción General

**CinetCore** es una aplicación de escritorio corporativa desarrollada en **.NET 8 (Windows Forms)** con arquitectura moderna y optimizada para operar sobre infraestructuras comerciales de franquicias y locales propios. Proporciona a los equipos técnicos, auditores, supervisores y operadores herramientas avanzadas para la comparación de auditorías fiscales, consulta de comprobantes en bases locales y remotas (Linked Servers), rescate de transacciones críticas (**SalvaVentas**), sincronización de precios vía API y administración de usuarios en **MOSTAZA_ERP** y **GMG_ERP**.

---

## 🚀 Arquitectura y Stack Tecnológico

- **Framework Core**: `.NET 8.0-windows` (C# 12, WinForms con diseño UI moderno y modo autocomtenido).
- **Distribución Single-File**: Configurado para publicarse como un único ejecutable auto-contenido (`PublishSingleFile=true`, `SelfContained=true`, x64), sin requerir la pre-instalación manual de los runtimes de .NET en las estaciones de usuario.
- **Acceso a Datos**: 
  - **Dapper + ADO.NET (`System.Data.SqlClient`)**: Consultas SQL optimizadas en rendimiento para bases de datos SQL Server locales y servidores remotos.
  - **Linked Servers Automáticos**: Gestión dinámica de servidores vinculados para consultar puntos de venta (PDV/Cajas) de forma directa desde nodos centrales o Backoffice.
- **Integración API REST**: Client HTTP (`System.Net.Http`, `System.Text.Json`) con soporte para consultas paginadas hacia microservicios y APIs de listas de precios.
- **Procesamiento de Archivos Excel**: **ClosedXML** para lectura de planillas de ventas masivas y exportación estructurada con filtros aplicados.
- **Configuración de Entorno**: Sistema híbrido basado en `dbconfig.ini` (ignorado en control de versiones para proteger credenciales) y recursos embebidos.

---

## 📂 Estructura del Proyecto

```text
CinetCore/
├── DTOs/                  # Objetos de transferencia de datos (Precios, Ventas, Usuarios, etc.)
├── Data/                  # Capa de Acceso a Datos (DataAccess.cs, gestión de conexiones, parsing de INI)
├── Forms/                 # Módulos y pantallas de la interfaz de usuario
│   ├── Inicio/            # Menú principal y barra de estado de la versión
│   ├── Ventas/            # Consultas de ventas puntuales y ventas CAEA (Linked Servers)
│   ├── Salvaventas/       # Módulo avanzado de diagnóstico y rescate/reparación de ventas
│   ├── Precios/           # Visualización paginada y filtrado de listas de precios API
│   ├── Sucursales/        # Consulta de sucursales/locales y alta de sucursal FE
│   ├── Usuarios/          # Gestión y consulta de usuarios MOSTAZA_ERP, GMG_ERP y Backoffice
│   ├── Comunes/           # Formulario de clave de seguridad autorizada (FrmClave)
│   └── ChangeLog/         # Visualización de novedades y notas de versión
├── Infrastructure/        # Servicios de infraestructura y clientes externos
├── Models/                # Entidades de dominio
├── Services/              # Lógica de negocio encapsulada (SalvaVentas, LinkedServers, API Precios)
├── Utils/                 # Utilidades comunes (Alertas, UIHelper, Logger, UpdateChecker, AppInfo)
├── dbconfig.example.ini   # Plantilla de configuración de conexiones y contraseñas SQL
├── build_release.bat      # Script automatizado para compilación y empaquetado Release
├── README.md              # Documentación técnica general del repositorio
└── manual_usuario.txt     # Manual de usuario final (distribuible con el ejecutable)
```

---

## 🛠️ Módulos y Funcionalidades del Sistema

### 1. Auditoría y Comparación de Ventas desde Excel (`FormComparaExcel`)
- Permite importar un archivo Excel conteniendo auditorías de ventas y validar transacción por transacción contra las bases de datos `MOSTAZA_ERP` o `GMG_ERP`.
- Descompone inteligentemente el campo `ID Unico` en sus elementos estructurales: **Sucursal**, **Número de Comprobante** y **Tipo de Comprobante**.
- Verifica coincidencias contables e informa diferencias de importe, CAE, fecha o inexistencia del registro.
- Soporta filtrado en grilla y **exportación a Excel** con ClosedXML conservando los filtros activos.

### 2. Consulta de Venta Puntual (`FormConsultaVenta`)
- Búsqueda en tiempo real por parámetros unívocos (`Sucursal`, `Número`, `Tipo`).
- Devuelve el detalle completo del comprobante: caja emisora, CAE, fecha/hora, local e importe total, indicando explícitamente el estado del registro en SQL Server.

### 3. Ventas con CAEA por Linked Server (`FormLinkedServer`)
- Conecta nodos Backoffice con cajas remotas (PDV) para supervisar comprobantes emitidos con CAEA.
- Verifica y crea automáticamente objetos **Linked Server** en el motor SQL en tiempo de ejecución utilizando la credencial configurada en `dbconfig.ini`.
- Lista cajas, sucursales y nombres de host (equipos) vinculados.

### 4. Modificación Autorizada de Importes (`FormModifImporte`)
- Permite el ajuste contable de comprobantes en tablas core (`VENTAS_T`, `VAL_MOVIMIENTOS`).
- Requiere autenticación mediante **clave de seguridad** (`FrmClave`) y audita los intentos en el sistema de logs.
- Recalcula automáticamente valores tributarios base (`SUBTOTAL`, `TOTAL`, `IVA1`, `NETO1`).

### 5. SalvaVentas (`FormMainSalvaventas` / `FormConexionSalvaventas`)
- Módulo especializado de **diagnóstico, rescate y re-inserción de transacciones**.
- Permite buscar ventas en equipos específicos o por Hostname, inspeccionando si el encabezado existe sin movimientos o viceversa.
- Ofrece capacidades para **insertar registros `VAL_MOVIMIENTOS`** faltantes (EFECTIVO, MERPAGO, HNC) o reinsertar comprobantes completos desde la base del PDV a la base central del local.

### 6. Gestión de Usuarios y Conexión Remota (`FormUsuariosMostazaERP`, `FormUsuariosGmgERP`, `FormConexionRemota`)
- Listados y administración de operadores en `MOSTAZA_ERP`, `GMG_ERP` o servidores **Backoffice remotos**.
- Búsqueda multi-criterio (DNI, Nombre, Apellido, Usuario Cinet) y filtrado por **Categoría** y **Estado**.

### 7. Consulta de Precios vía API REST (`FormPreciosMostaza_ERP`, `FormPreciosGmg_ERP`)
- Conexión al microservicio REST para consulta de listas de precios y artículos.
- Paginación dinámica en cliente y servidor con control optimizado de peticiones (debounce/timer).

### 8. Configuración de Sucursales FE (`FormInsertarSucursalFE` & `FormVerSucursales`)
- Visor integral de sucursales, cajas y locales activos.
- Módulo para alta y asociación de configuración de Facturación Electrónica (FE).

---

## ⚙️ Configuración del Entorno (`dbconfig.ini`)

Para ejecutar la aplicación en un entorno local o de producción, se debe copiar la plantilla `dbconfig.example.ini` como `dbconfig.ini` en el directorio de ejecución de la aplicación:

```ini
; Contraseña utilizada para crear servidores vinculados (Linked Servers)
LinkedServerPassword=PASSWORD_SQL_LINKED_SERVER

[MOSTAZA_ERP]
ConnectionString=Server=172.16.0.34;Database=MOSTAZA_ERP;User Id=sa;Password=PASSWORD_AQUI;

[GMG_ERP]
ConnectionString=Server=172.16.0.34;Database=GMG_ERP;User Id=sa;Password=PASSWORD_AQUI;
```

> [!IMPORTANT]
> El archivo `dbconfig.ini` está excluido en el `.gitignore` por contener credenciales sensibles. Nunca comitear contraseñas reales al repositorio.

---

## 📦 Compilación y Distribución

El proyecto está preparado para generar ejecutables independientes para arquitecturas Windows x64.

### Compilación mediante CLI (dotnet publish)
```powershell
dotnet publish CinetCore.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

### Script Batch Automatizado
También está disponible el script batch en el raíz del repositorio:
```powershell
.\build_release.bat
```

El resultado generado se ubicará en la carpeta de salida `bin\Release\net8.0-windows\win-x64\publish\`, conteniendo el ejecutable principal `CinetCore.exe` listo para su despliegue.

---

## 🔄 Sistema de Actualización Automática (`Updater.exe`)

1. Al iniciar la aplicación, `UpdateChecker` verifica si existe una versión superior consultando el servicio en línea y el archivo de versión (`version.txt`).
2. En caso de detectarse una actualización, se muestra un cuadro de diálogo al usuario, se lanza el proceso externo **`Updater.exe`** y `CinetCore.exe` finaliza su ejecución para permitir el reemplazo del binario.
3. Al terminar, el actualizador reinicia CinetCore generando una bandera local (`updated.flag`).
4. Si `CinetCore.exe` detecta dicha bandera al arrancar, abre automáticamente la ventana de **Novedades / Changelog** para informar los cambios de la versión.

---

## 📋 Sistema de Logging y Diagnóstico

CinetCore integra un motor de registro local que escribe reportes organizados por fecha en la carpeta `Logs` del directorio del ejecutable:

- **`error_YYYY-MM-DD.txt`**: Captura de excepciones no controladas, fallos de conexión SQL o errores de validación.
- **`query_YYYY-MM-DD.txt`**: Registro de auditoría de las consultas y procedimientos ejecutados en base de datos.
- **`info_YYYY-MM-DD.txt`**: Trazas informativas del ciclo de vida de la aplicación, inicios de sesión y actualizaciones.

---

## 🤝 Soporte y Contribución

- Para reportes de errores de usuarios finales, remitir el archivo de la carpeta `Logs/` correspondiente a la fecha del incidente junto al número de versión mostrado en el pie de página de la aplicación.
- Para cambios en el repositorio, mantener la coherencia de estilos de UI mediante `CinetCore.Utils.UIHelper.ApplyModernTheme(this)` y el patrón de acceso a datos centralizado en `DataAccess.cs`.
