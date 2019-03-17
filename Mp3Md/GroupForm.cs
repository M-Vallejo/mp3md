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
    public partial class GroupForm : Form
    {
        private List<TagLib.File> Files;

        public GroupForm(List<TagLib.File> Files)
        {
            this.Files = Files;
            var path = Files.Select(x => Path.GetDirectoryName(x.Name)).OrderBy(x=>x.Length).First();//.Where(x => x.Count() == x.Max(y => y.Count())).FirstOrDefault();
            InitializeComponent();
            txtBasePath.Text = path;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int count = 2;
            if (int.TryParse(txtNumber.Text, out count) && count > 0)
            {
                string basePath = txtBasePath.Text;
                if (Directory.Exists(basePath))
                {/*
                    IEnumerable<IGrouping<string, TagLib.File>> group;
                    if (chkbGroupbyGenre.Checked)
                    {
                        if (chkbGroupByGenreFirst.Checked)
                        {
                           var group2 = Files.GroupBy(g => new { g.Tag.FirstGenre, g.Tag.FirstPerformer });
                        }
                        else
                        {
                            var group2 = Files.GroupBy(g => new { g.Tag.FirstPerformer, g.Tag.FirstGenre });
                        }
                    }
                    else
                    {
                        group = Files.GroupBy(a => a.Tag.FirstPerformer);
                    }
                    */

                    var Groups = Files.GroupBy(a => a.Tag.FirstPerformer);

                    foreach (var group in Groups)
                    {
                        if (group.Count() >= count)
                        {
                            string path = Path.Combine(basePath, group.Key);
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            foreach (var file in group)
                            {
                                string filepath = Path.Combine(path, Path.GetFileName(file.Name));
                                if (File.Exists(file.Name) && !File.Exists(filepath))
                                {
                                    File.Move(file.Name, filepath);
                                }
                            }
                        }
                    }

                    MessageBox.Show("Archivos agrupados.", "Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("La ruta ingresada no existe.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Debes ingresar un número válido mayor a cero.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkbGroupbyGenre_CheckedChanged(object sender, EventArgs e)
        {
            chkbGroupByGenreFirst.Enabled = ((CheckBox)sender).Checked;
            chkbGroupByGenreFirst.Checked = ((CheckBox)sender).Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSearhFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog of = new FolderBrowserDialog();
            if (of.ShowDialog() == DialogResult.OK)
            {
                txtBasePath.Text = of.SelectedPath;
            }
        }
    }
}
