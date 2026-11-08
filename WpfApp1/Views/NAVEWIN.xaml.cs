using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Ventana Principal (NAVEWIN): Explorador de archivos con TreeView y navegación de carpetas.
    /// </summary>
    public partial class NAVEWIN : Window
    {
        // Propiedad para consultar el tema actual (Modo Noche / Normal)
        public bool EsModoNoche { get; set; } = false;

        private string rutaSeleccionada = string.Empty;
        private DETAILWIN? ventanaDetalle = null;

        public NAVEWIN()
        {
            InitializeComponent();
            IniciarExplorador();
        }

        /// <summary>
        /// Inicializa el explorador navegando a la carpeta de "Mis imágenes" por defecto según el enunciado.
        /// </summary>
        private void IniciarExplorador()
        {
            string rutaInicial = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // Si por alguna razón no existiera, fallback a la carpeta del proyecto o perfil de usuario
            if (string.IsNullOrWhiteSpace(rutaInicial) || !Directory.Exists(rutaInicial))
            {
                rutaInicial = AppDomain.CurrentDomain.BaseDirectory;
            }

            CargarDirectorio(rutaInicial);
        }

        /// <summary>
        /// Carga una carpeta raíz en el TreeView y añade sus subcarpetas y ficheros de forma dinámica.
        /// </summary>
        public void CargarDirectorio(string rutaDirectorio)
        {
            if (string.IsNullOrWhiteSpace(rutaDirectorio) || !Directory.Exists(rutaDirectorio))
                return;

            txtRutaActual.Text = rutaDirectorio;
            treeView.Items.Clear();

            try
            {
                var dirInfo = new DirectoryInfo(rutaDirectorio);
                var nodoRaiz = CrearNodoCarpeta(dirInfo);
                treeView.Items.Add(nodoRaiz);
                nodoRaiz.IsExpanded = true;

                lblEstado.Text = $"Explorando carpeta: {dirInfo.FullName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al acceder al directorio:\n{ex.Message}", "Error de navegación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Crea un nodo de carpeta en el TreeView con carga bajo demanda (Lazy Loading)
        /// para garantizar rendimiento fluido sin bloquear la interfaz.
        /// </summary>
        private TreeViewItem CrearNodoCarpeta(DirectoryInfo dir)
        {
            var nodo = new TreeViewItem
            {
                Header = $"📁 {dir.Name}",
                Tag = dir.FullName
            };

            // Añade un elemento dummy para que el TreeView muestre la flecha de expandir
            nodo.Items.Add(null);
            nodo.Expanded += Carpeta_Expanded;

            return nodo;
        }

        /// <summary>
        /// Evento que se dispara al desplegar una carpeta para cargar sus subdirectorios y ficheros.
        /// </summary>
        private void Carpeta_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem nodo && nodo.Items.Count == 1 && nodo.Items[0] == null)
            {
                nodo.Items.Clear();
                string? ruta = nodo.Tag as string;

                if (string.IsNullOrEmpty(ruta) || !Directory.Exists(ruta))
                    return;

                try
                {
                    var dirInfo = new DirectoryInfo(ruta);

                    // 1. Cargar subcarpetas
                    foreach (var subDir in dirInfo.GetDirectories())
                    {
                        // Omitir carpetas ocultas o de sistema
                        if ((subDir.Attributes & FileAttributes.Hidden) != 0 || (subDir.Attributes & FileAttributes.System) != 0)
                            continue;

                        nodo.Items.Add(CrearNodoCarpeta(subDir));
                    }

                    // 2. Cargar archivos
                    foreach (var file in dirInfo.GetFiles())
                    {
                        if ((file.Attributes & FileAttributes.Hidden) != 0)
                            continue;

                        string icono = ObtenerIconoArchivo(file.Extension);
                        var nodoArchivo = new TreeViewItem
                        {
                            Header = $"{icono} {file.Name}",
                            Tag = file.FullName
                        };
                        nodo.Items.Add(nodoArchivo);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    nodo.Items.Add(new TreeViewItem { Header = "⚠️ Acceso denegado", IsEnabled = false });
                }
                catch (Exception ex)
                {
                    nodo.Items.Add(new TreeViewItem { Header = $"⚠️ Error: {ex.Message}", IsEnabled = false });
                }
            }
        }

        /// <summary>
        /// Devuelve un icono emoji representativo según la extensión del archivo.
        /// </summary>
        private string ObtenerIconoArchivo(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" => "🖼️",
                ".mp4" or ".mp3" or ".wav" or ".avi" or ".mkv" or ".wmv" => "🎬",
                ".txt" or ".log" or ".csv" or ".json" or ".xml" => "📄",
                ".md" => "📝",
                _ => "📦"
            };
        }

        /// <summary>
        /// Evento al seleccionar un elemento en el árbol con un simple clic.
        /// Muestra la información del elemento seleccionado sin abrir la ventana de detalle automáticamente.
        /// </summary>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is TreeViewItem nodoSeleccionado && nodoSeleccionado.Tag is string ruta)
            {
                if (File.Exists(ruta))
                {
                    rutaSeleccionada = ruta;
                    txtInfoSeleccion.Text = $"Archivo seleccionado:\n{Path.GetFileName(ruta)}\n\nRuta:\n{ruta}";
                    btnAbrirVisor.IsEnabled = true;
                    lblEstado.Text = $"Seleccionado: {Path.GetFileName(ruta)} (Doble clic, Enter o botón para abrir)";
                }
                else if (Directory.Exists(ruta))
                {
                    rutaSeleccionada = string.Empty;
                    txtInfoSeleccion.Text = $"Carpeta seleccionada:\n{Path.GetFileName(ruta)}\n\n(Despliégala en el árbol para ver su contenido)";
                    btnAbrirVisor.IsEnabled = false;
                    lblEstado.Text = $"Carpeta: {ruta}";
                }
            }
        }

        /// <summary>
        /// Abre el visor al hacer doble clic sobre un archivo en el árbol.
        /// </summary>
        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (treeView.SelectedItem is TreeViewItem nodo && nodo.Tag is string ruta && File.Exists(ruta))
            {
                AbrirOActualizarVisor(ruta);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Abre el visor si el usuario pulsa Enter sobre un archivo seleccionado.
        /// </summary>
        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (treeView.SelectedItem is TreeViewItem nodo && nodo.Tag is string ruta && File.Exists(ruta))
                {
                    AbrirOActualizarVisor(ruta);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Abre la ventana DETAILWIN o actualiza su contenido si ya está abierta,
        /// evitando abrir múltiples ventanas descontroladas.
        /// </summary>
        private void AbrirOActualizarVisor(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo) || !File.Exists(rutaArchivo))
                return;

            if (ventanaDetalle == null || !ventanaDetalle.IsLoaded)
            {
                ventanaDetalle = new DETAILWIN(rutaArchivo);
                ventanaDetalle.Owner = this;
                ventanaDetalle.Show();
            }
            else
            {
                ventanaDetalle.CargarArchivo(rutaArchivo);
                if (!ventanaDetalle.IsVisible)
                {
                    ventanaDetalle.Show();
                }
                ventanaDetalle.Activate();
            }
        }

        /// <summary>
        /// Botón manual para abrir el visor en DETAILWIN si la ventana fue cerrada.
        /// </summary>
        private void BtnAbrirVisor_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(rutaSeleccionada))
            {
                AbrirOActualizarVisor(rutaSeleccionada);
            }
        }

        /// <summary>
        /// Selector de carpetas del sistema operativo (Folder Picker).
        /// </summary>
        private void BtnCambiarCarpeta_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFolderDialog
            {
                Title = "Selecciona una carpeta para explorar",
                InitialDirectory = string.IsNullOrEmpty(txtRutaActual.Text) ? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) : txtRutaActual.Text
            };

            if (ofd.ShowDialog() == true)
            {
                CargarDirectorio(ofd.FolderName);
            }
        }

        /// <summary>
        /// Alternador de tema Modo Noche / Modo Normal.
        /// </summary>
        private void ChkModoNoche_Checked(object sender, RoutedEventArgs e)
        {
            EsModoNoche = true;
            // Listo para conmutar paleta de estilos globales
        }

        private void ChkModoNoche_Unchecked(object sender, RoutedEventArgs e)
        {
            EsModoNoche = false;
        }
    }
}
