using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Renamer.Classes;
using System.IO;

namespace Renamer.Interfaces
{
    public partial class frmMain : Form
    {
        private Parser _parser;

        public Parser Parser
        {
            get
            {
                return _parser;
            }
        }

        public frmMain()
        {
            InitializeComponent();
            _parser = new Parser();
            initForm();
        }

        private void lblDirectory_Click(object sender, EventArgs e)
        {

        }

        private void grpParameters_Enter(object sender, EventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBrowseDirectory_Click(object sender, EventArgs e)
        {
            string directory = directoryChooser("Select the root directory to parse...");
            if (directory != "")
            {
                Parser.RootPath = directory;
                txtDirectory.Text = directory;
            }
        }

        private static string directoryChooser(string title)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dialog.Description = title;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            else
            {
                return "";
            }
        }

        private static string fileChooser(string title)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = title;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            else
            {
                return "";
            }
        }

        private void initForm()
        {
            txtExtract.Text = @"(?<group>[a-zA-Z0-9]+)";
            txtDirectory.Text = @"T:\";
            txtPut.Text = "<group>";
        }

        private void btnBrowseReferenceFile_Click(object sender, EventArgs e)
        {
            string file = fileChooser("Select the reference file...");
            if (file != "")
            {
                Parser.setReferenceFile(file);
                txtReferenceFile.Text = file;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Parser.RootPath))
            {
                Parser.parseDirectory(chkRecursive.Checked);
                refreshGrid();
            }
        }

        private void augmentBar(int currentNumber, int max)
        {
            int value = 0;
            decimal percentage;

            percentage = Utilities.CalculatePercentage(currentNumber, max);
            value = (int)Utilities.Trunc(percentage * 100);

            if (value > 0 && value <= 100)
            {
                barResults.Value = value;
                barResults.Refresh();
            }
        }

        private void refreshGrid()
        {
            grdResults.Rows.Clear();
            int currentNbFile = 1;


            foreach (FileRename file in Parser.Files)
            {
                grdResults.Rows.Add(file.Match, file.OldFilename, file.NewFilename);
                if (file.Match)
                    grdResults.Rows[grdResults.Rows.Count - 1].Cells[0].Style.BackColor = Color.LightGreen;
                else
                    grdResults.Rows[grdResults.Rows.Count - 1].Cells[0].Style.BackColor = Color.Red;

                augmentBar(currentNbFile, Parser.Files.Count);
                currentNbFile++;
            }

            barResults.Value = 0;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDirectory_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txtDirectory.Text))
                Parser.RootPath = txtDirectory.Text;
        }

        private void txtExtract_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Parser.setParserRegularExpression(txtExtract.Text);
                btnPreview.Enabled = true;
                btnRename.Enabled = true;
                lblExtract.ForeColor = Color.Black;
            }
            catch (ArgumentException)
            {
                btnPreview.Enabled = false;
                btnRename.Enabled = false;
                lblExtract.ForeColor = Color.Red;
            }
        }

        private void txtReference_TextChanged(object sender, EventArgs e)
        {
            //Parser.Reference = txtReference.Text;
        }

        private void txtReferenceFile_TextChanged(object sender, EventArgs e)
        {
            //if (File.Exists(txtReferenceFile.Text))
                //Parser.ReferenceFile = txtReferenceFile.Text;
        }
    }
}
