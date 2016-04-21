using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace Renamer.Classes
{
    public class Parser
    {
        #region Data members
        private string m_rootPath;
        private Regex m_extract;
        private string m_referenceFile;
        private string m_idGroup;
        private ArrayList m_files;
        #endregion

        #region Public properties
        public string RootPath
        {
            get
            {
                return m_rootPath;
            }
            set
            {
                m_rootPath = value;
            }
        }

        public string ReferenceFile
        {
            get
            {
                return m_referenceFile;
            }
        }

        public string IdGroup
        {
            get
            {
                return m_idGroup;
            }
            set
            {
                m_idGroup = value;
            }
        }

        public ArrayList Files
        {
            get
            {
                return m_files;
            }
        }

        public Regex Extract
        {
            get
            {
                return m_extract;
            }
        }
        #endregion

        #region Constructors
        public Parser()
        {
            m_files = new ArrayList();
        }
        #endregion

        #region Public methods
        public void setReferenceFile(string _referenceFile)
        {
            if (File.Exists(_referenceFile))
                m_referenceFile = _referenceFile;
        }

        public void addFile(FileRename file)
        {
            m_files.Add(file);
        }

        public void removeFile(FileRename file)
        {
            m_files.Remove(file);
        }

        public void clearFiles()
        {
            m_files.Clear();
        }

        public void setParserRegularExpression(string _regularExpression)
        {
            m_extract = new Regex(_regularExpression);
        }

        public void parseDirectory(bool recursive)
        {
            Files.Clear();

            List<string> filePaths = new List<string>();

            if (Extract != null)
            {
                if (recursive)
                    filePaths.AddRange(recursiveParseDirectory(RootPath));
                else
                    filePaths.AddRange(Directory.GetFiles(RootPath, "*.*"));

                bool choice = true;

                foreach (string file in filePaths)
                {
                    Match match = Extract.Match(file);
                    FileRename fileRename = new FileRename(Path.GetFileName(file),  Path.GetFileName(file), file, !choice);
                    addFile(fileRename);
                    replaceOldForNewFilename(fileRename);
                }
            }
        }

        public void replaceOldForNewFilename(FileRename _file)
        {
            // string spacer;
            Match match = Extract.Match(_file.OldFilename);
            _file.Match = match.Success;

            MatchCollection myMatchCollection = Regex.Matches(_file.OldFilename, Extract.ToString());
            _file.Match = allMatches(myMatchCollection);

            if (_file.Match)
            {
                foreach (Match myMatch in myMatchCollection)
                {
                    foreach (Group myGroup in myMatch.Groups)
                    {
                        foreach (Capture myCapture in myGroup.Captures)
                        {
                            //_file.NewFilename = _file.NewFilename.Replace("<" + myGroup.ToString() + ">", myCapture.Value);
                        }

                        GroupCollection gc = myMatch.Groups;

                        // Itération sur les groupes de capture de Extract
                        for (int j = 0; j < gc.Count; j++)
                        {
                            spacer = " ";
                            //Console.WriteLine(spacer + "Group[" + j + "]: " + gc[j].Value);
                            //Console.WriteLine(spacer + "Printing captures for this group...");
                            CaptureCollection cc = gc[j].Captures;

                            for (int k = 0; k < cc.Count; k++)
                            {
                                spacer = "  ";
                                //Console.WriteLine(spacer + "Capture[" + k + "]: " + cc[k].Value);
                                _file.NewFilename = _file.NewFilename.Replace("<" + myGroup.ToString() + ">", cc[k].Value);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Private methods

        private bool allMatches(MatchCollection matches)
        {
            bool success = true;
            foreach (Match myMatch in matches)
            {
                if (!myMatch.Success)
                    success = false;
            }

            return success;
        }

        private List<string> recursiveParseDirectory(string _directory)
        {
            List<string> directoryPaths = new List<string>();
            List<string> filePaths = new List<string>();
            
            directoryPaths.AddRange(Directory.GetDirectories(_directory, "*.*"));

            foreach (string directory in directoryPaths)
            {
                try
                {
                    filePaths.AddRange(recursiveParseDirectory(directory));
                }
                catch (UnauthorizedAccessException)
                {
                    //TODO accumulation des erreurs d'accès
                }
                catch (Exception)
                { }
            }

            filePaths.AddRange(Directory.GetFiles(_directory, "*.*"));

            return filePaths;
        }
        #endregion
    }
}
