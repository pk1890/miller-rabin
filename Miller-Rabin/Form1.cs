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
//////////////////////////////////////////////////////////
using System.Numerics; // :D #perfekcja
using System.Security.Cryptography; // :D

namespace Miller_Rabin
{
    public partial class Form1 : Form
    {
        bool kop = false;
        BigInteger liczba = 3;
        int bits_num;
        StreamWriter plik;
        bool Miller_Rabin(BigInteger n, int k) // zwraca prawdê, jesli liczba jest prawdopodobnie pierwsza
{
    Int32 s = 0;
    BigInteger d, a;
    BigInteger two = new BigInteger(2);
    bool cont;
    if(n%2 == 0) return false;
    BigInteger q = n-1;
    while(q%2 == 0)
    {
        s++;
        q /= 2;
    }
    d = (n - 1) / BigInteger.Pow(two, s);
    RandomNumberGenerator rand = RandomNumberGenerator.Create();
    byte[] bytes = new byte[n.ToByteArray().LongLength];
    for(int i = 0; i < k; i++)
    {
        cont = false;
        do
        {
            rand.GetBytes(bytes);
            a = new BigInteger(bytes);
        } while (a < 2 || a >= n - 2);
        BigInteger x = BigInteger.ModPow(a, d, n);
      if(x == 1 || x == n - 1)
        continue;
 
      for(int r = 1; r < s; r++)
      {
        x = BigInteger.ModPow(x, 2, n);
        if(x == 1)
          return false;
        if(x == n - 1)
          break;
      }
 
      if(x != n - 1)
        return false;
    }

    return true;
}



        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.SelectedPath == "")
            {
                button3_Click(sender, e);
            }
            numericUpDown1.ReadOnly = true;
            numericUpDown1.Maximum = numericUpDown1.Value;
            numericUpDown1.Minimum = numericUpDown1.Value;
            button3.Enabled = false;
            kop = true;
            plik = new StreamWriter(@folderBrowserDialog1.SelectedPath + "\\liczby_pierwsze_" + Convert.ToString(numericUpDown1.Value) + "-bitowe.prime", true);
            if (numericUpDown1.Value % 8 != 0)
            {
                numericUpDown1.Value = numericUpDown1.Value + (8-(numericUpDown1.Value%8));
            }
            bits_num = Convert.ToInt32(numericUpDown1.Value);
            progressBar1.MarqueeAnimationSpeed = 25;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (kop == true)
            {
                RandomNumberGenerator rnd = RandomNumberGenerator.Create();
                byte [] bytes = new byte[bits_num / 8];
                do
                {
                    rnd.GetBytes(bytes);
                    liczba = new BigInteger(bytes);
                    liczba = BigInteger.Abs(liczba);
                } while (liczba % 2 == 0);
                if (Miller_Rabin(liczba, 20))
                {
                    richTextBox1.AppendText(liczba.ToString() + "\n\n");
                    richTextBox1.ScrollToCaret();
                    plik.WriteLine(liczba);
                }
               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            kop = false;
            plik.Close();
            progressBar1.MarqueeAnimationSpeed = 0;
            numericUpDown1.ReadOnly = false;
            numericUpDown1.Maximum = 4096;
            numericUpDown1.Minimum = 1;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
