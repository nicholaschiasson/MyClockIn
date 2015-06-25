using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace MyClockIn
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool session = false;
            bool clockIn = false;
            bool clockOut = false;

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (arg.ToLower() == "-s")
                {
                    session = true;
                }
                if (arg.ToLower() == "-i")
                {
                    clockIn = true;
                }
                if (arg.ToLower() == "-o")
                {
                    clockOut = true;
                }
            }

            if (clockIn && clockOut)
            {
                clockIn = false;
                clockOut = false;
            }

            if (session)
            {
                clockIn = true;
                clockOut = true;
            }

            /*
            string filename = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\MyClockIn_Run";
            try
            {
                System.IO.File.WriteAllLines(filename, Environment.GetCommandLineArgs());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            */

            Application.Run(new InputCredentials(session, clockIn, clockOut));
        }
    }
}
