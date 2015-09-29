using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        // End of the common data

        bool flag_A = false;
        bool flag_B = false;
        int turn = 1;   // 0 - A, 1 - B

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

        }

        public void thread_B()
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

                if (inputed_coin < wanted_coins * count[wanted_coins])
                {
                    result = inputed_coin / wanted_coins;
                    count[wanted_coins] -= result;
                    count[inputed_coin]++;
                }
                else
                    result = 0;

                // End of critical section


                turn = 0;
                flag_B = false;
            }



        }
    }
}
