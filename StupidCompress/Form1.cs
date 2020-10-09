using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;

namespace StupidCompress
{
    public partial class FORM_Main : Form
    {
        public FORM_Main()
        {
            InitializeComponent();
            bgW_ImageConverter.WorkerReportsProgress = true;
            bgW_ImageConverter.WorkerSupportsCancellation = true;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            cb_OutpuSelect.SelectedIndex = 0;
        }

        private void bt_BrowseToCompress_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tb_PathToCompress.Text = openFileDialog1.FileName;
            }
        }

        private void bt_Clear_Click(object sender, EventArgs e)
        {
            tb_PathToCompress.Text = "";
            tb_PathToOutput.Text = "";
            tb_FileName.Text = "";
            cb_OutpuSelect.SelectedIndex = 0;
        }

        private void bt_Settings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet avaliable. More options coming soon.");
        }

        private void bt_BrowseOutputLocation_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tb_PathToOutput.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void bt_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bt_Start_Click(object sender, EventArgs e)
        {
            if(tb_PathToOutput.Text == "")
            {
                tb_PathToOutput.Text = Path.GetDirectoryName(tb_PathToCompress.Text);
            }
            if(tb_FileName.Text == "")
            {
                tb_FileName.Text = "New File";
            }
            if(File.Exists(tb_PathToCompress.Text) && Directory.Exists(tb_PathToOutput.Text))
            {
                if(cb_OutpuSelect.SelectedIndex == 0)
                {
                    if (bgW_ImageConverter.IsBusy != true)
                    {
                        bgW_ImageConverter.RunWorkerAsync();
                    }
                }
                else if(cb_OutpuSelect.SelectedIndex == 1)
                {
                    if(Path.GetExtension(tb_PathToCompress.Text) == ".png")
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



        private void updateStatus(string Text)
        {
            tb_Status.Text = Text;
        }

        private byte[] readFile(string path)
        {

            byte[] rawBytes = File.ReadAllBytes(path);
            int dataLength = rawBytes.Length;
            string fileType = Path.GetExtension(tb_PathToCompress.Text);

            byte[] outputBytes = bakeInFileInfo(dataLength,fileType);


            rawBytes.CopyTo(outputBytes, (BitConverter.GetBytes(dataLength).Length + Encoding.ASCII.GetBytes(fileType).Length + 1));

            return outputBytes;
        }

        private byte[] bakeInFileInfo(int dataLength, string fileType)
        {
            byte[] that = Encoding.ASCII.GetBytes(fileType);

            int lengthToAdd = ((Encoding.ASCII.GetBytes(fileType).Length) + (BitConverter.GetBytes(dataLength).Length) + 2);
            byte[] outputBytes = new byte[(dataLength + lengthToAdd)];
            bakeInFileLength(ref outputBytes,  dataLength);
            bakeInFileType(ref outputBytes, fileType);

            return outputBytes;


        }

        private void bakeInFileType(ref byte[] outputBytes, string fileType)
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
        private void bakeInFileLength(ref byte[] outputBytes,  int dataLength)
        {
            byte[] dataLengthAsByte = BitConverter.GetBytes(dataLength);
            byte[] dataLengthToBake = new byte[dataLengthAsByte.Length + 1];
            dataLengthAsByte.CopyTo(dataLengthToBake, 0);

            for (int i = 0; i < dataLengthToBake.Length; i++)
            {
                outputBytes[i] = dataLengthToBake[i];
            }
        }
        private int calculateResolution(int dataSize)
        {

            return (int)Math.Round(Math.Sqrt(dataSize / 4), MidpointRounding.AwayFromZero);
            //return (int)Math.Round(Math.Sqrt(dataSize), MidpointRounding.AwayFromZero);

        }
        private void decompress()
        {
            Bitmap bmp = new Bitmap(tb_PathToCompress.Text);
            BitmapData bmpDATA = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] input = new byte[(bmpDATA.Stride * bmp.Height)];
            IntPtr ptr = bmpDATA.Scan0;

            Marshal.Copy(ptr, input, 0, input.Length);

            if (!checkFileLegitimacy(ref input))
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
            for(int i = 0; input[i] != 0; i++)
            {
                dataLengthBytes[i] = input[i];
                startingByte++; //+1 for getting to the 0 
            }
            startingByte++;//and another +1 for getting to FileType start
            int fileTypeStart = startingByte;
            for (int i = 0; input[i + fileTypeStart] != 0; i++)
            {
                tempFileTypeBytes[i] = input[i + fileTypeStart];
                startingByte++; //+1 for getting to the 0 
            }
            startingByte++;//and another +1 for getting to Actual start

            byte[] fileTypeBytes = tempFileTypeBytes.Skip(0).Take(startingByte - fileTypeStart-1).ToArray();

            int dataLength = BitConverter.ToInt32(dataLengthBytes,0);
            string fileType = Encoding.Default.GetString(fileTypeBytes);
            byte[] realData = new byte[dataLength];

            Parallel.For(0, dataLength, i =>
              {
                  realData[i] = input[i + startingByte];
              });

            string path = tb_PathToOutput.Text + "\\" + tb_FileName.Text + fileType;

            File.WriteAllBytes(path, realData);
            bmp.Dispose();
            bmpDATA = null;
            input = null;
            realData = null;
            
        }

        private byte[] grabFirstBytes(int count, string path)
        {
            byte[] input = File.ReadAllBytes(path);
            byte[] output = input.Skip(0).Take(count).ToArray();
            return output;
        }
        private bool checkFileLegitimacy(ref byte[] input)
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
        private int[] findAllFactors(int findThis)
        {
            int[] factors = new int[findThis];
            int index = 0;
            double stopHere = Math.Sqrt(findThis);
            for(int i=1; i<stopHere; i++)
            {
                if(findThis%i == 0)
                {
                    factors[index] = i;
                    index++;
                }
            }
            return factors;

        }

        public static bool isPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        private void bgW_ImageConverter_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] integritycheck = grabFirstBytes(10, tb_PathToCompress.Text);

            if (checkFileLegitimacy(ref integritycheck))
            {
                DialogResult result = MessageBox.Show("This file is made by this application. The resulting file will not be any smaller. Continiue?","Continiue?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(result == DialogResult.No)
                {
                    return; //if user answers no, programm halts.
                }
            }
            BackgroundWorker worker = sender as BackgroundWorker;

            worker.ReportProgress(0);
            byte[] outputBytes = readFile(tb_PathToCompress.Text);

            worker.ReportProgress(15);

            int size = calculateResolution(outputBytes.Length);

            worker.ReportProgress(20);

            Bitmap bmp = new Bitmap(size, size+1);

            BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat); //something something bitlock link below
                                                                                                                                        //http://csharpexamples.com/fast-image-processing-c/
            worker.ReportProgress(22);                                                                                                  //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-simple-parallel-for-loop?redirectedfrom=MSDN
                                                                                                                                        //https://docs.microsoft.com/en-nz/dotnet/api/system.drawing.imaging.bitmapdata?redirectedfrom=MSDN&view=dotnet-plat-ext-3.1

            IntPtr ptrFirstPixel = bitmapData.Scan0;

            worker.ReportProgress(23);

            Marshal.Copy(outputBytes, 0, ptrFirstPixel, outputBytes.Length);
            bmp.UnlockBits(bitmapData);
            worker.ReportProgress(50);

            bmp.Save(tb_PathToOutput.Text + "//" + tb_FileName.Text + ".png");

            worker.ReportProgress(95);

            outputBytes = null;
            bmp.Dispose();
            GC.Collect();

            worker.ReportProgress(100);
        }

        private void bgW_ImageConverter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tb_Status.Text = (e.ProgressPercentage.ToString() + "%");
        }

        private void bgW_ImageConverter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
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
