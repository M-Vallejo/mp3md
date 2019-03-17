namespace Mp3Md
{
    partial class FindFiles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbMatch = new System.Windows.Forms.GroupBox();
            this.chkbMatchGenre = new System.Windows.Forms.CheckBox();
            this.chkbMatchTitle = new System.Windows.Forms.CheckBox();
            this.chkbMatchArtist = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eliminarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbMatch.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMatch
            // 
            this.gbMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMatch.Controls.Add(this.chkbMatchGenre);
            this.gbMatch.Controls.Add(this.chkbMatchTitle);
            this.gbMatch.Controls.Add(this.chkbMatchArtist);
            this.gbMatch.Location = new System.Drawing.Point(3, 5);
            this.gbMatch.Name = "gbMatch";
            this.gbMatch.Size = new System.Drawing.Size(209, 100);
            this.gbMatch.TabIndex = 2;
            this.gbMatch.TabStop = false;
            this.gbMatch.Text = "Coincidencias";
            // 
            // chkbMatchGenre
            // 
            this.chkbMatchGenre.AutoSize = true;
            this.chkbMatchGenre.Location = new System.Drawing.Point(6, 65);
            this.chkbMatchGenre.Name = "chkbMatchGenre";
            this.chkbMatchGenre.Size = new System.Drawing.Size(61, 17);
            this.chkbMatchGenre.TabIndex = 2;
            this.chkbMatchGenre.Text = "Género";
            this.chkbMatchGenre.UseVisualStyleBackColor = true;
            this.chkbMatchGenre.Visible = false;
            // 
            // chkbMatchTitle
            // 
            this.chkbMatchTitle.AutoSize = true;
            this.chkbMatchTitle.Checked = true;
            this.chkbMatchTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbMatchTitle.Enabled = false;
            this.chkbMatchTitle.Location = new System.Drawing.Point(6, 19);
            this.chkbMatchTitle.Name = "chkbMatchTitle";
            this.chkbMatchTitle.Size = new System.Drawing.Size(54, 17);
            this.chkbMatchTitle.TabIndex = 1;
            this.chkbMatchTitle.Text = "Título";
            this.chkbMatchTitle.UseVisualStyleBackColor = true;
            // 
            // chkbMatchArtist
            // 
            this.chkbMatchArtist.AutoSize = true;
            this.chkbMatchArtist.Checked = true;
            this.chkbMatchArtist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbMatchArtist.Location = new System.Drawing.Point(6, 42);
            this.chkbMatchArtist.Name = "chkbMatchArtist";
            this.chkbMatchArtist.Size = new System.Drawing.Size(55, 17);
            this.chkbMatchArtist.TabIndex = 0;
            this.chkbMatchArtist.Text = "Artista";
            this.chkbMatchArtist.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(3, 111);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(209, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Buscar";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eliminarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(175, 26);
            // 
            // eliminarToolStripMenuItem
            // 
            this.eliminarToolStripMenuItem.Name = "eliminarToolStripMenuItem";
            this.eliminarToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.eliminarToolStripMenuItem.Text = "Eliminar Archivo(s)";
            // 
            // FindFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 136);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.gbMatch);
            this.MinimumSize = new System.Drawing.Size(232, 175);
            this.Name = "FindFiles";
            this.ShowIcon = false;
            this.Text = "Archivos duplicados";
            this.Load += new System.EventHandler(this.FindFiles_Load);
            this.gbMatch.ResumeLayout(false);
            this.gbMatch.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbMatch;
        private System.Windows.Forms.CheckBox chkbMatchGenre;
        private System.Windows.Forms.CheckBox chkbMatchTitle;
        private System.Windows.Forms.CheckBox chkbMatchArtist;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem eliminarToolStripMenuItem;
    }
}