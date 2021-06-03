using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordCounter.Model.Logic
{
    /// <summary>
    /// The main thread program.
    /// </summary>
    public class DirectoryAccess
    {
        private string path;
        private string[] filePaths;
        private string wordToFind;
        private int countShow = 10;
        public string PathFile
        {
            get { return path; }
            set { path = value; }
        }
        public string WordToFind
        {
            get { return wordToFind; }
            set { wordToFind = value; }
        }

        public string[] FilePaths
        {
            get { return filePaths; }
            set { filePaths = value; }
        }

        public int CountShow
        {
            get { return countShow; }
            set { countShow = value; }
        }

        /// <summary>
        /// Initializes the first given path.
        /// </summary>
        public void SetInitialPath()
        {
            Console.WriteLine("Write the path: ");
            path = Console.ReadLine();

            bool isValid = false;
            while (!isValid)
            {
                try
                {
                    filePaths = Directory.GetFiles(path);
                    isValid = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Given path is not valid, please enter a valid path: ");
                    path = Console.ReadLine();
                }
            }
        }


        /// <summary>
        /// Initializes the first given path by parameter.
        /// </summary>
        public void SetInitialPath(string pathInitial)
        {
            bool isValid = false;
            while (!isValid)
            {
                try
                {
                    path = pathInitial;
                    Console.WriteLine("Llego: " + path);
                    filePaths = Directory.GetFiles(pathInitial);
                    isValid = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Given path is not valid, please enter a valid path: ");
                    pathInitial = Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Initializes the word.
        /// </summary>
        public void SetInitialWord()
        {
            Console.WriteLine("search> ");
            WordToFind = Console.ReadLine();
        }

        /// <summary>
        /// Initializes the word by parameter.
        /// </summary>
        public void SetInitialWord(string word)
        {
            WordToFind = word;
        }

        /// <summary>
        /// Gets all the directory paths.
        /// </summary>
        public void GetAllDirectoryPaths()
        {
            bool isValid = false;

            while (!isValid)
            {
                try
                {
                    Console.WriteLine(filePaths.Length + " files read in directory " + path);
                    isValid = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Given path is not valid");
                }
            }
        }

        /// <summary>
        /// Counts the words in the given files.
        /// </summary>
        public void ReadAllLinesInFile()
        {
            int noMatches = 0;
            Dictionary<string, int> fileCounter = new Dictionary<string, int>();

            foreach (string filePath in filePaths)
            {
                int count = File.ReadLines(filePath).Select(line => Regex.Matches(line, wordToFind).Count).Sum();
                if (count > 0)
                {
                    fileCounter.Add(Path.GetFileName(filePath), count);
                }
                else
                {
                    noMatches++;
                }
            }

            if (noMatches == filePaths.Length)
            {
                Console.WriteLine("no matches found");
            }
            else
            {
                var sortedDict = (from entry in fileCounter orderby entry.Value descending select entry).Take(countShow);

                foreach (var item in sortedDict)
                {
                    Console.WriteLine(item.Key + " : " + item.Value + " occurrences");
                }
            }

            System.Threading.Thread.Sleep(10000);

        }




        public string[] GetAllDirectoryPathsUI()
        {
            bool isValid = false;

            while (!isValid)
            {
                try
                {
                    Console.WriteLine(filePaths.Length + " files read in directory " + path);
                    isValid = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Given path is not valid");
                }
            }

            return filePaths;
        }

        /// <summary>
        /// Counts the words in the given files in UI.
        /// </summary>
        public IEnumerable<KeyValuePair<string,int>> ReadAllLinesInFileUI()
        {
            IEnumerable<KeyValuePair<string, int>> sortedDict = new Dictionary<string, int>();

            int noMatches = 0;
            Dictionary<string, int> fileCounter = new Dictionary<string, int>();

            foreach (string filePath in filePaths)
            {
                int count = File.ReadLines(filePath).Select(line => Regex.Matches(line, wordToFind).Count).Sum();
                if (count > 0)
                {
                    fileCounter.Add(Path.GetFileName(filePath), count);
                }
                else
                {
                    noMatches++;
                }
            }

            if (noMatches == filePaths.Length)
            {
                Console.WriteLine("no matches found");
            }
            else
            {
                sortedDict = (from entry in fileCounter orderby entry.Value descending select entry).Take(countShow);

                foreach (var item in sortedDict)
                {
                    Console.WriteLine(item.Key + " : " + item.Value + " occurrences");
                }
            }

            return sortedDict;
        }
    }

    public class DirectoryDirector
    {
        /// <summary>
        /// Builds the builder.
        /// </summary>
        public void Construct(Builder builder)
        {
            builder.BuildPath();
            builder.BuildWord();
            builder.BuildAllDirectoryPaths();
            builder.BuildAllLinesInFile();
        }
    
    }

    /// <summary>
    /// Abstract Builder class to have the DirectoryAccess.
    /// </summary>
    public abstract class Builder
    {
        public abstract void BuildPath();

        public abstract void BuildWord();

        public abstract void BuildAllDirectoryPaths();

        public abstract void BuildAllLinesInFile();

        public abstract DirectoryAccess GetResult();

    }

    /// <summary>
    /// Builds the constructor of the DirectoryAccess.
    /// </summary>
    public class BuilderDirectoryAccess : Builder
    {
        private string path;
        private string word;

        public BuilderDirectoryAccess(string pathToFind, string wordToFind)
        {
            path = pathToFind;
            word = wordToFind;
        }


        private DirectoryAccess _directory = new DirectoryAccess();

        public override void BuildPath()
        {
            _directory.SetInitialPath(path);
        }

        public override void BuildWord()
        {
            _directory.SetInitialWord(word);
        }

        public override void BuildAllDirectoryPaths()
        {
            _directory.GetAllDirectoryPaths();
        }

        public override void BuildAllLinesInFile()
        {
            _directory.ReadAllLinesInFile();
        }

        public override DirectoryAccess GetResult()
        {
            return _directory;
        }

    }

}
