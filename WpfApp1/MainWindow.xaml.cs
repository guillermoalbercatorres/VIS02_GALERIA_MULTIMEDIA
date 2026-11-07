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
        }


        string ruta;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(this.ruta))
            {
                MessageBox.Show(this.ruta.Contains(".txt")+" , contiene .txt");

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.ruta))
            using (StreamWriter sw = new StreamWriter(ruta, append: false))
            {
                MessageBox.Show("Primero introduce una ruta.");
                return;
            }

            File.WriteAllText(this.ruta, textoBloque.Text);

            MessageBox.Show("Archivo guardado correctamente.");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // C:\\Users\\Guillermo\\Desktop\\hola.txt

            this.ruta = ModificarRuta.Text.Trim();

            MessageBox.Show("Ruta establecida: " + this.ruta);
        }

    }

}