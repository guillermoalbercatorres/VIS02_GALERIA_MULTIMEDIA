using Markdig;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Ventana de Detalle (DETAILWIN): Muestra y procesa el archivo según su extensión.
    /// </summary>
    public partial class DETAILWIN : Window
    {
        private string ruta = string.Empty;

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
        /// Carga automáticamente el archivo según su extensión real usando Path.GetExtension
        /// en lugar de búsquedas de texto ambiguas (.Contains).
        /// </summary>
        public void CargarArchivo(string nuevaRuta)
        {
            if (string.IsNullOrWhiteSpace(nuevaRuta) || !File.Exists(nuevaRuta))
                return;

            this.ruta = nuevaRuta;
            this.Title = $"DETAILWIN - {Path.GetFileName(nuevaRuta)}";

            string ext = Path.GetExtension(nuevaRuta).ToLower();

            try
            {
                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                    case ".bmp":
                    case ".webp":
                        // Carga la imagen de forma nativa
                        Imagen.Source = new BitmapImage(new Uri(nuevaRuta, UriKind.Absolute));
                        break;

                    case ".mp4":
                    case ".mp3":
                    case ".wav":
                    case ".avi":
                    case ".wmv":
                    case ".mkv":
                        // Carga y reproduce el elemento multimedia
                        Video.Source = new Uri(nuevaRuta, UriKind.Absolute);
                        Video.Play();
                        break;

                    case ".txt":
                    case ".log":
                    case ".csv":
                    case ".json":
                    case ".xml":
                        // Lee el contenido completo del archivo de texto
                        textoBloque.Text = File.ReadAllText(nuevaRuta);
                        break;

                    case ".md":
                        // Renderizado de Markdown usando Markdig en WebBrowser
                        string markdown = File.ReadAllText(nuevaRuta);
                        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                        string contenidoHtml = Markdown.ToHtml(markdown, pipeline);
                        string pagina = "<!doctype html><html><head><meta charset='utf-8'>" +
                                        "<style>body{font-family:Segoe UI, Arial, sans-serif;margin:20px;line-height:1.5;color:#333;}</style>" +
                                        "</head><body>" + contenidoHtml + "</body></html>";
                        webBrowser1.NavigateToString(pagina);
                        break;

                    default:
                        // Si es una extensión desconocida, abrir con la aplicación predeterminada del sistema
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = nuevaRuta,
                            UseShellExecute = true
                        };
                        Process.Start(psi);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el archivo:\n{ex.Message}", "Error al visualizar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        // ==========================================
        // MÉTODOS DE SOPORTE PARA BOTONES MANUALES
        // ==========================================

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EsExtensionImagen(this.ruta))
            {
                Imagen.Source = new BitmapImage(new Uri(this.ruta, UriKind.Absolute));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (File.Exists(this.ruta))
            {
                Process.Start(new ProcessStartInfo { FileName = this.ruta, UseShellExecute = true });
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (File.Exists(this.ruta))
            {
                Video.Source = new Uri(this.ruta, UriKind.Absolute);
                Video.Play();
            }
        }

        private void Button_Click_Texto(object sender, RoutedEventArgs e)
        {
            if (Path.GetExtension(this.ruta).Equals(".txt", StringComparison.OrdinalIgnoreCase)
                || Path.GetExtension(this.ruta).Equals(".md", StringComparison.OrdinalIgnoreCase))
            {
                using (StreamWriter sw = new StreamWriter(this.ruta, append: false))
                {
                    sw.WriteLine(textoBloque.Text);
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Path.GetExtension(this.ruta).Equals(".txt", StringComparison.OrdinalIgnoreCase) 
                || Path.GetExtension(this.ruta).Equals(".md", StringComparison.OrdinalIgnoreCase))
            {
                if (File.Exists(this.ruta))
                {
                    textoBloque.Text = File.ReadAllText(this.ruta);
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Video.Pause();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (File.Exists(this.ruta))
            {
                Process.Start(new ProcessStartInfo { FileName = this.ruta, UseShellExecute = true });
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (File.Exists(this.ruta))
            {
                string markdown = File.ReadAllText(this.ruta);
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                string contenidoHtml = Markdown.ToHtml(markdown, pipeline);
                string pagina = "<!doctype html><html><head><meta charset='utf-8'>" +
                                "<style>body{font-family:Segoe UI, Arial;margin:20px}</style>" +
                                "</head><body>" + contenidoHtml + "</body></html>";
                webBrowser1.NavigateToString(pagina);
            }
        }

        private bool EsExtensionImagen(string rutaArchivo)
        {
            string ext = Path.GetExtension(rutaArchivo).ToLower();
            return ext is ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp";
        }
    }
}
