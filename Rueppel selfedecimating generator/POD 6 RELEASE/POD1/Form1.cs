using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace POD6
{
	public partial class Form1 : Form
	{
		public int d = 1;
		public int k = 1;



		public Form1()
		{
			InitializeComponent();
		}



		public void LFSR(long seed, int n)
		{
			long start_state = seed;
			long lfsr = start_state;
			long bit = 0;
			long period = 0;
			int i = k;
			String output ="";
			int tap1 = 32 - (int)numericUpDown5.Value;
			int tap2 = 32 - (int)numericUpDown6.Value;
			int tap3 = 32 - (int)numericUpDown7.Value;
			int tap4 = 32 - (int)numericUpDown8.Value;

			while (period < n)
			{
				
				for (; i > 0 ; i--)
				{
					/* taps: 16 14 13 11; feedback polynomial: x^16 + x^14 + x^13 + x^11 + 1 */
					bit = ((lfsr >> tap1) ^ (lfsr >> tap2) ^ (lfsr >> tap3) ^ (lfsr >> tap4)) & 1; 
					lfsr = (lfsr >> 1) | (bit << 31);
				}

				output += bit;
				
				if (bit == 0)
				{
					i = d;
				}
				if (bit == 1)
				{
					i = k;
				}
				period++;
			}
			richTextBox2.Text = output;
		}

		public char XORchar(char a, char b)
		{
			if (a!=b)
			{
				return '1';
			}
			else
			{
				return '0';
			}
		}

		//random
		private void button1_Click(object sender, EventArgs e)
		{
			String a = "";
			Random rnd = new Random();
			for (int i = 0; i < (int)numericUpDown1.Value; i++)
			{
				a += rnd.Next(0, 2); 
			}
			richTextBox2.Text = a;
		}

		//Generuj
		private void button4_Click(object sender, EventArgs e)
		{
			d = (int)numericUpDown2.Value;
			k = (int)numericUpDown3.Value;
			long x = (long)numericUpDown4.Value;

			LFSR(x, (int)numericUpDown1.Value);

		}

		public String TextToBits(String input)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			byte[] bytes = encoding.GetBytes(input);

			StringBuilder sr = new StringBuilder();
			foreach (byte b in bytes)
			{
				sr.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
			}
			return sr.ToString();
		}

		public String BitsToText(String input)
		{
			int numOfBytes = input.Length / 8;
			byte[] bytes = new byte[numOfBytes];
			for (int i = 0; i < numOfBytes; ++i)
			{
				bytes[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);
			}

			UTF8Encoding encoding = new UTF8Encoding();


			return encoding.GetString(bytes);
		}

		//Szyfruj
		private void button5_Click(object sender, EventArgs e)
		{
			String input = TextToBits(richTextBox1.Text);
			String output = "";
			String text = richTextBox4.Text;
		
			for (int i = 0; i < input.Length ; i++)
			{
				output += XORchar(input[i],text[i]);
			}
			richTextBox3.Text += output;
		}

		//deszyfruj
		private void button7_Click_1(object sender, EventArgs e)
		{
			String input = richTextBox1.Text;
			String output = "";
			String text = richTextBox4.Text;
			//MessageBox.Show(input);

			for (int i = 0; i < input.Length; i++)
			{
				output += XORchar(input[i], text[i]);
			}

			//MessageBox.Show(output);

			int numOfBytes = output.Length / 8;
			byte[] bytes = new byte[numOfBytes];
			for (int i = 0; i < numOfBytes; ++i)
			{
				bytes[i] = Convert.ToByte(output.Substring(8 * i, 8), 2);
			}

			UTF8Encoding encoding = new UTF8Encoding();
			//MessageBox.Show(encoding.GetString(bytes));
			
			richTextBox3.Text += encoding.GetString(bytes);
		}

		//Kopiuj z Generatora
		private void button10_Click(object sender, EventArgs e)
		{
			richTextBox4.Text = richTextBox2.Text;
		}

		//kopiuj z wyjscia
		private void button9_Click(object sender, EventArgs e)
		{
			richTextBox1.Text = richTextBox3.Text;
			richTextBox3.ResetText();
		}

		//wczytaj
		private void button12_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 2;
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
					{
						richTextBox1.Text = sr.ReadToEnd();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		//Wczytaj2
		private void button8_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 2;
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
					{
						richTextBox4.Text = sr.ReadToEnd();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		//zapisz
		private void button11_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.InitialDirectory = "c:\\";
			saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
			saveFileDialog1.FilterIndex = 2;
			saveFileDialog1.RestoreDirectory = true;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				using (StreamWriter outputFile = new StreamWriter(saveFileDialog1.FileName))
				{
					outputFile.Write(richTextBox3.Text);
				}
			}
		}

		//Zapisz1
		private void button2_Click(object sender, EventArgs e)
		{

			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.InitialDirectory = "c:\\";
			saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
			saveFileDialog1.FilterIndex = 2;
			saveFileDialog1.RestoreDirectory = true;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				using (StreamWriter outputFile = new StreamWriter(saveFileDialog1.FileName))
				{
					outputFile.Write(richTextBox2.Text);
				}
			}
		}

	   
		//Reset1
		private void button3_Click(object sender, EventArgs e)
		{
			richTextBox1.ResetText();
			richTextBox2.ResetText();
		}
		//Reset2
		private void button6_Click(object sender, EventArgs e)
		{
			richTextBox4.ResetText();
			richTextBox1.ResetText();
			richTextBox3.ResetText();
		}

		

	   

	   

	   

	   
	}
}
