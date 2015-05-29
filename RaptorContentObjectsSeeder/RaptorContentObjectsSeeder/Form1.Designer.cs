namespace RaptorContentObjectsSeeder
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.btnSeedImages = new System.Windows.Forms.Button();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLoadFromDatabase = new System.Windows.Forms.Button();
            this.flowLayoutPanelRight = new System.Windows.Forms.FlowLayoutPanel();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.btnAddFaces = new System.Windows.Forms.Button();
            this.btnSaveFaces = new System.Windows.Forms.Button();
            this.pbOriginal = new System.Windows.Forms.PictureBox();
            this.btnSkipImage = new System.Windows.Forms.Button();
            this.btnRemoveLastFace = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.timerPinger20Mins = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).BeginInit();
            this.SuspendLayout();
            // 
            // OFD
            // 
            this.OFD.Multiselect = true;
            this.OFD.RestoreDirectory = true;
            this.OFD.ShowReadOnly = true;
            this.OFD.SupportMultiDottedExtensions = true;
            this.OFD.Title = "Open Image to add to Database";
            // 
            // btnSeedImages
            // 
            this.btnSeedImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSeedImages.Location = new System.Drawing.Point(12, 272);
            this.btnSeedImages.Name = "btnSeedImages";
            this.btnSeedImages.Size = new System.Drawing.Size(100, 23);
            this.btnSeedImages.TabIndex = 0;
            this.btnSeedImages.Text = "Seed Database";
            this.btnSeedImages.UseVisualStyleBackColor = true;
            this.btnSeedImages.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.BackColor = System.Drawing.Color.Gainsboro;
            this.flowLayoutPanel.Location = new System.Drawing.Point(12, 313);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(905, 131);
            this.flowLayoutPanel.TabIndex = 1;
            // 
            // btnLoadFromDatabase
            // 
            this.btnLoadFromDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadFromDatabase.Location = new System.Drawing.Point(775, 450);
            this.btnLoadFromDatabase.Name = "btnLoadFromDatabase";
            this.btnLoadFromDatabase.Size = new System.Drawing.Size(142, 23);
            this.btnLoadFromDatabase.TabIndex = 2;
            this.btnLoadFromDatabase.Text = "Load from Database";
            this.btnLoadFromDatabase.UseVisualStyleBackColor = true;
            this.btnLoadFromDatabase.Click += new System.EventHandler(this.btnLoadFromDatabase_Click);
            // 
            // flowLayoutPanelRight
            // 
            this.flowLayoutPanelRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelRight.AutoScroll = true;
            this.flowLayoutPanelRight.BackColor = System.Drawing.Color.Gainsboro;
            this.flowLayoutPanelRight.Location = new System.Drawing.Point(531, 12);
            this.flowLayoutPanelRight.Name = "flowLayoutPanelRight";
            this.flowLayoutPanelRight.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.flowLayoutPanelRight.Size = new System.Drawing.Size(386, 254);
            this.flowLayoutPanelRight.TabIndex = 2;
            this.flowLayoutPanelRight.Scroll += new System.Windows.Forms.ScrollEventHandler(this.flowLayoutPanelRight_Scroll);
            // 
            // Progress
            // 
            this.Progress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Progress.Location = new System.Drawing.Point(0, 487);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(929, 10);
            this.Progress.TabIndex = 4;
            // 
            // btnAddFaces
            // 
            this.btnAddFaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFaces.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnAddFaces.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddFaces.Location = new System.Drawing.Point(711, 272);
            this.btnAddFaces.Name = "btnAddFaces";
            this.btnAddFaces.Size = new System.Drawing.Size(100, 23);
            this.btnAddFaces.TabIndex = 5;
            this.btnAddFaces.Text = "Add Faces";
            this.btnAddFaces.UseVisualStyleBackColor = false;
            this.btnAddFaces.Click += new System.EventHandler(this.btnAddFaces_Click);
            // 
            // btnSaveFaces
            // 
            this.btnSaveFaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveFaces.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSaveFaces.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveFaces.Location = new System.Drawing.Point(817, 272);
            this.btnSaveFaces.Name = "btnSaveFaces";
            this.btnSaveFaces.Size = new System.Drawing.Size(100, 23);
            this.btnSaveFaces.TabIndex = 6;
            this.btnSaveFaces.Text = "Save Faces";
            this.btnSaveFaces.UseVisualStyleBackColor = false;
            this.btnSaveFaces.Click += new System.EventHandler(this.btnSaveFaces_Click);
            // 
            // pbOriginal
            // 
            this.pbOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbOriginal.BackColor = System.Drawing.Color.Gainsboro;
            this.pbOriginal.Location = new System.Drawing.Point(13, 13);
            this.pbOriginal.Name = "pbOriginal";
            this.pbOriginal.Size = new System.Drawing.Size(512, 253);
            this.pbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbOriginal.TabIndex = 7;
            this.pbOriginal.TabStop = false;
            this.pbOriginal.Paint += new System.Windows.Forms.PaintEventHandler(this.pbOriginal_Paint_1);
            this.pbOriginal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbOriginal_MouseDown_1);
            this.pbOriginal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbOriginal_MouseMove_1);
            // 
            // btnSkipImage
            // 
            this.btnSkipImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSkipImage.Location = new System.Drawing.Point(118, 272);
            this.btnSkipImage.Name = "btnSkipImage";
            this.btnSkipImage.Size = new System.Drawing.Size(100, 23);
            this.btnSkipImage.TabIndex = 8;
            this.btnSkipImage.Text = "Skip Image";
            this.btnSkipImage.UseVisualStyleBackColor = true;
            this.btnSkipImage.Click += new System.EventHandler(this.btnSkipImage_Click);
            // 
            // btnRemoveLastFace
            // 
            this.btnRemoveLastFace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveLastFace.Location = new System.Drawing.Point(425, 272);
            this.btnRemoveLastFace.Name = "btnRemoveLastFace";
            this.btnRemoveLastFace.Size = new System.Drawing.Size(100, 23);
            this.btnRemoveLastFace.TabIndex = 9;
            this.btnRemoveLastFace.Text = "Remove Face";
            this.btnRemoveLastFace.UseVisualStyleBackColor = true;
            this.btnRemoveLastFace.Click += new System.EventHandler(this.btnRemoveLastFace_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 450);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "File Size";
            // 
            // lblFileSize
            // 
            this.lblFileSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(66, 450);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(13, 13);
            this.lblFileSize.TabIndex = 11;
            this.lblFileSize.Text = "0";
            // 
            // timerPinger20Mins
            // 
            this.timerPinger20Mins.Enabled = true;
            this.timerPinger20Mins.Interval = 60000;
            this.timerPinger20Mins.Tick += new System.EventHandler(this.timerPinger20Mins_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 497);
            this.Controls.Add(this.lblFileSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRemoveLastFace);
            this.Controls.Add(this.btnSkipImage);
            this.Controls.Add(this.pbOriginal);
            this.Controls.Add(this.btnSaveFaces);
            this.Controls.Add(this.btnAddFaces);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.flowLayoutPanelRight);
            this.Controls.Add(this.btnLoadFromDatabase);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.btnSeedImages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Content Object Seeder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.Button btnSeedImages;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Button btnLoadFromDatabase;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelRight;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Button btnAddFaces;
        private System.Windows.Forms.Button btnSaveFaces;
        private System.Windows.Forms.PictureBox pbOriginal;
        private System.Windows.Forms.Button btnSkipImage;
        private System.Windows.Forms.Button btnRemoveLastFace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Timer timerPinger20Mins;
    }
}

