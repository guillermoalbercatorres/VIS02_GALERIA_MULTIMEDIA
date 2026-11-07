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
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string ruta;
        public Window1(String ruta1)
        {
            InitializeComponent();
            ruta= ruta1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // string ruta = @"C:\Users\Guillermo\Desktop\images.jpg";
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
