namespace Mp3Md
{
    partial class GroupForm
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
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBasePath = new System.Windows.Forms.TextBox();
            this.btnSearhFolder = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.chkbGroupbyGenre = new System.Windows.Forms.CheckBox();
            this.chkbGroupByGenreFirst = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtNumber
            // 
            this.txtNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNumber.Location = new System.Drawing.Point(169, 21);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(428, 20);
            this.txtNumber.TabIndex = 0;
            this.txtNumber.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Número mínimo de canciones:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ruta base:";
            // 
            // txtBasePath
            // 
            this.txtBasePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBasePath.Location = new System.Drawing.Point(77, 45);
            this.txtBasePath.Name = "txtBasePath";
            this.txtBasePath.Size = new System.Drawing.Size(501, 20);
            this.txtBasePath.TabIndex = 3;
            // 
            // btnSearhFolder
            // 
            this.btnSearhFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearhFolder.Location = new System.Drawing.Point(581, 43);
            this.btnSearhFolder.Name = "btnSearhFolder";
            this.btnSearhFolder.Size = new System.Drawing.Size(24, 23);
            this.btnSearhFolder.TabIndex = 4;
            this.btnSearhFolder.Text = "...";
            this.btnSearhFolder.UseVisualStyleBackColor = true;
            this.btnSearhFolder.Click += new System.EventHandler(this.btnSearhFolder_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(530, 72);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccept.Location = new System.Drawing.Point(449, 72);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 6;
            this.btnAccept.Text = "Aceptar";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // chkbGroupbyGenre
            // 
            this.chkbGroupbyGenre.AutoSize = true;
            this.chkbGroupbyGenre.Location = new System.Drawing.Point(15, 73);
            this.chkbGroupbyGenre.Name = "chkbGroupbyGenre";
            this.chkbGroupbyGenre.Size = new System.Drawing.Size(117, 17);
            this.chkbGroupbyGenre.TabIndex = 7;
            this.chkbGroupbyGenre.Text = "Agrupar por género";
            this.chkbGroupbyGenre.UseVisualStyleBackColor = true;
            this.chkbGroupbyGenre.Visible = false;
            this.chkbGroupbyGenre.CheckedChanged += new System.EventHandler(this.chkbGroupbyGenre_CheckedChanged);
            // 
            // chkbGroupByGenreFirst
            // 
            this.chkbGroupByGenreFirst.AutoSize = true;
            this.chkbGroupByGenreFirst.Enabled = false;
            this.chkbGroupByGenreFirst.Location = new System.Drawing.Point(150, 73);
            this.chkbGroupByGenreFirst.Name = "chkbGroupByGenreFirst";
            this.chkbGroupByGenreFirst.Size = new System.Drawing.Size(154, 17);
            this.chkbGroupByGenreFirst.TabIndex = 8;
            this.chkbGroupByGenreFirst.Text = "Agrupar primero por género";
            this.chkbGroupByGenreFirst.UseVisualStyleBackColor = true;
            this.chkbGroupByGenreFirst.Visible = false;
            // 
            // GroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 102);
            this.Controls.Add(this.chkbGroupByGenreFirst);
            this.Controls.Add(this.chkbGroupbyGenre);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearhFolder);
            this.Controls.Add(this.txtBasePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNumber);
            this.MinimumSize = new System.Drawing.Size(625, 141);
            this.Name = "GroupForm";
            this.ShowIcon = false;
            this.Text = "Agrupar canciones por";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBasePath;
        private System.Windows.Forms.Button btnSearhFolder;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.CheckBox chkbGroupbyGenre;
        private System.Windows.Forms.CheckBox chkbGroupByGenreFirst;
    }
}