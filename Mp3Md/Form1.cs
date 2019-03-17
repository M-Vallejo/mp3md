using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mp3Md
{
    public partial class Form1 : Form
    {
        private List<string> _allfiles = new List<string>();
        public List<string> AllFiles
        {
            get
            {
                return _allfiles;
            }
            set
            {
                _allfiles = value;
                AddFileToListView(value,false);
            }
        }
        public List<TagLib.File> EditingFiles = new List<TagLib.File>();

        public static string[] SupportedFiles = { ".mp3",".m4a",".wma" };
        public static string[] SuppurtedImages = { ".jpg ",".jpeg"};

        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lvMetaData_DragEnter(object sender, DragEventArgs e)
        {

            var files = ((string[])e.Data.GetData(DataFormats.FileDrop));
            if (files.Any(x => SupportedFiles.Contains(Path.GetExtension(x).ToLower()) || File.GetAttributes(x).HasFlag(FileAttributes.Directory)))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lvMetaData_DragDrop(object sender, DragEventArgs e)
        {
            lblStatus.Text = "Buscando archivos...";
            //Obtenemos todos loas archivos arrastrados al control.
            var files = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();
            //Filtramos y almacenamos las carpetas.
            var directories = files.Where(x=>File.GetAttributes(x).HasFlag(FileAttributes.Directory)).ToList();
            //Eliminamos las carpetas de la lista y nos quedamos solo con los archivos.
            files.RemoveAll(x => File.GetAttributes(x).HasFlag(FileAttributes.Directory));

            foreach (var dir in directories)
            {
                var searchedFiles = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories).Where(f => 
                {
                    string ext = Path.GetExtension(f).ToLower();
                    return SupportedFiles.Contains(ext);
                });
                files.AddRange(searchedFiles);
            }

            lblStatus.Text = files.Count + " archivos encontrados";

            AddFileToListView(files);
        }

        /// <summary>
        /// Método que actualiza en el ListView solo los valores necesario y evita recargar todos los elementos de la lista.
        /// </summary>
        /// <param name="FilesToUpdate"></param>
        private void UpdateListView(List<TagLib.File> FilesToUpdate, bool isUpdate = true)
        {
            //Filtra las filas que coincidan con los elementos a actualizar.
            var rows = lvMetaData.Items.Cast<ListViewItem>().Where(y => FilesToUpdate.Contains((y.Tag as TagLib.File))).ToList();
            lvMetaData.BeginUpdate();

            //Recorre cada una de las filas a actualizar.
            foreach (var row in rows)
            {
                //Busca el indice del elemento actual de la lista, el que se desea actualiar.
                int index = lvMetaData.Items.IndexOf(row);
                lvMetaData.Items.RemoveAt(index);

                if (isUpdate)
                {
                    //Optiene la ruta del archivo editado.
                    string path = (row.Tag as TagLib.File).Name;
                    if (File.Exists(path))
                    {
                        //Lee los metadatos actualizados del archivo y con estos crea un nuevo item para el listView.
                        TagLib.File filedata = TagLib.File.Create(path);
                        var item = GetTagFileToListView(filedata);
                        //Actualiza el registro viejo con los metadatos nuevos.
                        
                        lvMetaData.Items.Insert(index, item);
                    }
                }
            }

            lvMetaData.EndUpdate();
        }

        /// <summary>
        /// Forma un nuevo ListViewitem a travez de un archivo de metadato.
        /// </summary>
        /// <param name="filedata">Conjunto de metadatos del archivo de audio.</param>
        /// <returns>un Item que se puede agragar a un ListView.</returns>
        ListViewItem GetTagFileToListView(TagLib.File filedata)
        {
            FileInfo fileInfo = new FileInfo(filedata.Name);

            var duration = filedata.Properties.Duration;
            string durationString = $"{duration.Hours.ToString("00")}:{duration.Minutes.ToString("00")}:{duration.Seconds.ToString("00")}";

            var fileSize = (fileInfo.Length / 1024.00 / 1024.00).ToString("0.00MB");
            ListViewItem Row = new ListViewItem(Path.GetFileName(filedata.Name));
            Row.SubItems.Add(filedata.Tag.Title);
            Row.SubItems.Add(filedata.Tag.Performers.FirstOrDefault());
            Row.SubItems.Add(filedata.Tag.Album);
            Row.SubItems.Add(filedata.Tag.FirstGenre);
            Row.SubItems.Add(filedata.Tag.Track.ToString());
            Row.SubItems.Add(filedata.Tag.Year.ToString());
            Row.SubItems.Add(fileSize);
            Row.SubItems.Add(filedata.Properties.AudioBitrate.ToString() + "kbps");
            Row.SubItems.Add(filedata.Properties.AudioSampleRate.ToString() + "Hz");
            Row.SubItems.Add(durationString);
            Row.SubItems.Add(filedata.Name);

            Row.Tag = filedata;

            return Row;
        }

        private void AddFileToListView(List<string> tmpfiles, bool showmessage = true)
        {
            lblStatus.Text = "Mostrando archivos";
            var allimportedfiles = tmpfiles.Count();
            var files = tmpfiles.Where(x => SupportedFiles.Contains(Path.GetExtension(x).ToLower())).OrderBy(x => x).ToList();

            _allfiles = files;
            lblFilesCount.Text = AllFiles.Count + " archivos";
            lvMetaData.BeginUpdate();
            lvMetaData.Items.Clear();
            int count = 0;

            pgStatus.Maximum = files.Count;
            pgStatus.Value = 0;
            foreach (var path in files)
            {
                try
                {
                    TagLib.File filedata = TagLib.File.Create(path);
                    var item = GetTagFileToListView(filedata);
                    lvMetaData.Items.Add(item);
                    count++;
                    pgStatus.Value = count;
                    lblStatus.Text = "Mostrando " + count + " de " + files.Count;
                }
                catch
                {
                    continue;
                }
            }
            lvMetaData.EndUpdate();
            if (showmessage)
            {
                MessageBox.Show(count + " de " + allimportedfiles + " archivos importados.", "Importación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            lblStatus.Text = "";
        }

        private void ShowInEdit()
        {
            ClearEditFields();

            if (lvMetaData.SelectedItems.Count == 1)
            {
                TagLib.File filedata = (TagLib.File)lvMetaData.SelectedItems[0].Tag;
                EditingFiles = new List<TagLib.File> { filedata };

                var metadata = filedata.Tag;

                if (metadata.Title != null) { cbTitle.Items.Add(metadata.Title); cbTitle.Text = metadata.Title; }

                if (metadata.FirstPerformer != null) { cbArtist.Items.Add(metadata.FirstPerformer); cbArtist.Text = metadata.FirstPerformer; }

                if (metadata.Album != null) { cbAlbum.Items.Add(metadata.Album); cbAlbum.Text = metadata.Album; }

                if (metadata.FirstGenre != null) { cbGenre.Items.Add(metadata.FirstGenre); cbGenre.Text = metadata.FirstGenre; }

                if (metadata.Track != 0) { cbTrack.Items.Add(metadata.Track); cbTrack.Text = metadata.Track.ToString(); }

                if (metadata.Year != 0) { cbYear.Items.Add(metadata.Year); cbYear.Text = metadata.Year.ToString(); }

                if (metadata.Pictures.Length > 0)
                {
                    try
                    {
                        var bin = metadata.Pictures[0].Data.Data;
                        if (bin.Length > 0)
                        {
                            // pbCover.Image = Image.FromStream(new MemoryStream(bin)).GetThumbnailImage(100, 100, null, IntPtr.Zero);
                            pbCover.Image = Image.FromStream(new MemoryStream(bin));
                        }
                    }
                    catch
                    {
                        pbCover.Image = null;
                    }
                }
            }
            else if (lvMetaData.SelectedItems.Count > 0)
            {
                DefaultValues();

                var values = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X=>(TagLib.File)X.Tag);

                EditingFiles = values.ToList();

                var titles = values.Select(x => x.Tag.Title).Where(x => x != null && x.Trim() != "").Distinct().OrderBy(x=>x).ToArray();
                var artists = values.Select(x => x.Tag.FirstPerformer).Where(x => x != null && x.Trim() != "").Distinct().OrderBy(x => x).ToArray();
                var albums = values.Select(x => x.Tag.Album).Where(x => x != null && x.Trim() != "").Distinct().OrderBy(x => x).ToArray();
                var years = values.Select(x => x.Tag.Year).Where(x => x != 0).Select(x => x).Distinct().OrderBy(x => x).ToArray();
                var tracks = values.Select(x => x.Tag.Track).Where(x => x != 0).Select(x => x).Distinct().OrderBy(x => x).ToArray();
                var genres = values.Select(x => x.Tag.FirstGenre).Where(x => x != null && x.Trim() != "").Distinct().OrderBy(x => x).ToArray();
                
                cbTitle.Items.AddRange(titles);
                cbArtist.Items.AddRange(artists);
                cbAlbum.Items.AddRange(albums);
                cbYear.Items.AddRange(years.Select(x=>x.ToString()).ToArray());
                cbTrack.Items.AddRange(tracks.Select(x => x.ToString()).ToArray());
                cbGenre.Items.AddRange(genres);

                if (titles.Length == 1){cbTitle.Text = titles.First();}
                if (artists.Length == 1) { cbArtist.Text = artists.First(); }
                if (albums.Length == 1) { cbAlbum.Text = albums.First(); }
                if (years.Length == 1) { cbYear.Text = years.First().ToString(); }
                if (tracks.Length == 1) { cbTrack.Text = tracks.First().ToString(); }
                if (genres.Length == 1) { cbGenre.Text = genres.First(); }


            }
        }


        private void ClearEditFields()
        {
            cbTitle.Items.Clear();
            cbTitle.Text = "";
            cbArtist.Items.Clear();
            cbArtist.Text = "";
            cbAlbum.Items.Clear();
            cbAlbum.Text = "";
            cbYear.Items.Clear();
            cbYear.Text = "";
            cbTrack.Items.Clear();
            cbTrack.Text = "";
            cbTrack.Items.Clear();
            cbTrack.Text = "";
            cbGenre.Items.Clear();
            cbGenre.Text = "";
            cbCover.Items.Clear();
            cbCover.Text = "";
            pbCover.Image = null;
        }

        private void DefaultValues()
        {
            addDefaultValuestoCombo(cbTitle);
            addDefaultValuestoCombo(cbArtist);
            addDefaultValuestoCombo(cbAlbum);
            addDefaultValuestoCombo(cbYear);
            addDefaultValuestoCombo(cbTrack);
            addDefaultValuestoCombo(cbGenre);
            addDefaultValuestoCombo(cbCover);
        }

        private void addDefaultValuestoCombo(ComboBox combo)
        {
            combo.Items.Add("<mantener>");
            combo.Text = "<mantener>";
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowInEdit();
        }

        private void lvMetaData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMetaData.SelectedItems.Count == 1)
            {
                ShowInEdit();
            }
        }

        private void lvMetaData_DoubleClick(object sender, EventArgs e)
        {
            ShowInEdit();
        }

        private void lvMetaData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowInEdit();
            }
        }

        private void btnToolSave_Click(object sender, EventArgs e)
        {
            //var values = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X => (TagLib.File)X.Tag);
            var values = EditingFiles;
            string keepname = "<mantener>";
            int count = 0;
            pgStatus.Maximum = values.Count;
            pgStatus.Value = 0;

            string title = cbTitle.Text.Trim();
            string performer = cbArtist.Text.Trim();
            string albm = cbAlbum.Text.Trim();
            string ye = cbYear.Text.Trim();
            string tra = cbTrack.Text.Trim();
            string gen = cbGenre.Text.Trim();

            Object countLock = new Object();
            string imagePath = pbCover.Image?.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(imagePath) && pbCover.Image != null)
            {
                imagePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".jpg");
                pbCover.Image.Save(imagePath);
            }
            Parallel.ForEach(values, file =>
            {
                try
                {
                    file.Tag.Title = title != keepname ? title : file.Tag.Title;
                    file.Tag.Performers = performer != keepname ? file.Tag.Performers = new string[] { performer } : file.Tag.Performers;
                    file.Tag.Album = albm != keepname ? albm : file.Tag.Album;
                    file.Tag.Year = (ye != keepname && !String.IsNullOrEmpty(ye)) ? file.Tag.Year = Convert.ToUInt32(ye) : file.Tag.Year;
                    file.Tag.Track = (tra != keepname && !String.IsNullOrEmpty(tra)) ? file.Tag.Track = Convert.ToUInt32(tra) : file.Tag.Track;
                    file.Tag.Genres = gen != keepname ? file.Tag.Genres = new string[] { gen } : file.Tag.Genres;

                    #region Image

                    if (!string.IsNullOrWhiteSpace(imagePath))
                    {
                        TagLib.Picture pic = new TagLib.Picture
                        {
                            Type = TagLib.PictureType.FrontCover,
                            Description = "Cover",
                            MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                            Data = TagLib.ByteVector.FromPath(imagePath)
                        };
                        file.Tag.Pictures = new TagLib.IPicture[] { pic };
                    }
                    else
                    {
                        // file.Tag.Pictures = null;
                    }

                    #endregion

                    file.Save();
                    count++;
                    //lock (countLock) { pgStatus.Value = count; lblStatus.Text = "Guardando: " + count + " de " + values.Count; }
                }
                catch (Exception m)
                {
                    string a = m.Message;
                }
            });
            if (!string.IsNullOrWhiteSpace(imagePath)) { File.Delete(imagePath); }
            UpdateListView(EditingFiles);
            //AddFileToListView(AllFiles,false);
            
            MessageBox.Show(count + " archivos editados","Editar",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void lvMetaData_Click(object sender, EventArgs e)
        {
            if (lvMetaData.SelectedItems.Count != 1)
            {
                ClearEditFields();
            }
        }

        private void btnToolRefresh_Click(object sender, EventArgs e)
        {
            AddFileToListView(AllFiles,false);
        }

        private void mAYUSCULASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var values = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X => (TagLib.File)X.Tag);
            _allfiles = AllFiles.Select(x => x.ToUpper()).ToList();
            int count = 0;
            foreach (var file in values)
            {
                try
                {
                    string newname = file.Name.ToUpper();
                    File.Move(file.Name, newname);
                    count++;
                }
                catch { }
            }
            AddFileToListView(AllFiles, false);
            MessageBox.Show(count + " nombre de archivos editados", "Cambiar nombre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void minusculasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var values = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X => (TagLib.File)X.Tag);
            _allfiles = AllFiles.Select(x => x.ToLower()).ToList();
            int count = 0;
            foreach (var file in values)
            {
                try
                {
                    string newname = file.Name.ToLower();
                    File.Move(file.Name, newname);
                    count++;
                }
                catch { }
            }
            AddFileToListView(AllFiles,false);
            MessageBox.Show(count + " nombre de archivos editados", "Cambiar nombre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnToolOpen_Click(object sender, EventArgs e)
        {
            SearchForFiles();
        }

        private void SearchForFiles()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            

            dialog.Filter = "Audio | " + GetFilter();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var files = dialog.FileNames.Cast<string>().ToList();
                AddFileToListView(files);
            }

        }

        public static string GetFilter(bool isWindows = false)
        {
            string filter = "";

            foreach (var sup in SupportedFiles)
            {
                if (filter == "")
                {
                    filter = isWindows ? $"Ext:{sup}" : "*" + sup;
                }
                else
                {
                    filter += isWindows ? $" OR Ext:{sup}" : "; *" + sup;
                }
            }
            return filter;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            pbCover.AllowDrop = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.E || e.KeyCode == Keys.A))
            {
                lvMetaData.BeginUpdate();
                lvMetaData.Items.Cast<ListViewItem>().Select(x => x.Selected = true).ToList();
                lvMetaData.EndUpdate();
                //lvMetaData.Items.Cast<ListViewItem>().Select(x => { x.Selected = true; return x; }).ToList();
            }
        }

        private void btnFileNameToTag_Click(object sender, EventArgs e)
        {
            OpenTagFileName(TagFilename.FileTagType.FileNameToTag);
        }

        private void btnTagToFileName_Click(object sender, EventArgs e)
        {
            OpenTagFileName(TagFilename.FileTagType.TagToFileName);
        }

        private void OpenTagFileName( TagFilename.FileTagType type)
        {
            var files = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X => (TagLib.File)X.Tag).ToList();
            if (files.Count > 0)
            {
                TagFilename tf = new TagFilename(files, type);
                if (tf.ShowDialog() == DialogResult.OK)
                {
                    /*
                    List<TagLib.File> WorkedFiles = new List<TagLib.File>();
                    foreach (var wf in tf.WorkedFiles)
                    {
                        var filedata = TagLib.File.Create(wf.NewFileName);
                        WorkedFiles.Add(filedata);
                    }
                    UpdateListView(WorkedFiles);
                    */
                    AllFiles = AllFiles.Select(file => { file = findworkedfiles(tf.WorkedFiles,file) ?? file ; return file; }).ToList();
                }
            }
        }

        private string findworkedfiles(List<TagFilename.WorkFiles> workedfiles,string file)
        {
            var tmp = workedfiles.Find(x => x.OldFileName == file);
            return tmp != null ? tmp.NewFileName : file;
        }

        private void btnSearchCover_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JPG | *.jpg; *.jpeg";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadCoverFromFile(dialog.FileName);
            }            
        }

        private void reproducirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var value = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X => (TagLib.File)X.Tag);

            if (value.Count() > 0)
            {
                //ProcessStartInfo startInfo = new ProcessStartInfo("wmplayer.exe", value.First().Name);
                //Process.Start(startInfo);
                string filepath = value.FirstOrDefault().Name;
                if (File.Exists(filepath))
                {
                    Process.Start(filepath);
                }
            }

        }

        private void lvMetaData_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (!e.CancelEdit && !String.IsNullOrEmpty(e.Label))
            {
                try
                {
                    var item = lvMetaData.Items[e.Item];
                    var file = (TagLib.File)item.Tag;

                    string outpath = Path.Combine(Path.GetDirectoryName(file.Name),e.Label);
                    File.Move(file.Name,outpath);
                    AllFiles.RemoveAll(x => x == file.Name);
                    AllFiles.Add(outpath);
                }
                catch (Exception m)
                {
                    e.CancelEdit = true;
                    MessageBox.Show(m.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

            }
        }

        public void LoadCoverFromFile(string path)
        {
            pbCover.Image = Image.FromFile(path);
            pbCover.Image.Tag = path;
        }

        private void pbCover_DragEnter(object sender, DragEventArgs e)
        {
            var files = ((string[])e.Data.GetData(DataFormats.FileDrop));

            if (files.Count() == 1 && !files.Any(x => SuppurtedImages.Contains(Path.GetExtension(x).ToLower())))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pbCover_DragDrop(object sender, DragEventArgs e)
        {
            var files = ((string[])e.Data.GetData(DataFormats.FileDrop));
            if (files.Count() == 1 && !files.Any(x => SuppurtedImages.Contains(Path.GetExtension(x).ToLower())))
            {
                var fl = files.First();
                LoadCoverFromFile(fl);

            }
        }

        private void nombreNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeNormalName();
        }

        private void ChangeNormalName()
        {
            var values = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X => (TagLib.File)X.Tag).ToList();
            int count = 0;
            AllFiles = values.Select(file =>
            {
                try
                {
                    file.Tag.Title = file.Tag.Title.Trim().ToUpperFirst();
                    if (file.Tag.FirstPerformer != null)
                    {
                        file.Tag.Performers = new string[] { file.Tag.FirstPerformer.Trim() };
                    }
                    //file.Tag.Album = file.Tag.Album.ToUpperFirst();
                    file.Save();
                    count++;
                }
                catch { }
                return file;
            }).AsParallel().Select(x => x.Name).ToList();
            MessageBox.Show(count + " nombre de archivos editados", "Cambiar nombre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string NormalContersion(string path,string splitter = "-",int artistPosition = 0)
        {
            string name = Path.GetFileName(path).ToLower();
            string result = "";
            if (splitter != "")
            {
                var textos = name.Split(char.Parse(splitter));

                for (int i = 0; i < textos.Length; i++)
                {
                    string text = textos[i];

                    if (i == artistPosition)
                    {
                        result += text.ToUpperCammel();
                    }
                    else
                    {
                        result += text.ToUpperFirst();
                    }

                    if (text != textos.Last())
                    {
                        result += splitter + " ";
                    }
                }
            }

            string outpath = Path.Combine(Path.GetDirectoryName(path), result);
            return outpath; ;
        }

        private void intercaladoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var values = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X => (TagLib.File)X.Tag);
            int count = 0;

            values = values.Select(file => 
            {
                try
                {
                    file.Tag.Title = file.Tag.Title.ToUpperCammel();
                    file.Save();
                    count++;
                }
                catch {}
                return file;
            });

            AllFiles = values.Select(x=>x.Name).ToList();
            MessageBox.Show(count + " nombre de archivos editados", "Cambiar nombre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void seleccionarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvMetaData.BeginUpdate();
            lvMetaData.Items.Cast<ListViewItem>().Select(x => x.Selected = true).ToList();
            lvMetaData.EndUpdate();
        }

        private void btnShowInEdit_Click(object sender, EventArgs e)
        {
            ShowInEdit();
        }

        private void lvMetaData_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sender is ListView)
            {
                ListView lvSender = (ListView)sender;
                var lvItems = lvSender.Items.Cast<ListViewItem>();

                List<ListViewItem> items = new List<ListViewItem>();

                switch (e.Column)
                {
                    case 0:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Name).ToList();
                        break;
                    case 1:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Tag.Title).ThenBy(x => ((TagLib.File)x.Tag).Tag.FirstPerformer).ToList();
                        break;
                    case 2:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Tag.FirstPerformer).ThenBy(x => ((TagLib.File)x.Tag).Tag.Title).ToList();
                        break;
                    case 3:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Tag.Album).ToList();
                        break;
                    case 4:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Tag.FirstGenre).ThenBy(x => ((TagLib.File)x.Tag).Tag.FirstPerformer).ToList();
                        break;
                    case 5:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Tag.Track).ToList();
                        break;
                    case 6:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Tag.Year).ToList();
                        break;
                    case 7:
                        items = lvItems.OrderBy(x => (new FileInfo(((TagLib.File)x.Tag).Name).Length)).ToList();
                        break;
                    case 8:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Properties.AudioBitrate).ToList();
                        break;
                    case 9:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Properties.AudioSampleRate).ToList();
                        break;
                    case 10:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Properties.Duration).ToList();
                        break;
                    case 11:
                        items = lvItems.OrderBy(x => ((TagLib.File)x.Tag).Name).ToList();
                        break;
                    default:
                        return;
                }

                lvSender.BeginUpdate();
                lvSender.Items.Clear();
                lvSender.Items.AddRange(items.ToArray());
                lvSender.EndUpdate();
            }
        }

        private void eliminarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listview = lvMetaData;
            if (listview.SelectedItems.Count > 0)
            {
                string message = listview.SelectedItems.Count == 1 ? "\n\"" + Path.GetFileName(((TagLib.File)listview.SelectedItems[0].Tag).Name) + "\"" : $"estos {listview.SelectedItems.Count} archivos seleccionados";
                if (MessageBox.Show($"¿Seguro que deseas eliminar {message}?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    int filesDeleted = 0;
                    int totalFiles = listview.SelectedItems.Count;

                    List<TagLib.File> RowsToRemove = new List<TagLib.File>();
                    foreach (ListViewItem item in listview.SelectedItems)
                    {
                        try
                        {
                            TagLib.File file = (TagLib.File)item.Tag;
                            File.Delete(file.Name);
                            RowsToRemove.Add(file);
                            filesDeleted++;
                        }
                        catch (Exception m)
                        {

                        }
                    }
                    UpdateListView(RowsToRemove, false);
                    //if (filesDeleted > 0) { AddFileToListView(AllFiles, false); }
                    MessageBox.Show($"{filesDeleted} de {totalFiles} archivo(s) eliminado(s)", "Eliminar archivos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void quitarDeLaListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvMetaData.SelectedItems.Count > 0)
            {
                var filesToRemove = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(x => x.Tag as TagLib.File).ToList();
                UpdateListView(filesToRemove, false);
            }
        }

        private void propiedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvMetaData.SelectedItems.Count == 1)
            {
                var file = (TagLib.File)lvMetaData.SelectedItems[0].Tag;
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var image = Clipboard.GetImage();
            if (image != null)
            {
                pbCover.Image = image;
            }
        }

        private void buscarMusicasDuplicadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindFiles fl = new FindFiles();
            if (fl.ShowDialog() == DialogResult.OK)
            {
                this.AllFiles = fl.DuplicatedFiles.Select(x=>x.Name).ToList();
            }
        }

        private void btnVerifyDuplicatedFiles_Click(object sender, EventArgs e)
        {
            if (AllFiles.Count > 1)
            {
                FindFiles fl = new FindFiles(this.AllFiles);
                if (fl.DialogResult == DialogResult.OK)
                {
                    this.AllFiles = fl.DuplicatedFiles.Select(x => x.Name).ToList();
                    fl.Close();
                }
            }
            else
            {
                MessageBox.Show("Debes agregar más de un archivo a la lista.", "Verificar archivos duplicados", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void eliminarCaracteresEspecialesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var values = lvMetaData.SelectedItems.Cast<ListViewItem>().Select(X => (TagLib.File)X.Tag);
            int count = 0;

            values = values.Select(file =>
            {
                try
                {
                    file.Tag.Title = file.Tag.Title.RemoveSpecialCharacters();
                    file.Save();
                    count++;
                }
                catch { }
                return file;
            });

            AllFiles = values.Select(x => x.Name).ToList();
            MessageBox.Show(count + " nombre de archivos editados", "Cambiar nombre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void agruparArchivosEnCarpetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvMetaData.Items.Count > 0)
            {
                var files = lvMetaData.Items.Cast<ListViewItem>().Select(x => x.Tag as TagLib.File).ToList();
                GroupForm gf = new GroupForm(files);
                gf.ShowDialog();
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbCover.Image = null;
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbCover.Image != null)
            {
                Clipboard.SetImage(pbCover.Image);
            }
        }
    }
}
