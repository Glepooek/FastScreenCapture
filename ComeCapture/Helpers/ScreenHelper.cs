using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ComeCapture.Helpers
{
    public static class ScreenHelper
    {
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(
            IntPtr hWnd,
            IntPtr hDc
            );

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(
            IntPtr hdc, // handle to DC
            int nIndex // index of capability
            );

        public static Size GetPhysicalDisplaySize()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.Desktopvertres);
            int physicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.Desktophorzres);
            ReleaseDC(IntPtr.Zero, desktop);
            g.Dispose();
            return new Size(physicalScreenWidth, physicalScreenHeight);
        }

        public enum DeviceCap
        {
            Desktopvertres = 117,
            Desktophorzres = 118
        }

        public static void ResetScreenScale()
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr desktop = g.GetHdc();
                int physicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.Desktophorzres);
                MainWindow.ScreenScale = physicalScreenWidth * 1.0000 / MainWindow.ScreenWidth;
            }
        }

        #region 获取屏幕缩放比
        /// <summary>
        /// 获取屏幕缩放比
        /// </summary>
        /// <returns></returns>
        private static double GetScreenScale()
        {
            double dpiX = 1;
            var source = System.Windows.PresentationSource.FromVisual(App.Current.MainWindow);
            if (source != null)
            {
                System.Windows.Media.Matrix m = source.CompositionTarget.TransformToDevice;
                dpiX = m.M11;
            }

            return dpiX;
        }
        #endregion
    }
}
