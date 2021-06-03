using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using ViewModelBase;
using YourAwesomeProject;
using WordCounter.Model;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace WordCounter
{
    static class Constants
    {
        public const int MAX_INDEX = 20;
        public const int DEFAULT_INDEX = 10;
    }

    class ViewModel : ViewModelBase.ViewModelBase
    {
        private string _wordToFind;
        public string WordToFind
        {
            get => _wordToFind;
            set => SetProperty(ref _wordToFind, value);
        }


        private string _WordToFindUI = "Word: ";
        public string WordToFindUI
        {
            get => _WordToFindUI;
            set => SetProperty(ref _WordToFindUI, value);
        }

        public string _pathToFind { get; set; }


        private string _UIPath = "Path: ";
        public string UIPath 
        { 
            get => _UIPath;
            set => SetProperty(ref _UIPath, value);
        }
        public string _countShow { get; set; }


        private string _palabra;
        public string palabra
        {
            get => _palabra;
            set => SetProperty(ref _palabra, value);
        }

        private string valor1 = "Hola";
        public string Valor1
        {
            get => valor1;
            set => SetProperty(ref valor1, value);
        }


        private List<Result> mysampleGrid;
        public List<Result> MySampleGrid
        {
            get => mysampleGrid;
            set => SetProperty(ref mysampleGrid, value);
        }



        private readonly DelegateCommand _changeNameCommand;
        public ICommand ChangeNameCommand => _changeNameCommand;

        private readonly DelegateCommand _onFolderOpen;
        public ICommand FolderOpenCommand => _onFolderOpen;

        private readonly DelegateCommand _onSearchWord;
        public ICommand SearchWordCommand => _onSearchWord;

        public ViewModel()
        {
            _onFolderOpen = new DelegateCommand(OnFolderOpen);
            _changeNameCommand = new DelegateCommand(OnChangeName);
            _onSearchWord = new DelegateCommand(OnSearchWord);
            this.SetUpCountSelection();
        }

        private void OnChangeName(object commandParameter)
        {
            palabra = _wordToFind;
        }



        private void OnFolderOpen(object commandParameter)
        {
            string _path = "";
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                _path = dialog.SelectedPath.ToString();
            }
            MessageBox.Show("Selected folder: " + _path, "WordCounter");
            _pathToFind = _path;
            UIPath = "Path: " + _pathToFind;
            _WordToFindUI = "Word: ";
        }



        private void OnSearchWord(object commandParameter)
        {
            UIPath = "Path: " + _pathToFind;
            WordToFindUI = "Word: " + _wordToFind;

            DirectoryAccess da = new DirectoryAccess();

            da.CountShow = SelectedItem;
            da.SetInitialPath(_pathToFind);
            da.SetInitialWord(_wordToFind);

            this.updateDataGridUsingData(da);
        }


        private ObservableCollection<int> _indexItems;

        public ObservableCollection<int> IndexItems
        {
            get { return _indexItems; }
            set { _indexItems = value; }
        }
        private int _selectedItem;

        public int SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }





        private void SetUpCountSelection()
        {
            ObservableCollection<int> auxList = new ObservableCollection<int>();
            for (int i = 1; i <= Constants.MAX_INDEX; i++)
            {
                auxList.Add(i);
            }
            IndexItems = auxList;
            SelectedItem = Constants.DEFAULT_INDEX;
        }


        public class Result
        {
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            private int occurrences;
            public int Occurrences
            {
                get { return occurrences; }
                set { occurrences = value; }
            }

            private string filePath;
            public string FilePath
            {
                get { return filePath; }
                set { filePath = value; }
            }

            public Result(string n, int o, string p)
            {
                name = n;
                occurrences = o;
                filePath = p;
            }
        }





        private void updateDataGridUsingData(DirectoryAccess da)
        {
            this.MySampleGrid = null;


            List<Result> auxList = new List<Result>();
            var sortedDict = da.ReadAllLinesInFileUI();
            foreach (var item in sortedDict)
            {
                Console.WriteLine(item.Key + " : " + item.Value + " occurrences");
                Result r = new Result(item.Key, item.Value, _pathToFind + @"\" + item.Key);
                auxList.Add(r);
            }

            MySampleGrid = auxList;
        }
    }
}
