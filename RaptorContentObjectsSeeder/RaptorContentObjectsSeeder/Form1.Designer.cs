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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.btnSeedImages = new System.Windows.Forms.Button();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLoadFromDatabase = new System.Windows.Forms.Button();
            this.flowLayoutPanelRight = new System.Windows.Forms.FlowLayoutPanel();
            this.pbOriginal = new System.Windows.Forms.PictureBox();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.btnAddFaces = new System.Windows.Forms.Button();
            this.btnSaveFaces = new System.Windows.Forms.Button();
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
            this.btnSeedImages.Location = new System.Drawing.Point(12, 313);
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
            this.flowLayoutPanel.Location = new System.Drawing.Point(12, 354);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(667, 131);
            this.flowLayoutPanel.TabIndex = 1;
            // 
            // btnLoadFromDatabase
            // 
            this.btnLoadFromDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadFromDatabase.Location = new System.Drawing.Point(537, 491);
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
            this.flowLayoutPanelRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelRight.Location = new System.Drawing.Point(454, 12);
            this.flowLayoutPanelRight.Name = "flowLayoutPanelRight";
            this.flowLayoutPanelRight.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.flowLayoutPanelRight.Size = new System.Drawing.Size(225, 295);
            this.flowLayoutPanelRight.TabIndex = 2;
            // 
            // pbOriginal
            // 
            this.pbOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbOriginal.BackColor = System.Drawing.Color.Gainsboro;
            this.pbOriginal.Location = new System.Drawing.Point(12, 12);
            this.pbOriginal.Name = "pbOriginal";
            this.pbOriginal.Size = new System.Drawing.Size(436, 295);
            this.pbOriginal.TabIndex = 3;
            this.pbOriginal.TabStop = false;
            // 
            // Progress
            // 
            this.Progress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Progress.Location = new System.Drawing.Point(0, 528);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(691, 10);
            this.Progress.TabIndex = 4;
            // 
            // btnAddFaces
            // 
            this.btnAddFaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFaces.Location = new System.Drawing.Point(348, 313);
            this.btnAddFaces.Name = "btnAddFaces";
            this.btnAddFaces.Size = new System.Drawing.Size(100, 23);
            this.btnAddFaces.TabIndex = 5;
            this.btnAddFaces.Text = "Add Faces";
            this.btnAddFaces.UseVisualStyleBackColor = true;
            this.btnAddFaces.Click += new System.EventHandler(this.btnAddFaces_Click);
            // 
            // btnSaveFaces
            // 
            this.btnSaveFaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveFaces.Location = new System.Drawing.Point(579, 313);
            this.btnSaveFaces.Name = "btnSaveFaces";
            this.btnSaveFaces.Size = new System.Drawing.Size(100, 23);
            this.btnSaveFaces.TabIndex = 6;
            this.btnSaveFaces.Text = "Save Faces";
            this.btnSaveFaces.UseVisualStyleBackColor = true;
            this.btnSaveFaces.Click += new System.EventHandler(this.btnSaveFaces_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 538);
            this.Controls.Add(this.btnSaveFaces);
            this.Controls.Add(this.btnAddFaces);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.pbOriginal);
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

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.Button btnSeedImages;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Button btnLoadFromDatabase;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelRight;
        private System.Windows.Forms.PictureBox pbOriginal;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Button btnAddFaces;
        private System.Windows.Forms.Button btnSaveFaces;
    }
}

