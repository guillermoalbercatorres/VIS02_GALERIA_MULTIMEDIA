# Práctica VIS02 - Galería Multimedia y Visor de Archivos (WPF)

## 📌 Documentación de Base
1. **Modelo de contenido de WPF, controles base y controles contenedores:**  
   [WPF Content Model](https://learn.microsoft.com/es-es/dotnet/desktop/wpf/controls/wpf-content-model?view=netframeworkdesktop-4.8)
2. **Diccionarios de recursos: Estilos y Templates de comportamiento en XAML:**  
   [Styles and Templates Overview](https://learn.microsoft.com/es-es/dotnet/desktop/wpf/controls/styles-templates-overview?view=netdesktop-6.0)
3. **Tutorial Galería de imágenes (Referencia Microsoft):**  
   [Image Gallery Overview](https://learn.microsoft.com/es-es/dotnet/desktop/wpf/controls/styles-templates-overview?view=netframeworkdesktop-4.8)
4. **Librería NuGet recomendada para el visor MarkDown (.MD):**  
   [WPF-UI.Markdown NuGet (versión 3.0.5.4 o compatible)](https://www.nuget.org/packages/WPF-UI.Markdown/3.0.5.4)

---

## 🎯 Objetivo de la Tarea
Desarrollar una aplicación de escritorio completa en **WPF (.NET)** que funcione como una **Galería Multimedia y Visor de Documentos** (fotos, audio/vídeo, ficheros de texto y documentos `.md`).

- **Nombre de la solución/proyecto:** `VIS02_ApellidosNombre` (albergará un único proyecto con un nombre enfocado al marketing, p. ej.: *HIPERGALERIA*, *NAVISION*, *SUPER-BROWSER*, etc.).
- La aplicación constará de **dos ventanas principales**:
  1. `NAVEWIN` (Ventana principal de navegación y ajustes).
  2. `DETAILWIN` (Ventana de detalle/visualización del contenido).

---

## 🎨 Gestión de Estilos y Recursos (Requisito Clave)
La arquitectura visual y de estilos debe dividirse de forma estricta en tres niveles:

1. **Recursos de Aplicación (`App.xaml`):**
   - Definición de paletas de color para el **Modo NORMAL** (claro) y **Modo NOCHE** (oscuro): colores `Foreground` y `Background` aplicables a `Grid`, `TextBox` y `Label`.
2. **Recursos a nivel de Ventana (`Window.Resources`):**
   - En `NAVEWIN` y `DETAILWIN`, definir estilos específicos y diferenciados (bordes, fuentes, márgenes...) para los botones de cada ventana, **manteniendo y heredando los colores Foreground/Background** definidos a nivel de aplicación.
3. **Diccionarios de Recursos (`ResourceDictionary`):**
   - Creados expresamente en archivos independientes (`.xaml`) para ser importados desde la ventana de detalle (`DETAILWIN`), con estilos propios para cada tipo de contenido:
     - Elementos Multimedia (Audio / Vídeo).
     - Imágenes.
     - Archivos de texto plano (`.txt`).
     - Archivos MarkDown (`.md`).

---

## 🖥️ Comportamiento de las Ventanas

### 1. Ventana Principal (`NAVEWIN`)
- **Ruta inicial:** Al arrancar la aplicación, navegará por defecto a la carpeta del sistema **"Mis imágenes"** (`Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)`).
- **Selector de carpeta:** Opcionalmente debe incluir un diálogo para que el usuario pueda seleccionar cualquier otra carpeta de su equipo.
- **Explorador:** El contenido de la carpeta se mostrará mediante un control de navegación básico: `ComboBox`, `ListBox` o preferiblemente un **`TreeView`**.
  - Si el elemento seleccionado es una **carpeta**, permitirá navegar e inspeccionar dentro de ella.
  - Si el elemento seleccionado es un **archivo**, el evento de clic o selección cargará el elemento y lo abrirá en la ventana de detalle (`DETAILWIN`).
- **Modo Noche / Normal:** Control o switch para alternar el tema visual entre Modo Normal y Modo Noche.

---

### 2. Ventana de Detalle (`DETAILWIN`)
El contenedor `Grid` de esta ventana adaptará dinámicamente sus controles y recursos en función de la extensión del fichero:

| Tipo de Archivo | Extensiones | Controles Requeridos | Comportamiento / Funcionalidad |
| :--- | :--- | :--- | :--- |
| **Imágenes** | `.jpg`, `.png`, `.gif`, `.jpeg`... | Componente `Image` | Controles de **Zoom In** / **Zoom Out**. |
| **Multimedia** | `.mp3`, `.mp4`, `.wav`, `.avi`... | `MediaElement` (o reproductor adecuado) | Controles mínimos: **Play** / **Pause**. |
| **Texto Plano** | `.txt` | `TextBox` (vista previa editable o lectura) | Botones de selección de **Fuente (Font)** y tamaño **FontSize (+ / -)**. |
| **Markdown** | `.md` | Visor Markdown (`WPF-UI.Markdown`) | Renderizado enriquecido y formateado de documentos Markdown. |
| **Otros Archivos** | *Extensiones conocidas* | Proceso del sistema (`Process.Start`) | Apertura del archivo utilizando la aplicación predeterminada del sistema operativo. |

#### Comportamiento General de `DETAILWIN`:
- **Herencia de Tema:** Debe mantener la visualización Noche / Normal coherente con la ventana principal `NAVEWIN` (consultando el estado a través de un método o propiedad pública de su `Owner`).
- **Cierre controlado:**
  - Debe disponer de un botón de tipo cancelación (`IsCancel=true`) para cerrarla.
  - El cierre mediante <kbd>Alt</kbd> + <kbd>F4</kbd> o el botón `X` de la ventana **no debe cerrar toda la aplicación**, sino únicamente la ventana de detalle actual.
