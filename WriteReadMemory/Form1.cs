using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WriteReadMemory
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);


        public Form1()
        {
            InitializeComponent();
        }

        const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        private void ReadFromMemory()
        {
            Process[] processlist = Process.GetProcesses(); // 1
            foreach (Process process in processlist) // 2
            {
                if (process.ProcessName.Contains("notepad")) // 3
                {
                    IntPtr handle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id); // 4
                    int bytes_written = 0;
                    byte[] buffer = new byte[50];
                    ReadProcessMemory((int)handle, adres, buffer, buffer.Length, ref bytes_written); // adres poprzedzony 0x

                    MessageBox.Show(Encoding.Unicode.GetString(buffer)); // 6
                }
            }
        }

        private void WriteToMemory()
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.ProcessName.Contains("notepad"))
                {
                    IntPtr handle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);
                    int bytes_written = 0;
                    byte[] buffer = Encoding.Unicode.GetBytes("123 Test!\0"); // \0 świadczy o końcu łańcucha tekstowego

                    try
                    {
                        WriteProcessMemory((int)handle, adres, buffer, buffer.Length, ref bytes_written); // adres poprzedzony 0x
                    }

                    catch
                    {
                        MessageBox.Show("Nie udalo sie zapisac informacji.");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WriteToMemory();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadFromMemory();
        }
    }
}
