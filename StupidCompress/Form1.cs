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


        public FORM_Main()
        {
            InitializeComponent();
            bgW_ImageConverter.WorkerReportsProgress = true;
            bgW_ImageConverter.WorkerSupportsCancellation = true;
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
        private void bt_Start_Click(object sender, EventArgs e) //Start Button
        {
            if(tb_PathToOutput.Text == "") //Default fill output path to input file path if field is empty
            {
                tb_PathToOutput.Text = Path.GetDirectoryName(tb_PathToCompress.Text);
            }
            if(tb_FileName.Text == "") //Default fill Filename to New File
            {
                tb_FileName.Text = "New File";
            }
            if(File.Exists(tb_PathToCompress.Text) && Directory.Exists(tb_PathToOutput.Text)) //Check if these 2 directories exist
            {
                if(cb_OutpuSelect.SelectedIndex == 0) //Compression code
                {
                    if (bgW_ImageConverter.IsBusy != true)
                    {
                        bgW_ImageConverter.RunWorkerAsync(); //Using background worker since the save file command causes app to hang for a bit. This avoids the user thinking the app has crashed and getting frustrated.
                    }
                }
                else if(cb_OutpuSelect.SelectedIndex == 1) //Decompression Code
                {
                    if(Path.GetExtension(tb_PathToCompress.Text) == ".png")//doesnt activate if input file is NOT png
                    {
                        decompress();
                    }
                    else
                    {
                        MessageBox.Show("Can only decompress PNG files.","Invalid Filetype",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("This is not supported yet. Coming soon.");
                }
            }
            else
            {
                MessageBox.Show("Please make sure the paths you have enterd are valid and try again.");
            }
        }
        private void updateStatus(string Text) //just updates status bar with whatever string input
        {
            tb_Status.Text = Text;
        }
        private byte[] readFile(string path) //reads file and creates an array with data length and File type baked in
        {
            byte[] rawBytes = File.ReadAllBytes(path);
            int dataLength = rawBytes.Length;
            string fileType = Path.GetExtension(tb_PathToCompress.Text);
            byte[] outputBytes = bakeInFileInfo(dataLength,fileType);
            rawBytes.CopyTo(outputBytes, (BitConverter.GetBytes(dataLength).Length + Encoding.ASCII.GetBytes(fileType).Length + 1));
            return outputBytes;
        }
        private byte[] bakeInFileInfo(int dataLength, string fileType) //Takes care of handeling the extra data encoding and combination of arrays
        {
            int lengthToAdd = ((Encoding.ASCII.GetBytes(fileType).Length) + (BitConverter.GetBytes(dataLength).Length) + 2);
            byte[] outputBytes = new byte[(dataLength + lengthToAdd)];
            bakeInFileLength(ref outputBytes,  dataLength);
            bakeInFileType(ref outputBytes, fileType);
            return outputBytes;
        }
        private void bakeInFileType(ref byte[] outputBytes, string fileType) //encodes and adds the filetype to the array
        {
            byte[] fileTypeAsByte = Encoding.ASCII.GetBytes(fileType);
            byte[] fileTypeToBake = new byte[fileTypeAsByte.Length + 1];
            fileTypeAsByte.CopyTo(fileTypeToBake, 0);
            int startingByte = 0;
            for (int i = 0; outputBytes[i] != 0; i++)
            {
                startingByte++; //+1 for getting to the 0 and another +1 for getting to actual start
            }
            startingByte++;
            for (int i = 0; i < fileTypeToBake.Length; i++)
            {
                outputBytes[i+startingByte] = fileTypeToBake[i];
            }
        }
        private void bakeInFileLength(ref byte[] outputBytes,  int dataLength) //encodes and adds the data length to the array
        {
            byte[] dataLengthAsByte = BitConverter.GetBytes(dataLength);
            byte[] dataLengthToBake = new byte[dataLengthAsByte.Length + 1];
            dataLengthAsByte.CopyTo(dataLengthToBake, 0);

            for (int i = 0; i < dataLengthToBake.Length; i++)
            {
                outputBytes[i] = dataLengthToBake[i];
            }
        }
        private int calculateResolution(int dataSize) //calculates the needed resolution really quick. Assumes that the resoluiton should be square. Option can be added to use different resolution
        {
            return (int)Math.Round(Math.Sqrt(dataSize / 4), MidpointRounding.AwayFromZero);
        }
        private void decompress() //All the decompression and decoding functions
        {
            Bitmap bmp = new Bitmap(tb_PathToCompress.Text);
            BitmapData bmpDATA = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] input = new byte[(bmpDATA.Stride * bmp.Height)];
            IntPtr ptr = bmpDATA.Scan0;
            Marshal.Copy(ptr, input, 0, input.Length);
            if (!checkFileLegitimacy(ref input)) //checks if file is legitimate (made by this application)
            {
                DialogResult result = MessageBox.Show("This file is not a file made by this application. To avoid errors, please only use PNG files made by this application", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result == DialogResult.No)
                {
                    return; //if user answers no, programm halts.
                }
            }
            byte[] dataLengthBytes = new byte[4];
            byte[] tempFileTypeBytes = new byte[8];
            int startingByte = 0;
            for(int i = 0; input[i] != 0; i++) //gets Datalength bytes
            {
                dataLengthBytes[i] = input[i];
                startingByte++; //+1 for getting to the 0 
            }
            startingByte++;//and another +1 for getting to FileType start
            int fileTypeStart = startingByte;
            for (int i = 0; input[i + fileTypeStart] != 0; i++) //gets Filetype bytes
            {
                tempFileTypeBytes[i] = input[i + fileTypeStart];
                startingByte++; //+1 for getting to the 0 
            }
            startingByte++;//and another +1 for getting to Actual start
            byte[] fileTypeBytes = tempFileTypeBytes.Skip(0).Take(startingByte - fileTypeStart-1).ToArray();
            int dataLength = BitConverter.ToInt32(dataLengthBytes,0);
            string fileType = Encoding.Default.GetString(fileTypeBytes);
            byte[] realData = new byte[dataLength];
            Parallel.For(0, dataLength, i => //parses out data from all bytes, using the data length extracted before. Parallel for max performance
              {
                  realData[i] = input[i + startingByte];
              });
            string path = tb_PathToOutput.Text + "\\" + tb_FileName.Text + fileType;
            File.WriteAllBytes(path, realData); //saves the data array to the path
            bmp.Dispose(); //cleanup
            bmpDATA = null;
            input = null;
            realData = null;            
        }
        private byte[] grabFirstBytes(int count, string path) //grabs the first few bytes of a file to check intgrity
        {
            byte[] input = File.ReadAllBytes(path);
            byte[] output = input.Skip(0).Take(count).ToArray();
            return output;
        }
        private bool checkFileLegitimacy(ref byte[] input) //checks for specific file pattern at the start of the file. Could be made more robust by fixing the datalength to a fixed byte count. If fixed, should be 4 because that fits maximum datalength
        {
            if((input[3] == 0 ^ input[4] == 0) && (input[4] == 46 ^ input[5] == 46))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void bgW_ImageConverter_DoWork(object sender, DoWorkEventArgs e) // compreses the image to png. Is background worker to have smoother operation while work is done
        {
            byte[] integritycheck = grabFirstBytes(10, tb_PathToCompress.Text);
            if (checkFileLegitimacy(ref integritycheck))
            {
                DialogResult result = MessageBox.Show("This file is made by this application. The resulting file will not be any smaller. Continiue?","Continiue?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(result == DialogResult.No)
                {
                    return; //if user answers no, function ends.
                }
            }
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0);
            byte[] outputBytes = readFile(tb_PathToCompress.Text);
            worker.ReportProgress(15);
            int size = calculateResolution(outputBytes.Length);
            worker.ReportProgress(20);
            Bitmap bmp = new Bitmap(size, size+1);
            BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            worker.ReportProgress(22);
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            worker.ReportProgress(23);
            Marshal.Copy(outputBytes, 0, ptrFirstPixel, outputBytes.Length);
            bmp.UnlockBits(bitmapData);
            worker.ReportProgress(50);
            bmp.Save(tb_PathToOutput.Text + "//" + tb_FileName.Text + ".png"); //this sucker takes FOREVER. Faster way would definetly be helpfull. WriteAllBytes might be an option
            worker.ReportProgress(95);
            bmp.Dispose();
            GC.Collect();
            worker.ReportProgress(100);
        }
        private void bgW_ImageConverter_ProgressChanged(object sender, ProgressChangedEventArgs e) //progress update event from background worker
        {
            tb_Status.Text = (e.ProgressPercentage.ToString() + "%");
        }
        private void bgW_ImageConverter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) //what to do when work is done
        {
            if (e.Cancelled == true) // ready to have a cancel function added
            {
                updateStatus("Canceled!");
            }
            else if (e.Error != null)
            {
                updateStatus("Error: " + e.Error.Message);
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
                updateStatus("Done and Ready");
            }
        }
    }
}
