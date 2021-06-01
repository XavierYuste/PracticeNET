using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PracticeNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string wordToFind { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _ButtonText;
        public string ButtonText
        {
            get { return _ButtonText ?? (_ButtonText = "Add"); }
            set
            {
                _ButtonText = value;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string _path = "";
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                _path = dialog.SelectedPath.ToString();
            }
            MessageBox.Show(_path);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            wordToFind = TextBoxInput.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("La palabra a buscar es: " + wordToFind);
        }
    }
}
