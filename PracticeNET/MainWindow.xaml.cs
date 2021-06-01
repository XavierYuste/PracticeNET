using System;
using System.Collections.Generic;
using System.Data;
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
using YourAwesomeProject;

namespace PracticeNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string _wordToFind { get; set; }
        public string _pathToFind { get; set; }

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
            _pathToFind = _path;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _wordToFind = TextBoxInput.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("La palabra a buscar es: " + _wordToFind);
            MessageBox.Show("El path donde voy a buscar es: " + _pathToFind);
            DirectoryAccess da = new DirectoryAccess();
            da.SetInitialPath(_pathToFind);
            da.SetInitialWord(_wordToFind);
            da.GetAllDirectoryPathsUI();
            da.ReadAllLinesInFile();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void fillingDataGridUsingDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn id = new DataColumn("id",typeof(int));
            DataColumn name = new DataColumn("name", typeof(string));

            dt.Columns.Add(id);
            dt.Columns.Add(name);

            DataRow firstRow = dt.NewRow();
            firstRow[0] = 1;
            firstRow[1] = "hola";

            dt.Rows.Add(firstRow);

            myDataGrid.ItemsSource = dt.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.fillingDataGridUsingDataTable();
        }
    }
}
