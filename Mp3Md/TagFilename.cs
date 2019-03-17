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
    public partial class TagFilename : Form
    {
        public enum FileTagType
        {
            TagToFileName = 0,
            FileNameToTag = 1
        }

        public class WorkFiles
        {
            public string OldFileName { get; set; }
            public string NewFileName { get; set; }
        }

        private FileTagType type = FileTagType.FileNameToTag;
        private List<TagLib.File> Files;
        public List<WorkFiles> WorkedFiles { get; private set; }
        private TagLib.File examplefile;

        private string performer = "%artist%";
        private string album = "%album%";
        private string track = "%track%";
        private string year = "%year%";
        private string genre = "%genre%";
        private string title = "%title%";

        public TagFilename(List<TagLib.File> files, FileTagType type)
        {
            DialogResult = DialogResult.None;
            if (files.Count > 0)
            {
                InitializeComponent();

                WorkedFiles = new List<WorkFiles>();
                Files = files;
                examplefile = Files.FirstOrDefault();
                this.type = type;

                txtFormat.Text = "%artist% - %title%";

                switch (type)
                {
                    case FileTagType.TagToFileName:
                        this.Text = "Etiquetas a nombre de archivo";
                        break;
                    case FileTagType.FileNameToTag:
                        this.Text = "Nombre de archivo a etiquetas";
                        lblPreview.Text = examplefile.Name.GetFileNameWithoutExtension();
                        break;
                }
            }
            else
            {
                Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int count = 0;
            int exist = 0;
            int error = 0;

            Parallel.ForEach(Files, file =>
            {
                try
                {
                    string filename = file.Name.GetFileNameWithoutExtension();

                    switch (type)
                    {
                        case FileTagType.TagToFileName:
                            string newfilename = getFileNameByTag(txtFormat.Text, file) + Path.GetExtension(file.Name);
                            string basedirectory = Path.GetDirectoryName(file.Name);
                            string outpath = Path.Combine(basedirectory, newfilename);

                            if (!File.Exists(outpath) || (file.Name.ToLower() == outpath.ToLower()))
                            {
                                File.Move(file.Name, outpath);
                                WorkedFiles.Add(new WorkFiles
                                {
                                    OldFileName = file.Name,
                                    NewFileName = outpath
                                });
                                count++;
                            }
                            else
                            {
                                exist++;
                            }
                            break;
                        case FileTagType.FileNameToTag:
                            var fnspl = filename.Split('-').Select(x => { return x.Trim(); }).ToList();
                            var txtspl = txtFormat.Text.Split('-').Select(x => { return x.Trim(); }).ToList();

                            if (txtFormat.Text.Contains(performer))
                            {
                                var index = txtspl.IndexOf(performer);
                                file.Tag.Performers = new string[] { fnspl[index] };
                            }
                            if (txtFormat.Text.Contains(title))
                            {
                                var index = txtspl.IndexOf(title);
                                file.Tag.Title = fnspl[index];
                            }
                            if (txtFormat.Text.Contains(track))
                            {
                                var index = txtspl.IndexOf(track);
                                uint tr = 0;
                                uint.TryParse(fnspl[index], out tr);
                                
                                file.Tag.Track = tr;
                            }
                            file.Save();
                            count++;
                            break;

                        default:
                            break;
                    }
                }
                catch { error++; }
            });

            string message = count + " de " + Files.Count + " archivos editados";

            if (exist > 0)
            {
                message += ", " + exist + " ya existian";
            }
            if (error > 0)
            {
                message += ", " + error + " presentaron error.";
            }
            MessageBox.Show(message,"Información",MessageBoxButtons.OK,MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtFormat_TextChanged(object sender, EventArgs e)
        {
            switch (type)
            {
                case FileTagType.TagToFileName:
                    lblPreview.Text = getFileNameByTag(txtFormat.Text, examplefile);
                    break;
                case FileTagType.FileNameToTag:
                    break;
                default:
                    break;
            }
        }

        private string getFileNameByTag(string format, TagLib.File fl)
        {
            string filename = fl.Name.GetFileNameWithoutExtension();
            var tag = fl.Tag;
            string newfilename = txtFormat.Text
                    .Replace(title, tag.Title ?? "")
                    .Replace(performer, tag.FirstPerformer ?? "")
                    .Replace(album, tag.Album ?? "")
                    .Replace(year, tag.Year.ToString())
                    .Replace(track, tag.Track.ToString())
                    .Replace(genre, tag.FirstGenre ?? "");

            return newfilename;
        }

        
    }
}
