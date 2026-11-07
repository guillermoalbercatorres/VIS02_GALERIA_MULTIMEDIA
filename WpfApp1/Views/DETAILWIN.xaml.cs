using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = this.ruta,
                UseShellExecute = true,
            };
            Process.Start(psi);

            /*Imagen.Source = new BitmapImage(
                new Uri(this.ruta)
            );*/
        }
    }
}
