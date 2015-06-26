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
using Microsoft.Win32;

namespace MyClockIn
{
    enum ClockType
    {
        Session,
        In,
        Out,
    }

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

        bool Session;
        bool ClockingIn;
        bool ClockingOut;

        public InputCredentials(bool session = false, bool clockIn = false, bool clockOut = false)
        {
            Session = session;
            ClockingIn = clockIn;
            ClockingOut = clockOut;
            InitializeComponent();
        }

        private void InputCredentials_Load(object sender, EventArgs e)
        {
            DomainTextBox.Text = Environment.MachineName;

            SystemAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            ProductSystemAppDataFolder = SystemAppData + "\\" + Application.ProductName;
            SystemDir = Directory.CreateDirectory(ProductSystemAppDataFolder);
            
            if (Session)
            {
                GetUserDependentFileInfo();
                DoClockOperation();
                SessionTimer.Enabled = true;
                Hide();
            }
            else
            {
                if (ClockingIn || ClockingOut)
                {
                    GetUserDependentFileInfo();
                    DoClockOperation();
                    Close();
                }
            }
        }

        private void RegisterTask()
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = true;
                startInfo.RedirectStandardError = false;
                startInfo.FileName = "cmd.exe";

                string xmlTmpName = Directory.GetCurrentDirectory() + "\\tmp.xml";
                FileInfo xmlTmpFile = new FileInfo(xmlTmpName);
                FileStream xmlTmp = null;
                if (xmlTmpFile.Exists)
                    xmlTmp = new FileStream(xmlTmpName, FileMode.Truncate, FileAccess.Write, FileShare.Read);
                else
                    xmlTmp = new FileStream(xmlTmpName, FileMode.Append, FileAccess.Write, FileShare.Read);
                StreamWriter stream = new StreamWriter(xmlTmp);
                stream.WriteLine(GetXmlTemplateString(ClockType.Session));
                stream.Close();

                startInfo.UseShellExecute = true;
                startInfo.RedirectStandardError = false;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C schtasks /create /f /tn \"" + Application.ProductName +
                    "-ClockIn\" /xml \"" + xmlTmpName + "\"";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                xmlTmpFile.Delete();
                /*
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = true;
                startInfo.RedirectStandardError = false;
                startInfo.FileName = "cmd.exe";

                string xmlTmpName = Directory.GetCurrentDirectory() + "\\tmp.xml";
                FileInfo xmlTmpFile = new FileInfo(xmlTmpName);
                FileStream xmlTmp = null;
                if (xmlTmpFile.Exists)
                    xmlTmp = new FileStream(xmlTmpName, FileMode.Truncate, FileAccess.Write, FileShare.Read);
                else
                    xmlTmp = new FileStream(xmlTmpName, FileMode.Append, FileAccess.Write, FileShare.Read);
                StreamWriter stream = new StreamWriter(xmlTmp);
                stream.WriteLine(GetXmlTemplateString(ClockType.In));
                stream.Close();

                startInfo.UseShellExecute = true;
                startInfo.RedirectStandardError = false;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C schtasks /create /f /tn \"" + Application.ProductName +
                    "-ClockIn\" /xml \"" + xmlTmpName + "\"";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                xmlTmpFile.Delete();

                xmlTmp = null;
                if (xmlTmpFile.Exists)
                    xmlTmp = new FileStream(xmlTmpName, FileMode.Truncate, FileAccess.Write, FileShare.Read);
                else
                    xmlTmp = new FileStream(xmlTmpName, FileMode.Append, FileAccess.Write, FileShare.Read);
                stream = new StreamWriter(xmlTmp);
                stream.WriteLine(GetXmlTemplateString(ClockType.Out));
                stream.Close();

                startInfo.UseShellExecute = true;
                startInfo.RedirectStandardError = false;
                startInfo.Arguments = "/C schtasks /create /f /tn \"" + Application.ProductName +
                    "-ClockOut\" /xml \"" + xmlTmpName + "\"";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                xmlTmpFile.Delete();
                */
            }
            catch (Exception exception)
            {
                MessageBox.Show("Current user has not been authorized.", "Exiting");
                Close();
            }
        }

        private string GetXmlTemplateString(ClockType clockType)
        {
            string now = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            string trigger = string.Empty;
            string arg = string.Empty;

            if (clockType == ClockType.Session)
            {
                trigger = @"
        <LogonTrigger>
          <Enabled>true</Enabled>
        </LogonTrigger>
        <SessionStateChangeTrigger>
          <Enabled>true</Enabled>
            <StateChange>ConsoleConnect</StateChange>
        </SessionStateChangeTrigger>";
                arg = "-s";
            }
            else if (clockType == ClockType.In)
            {
                trigger = @"
        <LogonTrigger>
          <Enabled>true</Enabled>
        </LogonTrigger>";
                arg = "-i";
            }
            else if (clockType == ClockType.Out)
            {
                trigger = @"
        <SessionStateChangeTrigger>
          <Enabled>true</Enabled>
            <StateChange>ConsoleDisconnect</StateChange>
        </SessionStateChangeTrigger>";
                arg = "o";
            }

            string xmlTemplate =
@"<?xml version=""1.0"" encoding=""UTF-16""?>
    <Task version=""1.3"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
      <RegistrationInfo>
        <Date>" + now + @"</Date>
        <Author>" + Environment.UserName + @"</Author>
      </RegistrationInfo>
      <Triggers>" + trigger + @"
      </Triggers>
      <Principals>
        <Principal id=""Author"">
          <UserId>" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + @"</UserId>
      <LogonType>InteractiveToken</LogonType>
          <RunLevel>HighestAvailable</RunLevel>
        </Principal>
      </Principals>
      <Settings>
        <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
        <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
        <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
        <AllowHardTerminate>true</AllowHardTerminate>
        <StartWhenAvailable>false</StartWhenAvailable>
        <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
        <IdleSettings>
          <StopOnIdleEnd>true</StopOnIdleEnd>
          <RestartOnIdle>false</RestartOnIdle>
        </IdleSettings>
        <AllowStartOnDemand>true</AllowStartOnDemand>
        <Enabled>true</Enabled>
        <Hidden>false</Hidden>
        <RunOnlyIfIdle>false</RunOnlyIfIdle>
        <DisallowStartOnRemoteAppSession>false</DisallowStartOnRemoteAppSession>
        <UseUnifiedSchedulingEngine>false</UseUnifiedSchedulingEngine>
        <WakeToRun>false</WakeToRun>
        <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
        <Priority>7</Priority>
      </Settings>
      <Actions Context=""Author"">
        <Exec>
          <Command>" + Application.ExecutablePath + @"</Command>
          <Arguments>" + arg + @"</Arguments>
        </Exec>
      </Actions>
    </Task>";
            return xmlTemplate;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (SessionRadioButton.Checked)
                Session = ClockingIn = ClockingOut = true;
            if (ClockingInRadioButton.Checked)
                ClockingIn = true;
            if (ClockingOutRadioButton.Checked)
                ClockingOut = true;

            LoginUserContext();

            DialogResult register = MessageBox.Show("Do you want to register " + Application.ProductName + " as a scheduled task to run " +
                "in session mode at logon?", (ClockingIn ? "Clocking In" : (ClockingOut ? "Clocking Out" : "Beginning Session")),
                MessageBoxButtons.YesNo);

            if (register == DialogResult.Yes)
            {
                RegisterTask();
            }

            GetUserDependentFileInfo();

            if (UserContext != null)
                UserContext.Leave();

            DoClockOperation();

            if (Session)
            {
                SessionTimer.Enabled = true;
                Hide();
            }
            else
            {
                Close();
            }
        }

        private void LoginUserContext()
        {
            if (!string.IsNullOrWhiteSpace(UsernameTextBox.Text) && UsernameTextBox.Text != Environment.UserName)
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
            if (SystemClockOutFile != null)
                SystemClockOutFile.Close();
            if (UserClockOutFile != null)
                UserClockOutFile.Close();
        }

        private void GetUserDependentFileInfo()
        {
            SystemClockInFileName = ProductSystemAppDataFolder + "\\" + Environment.UserDomainName +
                "-" + Environment.UserName + "-ClockInTimes";
            SystemClockOutFileName = ProductSystemAppDataFolder + "\\" + Environment.UserDomainName +
                "-" + Environment.UserName + "-ClockOutTimes";

            UserAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ProductUserAppDataFolder = UserAppData + "\\" + Application.ProductName;
            UserDir = Directory.CreateDirectory(ProductUserAppDataFolder);
            UserClockInFileName = ProductUserAppDataFolder + "\\ClockInTimes";
            UserClockOutFileName = ProductUserAppDataFolder + "\\ClockOutTimes";
        }

        private void DoClockOperation()
        {

            if (ClockingIn)
            {
                SystemClockInFile = OpenFileWriteStream(SystemClockInFileName);
                ClockIn();
                if (SystemClockInFile != null)
                    SystemClockInFile.Close();
                CopyFileTo(SystemClockInFileName, UserClockInFileName);
            }
            if (ClockingOut)
            {
                SystemClockOutFile = OpenFileWriteStream(SystemClockOutFileName);
                ClockOut();
                if (SystemClockOutFile != null)
                    SystemClockOutFile.Close();
                CopyFileTo(SystemClockOutFileName, UserClockOutFileName);
            }
        }

        private void DoSessionIntervalClockOut()
        {
            DateTime now = DateTime.Now;
            string addLine = now.ToString() + " - v" + Assembly.GetExecutingAssembly().GetName().Version;
            string[] lines = null;
            try
            {
                lines = System.IO.File.ReadAllLines(SystemClockOutFileName);
            }
            catch (Exception e) { }
            try
            {
                if (lines.Length > 0)
                {
                    lines[lines.Length - 1] = addLine;
                    System.IO.File.WriteAllLines(SystemClockOutFileName, lines);
                }
                else
                {
                    System.IO.File.WriteAllText(SystemClockOutFileName, addLine);
                }
            }
            catch (Exception e) { }

            CopyFileTo(SystemClockOutFileName, UserClockOutFileName);
        }

        private void ClockIn()
        {
            try
            {
                DateTime now = DateTime.Now;
                SystemClockInFile.WriteLine(now + " - v" + Assembly.GetExecutingAssembly().GetName().Version);
            }
            catch (Exception e) { }
        }

        private void ClockOut()
        {
            try
            {
                DateTime now = DateTime.Now;
                SystemClockOutFile.WriteLine(now + " - v" + Assembly.GetExecutingAssembly().GetName().Version);
            }
            catch (Exception e) { }
        }

        private StreamWriter OpenFileReadStream(string FileName, FileMode mode =
            FileMode.OpenOrCreate, FileAccess access = FileAccess.Read, FileShare share =
            FileShare.ReadWrite)
        {
            try
            {
                FileStream File = new FileStream(FileName, mode, access, share);
                StreamWriter FileStreamWriter = new StreamWriter(File);
                return FileStreamWriter;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private StreamWriter OpenFileWriteStream(string FileName, FileMode mode =
            FileMode.Append, FileAccess access = FileAccess.Write, FileShare share =
            FileShare.Read)
        {
            try
            {
                FileStream File = new FileStream(FileName, mode, access, share);
                StreamWriter FileStreamWriter = new StreamWriter(File);
                return FileStreamWriter;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void CopyFileTo(string FileName, string DestinationFileName)
        {
            try
            {
                FileStream File = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                FileStream DestFile = new FileStream(DestinationFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                DestFile.SetLength(0);
                File.CopyTo(DestFile);
                File.Close();
                DestFile.Close();
            }
            catch (Exception e) { }
        }

        private void SessionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Session = true;
            ClockingIn = true;
            ClockingOut = true;
        }

        private void ClockingInRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Session = false;
            ClockingIn = true;
            ClockingOut = false;
        }

        private void ClockingOutRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Session = false;
            ClockingIn = false;
            ClockingOut = true;
        }

        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            Hide();
            DoSessionIntervalClockOut();
        }
    }
}
