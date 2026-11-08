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
    ├── Resources/                  # Diccionarios de Recursos independientes (XAML)
    │   ├── ImageStyles.xaml        # Estilos para visor de imágenes y controles de Zoom
    │   ├── MediaStyles.xaml        # Estilos para reproductor multimedia (Play/Pause)
    │   ├── TextStyles.xaml         # Estilos para visualización de texto (Font y FontSize)
    │   └── MarkdownStyles.xaml     # Estilos para visor Markdown (.md)
    │
    └── Img_Txt/                    # Archivos de prueba locales (imágenes, textos, markdown)
```

---

## 📋 Tareas Pendientes por Implementar

### 🟠 Prioridad 1: Rediseño Integral de la Ventana de Detalle (`DETAILWIN`)

Actualmente `DETAILWIN` cuenta con controles de prueba superpuestos y botones con coordenadas absolutas que deben ser reemplazados por una interfaz limpia, responsiva y dinámica según el tipo de archivo:

- [ ] **Limpieza de Controles y Layout Dinámico**:
  - Eliminar todos los botones de prueba antiguos con márgenes fijos.
  - Implementar una estructura de paneles (`Grid` o paneles intercambiables) donde **solo sea visible el visor correspondiente al archivo cargado**, ocultando los demás:
    - Si es imagen $\rightarrow$ Mostrar únicamente panel de imagen y controles de zoom.
    - Si es multimedia $\rightarrow$ Mostrar únicamente reproductor y controles de reproducción.
    - Si es texto plano $\rightarrow$ Mostrar únicamente editor de texto y controles de tipografía.
    - Si es Markdown $\rightarrow$ Mostrar únicamente el visor web renderizado.
- [ ] **Visor de Imágenes y Zoom (Requisito Enunciado)**:
  - Contenedor con `ScrollViewer` o `Canvas` para permitir desplazamiento.
  - Botón funcional de **Zoom In (+)** para aumentar la escala (`ScaleTransform`).
  - Botón funcional de **Zoom Out (-)** para reducir la escala.
  - Botón de restablecer zoom / ajustar a ventana.
- [ ] **Visor Multimedia (Audio / Vídeo) (Requisito Enunciado)**:
  - Barra de control multimedia con botones estilizados de **Play** y **Pause**.
  - Barra de progreso o control de volumen (opcional/recomendado).
- [ ] **Visor de Texto Plano y Tipografía (Requisito Enunciado)**:
  - Botones o selector desplegable de **Fuente (FontFamily)** (ej. Consolas, Arial, Segoe UI, Courier New).
  - Botones de incremento y reducción de tamaño de letra (**FontSize +** / **FontSize -**).
- [ ] **Visor Markdown (`Markdig`)**:
  - Renderizado HTML estilizado e integrado en `WebBrowser` de manera fluida.
- [ ] **Manejo de Archivos no Soportados**:
  - Si el usuario selecciona un archivo cuya extensión no es visualizable internamente (ej. `.pdf`, `.zip`, `.exe`), delegar la apertura directamente a Windows con `Process.Start` desde `NAVEWIN` sin dejar la ventana `DETAILWIN` abierta o en blanco.

---

### 🟡 Prioridad 2: Sistema de Estilos y Recursos (Módulo 1)

El sistema de estilos debe dividirse estrictamente en tres niveles según el enunciado:

- [ ] **Nivel 1: Recursos Globales de Aplicación (`App.xaml`)**:
  - Definir la paleta para **Modo Normal (Claro)**: `FondoBrush`, `TextoBrush`, `BordeBrush`.
  - Definir la paleta para **Modo Noche (Oscuro)**: `FondoBrush`, `TextoBrush`, `BordeBrush`.
  - Crear estilos implícitos o dinámicos (`DynamicResource`) para `Grid`, `TextBox`, `Label` y `Button`.
- [ ] **Nivel 2: Recursos de Ventana (`Window.Resources`)**:
  - Definir en `NAVEWIN.xaml` estilos propios para sus botones (selector de carpetas, opciones).
  - Definir en `DETAILWIN.xaml` estilos propios para sus botones de acción (zoom, play/pause, fuentes).
- [ ] **Nivel 3: Diccionarios de Recursos Independientes (`Resources/`)**:
  - `ImageStyles.xaml`: Estilos específicos para los botones de Zoom.
  - `MediaStyles.xaml`: Estilos específicos para los controles de reproducción.
  - `TextStyles.xaml`: Estilos específicos para las herramientas de texto y tamaño.
  - `MarkdownStyles.xaml`: Estilos visuales del visor Markdown.
- [ ] **Lógica de Conmutación de Tema en Tiempo Real**:
  - Conectar el switch/checkbox `chkModoNoche` de `NAVEWIN` para alternar dinámicamente los recursos de `App.xaml` en caliente.
  - Sincronizar el tema en `DETAILWIN` consultando el estado de su ventana propietaria (`(Owner as NAVEWIN).EsModoNoche`).
