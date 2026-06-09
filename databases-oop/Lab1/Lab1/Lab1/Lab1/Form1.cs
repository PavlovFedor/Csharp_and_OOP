using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        public int mill = 0, sec = 0, min = 0, saveTimeMill,w;
        public bool work = false;
        public List<int> intervals = new List<int> { };
        public int[] array = {};

        private void StartClick(object sender, EventArgs e)
        {
            if (work)
            {
                richTextBox1.Text = "InvalidOperationExeption\n";
            }
            else
            {
                timer1.Enabled = true;
                work = true;
                saveTimeMill = mill + sec * 10 + min * 600;
                //richTextBox1.Text = "SaveTimeMll: " + saveTimeMill;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (work == false)
                richTextBox1.Text = "InvalidOperationExeption\n";
            else
            {
                timer1.Enabled = false;
                work = false;
                w = (mill + sec * 10 + min * 600) - saveTimeMill;
                intervals.Add(w);
                //richTextBox1.Text = "Add: " + w;
            }
        }
        private void buttonFunc_Click(object sender, EventArgs e)
        {
            array = intervals.ToArray<int>();
            if (array.Length < 1)
            {
                richTextBox1.Text = "Intervals = 0";
            }
            else
            {
                richTextBox1.Text = "Total: " + array.Aggregate((x, y) => x + y) + " milliseconds";
            }
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            mill += 1;
            if (mill == 10)
            {
                mill = 0;
                sec += 1;
            }
            if (sec == 60)
            {
                sec = 0;
                min += 1;
            }
            if (min == 60)
            {
                min = 0;

            }
            label1.Text = min + ":" + sec + ":" + mill;
        }

        public Form1()
        {
            InitializeComponent();
        }
    }
}
