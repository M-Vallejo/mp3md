using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mp3Md
{
    public partial class FindFiles : Form
    {
        public List<TagLib.File> DuplicatedFiles { get; private set; } = new List<TagLib.File>();

        public FindFiles()
        {
            this.DialogResult = DialogResult.None;
            InitializeComponent();
        }

        public FindFiles(IEnumerable<string> files)
        {
            InitializeComponent();
            Compare(files.ToArray());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchForFiles();
        }

        private void SearchForFiles(bool browseFolder = true)
        {
            if (!browseFolder)
            {
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.Filter = "Audio | " + Form1.GetFilter();
                //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var files = dialog.FileNames;
                    Compare(files);
                    this.Close();
                }
            }
            else
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var searchedFiles = Directory.EnumerateFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories).Where(f =>
                    {
                        if (File.Exists(f))
                        {
                            string ext = Path.GetExtension(f).ToLower();
                            return Form1.SupportedFiles.Contains(ext);
                        }
                        return false;
                    }).ToArray();
                    Compare(searchedFiles);
                    this.Close();
                }
            }
        }

        private void Compare(string[] files)
        {
            List<TagLib.File> Files = new List<TagLib.File>();
            System.Diagnostics.Stopwatch stw = new System.Diagnostics.Stopwatch();
            stw.Start();
            Files = files.Where(path=> File.Exists(path)).AsParallel().Select(path => 
            {
                TagLib.File filedata = TagLib.File.Create(path);
                filedata.Tag.Title = filedata.Tag.Title != null ? filedata.Tag.Title.Trim().ToLower() : "Unknow";
                filedata.Tag.Performers = filedata.Tag.FirstPerformer != null ? new string[] { filedata.Tag.FirstPerformer.Trim().ToLower() } : new string[] { "Unknow" };
                return filedata;
            }).ToList();
            /*
            var tm = stw.ElapsedMilliseconds;
            Files = new List<TagLib.File>();
            stw.Restart();

            foreach (var path in files)
            {
                if (File.Exists(path))
                {
                    TagLib.File filedata = TagLib.File.Create(path);
                    filedata.Tag.Title = filedata.Tag.Title != null ? filedata.Tag.Title.Trim().ToLower() : "Unknow";
                    filedata.Tag.Performers = filedata.Tag.FirstPerformer != null ? new string[] { filedata.Tag.FirstPerformer.Trim().ToLower() } : new string[] { "Unknow" };

                    Files.Add(filedata);
                }
            }
            var m = stw.ElapsedMilliseconds;
            stw.Stop();
            */
            List<TagLib.File> duplicates;

            if (chkbMatchArtist.Checked)
            {
                duplicates = Files.AsParallel().Where(x => 
                    Files.Where(y => 
                        y.Tag.Title == x.Tag.Title
                        && y.Tag.FirstPerformer == x.Tag.FirstPerformer
                    ).Count() > 1
                ).ToList();
            }
            else
            {
                duplicates = Files.AsParallel().Where(x =>
                    (Files.Where(y => y.Tag.Title == x.Tag.Title).Count() > 1)
                ).ToList();
            }

            if (duplicates.Count() == 0)
            {
                MessageBox.Show("No se han encontrado archivos duplicados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }

            this.DuplicatedFiles = duplicates.OrderBy(x => x.Tag.Title).ThenBy(x=>x.Tag.Title).ToList();
        }

        private void FindFiles_Load(object sender, EventArgs e)
        {

        }
    }
}
