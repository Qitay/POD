using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace POD11
{
    public partial class Form1 : Form
    {
        //private const int dwa = 2;
        private Bitmap[] EncryptedImages;

        public Form1()
        {
            InitializeComponent();
        }

        public static Bitmap ConvertToBlackAndWhite(Bitmap input)
        {
            var masks = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            var output = new Bitmap(input.Width, input.Height, PixelFormat.Format1bppIndexed);
            var data = new sbyte[input.Width, input.Height];
            var inputData = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                var scanLine = inputData.Scan0;
                var line = new byte[inputData.Stride];
                for (var y = 0; y < inputData.Height; y++, scanLine += inputData.Stride)
                {
                    Marshal.Copy(scanLine, line, 0, line.Length);
                    for (var x = 0; x < input.Width; x++)
                    {
                        data[x, y] = (sbyte)(64 * (GetGreyLevel(line[x * 3 + 2], line[x * 3 + 1], line[x * 3 + 0]) - 0.5));
                    }
                }
            }
            finally
            {
                input.UnlockBits(inputData);
            }
            var outputData = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
            try
            {
                var scanLine = outputData.Scan0;
                for (var y = 0; y < outputData.Height; y++, scanLine += outputData.Stride)
                {
                    var line = new byte[outputData.Stride];
                    for (var x = 0; x < input.Width; x++)
                    {
                        var j = data[x, y] > 0;
                        if (j) line[x / 8] |= masks[x % 8];
                        var error = (sbyte)(data[x, y] - (j ? 32 : -32));
                        if (x < input.Width - 1) data[x + 1, y] += (sbyte)(7 * error / 16);
                        if (y < input.Height - 1)
                        {
                            if (x > 0) data[x - 1, y + 1] += (sbyte)(3 * error / 16);
                            data[x, y + 1] += (sbyte)(5 * error / 16);
                            if (x < input.Width - 1) data[x + 1, y + 1] += (sbyte)(1 * error / 16);
                        }
                    }
                    Marshal.Copy(line, 0, scanLine, outputData.Stride);
                }
            }
            finally
            {
                output.UnlockBits(outputData);
            }
            return output;
        }

        public static double GetGreyLevel(byte r, byte g, byte b)
        {
            return (r * 0.299 + g * 0.587 + b * 0.114) / 255;
        }
        

        private Bitmap[] GenerateImage(Bitmap source)
        {
            int sourceWidth = source.Width;
            int sourceHeight = source.Height;

            Bitmap tempImage = new Bitmap(sourceWidth, sourceHeight);
            Bitmap[] images = new Bitmap[2];

            Random rand = new Random();

            Graphics gtemp = Graphics.FromImage(tempImage);

            Color foreColor;

            gtemp.DrawImage(source, 0, 0, tempImage.Width, tempImage.Height);


            for (int i = 0; i < 2; i++)
            {
                images[i] = new Bitmap(sourceWidth*2, sourceHeight);
            }


            int index = -1;
            int width = tempImage.Width;
            int height = tempImage.Height;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    foreColor = tempImage.GetPixel(x, y);
                    index = rand.Next(2);
                    if (foreColor.ToArgb() == Color.Empty.ToArgb() || foreColor.ToArgb() == Color.White.ToArgb())
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (index == 0)
                            {
                                images[i].SetPixel(x * 2, y, Color.Black);
                                images[i].SetPixel(x * 2 + 1, y, Color.Empty);
                            }
                            else
                            {
                                images[i].SetPixel(x * 2, y, Color.Empty);
                                images[i].SetPixel(x * 2 + 1, y, Color.Black);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < images.Length; i++)
                        {
                            if ((index + i) % images.Length == 0)
                            {
                                images[i].SetPixel(x * 2, y, Color.Black);
                                images[i].SetPixel(x * 2 + 1, y, Color.Empty);
                            }
                            else
                            {
                                images[i].SetPixel(x * 2, y, Color.Empty);
                                images[i].SetPixel(x * 2 + 1, y, Color.Black);
                            }
                        }
                    }
                }
            }

            //brush.Dispose();
            tempImage.Dispose();

            return images;
        }


        //generuj
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (EncryptedImages != null)
            {
                for (int i = EncryptedImages.Length - 1; i > 0; i--)
                {
                    EncryptedImages[i].Dispose();
                }
                Array.Clear(EncryptedImages, 0, EncryptedImages.Length);
            }

            
            Bitmap gSource = ConvertToBlackAndWhite(new Bitmap(pictureBox1.Image));
            pictureBox1.Image = gSource; 

            EncryptedImages = GenerateImage(gSource);
                       
                    
            pictureBox3.Image = EncryptedImages[0];
            pictureBox4.Image = EncryptedImages[1];
        
        }        



        //złacz
        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap[] BmpTab = new Bitmap[2];

            Bitmap bm1 = new Bitmap(pictureBox3.Image);
            Bitmap bm2 = new Bitmap(pictureBox4.Image);

            Rectangle rect = new Rectangle(0, 0, 0, 0);
            Image bmp = new Bitmap(EncryptedImages[0].Width, EncryptedImages[0].Height);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < EncryptedImages.Length; i++)
                {
                    rect.Size = EncryptedImages[i].Size;
                    gr.DrawImage(EncryptedImages[i], rect);
                }
            }

            pictureBox2.Image = bmp;
        }



        //wczytaj 1
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "All files (*.*)|*.*|Image Files(*.bmp)| *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(open.FileName);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message + "\n");
            }
        }

        //wczytaj 2
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.bmp)| *.bmp|All files (*.*)|*.*";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    pictureBox3.Image = new Bitmap(open.FileName);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message + "\n");
            }
        }

        //wczytaj 3
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.bmp)| *.bmp|All files (*.*)|*.*";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    pictureBox4.Image = new Bitmap(open.FileName);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message + "\n");
            }
        }

        //zapisz 1
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            try
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    {
                        pictureBox2.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
            }
        }

        //zapisz 2
        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            try
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    {
                        pictureBox3.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
            }
        }

        //zapisz 3
        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            try
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    {
                        pictureBox4.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
            }
        }



        
    }
}
