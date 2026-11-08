# VIS02 - Galería Multimedia y Visor de Archivos (WPF)

Este repositorio contiene la solución en WPF (.NET 10) para la práctica **VIS02**.

---

## 📁 Estructura del Proyecto

El proyecto sigue una organización por responsabilidades separando vistas, recursos de diseño y configuración:

```text
VIS02/
├── .gitignore                      # Exclusiones de Git (bin, obj, .vs, pdfs, temporales)
├── enunciado.md                    # Requisitos detallados y especificaciones de la práctica
├── README.md                       # Documentación de arquitectura, estado y roadmap
├── WpfApp1.slnx                    # Archivo de solución de Visual Studio
└── WpfApp1/                        # Proyecto WPF (.NET 10)
    ├── WpfApp1.csproj              # Archivo de proyecto (referencias a paquetes NuGet como Markdig)
    ├── App.xaml                    # Recursos globales (Colores Modo Normal / Modo Noche)
    ├── App.xaml.cs                 # Lógica de ciclo de vida de la aplicación
    │
    ├── Views/                      # Vistas / Ventanas XAML
    │   ├── NAVEWIN.xaml            # Ventana Principal (Explorador TreeView, Selección de carpeta)
    │   ├── NAVEWIN.xaml.cs         # Lógica de navegación y lanzamiento de detalle
    │   ├── DETAILWIN.xaml          # Ventana de Detalle dinámica según tipo de archivo
    │   └── DETAILWIN.xaml.cs       # Lógica de visualización (Zoom, Audio/Vídeo, Texto, Markdown)
    │
    ├── Resources/                  # Diccionarios de Recursos independientes (XAML)
    │   ├── ImageStyles.xaml        # Estilos para visor de imágenes y controles de Zoom
    │   ├── MediaStyles.xaml        # Estilos para reproductor multimedia (Play/Pause)
    │   ├── TextStyles.xaml         # Estilos para visualización de texto (Font y FontSize)
    │   └── MarkdownStyles.xaml     # Estilos para visor Markdown (.md)
    │
    └── Img_Txt/                    # Archivos de prueba locales (imágenes, textos, markdown)
```

---

## 📊 Estado Actual del Proyecto y Auditoría (Post-Merge `guille-rama`)

Tras descargar los últimos cambios fusionados en `main`, se ha realizado una auditoría exhaustiva comparando con el `enunciado.md`:

| Componente | Estado Real | Análisis / Problemas Detectados |
| :--- | :---: | :--- |
| **`WpfApp1.csproj`** | ⚠️ **Corregido** | Se subió a `main` con marcadores de conflicto de Git sin resolver (`<<<<<<< HEAD`), provocando error de compilación inmediato (`MSB4025`). |
| **`NAVEWIN` (Explorador)** | 🔴 **Prototipo estático** | Tiene un `TreeView` con 4 elementos cableados a mano con rutas absolutas a carpetas personales (`C:\Users\Guillermo\...`). **No explora el sistema de archivos real**, no arranca en `MyPictures`, no tiene selector de carpeta ni multihilo. |
| **`DETAILWIN` (Visor)** | 🟡 **Parcial / En bruto** | Contiene pruebas funcionales de `Image`, `MediaElement`, `TextBox` y `WebBrowser` (Markdig), pero colocados con márgenes fijos en un lienzo desordenado. Requiere pulsar botones manuales, no adapta la vista automáticamente, carece de zoom y de selección de fuentes/tamaño. |
| **Estilos y Temas** | ⚪ **Pendiente (0%)** | `App.xaml` y los diccionarios en `Resources/` están vacíos. No existe conmutación ni paleta de Modo Noche/Normal. |

---

## 🚦 Roadmap y Lista de Tareas Priorizadas

### 🔴 Prioridad 0: Correcciones Críticas y Portabilidad (Completado)
- [x] **Reparar `WpfApp1.csproj`**: Marcadores de conflicto eliminados y paquete `Markdig` funcionando.
- [x] **Eliminar rutas absolutas personales**: Sustituido el árbol estático con rutas personales por exploración real de disco y carga bajo demanda.
- [x] **Sustituir `Contains(".ext")` por `Path.GetExtension()`**: Normalizada la detección de extensiones en `NAVEWIN` y `DETAILWIN`.

---

### 🟠 Prioridad 1: Requisitos Funcionales del Enunciado (Módulos Principales)

#### Módulo 2: Ventana Principal (`NAVEWIN`)
- [x] **Explorador real de carpetas con `TreeView`**:
  - [x] Arranca por defecto en **"Mis imágenes"** (`Environment.SpecialFolder.MyPictures`).
  - [x] Carga bajo demanda (Lazy Loading) de subdirectorios para rendimiento óptimo.
  - [x] Iconos representativos según el tipo de archivo (🖼️, 🎬, 📄, 📝).
- [x] **Selector de carpeta (Folder Picker)**:
  - [x] Botón "📁 Cambiar Carpeta..." con diálogo nativo `OpenFolderDialog`.
- [x] **Lanzamiento controlado de `DETAILWIN`**:
  - [x] Apertura automática al seleccionar archivo en el árbol.
  - [x] Reutilización de instancia única (`Owner = this`, sin duplicar ventanas).
- [ ] **Selector Modo Noche / Modo Normal**:
  - [x] CheckBox / Switch en la interfaz (`EsModoNoche`).
  - [ ] Conectar con cambio de recursos y estilos dinámicos de `App.xaml`.

#### Módulo 3: Ventana de Detalle (`DETAILWIN`)
- [x] **Carga automática al recibir archivo**:
  - [x] Carga y renderizado automático según extensión (`.jpg`, `.mp4`, `.txt`, `.md`).
  - [x] Apertura con `Process.Start` para extensiones no reconocidas.
- [x] **Cierre controlado y Gestión de Ciclo de Vida**:
  - [x] Botón "✖ Cerrar Visor" con propiedad `IsCancel="True"` (soporta tecla <kbd>Esc</kbd>).
  - [x] Evento `OnClosing` sobreescrito para ocultar la ventana (`e.Cancel = true; this.Hide();`) sin cerrar `NAVEWIN`.
- [ ] **Visor de Imágenes**:
  - [x] Carga nativa con `BitmapImage`.
  - [ ] Controles funcionales de **Zoom In (+)** y **Zoom Out (-)** mediante `ScaleTransform`.
- [ ] **Visor Multimedia (Audio / Vídeo)**:
  - [x] `<MediaElement>` integrado con Play y Pause.
  - [ ] Diseño y estilo unificado de controles multimedia.
- [ ] **Visor de Texto Plano (`.txt`)**:
  - [x] Carga de texto en `TextBox`.
  - [ ] Controles para seleccionar **Fuente (FontFamily)** y cambiar tamaño (**FontSize +/-**).
- [ ] **Adaptación visual limpia (Layout)**:
  - [ ] Reemplazar posiciones absolutas por paneles responsivos que muestren sólo el visor activo.

---

### 🟡 Prioridad 2: Sistema de Estilos y Recursos (Módulo 1)
- [ ] **Paleta de Colores en `App.xaml`**:
  - Definir `SolidColorBrush` para Modo Normal (`FondoBrush` claro, `TextoBrush` oscuro).
  - Definir `SolidColorBrush` para Modo Noche (`FondoBrush` oscuro, `TextoBrush` claro).
  - Estilos base para `Grid`, `TextBox` y `Label` con `DynamicResource`.
- [ ] **Estilos a nivel de Ventana (`Window.Resources`)**:
  - Estilos de botones específicos para `NAVEWIN.xaml`.
  - Estilos de botones específicos para `DETAILWIN.xaml`.
- [ ] **Completar Diccionarios de Recursos (`Resources/`)**:
  - `ImageStyles.xaml`: Estilos para botones de Zoom.
  - `MediaStyles.xaml`: Estilos para controles Play/Pause.
  - `TextStyles.xaml`: Estilos para controles de tipografía.
  - `MarkdownStyles.xaml`: Estilos para la presentación de Markdown.

---

### 🟢 Prioridad 3: Diseño, Limpieza y Buenas Prácticas
- [ ] **Layout responsivo en `DETAILWIN`**: Sustituir el diseño de botones con márgenes fijos (`Margin="824,255,0,0"`) por una cuadrícula limpia (`Grid.RowDefinitions` / `StackPanel`).
- [ ] **Sincronización de tema**: Que `DETAILWIN` herede automáticamente el tema activo consultando a su ventana `Owner`.
