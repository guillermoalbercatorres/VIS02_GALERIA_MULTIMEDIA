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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*string txt = "Hola señor como esta usted";
            Window1 w = new Window1(txt);
            w.Show();*/
            string ruta = "C:\\Users\\Guillermo\\Desktop\\hola.txt";

            if (File.Exists(ruta))
            {
                string[] lineas = File.ReadAllLines(ruta);

                Console.WriteLine("---- Contenido del archivo ----");
                string text = "";

                foreach (string linea in lineas)
                {
                    text += lineas;
                    
                }

                

                textoBloque.Text= string.Join("\n", lineas);
            }

            }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // string ruta = "C:\\Users\\Guillermo\\Desktop\\hola.txt";
            string ruta = "C:\\Users\\raulms\\Desktop\\hola.txt";
            using (StreamWriter sw = new StreamWriter(ruta, append: false))
            {
                sw.WriteLine(textoBloque.Text);

            }
        }
    }

}