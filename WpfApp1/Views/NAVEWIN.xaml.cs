using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for NAVEWIN.xaml
    /// </summary>
    public partial class NAVEWIN : Window
    {
        // Propiedad para consultar el tema actual (Modo Noche / Normal)
        public bool EsModoNoche { get; set; } = false;

        string ruta = string.Empty;

        string[] rutas = new string[]
        {
            "","","",""
        };

        public NAVEWIN()
        {
            // 1. Dibuja en pantalla los botones y cajas definidos en NAVEWIN.xaml
            InitializeComponent();

            TreeViewItem nodoRaiz = new TreeViewItem { Header = "Gestor de Archivos" };
            TreeViewItem nodoDesarrollo = new TreeViewItem { Header = "Imagenes" };
            nodoDesarrollo.Items.Add(new TreeViewItem
            {
                Header = "Imagen 1",
                Tag = "C:\\Users\\Guillermo\\Desktop\\VS_WINDOWS\\WpfApp1\\WpfApp1\\Img_Txt\\images.jpg"

            });
            nodoDesarrollo.Items.Add(new TreeViewItem { Header = "Imagen 2",
                Tag = "C:\\Users\\Guillermo\\Desktop\\VS_WINDOWS\\WpfApp1\\WpfApp1\\Img_Txt\\images2.jpg"
            });

            TreeViewItem nodoSistemas = new TreeViewItem { Header = "Txt" };
            nodoSistemas.Items.Add(new TreeViewItem { Header = "Txt 1", Tag = "C:\\Users\\Guillermo\\Desktop\\VS_WINDOWS\\WpfApp1\\WpfApp1\\Img_Txt\\hola.txt" });
            nodoSistemas.Items.Add(new TreeViewItem { Header = "Txt 2", Tag = "C:\\Users\\Guillermo\\Desktop\\VS_WINDOWS\\WpfApp1\\WpfApp1\\Img_Txt\\images2.jpg" });
<<<<<<< HEAD

            TreeViewItem nodoVideos = new TreeViewItem { Header = "Videos" };
            nodoVideos.Items.Add(new TreeViewItem { Header = "Video 1", Tag = "C:\\Users\\Guillermo\\Desktop\\VS_WINDOWS\\WpfApp1\\WpfApp1\\Img_Txt\\7_967.mp4" });


            

            TreeViewItem nodoMark = new TreeViewItem { Header = "Markdown" };
            nodoMark.Items.Add(new TreeViewItem { Header = "Mrk 1", Tag = "D:\\VS_WINDOWS\\WpfApp1\\WpfApp1\\Img_Txt\\documento.md" });

=======
>>>>>>> 48edd83c334c69e52d3a2899f13b000abeb7f52f

            nodoRaiz.Items.Add(nodoDesarrollo);
            nodoRaiz.Items.Add(nodoSistemas);
            nodoRaiz.Items.Add(nodoVideos);
            nodoRaiz.Items.Add(nodoMark);
            treeView.Items.Add(nodoRaiz);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is TreeViewItem nodoSeleccionado)
            {
                ruta = nodoSeleccionado.Tag?.ToString() ?? string.Empty;
            }

            /*if(string.IsNullOrWhiteSpace(this.ruta))
            {
                // 2. Crea un cuadro de diálogo del sistema operativo para elegir un archivo
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Title = "Selecciona un archivo de datos CSV";
                ofd.Filter = "Todos los archivos (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // 3. Muestra el cuadro de diálogo. Si el usuario selecciona un archivo y pulsa 'Aceptar':
                if (ofd.ShowDialog() == true)
                {
                    ruta = ofd.FileName; // Guarda la ruta seleccionada (ej: "C:\Fotos\paisaje.jpg")
                    MessageBox.Show("Archivo seleccionado: " + ruta);
                }
            }*/
            // 4. Comprueba si el archivo existe en el disco duro
            if (File.Exists(this.ruta))
            {
                //MessageBox.Show(this.ruta.Contains(".txt") + " , contiene .txt");

                // Si el nombre contiene ".txt", lee todas las líneas y las escribe en el TextBox
                if (this.ruta.Contains(".txt"))
                {
                    DETAILWIN w1 = new DETAILWIN(this.ruta);
                    w1.Show();
                }
                // Si el nombre contiene ".jpg", crea la segunda ventana (DETAILWIN) y le pasa la ruta
                else if (this.ruta.Contains(".jpg"))
                {

                    DETAILWIN w1 = new DETAILWIN(this.ruta);
                    w1.Show();
                }
                else if (this.ruta.Contains(".mp4"))   
                {
                    DETAILWIN w1 = new DETAILWIN(this.ruta);
                    w1.Show();
                }
                else if (this.ruta.Contains(".md"))
                {
                    DETAILWIN w1 = new DETAILWIN(this.ruta);
                    w1.Show();
                }
            }
            else
            {

            }
        }
    }
}