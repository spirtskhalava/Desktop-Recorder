using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Threading;


namespace Cream
{
    public partial class Form2 : Form
    {


        [DllImport("user32.dll")]

        public static extern int ExitWindowsEx(int operationFlag, int operationReason);


        // Cream cream = new Cream();
      Cream f1  = new Cream();
        public int duration = 60;
        System.Windows.Forms.Timer timer6 = new System.Windows.Forms.Timer();

       


        public Form2()
        {
            

                InitializeComponent();
            timer6.Tick += new EventHandler(count_down);
            timer6.Interval = 1000;
            timer6.Start();


        }


        private void count_down(object sender, EventArgs e)
        {


            if (duration == 0)
            {


                //MessageBox.Show("stopped");
                timer6.Stop();
                f1.buttonRecord.PerformClick();
                Thread.Sleep(60000);
                ExitWindowsEx(0, 0);

            }
            else if (duration > 0)
            {
                duration--;
               label2.Text = duration.ToString();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
           f1.buttonRecord.PerformClick();
            // ExitWindowsEx(0, 0);

        }

        public void button4_Click(object sender, EventArgs e)
        {
            
           duration =duration + 600;
        }

       
    }




    }

