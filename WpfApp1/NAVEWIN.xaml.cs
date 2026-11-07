using System;
using System.IO;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para NAVEWIN.xaml (Ventana Principal)
    /// </summary>
    public partial class NAVEWIN : Window
    {
        // Propiedad o método público para consultar el tema actual desde DETAILWIN
        public bool EsModoNoche { get; set; } = false;

        public NAVEWIN()
        {
            InitializeComponent();
        }
    }
}
