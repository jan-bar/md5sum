using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace md5sum
{
    static class md5Main
    {
        /* https://www.pinvoke.net/default.aspx/kernel32.createmutex */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);
        [DllImport("kernel32.dll")]
        public static extern bool ReleaseMutex(IntPtr hMutex);
        public const int ERROR_ALREADY_EXISTS = 183;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hWnd, int cmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            IntPtr hMutex = IntPtr.Zero;
            try
            {
                if (args.Length == 1 && HandleRegistryKey(args[0].ToLower()))
                    return; /* 操作注册表,增删右键菜单 */

                hMutex = CreateMutex(IntPtr.Zero, true, Md5Form.Md5FormName);
                if (hMutex != IntPtr.Zero && Marshal.GetLastWin32Error() == ERROR_ALREADY_EXISTS)
                {
                    IntPtr hwnd = FindWindow(null, Md5Form.Md5FormName);
                    if (hwnd != IntPtr.Zero)
                    {
                        ShowWindow(hwnd, 9);
                        SetForegroundWindow(hwnd);
                        Md5Form.SendFile(args);
                    }
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Md5Form(args));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (hMutex != IntPtr.Zero)
                    ReleaseMutex(hMutex);
            }
        }

        static bool HandleRegistryKey(string cmd)
        {
            const string cmdRegister = "reg";
            if (cmd != cmdRegister && cmd != "unreg")
                return false;

            RegistryKey shellKey = null, md5sumKey = null, commandKey = null;
            try
            {
                shellKey = Registry.ClassesRoot.OpenSubKey("*\\shell", true);
                if (cmd == cmdRegister)
                {
                    if (Array.IndexOf(shellKey.GetSubKeyNames(), Md5Form.Md5FormName) >= 0)
                        md5sumKey = shellKey.OpenSubKey(Md5Form.Md5FormName, true);
                    else
                        md5sumKey = shellKey.CreateSubKey(Md5Form.Md5FormName);

                    string executablePath = "\"" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + "\"";
                    md5sumKey.SetValue("MUIVerb", "md5sum");
                    md5sumKey.SetValue("icon", executablePath);

                    const string keyCommand = "command";
                    if (Array.IndexOf(md5sumKey.GetSubKeyNames(), keyCommand) >= 0)
                        commandKey = md5sumKey.OpenSubKey(keyCommand, true);
                    else
                        commandKey = md5sumKey.CreateSubKey(keyCommand);
                    commandKey.SetValue("", executablePath + " \"%1\"");
                }
                else
                {
                    shellKey.DeleteSubKeyTree(Md5Form.Md5FormName);
                }
            }
            finally
            {
                if (commandKey != null)
                    commandKey.Dispose();
                if (md5sumKey != null)
                    md5sumKey.Dispose();
                if (shellKey != null)
                    shellKey.Dispose();
            }
            return true;
        }
    }
}
