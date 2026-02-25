using System;
//using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
//using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading; Leftover from threading ideas. 
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Collections;
//using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;

//Left in bacause comment Gets optemized out at compile

namespace StupidCompress
{
    public partial class FORM_Main : Form
    {
        //Settings vars go here -------------------------------------------------------
        //
        //
        //
        //
        //
        //Settings vars above ---------------------------------------------------------


        private readonly ImageCompressor _compressor = new ImageCompressor();

        public FORM_Main()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cb_OutpuSelect.SelectedIndex = 0; //Set (De)Compress selector to default
        }
        private void bt_BrowseToCompress_Click(object sender, EventArgs e) //Browse to file to (De)Compress
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tb_PathToCompress.Text = openFileDialog1.FileName;
            }
        }
        private void bt_Clear_Click(object sender, EventArgs e) //Clears all fields
        {
            tb_PathToCompress.Text = "";
            tb_PathToOutput.Text = "";
            tb_FileName.Text = "";
            cb_OutpuSelect.SelectedIndex = 0;
        }
        private void bt_Settings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet avaliable. More options coming soon.");
            /*Settings ideas:
             * option not to use alpha channel in order to send pictures without corruption over messeging apps
             * option to decompress non PNG files (for no reason)
             */
        }
        private void bt_BrowseOutputLocation_Click(object sender, EventArgs e) //Browse to file to output Location
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tb_PathToOutput.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void bt_Exit_Click(object sender, EventArgs e) //Exit code
        {
            Application.Exit();
        }
        private async void bt_Start_Click(object sender, EventArgs e) //Start Button
        {
            if (string.IsNullOrWhiteSpace(tb_PathToCompress.Text) || !File.Exists(tb_PathToCompress.Text))
            {
                MessageBox.Show("Please select a valid input file.");
                return;
            }

            if (string.IsNullOrWhiteSpace(tb_PathToOutput.Text))
            {
                tb_PathToOutput.Text = Path.GetDirectoryName(tb_PathToCompress.Text);
            }

            if (string.IsNullOrWhiteSpace(tb_FileName.Text))
            {
                tb_FileName.Text = Path.GetFileNameWithoutExtension(tb_PathToCompress.Text) + "_compressed";
            }

            if (!Directory.Exists(tb_PathToOutput.Text))
            {
                MessageBox.Show("Output directory does not exist.");
                return;
            }

            var progress = new Progress<int>(p =>
            {
                pb_Progress.Value = p;
                lblStatus.Text = $"Working... {p}%";
            });

            bt_Start.Enabled = false;
            updateStatus("Working...");

            try
            {
                if (cb_OutpuSelect.SelectedIndex == 0) // Compression
                {
                    if (_compressor.IsLegitimate(tb_PathToCompress.Text))
                    {
                        var result = MessageBox.Show("This file is already encoded. Re-encoding won't save space. Continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No) return;
                    }

                    var compressionResult = await _compressor.CompressAsync(tb_PathToCompress.Text, tb_PathToOutput.Text, tb_FileName.Text, progress);
                    HandleResult(compressionResult);
                }
                else if (cb_OutpuSelect.SelectedIndex == 1) // Decompression
                {
                    if (Path.GetExtension(tb_PathToCompress.Text).ToLower() != ".png")
                    {
                        MessageBox.Show("Can only decompress PNG files.", "Invalid Filetype", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var decompressionResult = await _compressor.DecompressAsync(tb_PathToCompress.Text, tb_PathToOutput.Text, tb_FileName.Text, progress);
                    HandleResult(decompressionResult);
                }
            }
            finally
            {
                bt_Start.Enabled = true;
                pb_Progress.Value = 0;
            }
        }

        private void tb_PathToCompress_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void tb_PathToCompress_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                tb_PathToCompress.Text = files[0];
                if (string.IsNullOrWhiteSpace(tb_FileName.Text) || tb_FileName.Text == "New File")
                {
                    tb_FileName.Text = Path.GetFileNameWithoutExtension(files[0]) + "_compressed";
                }
            }
        }

        private void HandleResult(ImageCompressor.CompressionResult result)
        {
            if (result.Success)
            {
                updateStatus("Done and Ready");
            }
            else
            {
                updateStatus("Error");
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void updateStatus(string Text) //just updates status bar with whatever string input
        {
            lblStatus.Text = Text;
        }
    }
}
