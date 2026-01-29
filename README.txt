### ComparaVentasExcel

## ¿Qué es ComparaVentasExcel?

**ComparaVentasExcel** es una aplicación de escritorio para Windows que te permite:

- **Comparar ventas** entre un archivo Excel y las bases de datos `MOSTAZA_ERP` / `GMG_ERP`.
- **Consultar una venta puntual** directamente en la base de datos.
- **Ver sucursales y locales** activos recientemente.
- **Consultar ventas con CAEA** desde equipos remotos usando linked servers.
- **Modificar importes de comprobantes** (solo usuarios autorizados).
- **Consultar usuarios** de:
  - `MOSTAZA_ERP`
  - `GMG_ERP`
  - Backoffice remoto
- **Ver las novedades (changelog)** de cada versión.
- **Actualizarse automáticamente** cuando hay una nueva versión disponible.

---

## Requisitos

- **Sistema operativo**: Windows 10 o superior (64 bits).
- **Permisos**:
  - Acceso de red a los servidores SQL correspondientes.
  - Permiso para ejecutar aplicaciones .exe.
- **Internet** (opcional pero recomendado):
  - Para actualizar la aplicación.
  - Para ver las novedades (changelog).

> No necesitás instalar .NET manualmente si usás el ejecutable que se distribuye (es self‑contained).

---

## Instalación y actualización

Normalmente vas a recibir una carpeta que contiene:

- `ComparaVentasExcel.exe`
- `Updater.exe` (actualizador)

### Instalación

1. Copiá la carpeta completa a la PC donde se va a usar.
2. No borres ni muevas los archivos que vienen junto al `.exe`.
3. Ejecutá `ComparaVentasExcel.exe` con doble clic.

### Actualización automática

Cuando inicias la aplicación:

1. Se verifica en internet si existe una **versión nueva**.
2. Si hay una nueva versión:
   - Se muestra un mensaje indicando que se va a actualizar.
   - Se ejecuta `Updater.exe` y la aplicación se cierra.
   - Una vez finalizada la actualización, al volver a abrir la app se muestran las **novedades** de la nueva versión.

Como usuario, solo tenés que aceptar el mensaje y dejar que el proceso termine.

---

## Pantalla principal

Al abrir la aplicación, aparece la ventana **Inicio** con un menú superior.  
Desde allí podés acceder a:

- **Importar Excel**: comparar ventas desde un archivo Excel.
- **Consultar venta**: buscar una venta específica en la base de datos.
- **Ventas con CAEA (Linked Server)**: consultar ventas CAEA en equipos remotos.
- **Modificar importe**: cambiar importes de un comprobante (requiere clave).
- **Ver sucursales**: listar sucursales y locales.
- **Usuarios MOSTAZA ERP**.
- **Usuarios GMG ERP**.
- **Usuarios Backoffice remoto**.
- **Novedades / Changelog**.

En la parte inferior se muestra siempre la **versión actual** de la aplicación.

---

## Funcionalidades principales

### 1. Comparar ventas desde Excel

Menú: `Importar Excel` (formulario de comparación).

Permite cargar un archivo Excel con ventas y verificar si existen en la base.

**Pasos:**

1. Elegí la **base de datos** en el combo (por ejemplo `MOSTAZA_ERP` o `GMG_ERP`).
2. Hacé clic en **Examinar** y seleccioná el archivo Excel.
3. Hacé clic en **Procesar**.

La aplicación:

- Lee el Excel (campos como `ID Unico`, importe, fecha, CAE, etc.).
- Desarma el `ID Unico` en:
  - Sucursal
  - Número de comprobante
  - Tipo de comprobante
- Consulta en la base si esa venta existe.

El resultado se muestra en una grilla con columnas como:

- ID Unico
- Local
- Sucursal
- Comprobante
- Tipo
- Importe
- Fecha
- CAE
- Resultado (si existe o no en la base)

**Filtros disponibles:**

- Por **Sucursal**.
- Por **Local**.
- Solo **existentes**.
- Solo **no existentes**.

**Exportar resultados:**

Podés exportar el resultado (incluyendo los filtros aplicados) a un nuevo archivo Excel usando el botón **Exportar**.

---

### 2. Consultar una venta puntual

Menú: `Consultar venta`.

Sirve para buscar una venta específica en la base de datos.

**Pasos:**

1. Seleccioná la base de datos.
2. Ingresá:
   - **Sucursal** (solo números).
   - **Número de comprobante**.
   - **Tipo de comprobante** (elegido en el combo).
3. Hacé clic en **Consultar**.

La aplicación valida los datos y luego muestra, si corresponde:

- Local.
- Fecha y hora.
- Sucursal.
- Número de comprobante.
- Tipo de comprobante.
- CAE.
- Caja.
- Importe total.

Si la venta existe, aparece un mensaje **“✅ Venta encontrada en la base de datos.”**  
Si no existe, aparece **“❌ No existe la venta en la base de datos.”**.

---

### 3. Ventas con CAEA (Linked Server)

Menú: `Ventas con CAEA`.

Permite consultar ventas con CAEA en equipos remotos (cajas) a través de linked servers.

**Pasos básicos:**

1. Ingresá la IP o nombre del **servidor madre** (BACKOFFICE).
2. Hacé clic en **Cargar equipos** para obtener la lista de equipos/cajas.
3. Seleccioná un equipo y consultá las ventas con CAEA.

La aplicación:

- Verifica si existe el linked server para ese equipo.
- Lo crea si no existe.
- Detecta la base de datos PDV adecuada.
- Consulta las ventas con CAEA y las muestra en una grilla.

También desde esta pantalla podés abrir una vista de **Sucursales / Equipos** que lista cajas, sucursales y hostnames relacionados.

---

### 4. Modificar importe de un comprobante

Menú: `Modificar importe`.

> **ATENCIÓN**: Esta función modifica datos contables. Debe usarse solo por usuarios autorizados.

Permite ajustar el importe total de un comprobante y recalcular sus componentes (IVA, neto, etc.).

**Pasos:**

1. Elegí la base de datos.
2. Ingresá:
   - Sucursal.
   - Número de comprobante.
   - Tipo de comprobante.
   - Importe nuevo.
3. Al confirmar, la aplicación te pedirá una **clave de seguridad**.
4. Si la clave es correcta, se aplican los cambios y se muestra un mensaje de éxito.

Internamente se actualizan tablas como:

- `VENTAS_T` (conceptos SUBTOTAL, TOTAL, IVA1, NETO1).
- `VAL_MOVIMIENTOS` (importe total).

Si la clave es incorrecta o se cancela, la operación no se realiza y se registra un mensaje en los logs.

---

### 5. Ver sucursales y locales

Menú: `Ver sucursales`.

Muestra las sucursales y locales recientes según la base seleccionada.

Funciones:

- Ver en una grilla:
  - Caja.
  - Sucursal.
  - Local.
- Filtrar por texto (sucursal / local).
- Cambiar de base de datos y recargar los datos.

En algunos escenarios (modo remoto) se abre una ventana que se conecta al servidor madre para obtener la lista de equipos y sucursales.

---

### 6. Usuarios de MOSTAZA ERP y GMG ERP

Menú: `Usuarios MOSTAZA ERP` / `Usuarios GMG ERP`.

Muestran un listado de usuarios con datos como:

- DNI.
- Nombre.
- Apellido.
- Nombre Cinet.
- Categoría.
- Estado.

Herramientas de filtrado:

- Búsqueda por texto (DNI, nombre, apellido, nombre Cinet).
- Filtro por **Categoría**.
- Filtro por **Estado**.

Podés refrescar los datos en cualquier momento con el botón **Refrescar**.

---

### 7. Usuarios Backoffice remoto

Menú: `Usuarios Backoffice` (conexión remota).

Esta opción permite consultar los usuarios de un Backoffice remoto ingresando la IP del servidor.

**Pasos:**

1. Ingresá la IP del servidor Backoffice.
2. Hacé clic en **Conectar**.
3. Si la conexión y la consulta son exitosas, se abre una ventana con el listado de usuarios.

En esa ventana también tenés:

- Búsqueda por texto.
- Filtro por Categoría.
- Filtro por Estado.

Si algo falla (conexión, permisos, etc.), se muestra un mensaje de error y el detalle se guarda en los logs.

---

### 8. Novedades / Changelog

Menú: `Novedades` o similar.

Abre una ventana que:

- Descarga el archivo de novedades desde internet.
- Muestra la lista de cambios por versión.

Si no hay conexión, se muestra un mensaje indicando que no se pudo cargar el changelog.

---

## Archivos y carpeta de logs

En la carpeta donde está `ComparaVentasExcel.exe` se pueden generar:

- **`Logs`** (carpeta):
  - `error_YYYY-MM-DD.txt`: errores de la aplicación.
  - `query_YYYY-MM-DD.txt`: consultas SQL ejecutadas.
  - `info_YYYY-MM-DD.txt`: mensajes informativos.
- **`version.txt`**:
  - Contiene la versión actual instalada (por ejemplo `1.12.0`).

Estos archivos son útiles para soporte técnico si algo falla.

---

## Buenas prácticas para el usuario

- No compartir la carpeta de la aplicación con personas no autorizadas.
- No modificar manualmente archivos de la carpeta (especialmente logs o archivos de configuración), salvo indicación del soporte técnico.
- En caso de error frecuente:
  - Anotar qué estabas haciendo y qué ventana usabas.
  - Ver la fecha y hora aproximada.
  - Enviar al equipo técnico los archivos de la carpeta `Logs` de ese día.

---

## Soporte

Cuando necesites ayuda técnica, es útil enviar:

- La **versión** de la aplicación (visible en la parte inferior de las ventanas).
- Una breve descripción de lo que estabas haciendo.
- Captura de pantalla del error (si existe).
- Los archivos de la carpeta `Logs` correspondientes al día del problema.

Con esa información el diagnóstico es mucho más rápido y preciso.

