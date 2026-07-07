using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Q4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] input = textBox1.Text
                .Split(new char[] { '/',' ' },StringSplitOptions.RemoveEmptyEntries);

            int a = int.Parse(input[0]);
            int b = int.Parse(input[1]);
            List<int> num = new List<int>();

            do
            {
                int c = a % b;
                if (b == 0) break;
                int ad = a / b;

                num.Add(ad);
                a = b;
                b = c;
                if (b == 0) break;
            } while (a % b != 0 || a / b != 0);

            if (b != 0)
                num.Add(a);
            textBox2.Text = string.Empty;
            for(int i = 0; i < num.Count; i++)
            {
                if (i == 0) textBox2.Text += $"{num.ElementAt(i)};";
                else if (i == num.Count - 1) textBox2.Text += $"{num.ElementAt(i),2}";
                else textBox2.Text += $"{num.ElementAt(i),2},";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }
    }
}
