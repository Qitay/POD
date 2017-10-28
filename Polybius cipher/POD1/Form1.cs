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

namespace POD1
{
    public partial class Form1 : Form
    {
        public String Key;
        public char[] Tab;
        
        public Form1()
        {
            InitializeComponent();
        }

        public Boolean checkKey(string key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                char test = key[i];
                for (int j = i+1; j < key.Length; j++)
                {
                    if ((test == 'i' && key[j] == 'j') || (test == 'j' && key[j] == 'i'))
                    {
                        MessageBox.Show("Błąd: W kluczu znajduje się 'i', oraz 'j'");
                        return false;
                    }
                    if (test == key[j])
                    {
                        MessageBox.Show("Błąd: W kluczu powtarzają się litery");
                        return false;
                    }
                }
            }
            return true;
        }
        public char removeSpecial(char a)
        {
            if (a == 'j')
            {
                return 'i';
            }
            if (a == 'ż')
            {
                return 'z';
            }
            if (a == 'ź')
            {
                return 'z';
            }
            if (a == 'ó')
            {
                return 'o';
            }
            if (a == 'ą')
            {
                return 'a';
            }
            if (a == 'ę')
            {
                return 'e';
            }
            if (a == 'ł')
            {
                return 'l';
            }
            if (a == 'ś')
            {
                return 's';
            }
            if (a == 'ć')
            {
                return 'c';
            }
            return a;
        }
        public void fillTab()
        {
            char Char = 'a';
            Tab = new char[25];
            for (int i = 0; i < Key.Length; i++)
            {
                Tab[i] = removeSpecial(Key[i]);
            }
            for (int i = Key.Length; i < 25; i++)
            {
                while (Key.Contains(Char) || Char == 'j')
                {
                    Char++;
                }
                Tab[i] = Char;
                Char++;
            }
            String a = new String(Tab);
            MessageBox.Show(a);
        }
        public String charToNum(char a)
        {
            int b = 0;
            a = removeSpecial(a);
            for (int i = 1; i <= 25; i++)
            {
                if (a == Tab[i-1])
                {
                    if(i<=5)
                    {
                        b = 10 + i;
                    }
                    if (i <= 10 && i >5)
                    {
                        b = 20 + i-5;
                    }
                    if (i <= 15 && i > 10)
                    {
                        b = 30 + i-10;
                    }
                    if (i <= 20 && i > 15)
                    {
                        b = 40 + i-15;
                    }
                    if (i <= 25 && i > 20)
                    {
                        b = 50 + i-20;
                    }
                }
            }
            return b.ToString();
        }
        


        public void szyfruj()
        {
            for (int i = 0; i < richTextBox1.Text.Length; i++)
            {
                if (removeSpecial(Char.ToLower(richTextBox1.Text[i])) < 'a' || removeSpecial(Char.ToLower(richTextBox1.Text[i])) > 'z')
                {
                    richTextBox2.Text += removeSpecial(richTextBox1.Text[i]);
                }
                else
                {
                    richTextBox2.Text += charToNum(Char.ToLower(richTextBox1.Text[i]));
                }
                    
            }
        }

        public void deszyfruj()
        {
            for (int i = 0; i < richTextBox4.Text.Length; i++)
            {
                if(richTextBox4.Text[i] < '1' || richTextBox4.Text[i] > '5')
                {
                    richTextBox3.Text += richTextBox4.Text[i];
                }
                else
                {
                    int tmp1 = (int)Char.GetNumericValue(richTextBox4.Text[i]);
                    int tmp2 = (int)Char.GetNumericValue(richTextBox4.Text[i + 1]);
                    i++;

                    int result = (tmp1 - 1) * 5 + tmp2;
                    richTextBox3.Text += Tab[result - 1];
                } 
            }
        }

        //Wczytaj1
        private void button1_Click(object sender, EventArgs e)
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
            textBox1.ResetText();
            richTextBox1.ResetText();
            richTextBox2.ResetText();
        }

        //Szyfruj
        private void button4_Click(object sender, EventArgs e)
        {
            Key = textBox1.Text;
            if (textBox1.Text.Length != 0 && richTextBox1.Text.Length != 0)
            {
                if (checkKey(Key))
                {
                    fillTab();
                    szyfruj();
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

        //Zapisz2
        private void button7_Click(object sender, EventArgs e)
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

        //Reset2
        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.ResetText();
            richTextBox4.ResetText();
            richTextBox3.ResetText();
        }

        //Deszyfruj
        private void button5_Click(object sender, EventArgs e)
        {
            Key = textBox2.Text;
            if (textBox2.Text.Length != 0 && richTextBox4.Text.Length != 0)
            {
                if (checkKey(Key))
                {
                    fillTab();
                    deszyfruj();
                }
            }
        }
    }
}
