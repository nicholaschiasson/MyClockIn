using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace MyClockIn
{
    public partial class InputCredentials : Form
    {
        string SystemAppData;
        string ProductSystemAppDataFolder;
        DirectoryInfo SystemDir;
        string SystemClockInFileName;
        string SystemClockOutFileName;
        StreamWriter SystemClockInFile;
        StreamWriter SystemClockOutFile;

        WrapperImpersonationContext UserContext;
        string UserAppData;
        string ProductUserAppDataFolder;
        DirectoryInfo UserDir;
        string UserClockInFileName;
        string UserClockOutFileName;
        StreamWriter UserClockInFile;
        StreamWriter UserClockOutFile;

        public InputCredentials()
        {
            InitializeComponent();
        }

        private void InputCredentials_Load(object sender, EventArgs e)
        {
            DomainTextBox.Text = Environment.MachineName;

            SystemAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            ProductSystemAppDataFolder = SystemAppData + "\\" + Application.ProductName;
            SystemDir = Directory.CreateDirectory(ProductSystemAppDataFolder);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DomainTextBox.Text) || DomainTextBox.Text == "." || DomainTextBox.Text.ToLower() == "localhost")
            {
                DomainTextBox.Text = Environment.MachineName;
            }
            UserContext = new WrapperImpersonationContext(DomainTextBox.Text, UsernameTextBox.Text, PasswordTextBox.Text);
            try
            {
                UserContext.Enter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login attempt failed. Please try again.", "Could not login");
                UserContext = null;
                return;
            }

            string StartupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            WshShell Shell = new WshShell();
            string ShortcutAddress = StartupFolder + "\\" + Application.ProductName + ".lnk";
            IWshShortcut Shortcut = (IWshShortcut)Shell.CreateShortcut(ShortcutAddress);
            Shortcut.Description = "A startup shortcut. If you delete this shortcut from your computer, LaunchOnStartup.exe will not launch on Windows Startup"; // set the description of the shortcut
            Shortcut.WorkingDirectory = Application.StartupPath; // working directory
            Shortcut.TargetPath = Application.ExecutablePath; // path of the executable
            Shortcut.Save(); // save the shortcut

            SystemClockInFileName = ProductSystemAppDataFolder + "\\" + UserContext.Domain +
                "-" + UserContext.Username + "-ClockInTimes";
            SystemClockOutFileName = ProductSystemAppDataFolder + "\\" + UserContext.Domain +
                "-" + UserContext.Username + "-ClockOutTimes";

            UserAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ProductUserAppDataFolder = UserAppData + "\\" + Application.ProductName;
            UserDir = Directory.CreateDirectory(ProductUserAppDataFolder);
            UserClockInFileName = ProductUserAppDataFolder + "\\ClockInTimes";
            UserClockOutFileName = ProductUserAppDataFolder + "\\ClockOutTimes";

            UserContext.Leave();

            CopyFileTo(SystemClockInFileName, UserClockInFileName);
            CopyFileTo(SystemClockOutFileName, UserClockOutFileName);

            if (SessionRadioButton.Checked)
            {
                // do session
                // AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnApplicationExit);
                // AppDomain.CurrentDomain.DomainUnload += new EventHandler(OnApplicationExit);
                // Hide();
                Close();
            }
            else if (ClockingInRadioButton.Checked)
            {
                SystemClockInFile = OpenFileWriteStream(SystemClockInFileName);
                UserClockInFile = OpenFileWriteStream(UserClockInFileName);
                ClockIn();
                SystemClockInFile.Close();
                UserClockInFile.Close();
                Close();
            }
            else if (ClockingOutRadioButton.Checked)
            {
                SystemClockOutFile = OpenFileWriteStream(SystemClockOutFileName);
                UserClockOutFile = OpenFileWriteStream(UserClockOutFileName);
                ClockOut();
                SystemClockOutFile.Close();
                UserClockOutFile.Close();
                Close();
            }

            
        }

        private void CancelLoginButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            SystemClockOutFile = OpenFileWriteStream(SystemClockOutFileName);
            UserClockOutFile = OpenFileWriteStream(UserClockOutFileName);
            ClockOut();
            SystemClockOutFile.Close();
            UserClockOutFile.Close();
        }

        private void ClockIn()
        {
            DateTime now = DateTime.Now;
            SystemClockInFile.WriteLine(now + " - v" + Assembly.GetExecutingAssembly().GetName().Version);
            UserClockInFile.WriteLine(now + " - v" + Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void ClockOut()
        {
            DateTime now = DateTime.Now;
            SystemClockOutFile.WriteLine(now + " - v" + Assembly.GetExecutingAssembly().GetName().Version);
            UserClockOutFile.WriteLine(now + " - v" + Assembly.GetExecutingAssembly().GetName().Version);
        }

        private StreamWriter OpenFileReadStream(string FileName, FileMode mode =
            FileMode.OpenOrCreate, FileAccess access = FileAccess.Read, FileShare share =
            FileShare.ReadWrite)
        {
            FileStream File = new FileStream(FileName, mode, access, share);
            StreamWriter FileStreamWriter = new StreamWriter(File);
            return FileStreamWriter;
        }

        private StreamWriter OpenFileWriteStream(string FileName, FileMode mode =
            FileMode.Append, FileAccess access = FileAccess.Write, FileShare share =
            FileShare.Read)
        {
            FileStream File = new FileStream(FileName, mode, access, share);
            StreamWriter FileStreamWriter = new StreamWriter(File);
            return FileStreamWriter;
        }

        private void CopyFileTo(string FileName, string DestinationFileName)
        {
            FileStream File = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            FileStream DestFile = new FileStream(DestinationFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            DestFile.SetLength(0);
            File.CopyTo(DestFile);
            File.Close();
            DestFile.Close();
        }
    }
}
