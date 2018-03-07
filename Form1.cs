using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scheduler_CPU
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int n;//number of processes
        private void button1_Click(object sender, EventArgs e)
        {
            n = int.Parse(nOfProcesses.Text);
        }
        int []burst_time=new int[10];//max number of processe is 10
        int i = 0;
        int wtime=0,averageWtime=0;
        private void button2_Click(object sender, EventArgs e)
        {
            burst_time[i] = int.Parse(burstTime_text.Text);
            i++;
            if (n == i)
            {
                button2.Enabled = false;
                calculateWaitingTime();
            }
        }
        private void calculateWaitingTime()
        {
            for(int i=0;i<n;i++)
            {
                //gantt chart code
                averageWtime += wtime;
                wtime += burst_time[i];
            }
            waitingTime_text.Text = (averageWtime / n).ToString();
        }

    }
}
