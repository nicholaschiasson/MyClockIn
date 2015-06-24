using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyClockIn
{
    public class WrapperImpersonationContext
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain,
        String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        private const int LOGON32_PROVIDER_DEFAULT = 0;
        private const int LOGON32_LOGON_INTERACTIVE = 2;

        public string Domain { get; private set; }
        public string Username { get; private set; }
        private string m_Password;
        private IntPtr m_Token;

        private WindowsImpersonationContext m_Context = null;


        protected bool IsInContext
        {
            get { return m_Context != null; }
        }

        public WrapperImpersonationContext(string domain, string username, string password)
        {
            Domain = domain;
            Username = username;
            m_Password = password;
        }
        
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public void Enter()
        {
            if (this.IsInContext) return;
            m_Token = new IntPtr(0);
            try
            {
                m_Token = IntPtr.Zero;
                bool logonSuccessfull = LogonUser(
                Username,
                Domain,
                m_Password,
                LOGON32_LOGON_INTERACTIVE,
                LOGON32_PROVIDER_DEFAULT,
                ref m_Token);
                if (logonSuccessfull == false)
                {
                int error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
                }
                WindowsIdentity identity = new WindowsIdentity(m_Token);
                m_Context = identity.Impersonate();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public void Leave()
        {
            if (this.IsInContext == false) return;
            m_Context.Undo();

            if (m_Token != IntPtr.Zero) CloseHandle(m_Token);
            m_Context = null;
        }
    }
}
