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

        private void RenderizarMarkdown(string markdown)
        {
            bool esModoNoche = Application.Current.Resources["AppBackgroundBrush"] is SolidColorBrush fondo
                && fondo.Color == Color.FromRgb(0x18, 0x18, 0x1B);

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
                        var transformacion = (ScaleTransform)Imagen.RenderTransform;
                        transformacion.ScaleX = 1;
                        transformacion.ScaleY = 1;
                        txtEstado.Text = "Imagen cargada. Utiliza los controles para ajustar el zoom.";
                        break;

                    case ".mp4":
                    case ".mp3":
                    case ".wav":
                    case ".avi":
                    case ".wmv":
                    case ".mkv":
                        MostrarPanel(panelMultimedia);
                        Video.Source = new Uri(nuevaRuta, UriKind.Absolute);
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
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }

        private void Button_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (Imagen.Source is not null)
            {
                var transformacion = (ScaleTransform)Imagen.RenderTransform;
                transformacion.ScaleX = Math.Min(5, transformacion.ScaleX * 1.2);
                transformacion.ScaleY = Math.Min(5, transformacion.ScaleY * 1.2);
            }
        }

        private void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (Imagen.Source is not null)
            {
                var transformacion = (ScaleTransform)Imagen.RenderTransform;
                transformacion.ScaleX = Math.Max(0.2, transformacion.ScaleX / 1.2);
                transformacion.ScaleY = Math.Max(0.2, transformacion.ScaleY / 1.2);
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
