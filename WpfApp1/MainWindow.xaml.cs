using Microsoft.Win32;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Selecciona un archivo de datos CSV";
            ofd.Filter = "Todos los archivos (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (ofd.ShowDialog() == true)
            {
                ruta = ofd.FileName;
                MessageBox.Show("Archivo seleccionado: " + ruta);
            }

            if (File.Exists(this.ruta))
            {
                //MessageBox.Show(this.ruta.Contains(".txt") + " , contiene .txt");

                if (this.ruta.Contains(".txt"))
                {
                    string[] lineas = File.ReadAllLines(this.ruta);

                    textoBloque.Text = string.Join("\n", lineas);
                }
                else if (this.ruta.Contains(".jpg"))
                {
                    Window1 w1 = new Window1(this.ruta);
                    w1.Show();
                }
            }
            else
            {
                MessageBox.Show("El archivo no existe o la ruta no es válida.");
            }
        }


        string ruta;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(ruta, append: false))
            {
                sw.WriteLine(textoBloque.Text);

            }
        }

    }

}