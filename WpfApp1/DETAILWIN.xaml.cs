using System;
using System.IO;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para DETAILWIN.xaml (Ventana de Detalle)
    /// </summary>
    public partial class DETAILWIN : Window
    {
        private string _rutaArchivo = string.Empty;

        public DETAILWIN()
        {
            InitializeComponent();
        }

        public DETAILWIN(string rutaArchivo) : this()
        {
            _rutaArchivo = rutaArchivo;
        }
    }
}
