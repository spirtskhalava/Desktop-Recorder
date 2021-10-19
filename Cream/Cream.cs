

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

    public enum State
    {
        Recording,
        NotRecording
    }

    public partial class Cream : Form
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point p);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool BlockInput(bool block);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr parentToLookFrom, IntPtr childToLookFrom, string className, IntPtr windowName);

        internal const int CTRL_C_EVENT = 0;
        [DllImport("user32.dll")]
        public static extern int ExitWindowsEx(int operationFlag, int operationReason);

        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]





        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);

        DefaultSetting setting;
        private State state = State.NotRecording;
        public Process process = null;
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        Form1 form = new Form1();
        System.Windows.Forms.Timer timer4 = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timer5 = new System.Windows.Forms.Timer();
        private string _lastMovex, _lastMovey, x, y;
        public int duration = 900000;
        System.Windows.Forms.Timer timer6 = new System.Windows.Forms.Timer();

        public Cream()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string folderName = @"c:\Videos";
            string pathString = System.IO.Path.Combine(folderName, "SubFolder");
            string pathString2 = @"c:\Videos\" + userName;

            InitializeComponent();

            //SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;

            //void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
            //{
            //    if (Environment.HasShutdownStarted)
            //    {
            //        buttonRecord.PerformClick();
            //        e.Cancel = true;

            //    }
            //    else
            //    {
            //        buttonRecord.PerformClick();
            //        e.Cancel = true;


            //    }
            //}
            if (Directory.Exists(pathString2))
            {
                //  Directory.Delete(root);
            }
            else
            {
                System.IO.Directory.CreateDirectory(pathString2);
            }

            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1800000;
            // timer1.Interval = 500; //is this one min ?  
            //Process[] processes = Process.GetProcessesByName("Cream");
            WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            Hide();
            setting = DefaultSetting.GetDefaultSetting();
            timer1.Start();
            timer1.Tick += new EventHandler(Ticks);
             BlockInput(true);
            form.Show();
            form.TopMost = true;
            form.Activate();
             form.MaximizeBox = true;
            form.MinimizeBox = false;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
            timer2.Interval = 10000;
            timer2.Start();
            timer2.Tick += new EventHandler(OnTimedEvent);
            SystemEvents.SessionEnding +=
             new SessionEndingEventHandler(SystemEvents_SessionEnding);
            timer4.Interval = 5000;
            timer4.Tick += new EventHandler(Timer_Tick);
            timer4.Enabled = true;
            timer4.Start();

            timer5.Interval = 4000;
            timer5.Tick += new EventHandler(Timer_Ticker);
            timer5.Enabled = true;
            timer5.Start();


            timer6.Tick += new EventHandler(count_down);
            timer6.Interval = 1000;
            timer6.Start();


        }




        private void Timer_Ticker(object sender, EventArgs e)
        {

            _lastMovex = Cursor.Position.X.ToString();
            _lastMovey = Cursor.Position.Y.ToString();



        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            //  textBox1.Text = Cursor.Position.X.ToString();
            // textBox2.Text = Cursor.Position.Y.ToString();
            // MessageBox.Show(Cursor.Position.X.ToString());
            x = Cursor.Position.X.ToString();
            y = Cursor.Position.Y.ToString();
            if ((_lastMovex != x) && (_lastMovey != y))
            {
                //timer4.Stop();

            }
            else
            {
                // MessageBox.Show("stopped");
                // buttonRecord.PerformClick();
                //ExitWindowsEx(0, 0);

            }


        }



        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {

            switch (e.Reason)
            {
                case SessionEndReasons.Logoff:
                    //MessageBox.Show("User logging off");
                    if (state == State.Recording)
                    {
                        // state = State.NotRecording;
                        //this.buttonRecord_Click(sender, e);
                    }

                    break;

                case SessionEndReasons.SystemShutdown:
                    //  MessageBox.Show("System is shutting down");
                    if (state == State.Recording)
                    {
                        // state = State.NotRecording;
                        //this.buttonRecord_Click(sender, e);
                    }

                    break;
            }


        }

        //void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        //{

        //}


        void OnTimedEvent(object sender, EventArgs e)
        {

            BlockInput(false);
            form.Close();
            //form2.Show();
            //form2.Show();
            //form2.TopMost = true;
            //form2.Activate();

        }


        private void Ticks(object sender, EventArgs e)

        {

            this.Close();

        }
        private void Cream_Load(object sender, EventArgs e)
        {

            IntPtr trayWindowHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWn d", IntPtr.Zero);
            IntPtr notifyWindowHandle = FindWindowEx(trayWindowHandle, IntPtr.Zero, "TrayNot ifyWnd", IntPtr.Zero);
            if (notifyWindowHandle.ToInt32() != 0)
            {

                Hide();
                Form myForm = new Cream();
                myForm.ShowInTaskbar = false;
                myForm.Visible = true;

            }

            buttonRecord.PerformClick();


        }



        private void Cream_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.BalloonTipText = "Your session is recorded";
                notifyIcon1.BalloonTipTitle = "VG AUTO";
                notifyIcon1.ShowBalloonTip(1);
            }

        }


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonRecord.PerformClick();
            ExitWindowsEx(0, 0);
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            MessageBox.Show(e.Mode.ToString());
        }

        private void buttonRecord_Click(object sender, EventArgs e)
        {
            if (state == State.Recording)
            {
                (sender as Button).Text = "Start";
                state = State.NotRecording;
                if (process != null)
                {
                    if (AttachConsole((uint)process.Id))
                    {
                        SetConsoleCtrlHandler(null, true);
                        try
                        {
                            if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0))
                                return;
                            process.WaitForExit();
                        }
                        finally
                        {
                            FreeConsole();
                            SetConsoleCtrlHandler(null, false);
                        }
                        this.Close();
                        return;
                    }

                }

            }
            else
            {
                (sender as Button).Text = "Stop";
                state = State.Recording;
                ProcessStartInfo psi = new ProcessStartInfo("c:\\ffmpeg\\bin\\ffmpeg.exe",
                        "-i cars1.flv -same_quant intermediate1.mpg");
                //ProcessStartInfo psi = new ProcessStartInfo("ffmpeg");
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                string fileName = "creamy_" + DateTime.Now.Ticks + ".mp4";
                //string filePath = @"C:\Videos\" + userName + "\\" + fileName;
                psi.Arguments = "-f gdigrab -i desktop -vcodec libx264 -pix_fmt yuv420p c:\\Videos\\" + userName + "\\" + fileName;
                //psi.Arguments = "-f gdigrab -i desktop -vcodec libx264 c:\\Videos\\" + userName + "\\" + fileName;
                psi.UseShellExecute = false;
                //File.SetAttributes(filePath, System.IO.FileAttributes.ReadOnly);
                process = Process.Start(psi);


            }

        }

        private void count_down(object sender, EventArgs e)
        {


            if (duration == 0)
            {


                //MessageBox.Show("stopped");
                timer6.Stop();
                buttonRecord.PerformClick();
                this.Close();
                //Thread.Sleep(30000);

            }
            else if (duration > 0)
            {
                duration--;
                label1.Text = duration.ToString();
            }
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(setting.Location);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            duration = duration + 300000;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            new Settings().Show();
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            new About().Show();

        }

        private void Cream_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonRecord.PerformClick();
            ExitWindowsEx(0, 0);
        }
    }

}
