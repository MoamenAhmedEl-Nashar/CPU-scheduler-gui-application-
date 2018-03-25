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

        /* ------------------------ FCFS ----------------------------------- */

        int n_fcfs;//number of processes
        private void button1_Click(object sender, EventArgs e)
        {
            n_fcfs = int.Parse(nOfProcesses.Text);
        }
        Queue<Process> queue_fcfs = new Queue<Process>();
        int counter_fcfs = 0;
        float averageWtime_fcfs = 0;
        
        private void button2_Click(object sender, EventArgs e)
        {
            Process p_fcfs = new Process(); p_fcfs.Pid = counter_fcfs + 1;
            p_fcfs.burst_time = int.Parse(burstText_fcfs.Text);
            p_fcfs.arrival_time = int.Parse(arrivalText_fcfs.Text);
            queue_fcfs.Enqueue(p_fcfs);
            counter_fcfs++;
            if (n_fcfs == counter_fcfs)
            {
                button2.Enabled = false;
                calculateWaitingTime_fcfs();
            }
        }
        int time_fcfs = 0;
        private void calculateWaitingTime_fcfs()
        {
            Process tempp_fcfs;
            while (queue_fcfs.Count != 0)
            {

                tempp_fcfs = queue_fcfs.Dequeue();
                if (tempp_fcfs.arrival_time > time_fcfs)
                {
                    queue_fcfs.Enqueue(tempp_fcfs);
                }
                else
                {
                    tempp_fcfs.waiting_time = time_fcfs - tempp_fcfs.last_active;
                    averageWtime_fcfs += tempp_fcfs.waiting_time;
                    //////////
                    Label num_fcfs = new Label();
                    num_fcfs.Location = new Point(10 + time_fcfs * 10, 305);
                    num_fcfs.Text = time_fcfs.ToString();
                    num_fcfs.Width = tempp_fcfs.burst_time * 10;
                    flowLayoutPanel_fcfs_nums.Controls.Add(num_fcfs);
                    /////////
                    time_fcfs += tempp_fcfs.burst_time;
                    //gantt chart code
                    Label tempLabel_fcfs = new Label();
                    tempLabel_fcfs.Enabled = true;
                    tempLabel_fcfs.BorderStyle = BorderStyle.FixedSingle;
                    tempLabel_fcfs.Font = new Font("Arial", 12, FontStyle.Bold);
                    tempLabel_fcfs.Location = new Point(10 + time_fcfs * 10, 300);
                    string s = "p" + tempp_fcfs.Pid.ToString();
                    tempLabel_fcfs.Text = s;
                    tempLabel_fcfs.Width = tempp_fcfs.burst_time * 10;
                    flowLayoutPanel_fcfs.Controls.Add(tempLabel_fcfs);
                    
                    

                   

                    ////////////////////////
                    tempp_fcfs.burst_time -= tempp_fcfs.burst_time;
                    tempp_fcfs.last_active = time_fcfs;
                    
                }

            }
            waitingText_fcfs.Text = (averageWtime_fcfs / n_fcfs).ToString();
            
        }
        

        /* -------------------------- END FCFS ------------------------------ */


        /*---------------------- SJF mode Code --------------------------- */

        // number of process 
        int num_process;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            num_process = (int)numericUpDown1.Value;
        }

        // Preemptive check box
        private void checkBox1_Checked(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label7.Visible = true;
                numericUpDown3.Visible = true;
            }
            else
            {
                label7.Visible = false;
                numericUpDown3.Visible = false;
            }
        }

        // store values accending
        double sjwtime, sjaverageWtime;
        List<int> vals = new List<int>();
        private void button3_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                if (vals.Count != num_process)
                {
                    vals.Add((int)numericUpDown2.Value);
                    vals.Sort();
                }
                else if (vals.Count == num_process)
                {
                    button3.Enabled = false;

                    //cal avg waiting time code
                    for (int j = 0; j < num_process; j++)
                    {
                        //gantt chart code
                        sjaverageWtime += sjwtime;
                        sjwtime += vals[j];
                    }

                    label9.Text = (sjaverageWtime / (double)num_process).ToString() + " msec";
                }
            }


            else
            {
                //preemtive code
                MessageBox.Show("shit still not done");
            }

            MessageBox.Show("Inserted");

        }

        /*---------------------------- End SJF mode Code-----------------------------*/


        /*----------------------------- priority mode code---------------------------*/

        //num of process
        int num_process_prio;
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            num_process_prio = (int)numericUpDown5.Value;
        }

        // Preemptive checkbox
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                label14.Visible = true;
                numericUpDown6.Visible = true;
            }
            else
            {
                label14.Visible = false;
                numericUpDown6.Visible = false;
            }
        }

        // store values and priority
        double prwtime, praverageWtime;
        List<int[]> pri_vals = new List<int[]>();
        private void button4_Click(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                int[] process = new int[2];
                process[0] = (int)numericUpDown4.Value; //time
                process[1] = (int)numericUpDown7.Value; //priority
                pri_vals.Add(process);
                pri_vals = pri_vals.OrderBy(arr => arr[1]).ToList(); //sorting based on priority

                if (pri_vals.Count == num_process_prio)
                {
                    button4.Enabled = false;

                    //cal avg waiting time code
                    for (int j = 0; j < num_process_prio; j++)
                    {
                        //gantt chart code
                        praverageWtime += prwtime;
                        prwtime += pri_vals[j][0];
                    }

                    label12.Text = (praverageWtime / (double)num_process_prio).ToString() + " msec";
                }
            }

            else
            {
                //preemtive code
                MessageBox.Show("shit still not done");
            }

            MessageBox.Show("Inserted");
        }

        /*-------------------- End priority mode code -------------------------- */

        /*---------------------Round Robin mode code ---------------------------*/
        int n_RR;//number of processes
        int quantum;
        private void button6_Click_1(object sender, EventArgs e)
        {
            n_RR = int.Parse(textBox2.Text);
            quantum = int.Parse(textBox4.Text);
            MessageBox.Show("ok!");
        }
        Queue<Process> queue = new Queue<Process>();
        int counter_RR = 0;
        float averageWtime_RR = 0;
        private void insertButton_RR_Click(object sender, EventArgs e)
        {
            Process p = new Process(); p.Pid = counter_RR + 1;
            p.burst_time = int.Parse(burstText_RR.Text);
            p.arrival_time = int.Parse(arrivalText_RR.Text);
            queue.Enqueue(p);
            counter_RR++;
            if (n_RR == counter_RR)
            {
                insertButton_RR.Enabled = false;
                calculateWaitingTime_RR();
            }

        }
        int time_RR = 0;
        public void calculateWaitingTime_RR()
        {
            Process tempp;
            while (queue.Count != 0)
            {

                tempp = queue.Dequeue();
                if (tempp.arrival_time > time_RR)
                {
                    queue.Enqueue(tempp);
                }
                else
                {
                    if (tempp.burst_time < quantum)
                    {
                        tempp.waiting_time = time_RR - tempp.last_active;
                        averageWtime_RR += tempp.waiting_time;
                        //////////
                        Label num_RR = new Label();
                        num_RR.Location = new Point(10 + time_RR * 10, 305);
                        num_RR.Text = time_RR.ToString();
                        num_RR.Width = tempp.burst_time * 10;
                        flowLayoutPanel_RR_nums.Controls.Add(num_RR);
                        /////////
                        time_RR += tempp.burst_time;
                        //gantt chart code
                        Label tempLabel_RR = new Label();
                        tempLabel_RR.Enabled = true;
                        tempLabel_RR.BorderStyle = BorderStyle.FixedSingle;
                        tempLabel_RR.Font = new Font("Arial", 12, FontStyle.Bold);
                        tempLabel_RR.Location = new Point(10 + time_RR * 10, 300);
                        string s = "p" + tempp.Pid.ToString();
                        tempLabel_RR.Text = s;
                        tempLabel_RR.Width = tempp.burst_time * 10;
                        flowLayoutPanel_RR.Controls.Add(tempLabel_RR);

                        
                        

                        ////////////////////////
                        tempp.burst_time -= tempp.burst_time;
                        tempp.last_active = time_RR;

                    }
                    else
                    {
                        tempp.waiting_time = time_RR - tempp.last_active;
                        averageWtime_RR += tempp.waiting_time;
                        //////////
                        Label num_RR = new Label();
                        num_RR.Location = new Point(10 + time_RR * 10, 305);
                        num_RR.Text = time_RR.ToString();
                        num_RR.Width = quantum * 10;
                        flowLayoutPanel_RR_nums.Controls.Add(num_RR);
                        /////////
                        time_RR += quantum;
                        //gantt chart code
                        Label tempLabel_RR = new Label();
                        tempLabel_RR.Enabled = true;
                        tempLabel_RR.BorderStyle = BorderStyle.FixedSingle;
                        tempLabel_RR.Font = new Font("Arial", 12, FontStyle.Bold);
                        tempLabel_RR.Location = new Point(10 + time_RR * 10, 300);
                        string s = "p" + tempp.Pid.ToString();
                        tempLabel_RR.Text = s;
                        tempLabel_RR.Width = quantum * 10;
                        flowLayoutPanel_RR.Controls.Add(tempLabel_RR);


                        

                        ////////////////////////
                        tempp.burst_time -= quantum;
                        tempp.last_active = time_RR;
                        if (tempp.burst_time != 0) queue.Enqueue(tempp);//20  16

                    }

                    //gantt chart code


                }
            }




            waitingText_RR.Text = (averageWtime_RR / n_RR).ToString();
        }
        /*---------------------End Round Robin mode code-------------------------*/


    }
}
class Process
{
    public int Pid;
    public int burst_time;
    public int waiting_time = 0;
    public int last_active = 0;
    public int arrival_time;
}