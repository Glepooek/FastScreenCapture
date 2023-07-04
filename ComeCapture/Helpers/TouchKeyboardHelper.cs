using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

/***
 * TabTip.exe为Windows内置的虚拟键盘。
 * C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe
 * 
 * Win11若无法通过双击启动TabTip.exe，
 * 需要修改注册表：在HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7中新建名为EnableDesktopModeAutoInvoke的DWORD，将其设置为1
 * 
 * ****/

namespace ComeCapture.Helpers
{
    public class TouchKeyboardHelper
    {
        public static bool ShowTaptip()
        {
            try
            {
                string tabTipPath = "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe";
                if (!File.Exists(tabTipPath))
                {
                    return false;
                }
                Process.Start(tabTipPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void CloseTaptip()
        {
            var touchhWnd = FindWindow("IPTip_Main_Window", null);
            if (touchhWnd == IntPtr.Zero)
            {
                return;
            }
            PostMessage(touchhWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
        }

        #region WinAPI
        private const int WM_SYSCOMMAND = 274;
        private const uint SC_CLOSE = 61536;
        private const uint SC_RESTORE = 0xF120;
        private const uint SC_MAXIMIZE = 0xF030;
        private const uint SC_MINIMIZE = 0xF020;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int RegisterWindowMessage(string lpString);
        #endregion
    }
}
