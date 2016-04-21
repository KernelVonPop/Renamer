using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renamer.Classes
{
    public class FileRename
    {
        #region Data members
        private bool m_match;
        private string m_oldFilename;
        private string m_newFilename;
        private string m_directory;
        #endregion

        #region Public properties
        public bool Match
        {
            get
            {
                return m_match;
            }
            set
            {
                m_match = value;
            }
        }

        public string OldFilename
        {
            get
            {
                return m_oldFilename;
            }
        }

        public string NewFilename
        {
            get
            {
                return m_newFilename;
            }
            set
            {
                m_newFilename = value;
            }
        }

        public string Directory
        {
            get
            {
                return m_directory;
            }
        }

        #endregion

        #region Constructors
        public FileRename(string _oldFilename, string _newFilename, string _directory, bool _choice)
        {
            m_oldFilename = _oldFilename;
            m_newFilename = _newFilename;
            m_directory = _directory;
            m_match = _choice;
        }
        #endregion

        #region Public methods
        #endregion

        #region Private methods
        #endregion
    }
}
