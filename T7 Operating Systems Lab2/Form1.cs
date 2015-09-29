using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T7_Operating_Systems_Lab2
{
    public partial class Form1 : Form
    {
        // Common data
        int inputed_coin;
        int wanted_coins;
        int result; // 0 - unsuccessfull, other - number of coins
        List<int> count = new List<int>(101);
        bool have_new_coin = false;
        // End of the common data

        bool flag_A = false;
        bool flag_B = false;
        int turn = 1;   // 0 - A, 1 - B

        bool coin_inputed = false;
        bool button_pressed = false;
        bool go = false;

        const int time_delay = 50;
        const string text_insert = "Insert your coin";
        const string text_select_value = "Select value of coins you want";
        const string text_wait = "Please wait for a little";

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < count.Capacity; i++)
                count.Add(0);
            count[1] = 10;
            count[2] = 10;
            count[5] = 10;
            count[10] = 10;
            count[25] = 10;
            count[50] = 10;
            count[100] = 10;

            l1.Text = String.Format("  1-cent - {0}", count[1]);
            l2.Text = String.Format("  2-cent - {0}", count[2]);
            l5.Text = String.Format("  5-cent - {0}", count[5]);
            l10.Text = String.Format(" 10-cent - {0}", count[10]);
            l25.Text = String.Format(" 25-cent - {0}", count[25]);
            l50.Text = String.Format(" 50-cent - {0}", count[50]);
            l100.Text = String.Format("100-cent - {0}", count[100]);


            Thread TA = new Thread(() => thread_A(this), 1000);
            Thread TB = new Thread(() => thread_B(this), 1000);
            TA.Start();
            TB.Start();
            TA.Join();
            TB.Join();
        }

        async public void thread_B(object sender)
        {
            while (true)
            {
                flag_B = true;
                while (flag_A)
                {
                    if (turn != 1)
                    {
                        flag_B = false;

                        while (turn != 1)
                        { }
                        flag_B = true;
                    }
                }


                // Critical section
                if (have_new_coin)
                {
                    string text;
                    // Trying to exchange coins
                    if (inputed_coin <= wanted_coins * count[wanted_coins] && wanted_coins <= inputed_coin)
                    {
                        result = inputed_coin / wanted_coins;
                        count[wanted_coins] -= result;
                        count[inputed_coin]++;

                        text = String.Format("{0} {1}-cent coin(s)", result, wanted_coins);
                    }
                    else
                    {
                        result = 0;
                        text = "nothing";
                    }

                    // Updating text fields
                    if (tbOutput.InvokeRequired)
                    {
                        Invoke((MethodInvoker)(() => tbOutput.Text = text));
                    }
                    else
                    {
                        tbOutput.Text = text;
                    }

                    if (l1.InvokeRequired || l2.InvokeRequired || l5.InvokeRequired || l10.InvokeRequired || l25.InvokeRequired || l50.InvokeRequired || l100.InvokeRequired)
                    {
                        Invoke((MethodInvoker)(() => l1.Text = String.Format("  1-cent - {0}", count[1])));
                        Invoke((MethodInvoker)(() => l2.Text = String.Format("  2-cent - {0}", count[2])));
                        Invoke((MethodInvoker)(() => l5.Text = String.Format("  5-cent - {0}", count[5])));
                        Invoke((MethodInvoker)(() => l10.Text = String.Format(" 10-cent - {0}", count[10])));
                        Invoke((MethodInvoker)(() => l25.Text = String.Format(" 25-cent - {0}", count[25])));
                        Invoke((MethodInvoker)(() => l50.Text = String.Format(" 50-cent - {0}", count[50])));
                        Invoke((MethodInvoker)(() => l100.Text = String.Format("100-cent - {0}", count[100])));
                    }
                    else
                    {
                        l1.Text = String.Format("  1-cent - {0}", count[1]);
                        l2.Text = String.Format("  2-cent - {0}", count[2]);
                        l5.Text = String.Format("  5-cent - {0}", count[5]);
                        l10.Text = String.Format(" 10-cent - {0}", count[10]);
                        l25.Text = String.Format(" 25-cent - {0}", count[25]);
                        l50.Text = String.Format(" 50-cent - {0}", count[50]);
                        l100.Text = String.Format("100-cent - {0}", count[100]);
                    }

                    if (lWhatToDo.InvokeRequired)
                    {
                        Invoke((MethodInvoker)(() => lWhatToDo.Text = text_insert));
                    }
                    else
                    {
                        lWhatToDo.Text = text_insert;
                    }

                    have_new_coin = false;
                }
                // End of critical section

                turn = 0;
                flag_B = false;

                await Task.Delay(time_delay);
            }
        }

        async public void thread_A(object sender)
        {
            while (true)
            {
                flag_A = true;
                while (flag_B)
                {
                    if (turn != 0)
                    {
                        flag_A = false;

                        while (turn != 0)
                        { }
                        flag_A = true;
                    }
                }


                // Critical section
                if (go)
                {
                    have_new_coin = true;
                    inputed_coin = Convert.ToInt32(tbCoin.Text);
                    wanted_coins = Convert.ToInt32(tbWant.Text);

                    go = false;
                }
                // End of critical section

                turn = 1;
                flag_A = false;

                await Task.Delay(time_delay);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (0 < tbCoin.Text.Length)
            {
                b1.Enabled = true;
                b2.Enabled = true;
                b5.Enabled = true;
                b10.Enabled = true;
                b25.Enabled = true;
                b50.Enabled = true;
                b100.Enabled = true;

                coin_inputed = true;
                if (coin_inputed && button_pressed)
                    bGo.Enabled = true;

                lWhatToDo.Text = "Select value of coins you want";
            }
            else
            {
                b1.Enabled = false;
                b2.Enabled = false;
                b5.Enabled = false;
                b10.Enabled = false;
                b25.Enabled = false;
                b50.Enabled = false;
                b100.Enabled = false;

                coin_inputed = false;

                bGo.Enabled = false;

                tbOutput.Text = "";
                tbWant.Text = "";
                lWhatToDo.Text = text_insert;
            }
        }

        private void do_button_pressed()
        {
            button_pressed = true;
            if (coin_inputed && button_pressed)
            {
                bGo.Enabled = true;
            }
            tbOutput.Text = "";
        }

        private void b1_Click(object sender, EventArgs e)
        {
            do_button_pressed();
            tbWant.Text = "1";
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            string icoin = tbCoin.Text;
            if (icoin == "1" || icoin == "2" || icoin == "5" || icoin == "10" || icoin == "25" || icoin == "50" || icoin == "100")
            {
                go = true;

                b1.Enabled = false;
                b2.Enabled = false;
                b5.Enabled = false;
                b10.Enabled = false;
                b25.Enabled = false;
                b50.Enabled = false;
                b100.Enabled = false;
                coin_inputed = false;

                bGo.Enabled = false;
                lWhatToDo.Text = text_insert;
            }
            else
                tbOutput.Text = "Wrong coin inputed";
        }

        private void b2_Click(object sender, EventArgs e)
        {
            do_button_pressed();
            tbWant.Text = "2";
        }

        private void b5_Click(object sender, EventArgs e)
        {
            do_button_pressed();
            tbWant.Text = "5";
        }

        private void b10_Click(object sender, EventArgs e)
        {
            do_button_pressed();
            tbWant.Text = "10";
        }

        private void b25_Click(object sender, EventArgs e)
        {
            do_button_pressed();
            tbWant.Text = "25";
        }

        private void b50_Click(object sender, EventArgs e)
        {
            do_button_pressed();
            tbWant.Text = "50";
        }

        private void b100_Click(object sender, EventArgs e)
        {
            do_button_pressed();
            tbWant.Text = "100";
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            tbWant.Text = "";
            tbCoin.Text = "";
            tbOutput.Text = "";

            bGo.Enabled = false;
            b2.Enabled = false;
            b5.Enabled = false;
            b10.Enabled = false;
            b25.Enabled = false;
            b50.Enabled = false;
            b100.Enabled = false;

            coin_inputed = false;
            lWhatToDo.Text = text_insert;
        }

        private void tbCoin_Click(object sender, EventArgs e)
        {
            tbCoin.Text = "";
        }
    }
}
