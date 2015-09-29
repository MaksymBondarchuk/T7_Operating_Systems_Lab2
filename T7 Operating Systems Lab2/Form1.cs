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
                    if (inputed_coin < wanted_coins * count[wanted_coins] && wanted_coins <= inputed_coin)
                    {
                        result = inputed_coin / wanted_coins;
                        count[wanted_coins] -= result;
                        count[inputed_coin]++;
                    }
                    else
                        result = 0;

                    if (tbOutput.InvokeRequired)
                    {
                        Invoke((MethodInvoker)(() => tbOutput.Text = Convert.ToString(result)));
                    }
                    else
                    {
                        tbOutput.Text = Convert.ToString(result);
                    }
                    //tbOutput.Text = Convert.ToString(result);

                    have_new_coin = false;
                }
                // End of critical section
                
                turn = 0;
                flag_B = false;

                await Task.Delay(1000);
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

                await Task.Delay(1000);
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

                if (!coin_inputed || !button_pressed)
                    bGo.Enabled = false;
            }
        }

        private void do_button_pressed()
        {
            button_pressed = true;
            if (coin_inputed && button_pressed)
                bGo.Enabled = true;
        }

        private void b1_Click(object sender, EventArgs e)
        {
            do_button_pressed();
            tbWant.Text = "1";
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            go = true;
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
    }
}
