================================================================================
                    CINETCORE - PLATAFORMA INTEGRAL DE GESTIÓN
================================================================================

Este proyecto contiene el repositorio oficial de CinetCore, una suite de
escritorio desarrollada en .NET 8 (Windows Forms) que consolida utilidades de
auditoría, control fiscal, rescate de transacciones, consulta de precios y
administración de usuarios sobre sistemas Cinet / Mostaza ERP / GMG ERP.

--------------------------------------------------------------------------------
DOCUMENTACIÓN DISPONIBLE EN EL REPOSITORIO
--------------------------------------------------------------------------------
El repositorio cuenta con dos documentos especializados actualizados:

1. README.md (Documentación Técnica para Desarrolladores y DevOps)
   -----------------------------------------------------------------------------
   - Descripción general de arquitectura de CinetCore (.NET 8 WinForms, C# 12).
   - Patrón de publicación Self-Contained / Single-File (PublishSingleFile=true).
   - Estructura y propósito de las capas:
     * Data/ (Acceso a datos con Dapper + ADO.NET SQL Client, archivos INI).
     * Forms/ (Pantallas y lógica de UI por módulo).
     * Services/ y Utils/ (LinkedServers, SalvaVentas, API Precios, Alertas).
   - Detalle técnico de los 10 módulos operativos:
     1. Importación y Comparación de Ventas desde Excel (ClosedXML).
     2. Consulta de venta puntual por Sucursal / Comprobante / Tipo.
     3. Ventas con CAEA por Linked Servers remotos sobre PDVs / Cajas.
     4. Modificación autorizada de importes contables en SQL (FrmClave).
     5. SalvaVentas (Diagnóstico, re-inserción y agregado de VAL_MOVIMIENTOS).
     6. Ver sucursales y locales activos.
     7. Administración de usuarios en MOSTAZA_ERP, GMG_ERP y Backoffice Remoto.
     8. Configuración e inserción de sucursal en Facturación Electrónica (FE).
     9. Consulta API REST de listas de precios (Paginación + HTTP Client).
     10. Sistema de Novedades y Changelog en línea.
   - Configuración de base de datos y Linked Servers (dbconfig.ini / example).
   - Instrucciones CLI de publicación (dotnet publish) y script build_release.bat.
   - Flujo de actualización automática vía Updater.exe y flags (version.txt).

2. manual_usuario.txt (Manual para el Usuario Final / Operadores / Soporte)
   -----------------------------------------------------------------------------
   - Guía en formato texto ASCII limpio distribuible junto a CinetCore.exe.
   - Requisitos de instalación (Windows 10/11 64-bits, permisos de red SQL).
   - Instrucciones paso a paso para usar cada una de las 10 herramientas del menú.
   - Procedimiento ante actualizaciones automáticas de la aplicación.
   - Sección de Resolución de Problemas (Troubleshooting), Preguntas Frecuentes
     y localización de archivos de log diarios (Logs/error_*.txt, query_*.txt).
   - Indicaciones sobre qué información recolectar al solicitar soporte técnico.

================================================================================
PARA MÁS INFORMACIÓN:
- Consulte "README.md" si necesita ver el detalle de arquitectura y código.
- Consulte "manual_usuario.txt" para la guía práctica de operación de CinetCore.
================================================================================
