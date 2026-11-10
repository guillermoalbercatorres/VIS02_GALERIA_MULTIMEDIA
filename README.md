# VIS02 - Galería Multimedia y Visor de Archivos (WPF)

Este repositorio contiene la solución en WPF (.NET 10) para la práctica **VIS02**.

---

## 📁 Estructura del Proyecto

```text
VIS02/
├── .gitignore                      # Exclusiones de Git (bin, obj, .vs, pdfs, temporales)
├── enunciado.md                    # Requisitos detallados y especificaciones de la práctica
├── README.md                       # Tareas estrictamente pendientes por implementar
├── WpfApp1.slnx                    # Archivo de solución de Visual Studio
└── WpfApp1/                        # Proyecto WPF (.NET 10)
    ├── WpfApp1.csproj              # Archivo de proyecto (paquete Markdig)
    ├── App.xaml                    # Recursos globales (Colores Modo Normal / Modo Noche)
    ├── App.xaml.cs                 # Lógica de ciclo de vida de la aplicación
    │
    ├── Views/                      # Vistas / Ventanas XAML
    │   ├── NAVEWIN.xaml            # Ventana Principal (Explorador TreeView, Selección de carpeta)
    │   ├── NAVEWIN.xaml.cs         # Lógica de navegación y lanzamiento de detalle
    │   ├── DETAILWIN.xaml          # Ventana de Detalle dinámica según tipo de archivo
    │   └── DETAILWIN.xaml.cs       # Lógica de visualización (Zoom, Audio/Vídeo, Texto, Markdown)
    │
    └── Resources/                  # Diccionarios de Recursos independientes (XAML)
        ├── ImageStyles.xaml        # Estilos para visor de imágenes y controles de Zoom
        ├── MediaStyles.xaml        # Estilos para reproductor multimedia (Play/Pause)
        ├── TextStyles.xaml         # Estilos para visualización de texto (Font y FontSize)
        └── MarkdownStyles.xaml     # Estilos para visor Markdown (.md)
```

---

## ✅ Estado de Implementación y Requisitos Cumplidos

Todas las funcionalidades y requisitos especificados en el [enunciado.md](enunciado.md) han sido completamente implementados y verificados:

### 🟢 Módulo 1: Sistema de Estilos y Recursos (Tres Niveles)

- [x] **Nivel 1: Recursos Globales de Aplicación (`App.xaml`)**:
  - Paleta base para **Modo Normal (Claro)** y **Modo Noche (Oscuro)** mediante `DynamicResource`.
  - Estilos globales implícitos para `Grid`, `TextBox`, `Label`, `Button`, `TreeView`, `TreeViewItem`, `TextBlock`, `CheckBox` y `ComboBox`.
  - Plantilla personalizada de `CheckBox` con tick vectorial de alto contraste y visibilidad perfecta en ambos temas.
- [x] **Nivel 2: Recursos de Ventana (`Window.Resources`)**:
  - En `NAVEWIN.xaml`: Estilos propios para botones de navegación.
  - En `DETAILWIN.xaml`: Estilos diferenciados con bordes redondeados y sombras para botones de acción.
- [x] **Nivel 3: Diccionarios de Recursos Independientes (`Resources/`)**:
  - `ImageStyles.xaml`: Estilos para contenedor, imagen y botones de Zoom.
  - `MediaStyles.xaml`: Estilos para reproductor y botones Play, Pause, Stop y Slider de volumen.
  - `TextStyles.xaml`: Estilos para editor de texto, tamaños de fuente y selector de tipografía.
  - `MarkdownStyles.xaml`: Estilos para visor WebBrowser y contenedor Markdown.
- [x] **Conmutación y Herencia de Tema**:
  - Switch interactivo `chkModoNoche` en `NAVEWIN` para alternar la paleta en caliente.
  - Herencia estricta de tema en `DETAILWIN` consultando el estado a través de la propiedad pública `EsModoNoche` de su `Owner`.

---

### 🟢 Módulo 2: Ventana Principal (`NAVEWIN`)

- [x] **Ruta Inicial del Sistema**: Navegación por defecto a la carpeta "Mis imágenes" (`SpecialFolder.MyPictures`).
- [x] **Selector de Carpeta**: Diálogo `OpenFolderDialog` para cambiar el directorio raíz en cualquier momento.
- [x] **Explorador Dinámico `TreeView`**:
  - Carga bajo demanda (*Lazy Loading*) para máximo rendimiento sin bloqueos.
  - Iconos contextuales por tipo de fichero.
  - Apertura en `DETAILWIN` mediante doble clic, tecla <kbd>Enter</kbd> o botón dedicado.
- [x] **Cierre Limpio**: Liberación controlada de ventanas hijas y cierre ordenado de la aplicación (`ShutdownMode="OnMainWindowClose"`).

---

### 🟢 Módulo 3: Ventana de Detalle (`DETAILWIN`)

- [x] **Layout Dinámico y Responsivo**: Visores especializados que se activan según la extensión del archivo, ocultando el resto de paneles.
- [x] **Visor de Imágenes**:
  - Autoajuste inicial inteligente al tamaño de la ventana (`ViewportWidth` / `ViewportHeight`).
  - Renderizado de alta resolución (`RenderOptions.BitmapScalingMode="HighQuality"`).
  - Controles de **Zoom In (+)**, **Zoom Out (-)** y botón de **Ajustar (`↺ Ajustar`)**.
  - Zoom interactivo mediante combinación <kbd>Ctrl</kbd> + Rueda del ratón.
  - Centrado automático del scroll en el medio de la imagen al realizar zoom.
- [x] **Visor Multimedia (Audio / Vídeo)**:
  - Control de reproducción completo: **Play**, **Pause** y **Stop**.
  - Control deslizante (`Slider`) para ajuste de **Volumen**.
  - Detención automática de la reproducción al cerrar u ocultar el visor.
- [x] **Visor de Texto Plano**:
  - Editor `TextBox` multilínea con barras de desplazamiento automáticas.
  - Selector desplegable de **Fuente tipográfica** (`FontFamily`: Consolas, Segoe UI, Arial, Courier New, Times New Roman).
  - Botones de tamaño de texto (**FontSize + / -**).
  - Botón de guardado persistente de cambios en disco (`File.WriteAllText`).
- [x] **Visor Markdown**:
  - Conversión de `.md` a HTML enriquecido mediante `Markdig`.
  - Integración en `WebBrowser` con estilos CSS adaptados dinámicamente a Modo Día o Modo Noche.
- [x] **Archivos no Soportados**:
  - Delegación al sistema operativo mediante `Process.Start` con la aplicación predeterminada del usuario.
- [x] **Cierre Controlado**:
  - Botón de cancelación con `IsCancel="True"` y cierre mediante <kbd>Esc</kbd>, <kbd>Alt</kbd>+<kbd>F4</kbd> o botón `X` sin cerrar la aplicación principal.
