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

### 🔴 Prioridad 0: Correcciones Críticas y Portabilidad (Inmediato)
- [x] **Reparar `WpfApp1.csproj`**: Eliminar marcadores residuales de conflicto de Git y asegurar compilación limpia con el paquete `Markdig`.
- [ ] **Eliminar rutas absolutas personales**: Reemplazar cualquier ruta fija como `C:\Users\Guillermo\...` por rutas relativas al proyecto (`AppDomain.CurrentDomain.BaseDirectory` o carpeta `Img_Txt/`) para que funcione en cualquier ordenador.
- [ ] **Sustituir `Contains(".ext")` por `Path.GetExtension()`**: Prevenir falsos positivos al detectar extensiones de archivo (ej. `archivo.jpg.txt`).

---

### 🟠 Prioridad 1: Requisitos Funcionales del Enunciado (Módulos Principales)

#### Módulo 2: Ventana Principal (`NAVEWIN`)
- [ ] **Explorador real de carpetas con `TreeView`**:
  - Cargar por defecto la ruta del sistema **"Mis imágenes"** (`Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)`).
  - Carga asíncrona / multihilo de directorios y archivos para evitar congelar la interfaz.
  - Iconos o diferenciación visual entre carpetas y archivos.
- [ ] **Selector de carpeta (Folder Picker)**:
  - Botón para abrir diálogo de selección de carpeta (`OpenFolderDialog` o `FolderBrowserDialog`) para cambiar el directorio raíz explorado.
- [ ] **Lanzamiento controlado de `DETAILWIN`**:
  - Al hacer clic en un archivo del árbol, abrir o actualizar `DETAILWIN` asignándole `Owner = this`.
  - Evitar instanciar múltiples ventanas descontroladas si ya hay una abierta.
- [ ] **Selector Modo Noche / Modo Normal**:
  - Control en la ventana para cambiar entre temas y notificar a la ventana de detalle.

#### Módulo 3: Ventana de Detalle (`DETAILWIN`)
- [ ] **Adaptación dinámica de la interfaz**:
  - Al abrirse con una ruta, ocultar los controles innecesarios y mostrar únicamente el visor correspondiente al tipo de archivo (en lugar de tener botones manuales dispersos).
- [ ] **Visor de Imágenes**:
  - Componente `<Image>` con controles funcionales de **Zoom In (+)** y **Zoom Out (-)** mediante `ScaleTransform` o ajuste de dimensiones.
- [ ] **Visor Multimedia (Audio / Vídeo)**:
  - `<MediaElement>` integrado con controles claros de **Play** y **Pause**.
- [ ] **Visor de Texto Plano (`.txt`)**:
  - Vista previa en `TextBox` con controles funcionales para seleccionar **Fuente (FontFamily)** y cambiar tamaño de letra (**FontSize +/-**).
- [ ] **Visor Markdown (`.md`)**:
  - Renderizado automático del contenido Markdown enriquecido al cargar el archivo.
- [ ] **Fallback para otros archivos**:
  - Si la extensión no es soportada internamente, ejecutar automáticamente `Process.Start` con la aplicación predeterminada del sistema.
- [ ] **Cierre controlado y Gestión de Ciclo de Vida**:
  - Botón "Cerrar" / "Cancelar" con propiedad `IsCancel="True"`.
  - Capturar evento `Closing` para que cerrar `DETAILWIN` (`X` o <kbd>Alt</kbd>+<kbd>F4</kbd>) no cierre toda la aplicación, sino que use `e.Cancel = true; this.Hide();` o cierre solo dicha ventana.

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
