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
            MessageBox.Show("Selected folder: " + _path, "WordCounter");
            _pathToFind = _path;
            pathLabel.Content = "Path: " + _pathToFind;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _wordToFind = TextBoxInput.Text;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pathLabel.Content = "Path: " + _pathToFind;
            wordLabel.Content = "Word: " + _wordToFind;

            DirectoryAccess da = new DirectoryAccess();
            da.SetInitialPath(_pathToFind);
            da.SetInitialWord(_wordToFind);

            this.updateDataGridUsingData(da);
        }


        private void updateDataGridUsingData(DirectoryAccess da)
        {
            var sortedDict = da.ReadAllLinesInFileUI();

            DataTable dt = new DataTable();
            DataColumn id = new DataColumn("Document Name", typeof(string));
            DataColumn name = new DataColumn("Occurences", typeof(string));
            DataColumn filePath = new DataColumn("File Path", typeof(string));

            dt.Columns.Add(id);
            dt.Columns.Add(name);
            dt.Columns.Add(filePath);

            foreach (var item in sortedDict)
            {
                Console.WriteLine(item.Key + " : " + item.Value + " occurrences");

                DataRow firstRow = dt.NewRow();
                firstRow[0] = item.Key;
                firstRow[1] = item.Value;
                firstRow[2] = _pathToFind + @"\" + item.Key;
                dt.Rows.Add(firstRow);

            }

            if (sortedDict.Count() == 0)
            {
                MessageBox.Show("There are no occurences with this word", "WordCounter", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            myDataGrid.ItemsSource = dt.DefaultView;
        }


        private void fillingDataGridUsingDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn id = new DataColumn("id",typeof(string));
            DataColumn name = new DataColumn("name", typeof(string));
            DataColumn filePath = new DataColumn("path", typeof(string));

            dt.Columns.Add(id);
            dt.Columns.Add(name);
            dt.Columns.Add(filePath);

            myDataGrid.ItemsSource = dt.DefaultView;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.fillingDataGridUsingDataTable();
        }
    }
}
