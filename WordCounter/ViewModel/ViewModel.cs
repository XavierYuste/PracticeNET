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
using System.ComponentModel;
using System.Threading;

namespace WordCounter
{
    static class Constants
    {
        /// <summary>
        /// Maximum index to show in the word counter.
        /// </summary>
        public const int MAX_INDEX = 20;
        /// <summary>
        /// Default index to show in the word counter.
        /// </summary>
        public const int DEFAULT_INDEX = 10;
    }

    class ViewModel : ViewModelBase.ViewModelBase
    {
        /// <summary>
        /// Word to find in the word counter.
        /// </summary>
        private string _wordToFind;
        public string WordToFind
        {
            get => _wordToFind;
            set => SetProperty(ref _wordToFind, value);
        }

        /// <summary>
        /// UI string used to show the word.
        /// </summary>
        private string _WordToFindUI = "Word: ";
        public string WordToFindUI
        {
            get => _WordToFindUI;
            set => SetProperty(ref _WordToFindUI, value);
        }

        /// <summary>
        /// Path to find the word in the word counter.
        /// </summary>
        public string _pathToFind { get; set; }

        /// <summary>
        /// UI string that shows the path to find the word in the word counter.
        /// </summary>
        private string _UIPath = "Path: ";
        public string UIPath 
        { 
            get => _UIPath;
            set => SetProperty(ref _UIPath, value);
        }

        /// <summary>
        /// UI string that indicates the count of files that wants to show.
        /// </summary>
        public string _countShow { get; set; }

        /// <summary>
        /// List of the search results.
        /// </summary>
        private List<Result> searchResults;
        public List<Result> SearchResults
        {
            get => searchResults;
            set => SetProperty(ref searchResults, value);
        }

        /// <summary>
        /// Delegate command for folder open function.
        /// </summary>
        private readonly DelegateCommand _onFolderOpen;

        /// <summary>
        /// ICommand for folder open function.
        /// </summary>
        public ICommand FolderOpenCommand => _onFolderOpen;

        /// <summary>
        /// Delegate command for the search word function.
        /// </summary>
        private readonly DelegateCommand _onSearchWord;

        /// <summary>
        /// ICommand for the search word function.
        /// </summary>
        public ICommand SearchWordCommand => _onSearchWord;

        /// <summary>
        /// Collection of the indexItems
        /// </summary>
        private ObservableCollection<int> _indexItems;

        /// <summary>
        /// UI reference for the index items of the number of files to show.
        /// </summary>
        public ObservableCollection<int> IndexItems
        {
            get { return _indexItems; }
            set { _indexItems = value; }
        }

        /// <summary>
        /// Index of the selected item.
        /// </summary>
        private int _selectedItem;

        /// <summary>
        /// UI reference for the index of the selected item.
        /// </summary>
        public int SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }

        /// <summary>
        /// Class that stores a result of a search.
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Name of the file.
            /// </summary>
            private string name;

            /// <summary>
            /// Public reference of the name.
            /// </summary>
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            /// <summary>
            /// Occurences of a word in a specific search.
            /// </summary>
            private int occurrences;

            /// <summary>
            /// Public reference of the occurences of a word in a specific search.
            /// </summary>
            public int Occurrences
            {
                get { return occurrences; }
                set { occurrences = value; }
            }

            /// <summary>
            /// File path of the searched file.
            /// </summary>
            private string filePath;

            /// <summary>
            /// Public reference of the file path of the searched file.
            /// </summary>
            public string FilePath
            {
                get { return filePath; }
                set { filePath = value; }
            }

            /// <summary>
            /// Constructor of a result.
            /// </summary>
            public Result(string n, int o, string p)
            {
                name = n;
                occurrences = o;
                filePath = p;
            }
        }

        /// <summary>
        /// Constructor of the view model.
        /// </summary>
        public ViewModel()
        {
            _onFolderOpen = new DelegateCommand(OnFolderOpen);
            _onSearchWord = new DelegateCommand(OnSearchWord);
            this.SetUpCountSelection();
        }

        /// <summary>
        /// Function On Folder Open
        /// </summary>
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

        /// <summary>
        /// Function that sets the initial path and the initial word for the directory access.
        /// </summary>
        private void OnSearchWord(object commandParameter)
        {
            if (_pathToFind == null || _pathToFind == "")
            {
                MessageBox.Show("The path has not been initialized", "WordCounter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (_wordToFind == null)
                {
                    MessageBox.Show("The word has not been initialized", "WordCounter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    UIPath = "Path: " + _pathToFind;
                    WordToFindUI = "Word: " + _wordToFind;

                    DirectoryAccess da = new DirectoryAccess();

                    da.CountShow = SelectedItem;
                    da.SetInitialPath(_pathToFind);
                    da.SetInitialWord(_wordToFind);
                    this.updateDataGridUsingData(da);
                }
            }
        }

        /// <summary>
        /// This function sets up the list of the count selection.
        /// </summary>
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

        /// <summary>
        /// This function sets up the list of binded results in the UI.
        /// </summary>
        private void updateDataGridUsingData(DirectoryAccess da)
        {
            this.SearchResults = null;
            List<Result> auxList = new List<Result>();
            var sortedDict = da.ReadAllLinesInFileUI();

            foreach (var item in sortedDict)
            {
                Console.WriteLine(item.Key + " : " + item.Value + " occurrences");
                Result r = new Result(item.Key, item.Value, _pathToFind + @"\" + item.Key);
                auxList.Add(r);
            }
            if (auxList.Count == 0)
            {
                MessageBox.Show("No occurrences found", "WordCounter");
            }
            SearchResults = auxList;
        }
    }
}