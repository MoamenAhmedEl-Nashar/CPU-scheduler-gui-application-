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

        #region FCFS Code mode
        /* ------------------------ FCFS ----------------------------------- */

        int n_fcfs;//number of processes
        private void button1_Click(object sender, EventArgs e)
        {
            n_fcfs = int.Parse(nOfProcesses.Text);
            MessageBox.Show("ok!number of processes inserted");
            flowLayoutPanel_fcfs.Controls.Clear();
            flowLayoutPanel_fcfs_nums.Controls.Clear();
            button2.Enabled=true;
            counter_fcfs = 0;
            averageWtime_fcfs = 0;
            time_fcfs = 0;
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
            string ss_fcfs = "Inserted " + (counter_fcfs.ToString()) + " from " + (n_fcfs.ToString());
            MessageBox.Show(ss_fcfs);
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
                    tempp_fcfs.waiting_time = time_fcfs - tempp_fcfs.arrival_time;
                    averageWtime_fcfs += tempp_fcfs.waiting_time;
                    //time chart//
                    Label num_fcfs = new Label();
                    num_fcfs.Text = time_fcfs.ToString();
                    if (tempp_fcfs.burst_time < 3) num_fcfs.Width = 27;
                    else num_fcfs.Width = tempp_fcfs.burst_time * 10;
                    flowLayoutPanel_fcfs_nums.Controls.Add(num_fcfs);
                    //end time chart//
                    time_fcfs += tempp_fcfs.burst_time;
                    //gantt chart code
                    Label tempLabel_fcfs = new Label();
                    tempLabel_fcfs.Enabled = true;
                    tempLabel_fcfs.BorderStyle = BorderStyle.FixedSingle;
                    tempLabel_fcfs.Font = new Font("Arial", 10, FontStyle.Bold);
                   
                    string s = "P" + tempp_fcfs.Pid.ToString();
                    tempLabel_fcfs.Text = s;
                    if (tempp_fcfs.burst_time < 3) tempLabel_fcfs.Width = 27;
                    else tempLabel_fcfs.Width = tempp_fcfs.burst_time * 10;
                    flowLayoutPanel_fcfs.Controls.Add(tempLabel_fcfs);
                    ////////////////////////
                    tempp_fcfs.burst_time -= tempp_fcfs.burst_time;
                    tempp_fcfs.last_active = time_fcfs;

                }
                
            }
            //last time label
            Label num_fcfsLast = new Label();
            num_fcfsLast.Text = time_fcfs.ToString();
            
            flowLayoutPanel_fcfs_nums.Controls.Add(num_fcfsLast);
            //////////////////
            waitingText_fcfs.Text = (averageWtime_fcfs / n_fcfs).ToString();

        }


        /* -------------------------- END FCFS ------------------------------ */
        #endregion

        #region SJF mode Code
        /*---------------------- SJF mode Code --------------------------- */

        // number of process 
        int num_process;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            num_process = (int)numericUpDown1.Value;

            // reset and start again
            button3.Enabled = true;
            // gant.Clear();
            flowLayoutPanel5.Controls.Clear();
            flowLayoutPanel4.Controls.Clear();
            sjwtime = 0;
            sjaverageWtime = 0;
            prsjaverageWtime = 0;
            prsjwtime = 0;
            pd = 1;
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
        double sjwtime = 0, sjaverageWtime = 0, prsjaverageWtime = 0, prsjwtime = 0;
        List<Process> vals = new List<Process>();
        List<Process> premtive_vals = new List<Process>();
        int pd = 1;
        private void button3_Click(object sender, EventArgs e)
        {
            Process x = new Process(); // dummy process
            x.Pid = pd;
            pd++;
            x.burst_time = (int)numericUpDown2.Value;
            if (checkBox1.Checked) x.arrival_time = (int)numericUpDown3.Value;
            vals.Add(x);
            string ss_sjf = "Inserted " + (vals.Count.ToString()) + " from " + (num_process.ToString());
            MessageBox.Show(ss_sjf);

            if (vals.Count == num_process && !checkBox1.Checked)
            {
                button3.Enabled = false;
                vals = vals.OrderBy(arr => arr.burst_time).ToList();
                int st_t = 0;
                for (int j = 0; j < num_process; j++)
                {
                    //gantt chart code
                    ///////// type time lable
                    Label num_fcfs = new Label();
                    num_fcfs.Location = new Point(10 + st_t * 10, 305);
                    num_fcfs.Text = st_t.ToString();
                    if (vals[j].burst_time < 3) num_fcfs.Width = 28;
                    else num_fcfs.Width = vals[j].burst_time * 10 - 2;
                    flowLayoutPanel5.Controls.Add(num_fcfs);
                    /////////
                    st_t += vals[j].burst_time;
                    //// type process number lable
                    Label tempLabel_fcfs = new Label();
                    tempLabel_fcfs.Enabled = true;
                    tempLabel_fcfs.BorderStyle = BorderStyle.FixedSingle;
                    tempLabel_fcfs.Font = new Font("Arial", 10, FontStyle.Bold);
                    tempLabel_fcfs.Location = new Point(10 + time_fcfs * 10, 300);
                    string s = "P" + vals[j].Pid.ToString();
                    tempLabel_fcfs.Text = s;
                    tempLabel_fcfs.TextAlign = ContentAlignment.MiddleCenter;
                    if (vals[j].burst_time < 3) tempLabel_fcfs.Width = 30;
                    else tempLabel_fcfs.Width = vals[j].burst_time * 10;
                    flowLayoutPanel4.Controls.Add(tempLabel_fcfs);
                    ////////////////////////

                    //cal average waiting time
                    sjaverageWtime += sjwtime;
                    sjwtime += vals[j].burst_time;
                }
                //print last timelable in gant
                Label num_fcfss = new Label();
                num_fcfss.Location = new Point(10 + st_t * 10, 300);
                num_fcfss.Text = st_t.ToString();
                flowLayoutPanel5.Controls.Add(num_fcfss);
                /////////////////

                //print average time
                label9.Text = (sjaverageWtime / (double)num_process).ToString() + " msec";
                vals.Clear();
            }

            //preemtive code
            else if (vals.Count == num_process && checkBox1.Checked)
            {
                button3.Enabled = false;
                vals = vals.OrderBy(arr => arr.arrival_time).ToList();
                int indx = 0;
                for (int i = 0; i < vals.Count - 1; i++)
                {
                    Process pre_sjf = new Process();
                    pre_sjf.burst_time = vals[i + 1].arrival_time - vals[i].arrival_time;
                    pre_sjf.Pid = vals[indx].Pid;
                    pre_sjf.arrival_time = vals[indx].arrival_time;
                    premtive_vals.Add(pre_sjf);
                    vals[indx].burst_time -= pre_sjf.burst_time;
                    int bst_tm = vals[indx].burst_time;
                    for (int j = i + 1; j >= 0; j--)
                        if (vals[j].burst_time < bst_tm) indx = j;
                }

                vals = vals.OrderBy(arr => arr.burst_time).ToList();
                for (int xx = 0; xx < vals.Count; xx++) premtive_vals.Add(vals.ElementAt(xx));//join lists
                for (int jj = 0; jj < premtive_vals.Count - 1; jj++)//join similar PID
                {
                    if (premtive_vals[jj].Pid == premtive_vals[jj + 1].Pid)
                    {
                        premtive_vals[jj + 1].burst_time += premtive_vals[jj].burst_time;
                        premtive_vals.RemoveAt(jj);
                        jj = -1;
                    }
                }

                int st_t = 0; int bs = 0;
                for (int j = 0; j < premtive_vals.Count; j++)
                {
                    //gantt chart code
                    ///////// type time lable
                    Label num_fcfs = new Label();
                    num_fcfs.Location = new Point(10 + st_t * 10, 305);
                    num_fcfs.Text = st_t.ToString();
                    if (premtive_vals[j].burst_time < 3) num_fcfs.Width = 28;
                    else num_fcfs.Width = premtive_vals[j].burst_time * 10 - 2;
                    flowLayoutPanel5.Controls.Add(num_fcfs);
                    /////////
                    st_t += premtive_vals[j].burst_time;
                    //// type process number lable
                    Label tempLabel_fcfs = new Label();
                    tempLabel_fcfs.Enabled = true;
                    tempLabel_fcfs.BorderStyle = BorderStyle.FixedSingle;
                    tempLabel_fcfs.Font = new Font("Arial", 10, FontStyle.Bold);
                    tempLabel_fcfs.Location = new Point(10 + time_fcfs * 10, 300);
                    string s = "P" + premtive_vals[j].Pid.ToString();
                    tempLabel_fcfs.Text = s;
                    tempLabel_fcfs.TextAlign = ContentAlignment.MiddleCenter;
                    if (premtive_vals[j].burst_time < 3) tempLabel_fcfs.Width = 30;
                    else tempLabel_fcfs.Width = premtive_vals[j].burst_time * 10;
                    flowLayoutPanel4.Controls.Add(tempLabel_fcfs);
                    ////////////////////////

                    //cal average waiting time
                    for (int z = j - 1; z >= 0; z--)
                        if (premtive_vals[j].Pid == premtive_vals[z].Pid)
                            bs = premtive_vals[z].burst_time;
                    prsjaverageWtime += (prsjwtime - bs - premtive_vals[j].arrival_time);
                    bs = 0;
                    prsjwtime += premtive_vals[j].burst_time;
                }

                //print last timelable in gant
                Label num_fcfss = new Label();
                num_fcfss.Location = new Point(10 + st_t * 10, 300);
                num_fcfss.Text = st_t.ToString();
                flowLayoutPanel5.Controls.Add(num_fcfss);
                /////////////////

                //print avg
                label9.Text = (prsjaverageWtime / (double)num_process).ToString() + " msec";
                vals.Clear();
                premtive_vals.Clear();
            }

        }

        /*---------------------------- End SJF mode Code-----------------------------*/
        #endregion

        #region priority mode code
        /*----------------------------- priority mode code---------------------------*/

        //num of process
        int num_process_prio;
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            num_process_prio = (int)numericUpDown5.Value;

            //reset values and start again
            button4.Enabled = true;
            flowLayoutPanel7.Controls.Clear();
            flowLayoutPanel8.Controls.Clear();
            prwtime = 0;
            praverageWtime = 0;
            prtpraverageWtime = 0;
            prtprwtime = 0;
            pr_pd = 1;
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
        double prwtime = 0, praverageWtime = 0, prtprwtime = 0, prtpraverageWtime = 0; int pr_pd = 1;
        List<Process> pri_vals = new List<Process>();
        List<Process> premt_pri_vals = new List<Process>();
        private void button4_Click(object sender, EventArgs e)
        {

            Process prix = new Process();
            int[] process = new int[2];
            prix.Pid = pr_pd;
            pr_pd++;
            prix.burst_time = (int)numericUpDown4.Value; //time
            prix.priority = (int)numericUpDown7.Value; //priority
            if (checkBox2.Checked) prix.arrival_time = (int)numericUpDown6.Value; //arival time
            pri_vals.Add(prix);
            string ss_prio = "Inserted " + (pri_vals.Count.ToString()) + " from " +( num_process_prio.ToString());
            MessageBox.Show(ss_prio);

            if (pri_vals.Count == num_process_prio && !checkBox2.Checked)
            {
                button4.Enabled = false;
                pri_vals = pri_vals.OrderBy(arr => arr.priority).ToList(); //sorting based on priority
                int st_t = 0;
                for (int j = 0; j < num_process_prio; j++)
                {
                    //gantt chart code
                    ///////// type time lable
                    Label num_fcfs = new Label();
                    num_fcfs.Location = new Point(10 + st_t * 10, 305);
                    num_fcfs.Text = st_t.ToString();
                    if (pri_vals[j].burst_time < 3) num_fcfs.Width = 28;
                    else num_fcfs.Width = pri_vals[j].burst_time * 10 - 2;
                    flowLayoutPanel8.Controls.Add(num_fcfs);
                    /////////
                    st_t += pri_vals[j].burst_time;
                    //// type process number lable
                    Label tempLabel_fcfs = new Label();
                    tempLabel_fcfs.Enabled = true;
                    tempLabel_fcfs.BorderStyle = BorderStyle.FixedSingle;
                    tempLabel_fcfs.Font = new Font("Arial", 10, FontStyle.Bold);
                    tempLabel_fcfs.Location = new Point(10 + time_fcfs * 10, 300);
                    string s = "P" + pri_vals[j].Pid.ToString();
                    tempLabel_fcfs.Text = s;
                    tempLabel_fcfs.TextAlign = ContentAlignment.MiddleCenter;
                    if (pri_vals[j].burst_time < 3) tempLabel_fcfs.Width = 30;
                    else tempLabel_fcfs.Width = pri_vals[j].burst_time * 10;
                    flowLayoutPanel7.Controls.Add(tempLabel_fcfs);
                    ////////////////////////

                    //cal average waiting time
                    praverageWtime += prwtime;
                    prwtime += pri_vals[j].burst_time;
                }

                //print last timelable in gant
                Label num_fcfss = new Label();
                num_fcfss.Location = new Point(10 + st_t * 10, 300);
                num_fcfss.Text = st_t.ToString();
                flowLayoutPanel8.Controls.Add(num_fcfss);
                /////////////////
                /// print average waiting time
                label12.Text = (praverageWtime / (double)num_process_prio).ToString() + " msec";
                pri_vals.Clear();
            }

            //preemtive code
            else if (pri_vals.Count == num_process_prio && checkBox2.Checked)
            {
                button4.Enabled = false;
                pri_vals = pri_vals.OrderBy(arr => arr.arrival_time).ToList();
                int indx = 0;
                for (int i = 0; i < pri_vals.Count - 1; i++)
                {
                    Process pre_sjf = new Process();
                    pre_sjf.burst_time = pri_vals[i + 1].arrival_time - pri_vals[i].arrival_time;
                    pre_sjf.Pid = pri_vals[indx].Pid;
                    pre_sjf.arrival_time = pri_vals[indx].arrival_time;
                    premt_pri_vals.Add(pre_sjf);
                    pri_vals[indx].burst_time -= pre_sjf.burst_time;
                    for (int j = i + 1; j >= 0; j--)
                        if (pri_vals[j].priority < pri_vals[indx].priority) indx = j;
                }

                pri_vals = pri_vals.OrderBy(arr => arr.priority).ToList();
                for (int xx = 0; xx < pri_vals.Count; xx++) premt_pri_vals.Add(pri_vals.ElementAt(xx));//join lists
                for (int jj = 0; jj < premt_pri_vals.Count - 1; jj++)//join similar PID
                {
                    if (premt_pri_vals[jj].Pid == premt_pri_vals[jj + 1].Pid)
                    {
                        premt_pri_vals[jj + 1].burst_time += premt_pri_vals[jj].burst_time;
                        premt_pri_vals.RemoveAt(jj);
                        jj = -1;
                    }
                }
                int st_t = 0; int bzs = 0;
                for (int j = 0; j < premt_pri_vals.Count; j++)
                {
                    //gantt chart code
                    ///////// type time lable
                    Label num_fcfs = new Label();
                    num_fcfs.Location = new Point(10 + st_t * 10, 305);
                    num_fcfs.Text = st_t.ToString();
                    if (premt_pri_vals[j].burst_time < 3) num_fcfs.Width = 28;
                    num_fcfs.Width = premt_pri_vals[j].burst_time * 10 - 2;
                    flowLayoutPanel8.Controls.Add(num_fcfs);
                    /////////
                    st_t += premt_pri_vals[j].burst_time;
                    //// type process number lable
                    Label tempLabel_fcfs = new Label();
                    tempLabel_fcfs.Enabled = true;
                    tempLabel_fcfs.BorderStyle = BorderStyle.FixedSingle;
                    tempLabel_fcfs.Font = new Font("Arial", 10, FontStyle.Bold);
                    tempLabel_fcfs.Location = new Point(10 + time_fcfs * 10, 300);
                    string s = "P" + premt_pri_vals[j].Pid.ToString();
                    tempLabel_fcfs.Text = s;
                    tempLabel_fcfs.TextAlign = ContentAlignment.MiddleCenter;
                    if (premt_pri_vals[j].burst_time < 3) tempLabel_fcfs.Width = 30;
                    else tempLabel_fcfs.Width = premt_pri_vals[j].burst_time * 10;
                    flowLayoutPanel7.Controls.Add(tempLabel_fcfs);
                    ////////////////////////

                    //cal average waiting time
                    for (int z = j - 1; z >= 0; z--)
                        if (premt_pri_vals[j].Pid == premt_pri_vals[z].Pid)
                            bzs = premt_pri_vals[z].burst_time;
                    prtpraverageWtime += (prtprwtime - bzs - premt_pri_vals[j].arrival_time);
                    bzs = 0;
                    prtprwtime += premt_pri_vals[j].burst_time;
                }

                //print last timelable in gant
                Label num_fcfss = new Label();
                num_fcfss.Location = new Point(10 + st_t * 10, 300);
                num_fcfss.Text = st_t.ToString();
                flowLayoutPanel8.Controls.Add(num_fcfss);
                /////////////////
                /// print average waiting time
                label12.Text = (prtpraverageWtime / (double)num_process_prio).ToString() + " msec";
                pri_vals.Clear();
                premt_pri_vals.Clear();
            }
        }

        /*-------------------- End priority mode code -------------------------- */
        #endregion

        #region Round Robin mode code
        /*---------------------Round Robin mode code ---------------------------*/
        int n_RR;//number of processes
        int quantum;
        private void button6_Click_1(object sender, EventArgs e)
        {
            n_RR = int.Parse(textBox2.Text);
            quantum = int.Parse(textBox4.Text);
            MessageBox.Show("ok!number of processes inserted");
            flowLayoutPanel_RR.Controls.Clear();
            flowLayoutPanel_RR_nums.Controls.Clear();
            insertButton_RR.Enabled = true;
            counter_RR = 0;
            averageWtime_RR = 0;
            time_RR = 0;
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
            string ss_RR = "Inserted " + (counter_RR.ToString()) + " from " + (n_RR.ToString());
            MessageBox.Show(ss_RR);
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
                        tempp.waiting_time = time_RR - tempp.last_active-tempp.arrival_time;
                        averageWtime_RR += tempp.waiting_time;
                        //////////
                        Label num_RR = new Label();
                        
                        num_RR.Text = time_RR.ToString();
                        if (tempp.burst_time < 3) num_RR.Width = 27;
                        else num_RR.Width = tempp.burst_time * 10;
                        flowLayoutPanel_RR_nums.Controls.Add(num_RR);
                        /////////
                        time_RR += tempp.burst_time;
                        //gantt chart code
                        Label tempLabel_RR = new Label();
                        tempLabel_RR.Enabled = true;
                        tempLabel_RR.BorderStyle = BorderStyle.FixedSingle;
                        tempLabel_RR.Font = new Font("Arial", 10, FontStyle.Bold);
                        
                        string s = "p" + tempp.Pid.ToString();
                        tempLabel_RR.Text = s;
                        if (tempp.burst_time < 3) tempLabel_RR.Width = 27;
                        else tempLabel_RR.Width = tempp.burst_time * 10;
                        flowLayoutPanel_RR.Controls.Add(tempLabel_RR);




                        ////////////////////////
                        tempp.burst_time -= tempp.burst_time;
                        tempp.last_active = time_RR;

                    }
                    else
                    {
                        tempp.waiting_time = time_RR - tempp.last_active - tempp.arrival_time;
                        averageWtime_RR += tempp.waiting_time;
                        //////////
                        Label num_RR = new Label();
                        
                        num_RR.Text = time_RR.ToString();
                        if (quantum < 3) num_RR.Width = 27;
                        else num_RR.Width = quantum * 10;
                        flowLayoutPanel_RR_nums.Controls.Add(num_RR);
                        /////////
                        time_RR += quantum;
                        //gantt chart code
                        Label tempLabel_RR = new Label();
                        tempLabel_RR.Enabled = true;
                        tempLabel_RR.BorderStyle = BorderStyle.FixedSingle;
                        tempLabel_RR.Font = new Font("Arial", 10, FontStyle.Bold);
                        
                        string s = "p" + tempp.Pid.ToString();
                        tempLabel_RR.Text = s;
                        if (quantum < 3) tempLabel_RR.Width = 27;
                        else tempLabel_RR.Width = quantum * 10;
                        flowLayoutPanel_RR.Controls.Add(tempLabel_RR);




                        ////////////////////////
                        tempp.burst_time -= quantum;
                        tempp.last_active = time_RR;
                        if (tempp.burst_time != 0) queue.Enqueue(tempp);//20  16

                    }

                    //gantt chart code


                }
            }
            //last time label
            Label num_RRLast = new Label();
            num_RRLast.Text = time_RR.ToString();
          
            flowLayoutPanel_RR_nums.Controls.Add(num_RRLast);
            //////////////////
            waitingText_RR.Text = (averageWtime_RR / n_RR).ToString();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
        /*---------------------End Round Robin mode code-------------------------*/
        #endregion

    }
}
class Process
{
    public int Pid;
    public int burst_time;
    public int waiting_time = 0;
    public int last_active = 0;
    public int arrival_time;
    public int priority;

}