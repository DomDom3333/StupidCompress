namespace StupidCompress
{
    partial class FORM_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FORM_Main));
            this.bt_Start = new System.Windows.Forms.Button();
            this.bt_Clear = new System.Windows.Forms.Button();
            this.bt_Settings = new System.Windows.Forms.Button();
            this.bt_Exit = new System.Windows.Forms.Button();
            this.bt_BrowseToCompress = new System.Windows.Forms.Button();
            this.bt_BrowseOutputLocation = new System.Windows.Forms.Button();
            this.cb_OutpuSelect = new System.Windows.Forms.ComboBox();
            this.tb_PathToCompress = new System.Windows.Forms.TextBox();
            this.tb_PathToOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tb_FileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pb_Progress = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_Start
            // 
            this.bt_Start.Location = new System.Drawing.Point(12, 83);
            this.bt_Start.Name = "bt_Start";
            this.bt_Start.Size = new System.Drawing.Size(75, 23);
            this.bt_Start.TabIndex = 10;
            this.bt_Start.Text = "Start";
            this.bt_Start.UseVisualStyleBackColor = true;
            this.bt_Start.Click += new System.EventHandler(this.bt_Start_Click);
            // 
            // bt_Clear
            // 
            this.bt_Clear.Location = new System.Drawing.Point(93, 83);
            this.bt_Clear.Name = "bt_Clear";
            this.bt_Clear.Size = new System.Drawing.Size(75, 23);
            this.bt_Clear.TabIndex = 11;
            this.bt_Clear.Text = "Clear";
            this.bt_Clear.UseVisualStyleBackColor = true;
            this.bt_Clear.Click += new System.EventHandler(this.bt_Clear_Click);
            // 
            // bt_Settings
            // 
            this.bt_Settings.Location = new System.Drawing.Point(174, 83);
            this.bt_Settings.Name = "bt_Settings";
            this.bt_Settings.Size = new System.Drawing.Size(75, 23);
            this.bt_Settings.TabIndex = 12;
            this.bt_Settings.Text = "Settings";
            this.bt_Settings.UseVisualStyleBackColor = true;
            this.bt_Settings.Click += new System.EventHandler(this.bt_Settings_Click);
            // 
            // bt_Exit
            // 
            this.bt_Exit.Location = new System.Drawing.Point(255, 83);
            this.bt_Exit.Name = "bt_Exit";
            this.bt_Exit.Size = new System.Drawing.Size(75, 23);
            this.bt_Exit.TabIndex = 13;
            this.bt_Exit.Text = "Exit";
            this.bt_Exit.UseVisualStyleBackColor = true;
            this.bt_Exit.Click += new System.EventHandler(this.bt_Exit_Click);
            // 
            // bt_BrowseToCompress
            // 
            this.bt_BrowseToCompress.Location = new System.Drawing.Point(659, 4);
            this.bt_BrowseToCompress.Name = "bt_BrowseToCompress";
            this.bt_BrowseToCompress.Size = new System.Drawing.Size(75, 23);
            this.bt_BrowseToCompress.TabIndex = 5;
            this.bt_BrowseToCompress.Text = "Browse";
            this.bt_BrowseToCompress.UseVisualStyleBackColor = true;
            this.bt_BrowseToCompress.Click += new System.EventHandler(this.bt_BrowseToCompress_Click);
            // 
            // bt_BrowseOutputLocation
            // 
            this.bt_BrowseOutputLocation.Location = new System.Drawing.Point(659, 28);
            this.bt_BrowseOutputLocation.Name = "bt_BrowseOutputLocation";
            this.bt_BrowseOutputLocation.Size = new System.Drawing.Size(75, 23);
            this.bt_BrowseOutputLocation.TabIndex = 7;
            this.bt_BrowseOutputLocation.Text = "Browse";
            this.bt_BrowseOutputLocation.UseVisualStyleBackColor = true;
            this.bt_BrowseOutputLocation.Click += new System.EventHandler(this.bt_BrowseOutputLocation_Click);
            // 
            // cb_OutpuSelect
            // 
            this.cb_OutpuSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_OutpuSelect.FormattingEnabled = true;
            this.cb_OutpuSelect.Items.AddRange(new object[] {
            "Compress",
            "Decompress"});
            this.cb_OutpuSelect.Location = new System.Drawing.Point(105, 56);
            this.cb_OutpuSelect.Name = "cb_OutpuSelect";
            this.cb_OutpuSelect.Size = new System.Drawing.Size(121, 21);
            this.cb_OutpuSelect.TabIndex = 8;
            // 
            // tb_PathToCompress
            // 
            this.tb_PathToCompress.AllowDrop = true;
            this.tb_PathToCompress.Location = new System.Drawing.Point(105, 6);
            this.tb_PathToCompress.Name = "tb_PathToCompress";
            this.tb_PathToCompress.Size = new System.Drawing.Size(548, 20);
            this.tb_PathToCompress.TabIndex = 4;
            this.tb_PathToCompress.DragDrop += new System.Windows.Forms.DragEventHandler(this.tb_PathToCompress_DragDrop);
            this.tb_PathToCompress.DragEnter += new System.Windows.Forms.DragEventHandler(this.tb_PathToCompress_DragEnter);
            // 
            // tb_PathToOutput
            // 
            this.tb_PathToOutput.Location = new System.Drawing.Point(105, 30);
            this.tb_PathToOutput.Name = "tb_PathToOutput";
            this.tb_PathToOutput.Size = new System.Drawing.Size(548, 20);
            this.tb_PathToOutput.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Output Location:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Action:";
            // 
            // tb_FileName
            // 
            this.tb_FileName.Location = new System.Drawing.Point(295, 56);
            this.tb_FileName.Name = "tb_FileName";
            this.tb_FileName.Size = new System.Drawing.Size(282, 20);
            this.tb_FileName.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(232, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "File Name:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.pb_Progress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 114);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(745, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(628, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Ready";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pb_Progress
            // 
            this.pb_Progress.Name = "pb_Progress";
            this.pb_Progress.Size = new System.Drawing.Size(100, 16);
            // 
            // FORM_Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 136);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_FileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_PathToOutput);
            this.Controls.Add(this.tb_PathToCompress);
            this.Controls.Add(this.cb_OutpuSelect);
            this.Controls.Add(this.bt_BrowseOutputLocation);
            this.Controls.Add(this.bt_BrowseToCompress);
            this.Controls.Add(this.bt_Exit);
            this.Controls.Add(this.bt_Settings);
            this.Controls.Add(this.bt_Clear);
            this.Controls.Add(this.bt_Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FORM_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stupid Compress";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Start;
        private System.Windows.Forms.Button bt_Clear;
        private System.Windows.Forms.Button bt_Settings;
        private System.Windows.Forms.Button bt_Exit;
        private System.Windows.Forms.Button bt_BrowseToCompress;
        private System.Windows.Forms.Button bt_BrowseOutputLocation;
        private System.Windows.Forms.ComboBox cb_OutpuSelect;
        private System.Windows.Forms.TextBox tb_PathToCompress;
        private System.Windows.Forms.TextBox tb_PathToOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox tb_FileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar pb_Progress;
    }
}