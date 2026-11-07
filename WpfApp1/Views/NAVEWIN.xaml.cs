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

        public NAVEWIN()
        {
            // 1. Dibuja en pantalla los botones y cajas definidos en NAVEWIN.xaml
            InitializeComponent();

            TreeViewItem nodoRaiz = new TreeViewItem { Header = "Gestor de Archivos" };
            TreeViewItem nodoDesarrollo = new TreeViewItem { Header = "Imagenes" };
            nodoDesarrollo.Items.Add(new TreeViewItem
            {
                Header = "Imagen 1",
                Tag = ruta

            });
            nodoDesarrollo.Items.Add(new TreeViewItem { Header = "Imagen 2",
                Tag = ruta
            });
            nodoDesarrollo.Items.Add(new TreeViewItem { Header = "Imagen 3",
                Tag = ruta
            });

            TreeViewItem nodoSistemas = new TreeViewItem { Header = "Txt" };
            nodoSistemas.Items.Add(new TreeViewItem { Header = "Txt 1", Tag = ruta });
            nodoSistemas.Items.Add(new TreeViewItem { Header = "Txt 2", Tag = ruta });

            nodoRaiz.Items.Add(nodoDesarrollo);
            nodoRaiz.Items.Add(nodoSistemas);
            treeView.Items.Add(nodoRaiz);
        }

        // Al pulsar el botón "Añadir", sobrescribe el archivo de texto con lo que haya en la caja
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(ruta, append: false))
            {
                sw.WriteLine(textoBloque.Text); // Sobrescribe el archivo de texto con lo que haya en la caja
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(string.IsNullOrWhiteSpace(this.ruta))
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
            }
            // 4. Comprueba si el archivo existe en el disco duro
            if (File.Exists(this.ruta))
            {
                //MessageBox.Show(this.ruta.Contains(".txt") + " , contiene .txt");

                // Si el nombre contiene ".txt", lee todas las líneas y las escribe en el TextBox
                if (this.ruta.Contains(".txt"))
                {
                    string[] lineas = File.ReadAllLines(this.ruta);

                    textoBloque.Text = string.Join("\n", lineas);
                }
                // Si el nombre contiene ".jpg", crea la segunda ventana (DETAILWIN) y le pasa la ruta
                else if (this.ruta.Contains(".jpg"))
                {

                    DETAILWIN w1= null;
                    bool abriendoVentana = false;
                    if (w1 != null)
                    {
                        if (w1.IsVisible)
                        {
                            w1.Activate();
                            return;
                        }
                    }

                    if (!abriendoVentana)
                    {
                        abriendoVentana = true;

                        w1 = new DETAILWIN(this.ruta);
                        w1.Owner = this;

                        w1.Closed += (s, args) =>
                        {
                            w1 = null;
                            abriendoVentana = false;
                        };

                        w1.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("El archivo no existe o la ruta no es válida.");
            }
            this.ruta = "";
        }
    }
}