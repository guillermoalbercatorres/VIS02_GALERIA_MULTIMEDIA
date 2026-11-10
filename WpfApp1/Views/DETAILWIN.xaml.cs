using Markdig;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Ventana de Detalle (DETAILWIN): Muestra y procesa el archivo según su extensión.
    /// </summary>
    public partial class DETAILWIN : Window
    {
        private string ruta = string.Empty;
        private string markdownActual = string.Empty;

        /// <summary>
        /// Permite a la ventana principal forzar el cierre definitivo al salir de la aplicación.
        /// </summary>
        public bool ForzarCierre { get; set; } = false;

        // Constructor por defecto
        public DETAILWIN()
        {
            InitializeComponent();
        }

        // Constructor que recibe la ruta inicial que le envía NAVEWIN
        public DETAILWIN(string rutaInicial) : this()
        {
            CargarArchivo(rutaInicial);
        }

        /// <summary>
        /// Consulta el tema activo a través de su ventana propietaria (Owner) cumpliendo la especificación.
        /// </summary>
        public bool EsModoNocheActivo()
        {
            if (this.Owner is NAVEWIN navewin)
            {
                return navewin.EsModoNoche;
            }

            return Application.Current.Resources["AppBackgroundBrush"] is SolidColorBrush fondo
                && fondo.Color == Color.FromRgb(0x18, 0x18, 0x1B);
        }

        private void RenderizarMarkdown(string markdown)
        {
            bool esModoNoche = EsModoNocheActivo();

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            string contenidoHtml = Markdown.ToHtml(markdown, pipeline);
            string estilos = esModoNoche
                ? "<style>html, body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; line-height: 1.5; background-color: #18181B !important; color: #F4F4F5 !important; } a { color: #60A5FA !important; } code, pre { background-color: #27272A !important; color: #F4F4F5 !important; padding: 2px 4px; border-radius: 4px; }</style>"
                : "<style>html, body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; line-height: 1.5; background-color: #FFFFFF !important; color: #333333 !important; }</style>";

            string pagina = "<!doctype html><html><head><meta charset='utf-8'>" +
                            estilos +
                            "</head><body>" + contenidoHtml + "</body></html>";
            webBrowser1.NavigateToString(pagina);
        }

        public void ActualizarTema()
        {
            if (!string.IsNullOrEmpty(markdownActual))
            {
                RenderizarMarkdown(markdownActual);
            }
        }

        /// <summary>
        /// Carga automáticamente el archivo según su extensión real usando Path.GetExtension
        /// en lugar de búsquedas de texto ambiguas (.Contains).
        /// </summary>
        public void CargarArchivo(string nuevaRuta)
        {
            if (string.IsNullOrWhiteSpace(nuevaRuta) || !File.Exists(nuevaRuta))
                return;

            ruta = nuevaRuta;
            Title = $"NaviVision - {Path.GetFileName(nuevaRuta)}";
            markdownActual = string.Empty;
            Imagen.Source = null;
            Video.Stop();
            Video.Source = null;
            textoBloque.Clear();
            MostrarPanel(panelVacio);

            try
            {
                string ext = Path.GetExtension(nuevaRuta).ToLowerInvariant();
                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                    case ".bmp":
                    case ".webp":
                        MostrarPanel(panelImagen);
                        Imagen.Source = new BitmapImage(new Uri(nuevaRuta, UriKind.Absolute));
                        zoomTransform.ScaleX = 1.0;
                        zoomTransform.ScaleY = 1.0;
                        CentrarScrollImagen();
                        txtEstado.Text = "Imagen cargada y ajustada al visor. Usa los botones o Ctrl+Rueda para zoom.";
                        break;

                    case ".mp4":
                    case ".mp3":
                    case ".wav":
                    case ".avi":
                    case ".wmv":
                    case ".mkv":
                        MostrarPanel(panelMultimedia);
                        Video.Source = new Uri(nuevaRuta, UriKind.Absolute);
                        if (sliderVolumen != null)
                        {
                            Video.Volume = sliderVolumen.Value;
                        }
                        txtEstado.Text = "Archivo multimedia cargado. Pulsa Reproducir para comenzar.";
                        break;

                    case ".txt":
                    case ".log":
                    case ".csv":
                    case ".json":
                    case ".xml":
                        MostrarPanel(panelTexto);
                        textoBloque.Text = File.ReadAllText(nuevaRuta);
                        txtEstado.Text = "Archivo de texto cargado.";
                        break;

                    case ".md":
                        MostrarPanel(panelMarkdown);
                        markdownActual = File.ReadAllText(nuevaRuta);
                        RenderizarMarkdown(markdownActual);
                        txtEstado.Text = "Documento Markdown cargado.";
                        break;

                    default:
                        // Si es una extensión desconocida, abrir con la aplicación predeterminada del sistema
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = nuevaRuta,
                            UseShellExecute = true
                        };
                        Process.Start(psi);
                        txtEstado.Text = "Archivo abierto con la aplicación predeterminada.";
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el archivo:\n{ex.Message}", "Error al visualizar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MostrarPanel(Border panelVisible)
        {
            panelImagen.Visibility = Visibility.Collapsed;
            panelMultimedia.Visibility = Visibility.Collapsed;
            panelTexto.Visibility = Visibility.Collapsed;
            panelMarkdown.Visibility = Visibility.Collapsed;
            panelVacio.Visibility = Visibility.Collapsed;
            panelVisible.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Evita que cerrar DETAILWIN (mediante la 'X' o Alt+F4) cierre toda la aplicación.
        /// Oculta la ventana para que pueda volver a ser mostrada sin perder estado.
        /// Detiene la reproducción multimedia al cerrarse u ocultarse.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            Video.Stop();
            if (!ForzarCierre)
            {
                e.Cancel = true;
                this.Hide();
            }
            base.OnClosing(e);
        }

        private void Button_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Video.Stop();
            this.Hide();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (System.Windows.Input.Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Control) && Imagen.Source != null)
            {
                if (e.Delta > 0)
                {
                    BtnZoomIn_Click(sender, e);
                }
                else if (e.Delta < 0)
                {
                    BtnZoomOut_Click(sender, e);
                }
                e.Handled = true;
            }
        }

        private void CentrarScrollImagen()
        {
            if (scrollImagen == null) return;
            scrollImagen.UpdateLayout();
            if (scrollImagen.ScrollableWidth > 0)
            {
                scrollImagen.ScrollToHorizontalOffset(scrollImagen.ScrollableWidth / 2.0);
            }
            if (scrollImagen.ScrollableHeight > 0)
            {
                scrollImagen.ScrollToVerticalOffset(scrollImagen.ScrollableHeight / 2.0);
            }
        }

        private void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (Imagen.Source is not null)
            {
                zoomTransform.ScaleX = Math.Min(8.0, zoomTransform.ScaleX * 1.25);
                zoomTransform.ScaleY = Math.Min(8.0, zoomTransform.ScaleY * 1.25);
                CentrarScrollImagen();
                txtEstado.Text = $"Zoom: {(int)(zoomTransform.ScaleX * 100)}%";
            }
        }

        private void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (Imagen.Source is not null)
            {
                zoomTransform.ScaleX = Math.Max(0.15, zoomTransform.ScaleX / 1.25);
                zoomTransform.ScaleY = Math.Max(0.15, zoomTransform.ScaleY / 1.25);
                CentrarScrollImagen();
                txtEstado.Text = $"Zoom: {(int)(zoomTransform.ScaleX * 100)}%";
            }
        }

        private void BtnZoomReset_Click(object sender, RoutedEventArgs e)
        {
            if (Imagen.Source is not null)
            {
                zoomTransform.ScaleX = 1.0;
                zoomTransform.ScaleY = 1.0;
                CentrarScrollImagen();
                txtEstado.Text = "Zoom ajustado al espacio (100%).";
            }
        }

        private void BtnFontPlus_Click(object sender, RoutedEventArgs e)
        {
            textoBloque.FontSize = Math.Min(48, textoBloque.FontSize + 1);
        }

        private void BtnFontMinus_Click(object sender, RoutedEventArgs e)
        {
            textoBloque.FontSize = Math.Max(8, textoBloque.FontSize - 1);
        }

        private void CmbFuentes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (textoBloque != null && cmbFuentes?.SelectedItem is ComboBoxItem item && item.Content is string fuente)
            {
                textoBloque.FontFamily = new FontFamily(fuente);
            }
        }

        private void SliderVolumen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Video != null)
            {
                Video.Volume = e.NewValue;
            }
        }

        private void BtnGuardarTexto_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(ruta) && Path.GetExtension(ruta).Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    File.WriteAllText(ruta, textoBloque.Text);
                    txtEstado.Text = "Cambios guardados.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo guardar el archivo:\n{ex.Message}", "Error al guardar", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            Video.Play();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            Video.Pause();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            Video.Stop();
        }

        private void BtnAbrirExterno_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(ruta))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = ruta,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo abrir el archivo:\n{ex.Message}", "Error al abrir", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
