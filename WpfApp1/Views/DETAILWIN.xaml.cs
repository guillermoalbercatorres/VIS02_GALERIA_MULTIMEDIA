using Markdig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para DETAILWIN.xaml
    /// </summary>
    public partial class DETAILWIN : Window
    {
        string ruta;

        // Constructor por defecto
        public DETAILWIN()
        {
            InitializeComponent();
        }

        // Constructor que recibe la ruta que le envió NAVEWIN
        public DETAILWIN(String ruta1) : this()
        {
            ruta = ruta1; // Guarda la ruta recibida
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // string ruta = @"C:\Users\Guillermo\Desktop\images.jpg";

            // Al pulsar el botón, en lugar de mostrar la foto dentro de la ventana de WPF,
            // le dice a Windows: "Abre este archivo con el programa que tengas predeterminado"

            if(this.ruta.Contains(".jpg"))
            {
                Imagen.Source = new BitmapImage(
                new Uri(this.ruta)
            );
            }

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.ruta.Contains(".jpg"))
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = this.ruta,
                    UseShellExecute = true,
                };
                Process.Start(psi);
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



            if (this.ruta.Contains(".txt"))
            {

                using (StreamWriter sw = new StreamWriter(ruta, append: false))
                {
                    sw.WriteLine(textoBloque.Text); // Sobrescribe el archivo de texto con lo que haya en la caja
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (this.ruta.Contains(".txt"))
            {
                string[] lineas = File.ReadAllLines(this.ruta);

                textoBloque.Text = string.Join("\n", lineas);
        }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (this.ruta.Contains(".mp4"))
            {
                Video.Pause();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (this.ruta.Contains(".mp4"))
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = this.ruta,
                    UseShellExecute = true,
                };
                Process.Start(psi);
            }
            }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string markdown = File.ReadAllText(ruta);
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            string contenidoHtml = Markdown.ToHtml(markdown, pipeline);
            string pagina = "<!doctype html><html><head><meta charset='utf-8'>" +
                "<style>body{font-family:Arial;margin:24px}</style>" +
                "</head><body>" + contenidoHtml + "</body></html>";
            webBrowser1.NavigateToString(pagina);
        }
    }
}
