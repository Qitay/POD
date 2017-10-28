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


namespace POD7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        String singlebits(String input)
        {
            String output = "_________________ Test pojedynczych bitów _______________\n";
            if (input.Length < 20000)
            {
                return output + "Test zakończony niepowodzeniem - zbyt krótkie wejście\n\n";
            }
            int count = input.Length / 20000;
            String[] tabin = new String[count];
            for (int i = 0; i < count; i++)
            {
                tabin[i] = input.Substring(i * 20000, 20000);
            }
            int ok = 0;
            double srednia = 0;
            for (int i = 0; i < count; i++)
            {
                int n0 = 0, n1 = 0; 
                foreach (char c in tabin[i])
                {
                    if (c == '0')
                        n0++;
                }
                n1 = 20000 - n0;
                int test = n0 - n1;
                srednia += Math.Abs(test);
                if (test < 550 && test > -550)
                {
                    output += "Test zakończony powodzeniem:\t" + n0 + "\t" + n1 + "\n";
                    ok++;
                }
                else
                    output += "Test zakończony niepowodzeniem:\t" + n0 + "\t" + n1 + "\n";
            }
            srednia = srednia / count;
            return output + "\nTesty zakończone powodzeniem: " + ok + "/" + count + ", Średnia różnica: " + srednia + "\n_________________________________________________________\n\n";
        }

        String pairs(string input)
        {
            String output = "_____________________ Test par bitów ____________________\n";
            if (input.Length < 20000)
            {
                return output + "Test zakończony niepowodzeniem - zbyt krótkie wejście\n\n";
            }
            int count = input.Length / 20000;
            String[] tabin = new String[count];
            for (int i = 0; i < count; i++)
            {
                tabin[i] = input.Substring(i * 20000, 20000);
            }
            int ok = 0;
            double srednia = 0;
           
            for (int i = 0; i < count; i++)
            {
                int n00 = 0, n01 = 0, n10 = 0, n11 = 0;
                
                String tmp = "";
                for (int j = 0; j < 20000 -1; j++)
                {
                    tmp = tabin[i].Substring(j, 2);
                    if (tmp == "00")
                        n00++;
                    else if (tmp == "01")
                        n01++;
                    else if (tmp == "10")
                        n10++;
                    else
                        n11++;
                }
                int test1 = n00 - n01, test2 = n00 - n10, test3 = n00 - n11, test4 = n01 - n10, test5 = n01 - n11, test6 = n10 - n11;
                srednia += (Math.Abs(test1) + Math.Abs(test2) + Math.Abs(test3) + Math.Abs(test4) + Math.Abs(test5) + Math.Abs(test6)) / 6;
                if ((test1 < 275 && test1 > -275) && (test2 < 275 && test2 > -275) && (test3 < 275 && test3 > -275) && (test4 < 275 && test4 > -275) && (test5 < 275 && test5 > -275) && (test6 < 275 && test6 > -275))
                {
                    output += "Test zakończony powodzeniem\t" + n00 + " " + n01 + " " + n10 + " " + n11 + "\n";
                    ok++;
                }
                else
                    output += "Test zakończony niepowodzeniem\t" + n00 + " " + n01 + " " + n10 + " " + n11 + "\n";
            }
            srednia = srednia / count;
            return output + "\nTesty zakończone powodzeniem: " + ok + "/" + count + ", Średnia różnica: " + srednia + "\n_________________________________________________________\n\n";
        }

        String series(string input)
        {
            String output = "_______________________ Test serii ______________________\n";
            if (input.Length < 20000)
            {
                return output + "Test zakończony niepowodzeniem - zbyt krótkie wejście\n\n";
            }
            int count = input.Length / 20000;
            String[] tabin = new String[count];
            for (int i = 0; i < count; i++)
            {
                tabin[i] = input.Substring(i * 20000, 20000);
            }
            int ok = 0;
           
            for (int i = 0; i < count; i++)
            {
                int ser1 = 0, ser2 = 0, ser3 = 0, ser4 = 0, ser5 = 0, ser6 = 0;
                int seria = 0;
                char compare = tabin[i][0];

                foreach (char c in tabin[i])
                {
                    if (compare == c)
                        seria++;
                    else
                    {
                        if (seria == 1)
                            ser1++;
                        if (seria == 2)
                            ser2++;
                        if (seria == 3)
                            ser3++;
                        if (seria == 4)
                            ser4++;
                        if (seria == 5)
                            ser5++;
                        if (seria >= 6)
                            ser6++;

                        compare = c;
                        seria = 0;
                    }
                }
                if ((ser1 < 2685 && ser1 > 2315) && (ser2 < 1386 && ser2 > 1114) && (ser3 < 723 && ser3 > 527) &&
                (ser4 < 384 && ser4 > 240) && (ser5 < 209 && ser5 > 103) && (ser6 < 209 && ser6 > 103))
                {
                    output += "Test zakończony powodzeniem\t" + ser1 + " " + ser2 + " " + ser3 + " " + ser4 + " " + ser5 + " " + ser6 + "\n";
                    ok++;
                }
                else
                    output += "Test zakończony niepowodzeniem\t" + ser1 + " " + ser2 + " " + ser3 + " " + ser4 + " " + ser5 + " " + ser6 + "\n";
            }
            return output + "\nPodsumowanie:" + " Testy zakończone powodzeniem: " + ok + "/" + count + "\n_________________________________________________________\n\n";
        }

        String longseries(string input)
        {
            String output = "___________________ Test długich serii __________________\n";
            if (input.Length < 20000)
            {
                return output + "Test zakończony niepowodzeniem - zbyt krótkie wejście\n\n";
            }
            int count = input.Length / 20000;
            String[] tabin = new String[count];
            for (int i = 0; i < count; i++)
            {
                tabin[i] = input.Substring(i * 20000, 20000);
            }
            int ok = 0;

            for (int i = 0; i < count; i++)
            {
                int seria = 0;
                int seriamax = 1;
                char compare = tabin[i][0];

                for (int j = 0; j < tabin[i].Length; j++ )
                {
                    if (compare == tabin[i][j])
                        seria++;
                    else
                    {
                        if (seriamax < seria)
                        {
                            seriamax = seria;
                        }
                        compare = tabin[i][j];
                        seria = 0;
                    }
                }
                if (seriamax < seria)
                {
                    seriamax = seria;
                }
                if (seriamax <= 26)
                {
                    output += "Test zakończony powodzeniem:\t" + seriamax +"\n";
                    ok++;
                }
                else
                    output += "Test zakończony niepowodzeniem\t" + seriamax + "\n";

            }
            return output + "\nPodsumowanie:" + " Testy zakończone powodzeniem: " + ok + "/" + count + "\n_________________________________________________________\n\n";
        }


        //pojedyncze bity
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += singlebits(richTextBox2.Text);
        }

        //Pary bitow
        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += pairs(richTextBox2.Text);
        }

        //serie
        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += series(richTextBox2.Text);
        }

        //Długie serie
        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += longseries(richTextBox2.Text);
        }

        //wszystko
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += singlebits(richTextBox2.Text);
            richTextBox1.Text += pairs(richTextBox2.Text);
            richTextBox1.Text += series(richTextBox2.Text);
            richTextBox1.Text += longseries(richTextBox2.Text);
        }

        //Wczytej z pliku
        private void button2_Click(object sender, EventArgs e)
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
                        richTextBox2.Text = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        //Reset
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox2.ResetText();
            richTextBox1.ResetText();
        }

        //zapisz
        private void button4_Click(object sender, EventArgs e)
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
                    String[] tab = richTextBox1.Text.Split('\n');
                    for (int i = 0; i < tab.Length; i++)
                    {
                        outputFile.WriteLine(tab[i]);
                    }
                }
            }
        }
    }
}
