using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StupidCompress
{
    public class ImageCompressor
    {
        private const uint MagicNumber = 0x53545043; // "STPC" in ASCII

        public struct CompressionResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }

        public async Task<CompressionResult> CompressAsync(string inputPath, string outputPath, string fileName, IProgress<int> progress)
        {
            try
            {
                progress?.Report(0);
                string extension = Path.GetExtension(inputPath);
                byte[] extBytes = Encoding.ASCII.GetBytes(extension);

                using (FileStream fs = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
                {
                    long dataLength = fs.Length;
                    // Header: Magic(4) + DataLength(8) + ExtLength(4) + Extension(var)
                    long headerSize = 4 + 8 + 4 + extBytes.Length;
                    long totalSize = headerSize + dataLength;

                    int resolution = CalculateResolution(totalSize);
                    progress?.Report(10);

                    using (Bitmap bmp = new Bitmap(resolution, resolution + 1, PixelFormat.Format32bppArgb))
                    {
                        BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                        try
                        {
                            IntPtr ptr = bitmapData.Scan0;
                            long maxBytes = (long)bitmapData.Stride * bitmapData.Height;
                            
                            // Write header
                            byte[] header = new byte[headerSize];
                            BitConverter.TryWriteBytes(header.AsSpan(0, 4), MagicNumber);
                            BitConverter.TryWriteBytes(header.AsSpan(4, 8), dataLength);
                            BitConverter.TryWriteBytes(header.AsSpan(12, 4), extBytes.Length);
                            extBytes.CopyTo(header, 16);

                            Marshal.Copy(header, 0, ptr, (int)headerSize);
                            
                            // Write data
                            byte[] buffer = new byte[64 * 1024];
                            long bytesWritten = headerSize;
                            int bytesRead;
                            while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                if (bytesWritten + bytesRead > maxBytes)
                                {
                                    bytesRead = (int)(maxBytes - bytesWritten);
                                }
                                
                                if (bytesRead <= 0) break;

                                Marshal.Copy(buffer, 0, ptr + (IntPtr)bytesWritten, bytesRead);
                                bytesWritten += bytesRead;
                                
                                int prog = (int)((bytesWritten * 100) / totalSize);
                                progress?.Report(Math.Min(prog, 99));
                            }
                        }
                        finally
                        {
                            bmp.UnlockBits(bitmapData);
                        }

                        progress?.Report(90);
                        string fullPath = Path.Combine(outputPath, fileName + ".png");
                        await Task.Run(() => bmp.Save(fullPath, ImageFormat.Png));
                    }
                }

                progress?.Report(100);
                return new CompressionResult { Success = true, Message = "Compression complete." };
            }
            catch (Exception ex)
            {
                return new CompressionResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<CompressionResult> DecompressAsync(string inputPath, string outputDirectory, string fileName, IProgress<int> progress)
        {
            try
            {
                progress?.Report(0);
                using (Bitmap bmp = new Bitmap(inputPath))
                {
                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    try
                    {
                        IntPtr ptr = bmpData.Scan0;
                        
                        // Read first 16 bytes to get Magic, DataLength, and ExtLength
                        byte[] headerStart = new byte[16];
                        Marshal.Copy(ptr, headerStart, 0, 16);

                        if (BitConverter.ToUInt32(headerStart, 0) != MagicNumber)
                        {
                            return new CompressionResult { Success = false, Message = "This file was not created by StupidCompress or is corrupted." };
                        }

                        long dataLength = BitConverter.ToInt64(headerStart, 4);
                        int extLength = BitConverter.ToInt32(headerStart, 12);

                        byte[] extBytes = new byte[extLength];
                        Marshal.Copy(ptr + 16, extBytes, 0, extLength);
                        string extension = Encoding.ASCII.GetString(extBytes);

                        string fullPath = Path.Combine(outputDirectory, fileName + extension);
                        using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] buffer = new byte[64 * 1024];
                            long bytesToRead = dataLength;
                            long offset = 16 + extLength;
                            
                            while (bytesToRead > 0)
                            {
                                int toCopy = (int)Math.Min(buffer.Length, bytesToRead);
                                Marshal.Copy(ptr + (IntPtr)offset, buffer, 0, toCopy);
                                await fs.WriteAsync(buffer, 0, toCopy);
                                
                                bytesToRead -= toCopy;
                                offset += toCopy;

                                int prog = (int)(((dataLength - bytesToRead) * 100) / dataLength);
                                progress?.Report(prog);
                            }
                        }
                    }
                    finally
                    {
                        bmp.UnlockBits(bmpData);
                    }
                }

                progress?.Report(100);
                return new CompressionResult { Success = true, Message = "Decompression complete." };
            }
            catch (Exception ex)
            {
                return new CompressionResult { Success = false, Message = ex.Message };
            }
        }

        public bool IsLegitimate(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length < 4) return false;
                    byte[] buffer = new byte[4];
                    fs.Read(buffer, 0, 4);
                    return BitConverter.ToUInt32(buffer, 0) == MagicNumber;
                }
            }
            catch
            {
                return false;
            }
        }


        private int CalculateResolution(long dataSize)
        {
            // We use 4 bytes per pixel (ARGB)
            return (int)Math.Ceiling(Math.Sqrt(dataSize / 4.0));
        }
    }
}
