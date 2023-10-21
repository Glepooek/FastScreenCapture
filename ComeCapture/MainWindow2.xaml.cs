using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComeCapture
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        /// <summary>
        /// 是否截图开始
        /// </summary>
        private bool _IsMouseDown = false;
        /// <summary>
        /// 是否截图完毕
        /// </summary>
        private bool _IsCapture = false;
        /// <summary>
        /// 截图区域宽高的最小尺寸
        /// </summary>
        public const int MinSize = 10;

        private double _X0 = 0;
        private double _Y0 = 0;

        public MainWindow2()
        {
            InitializeComponent();
        }

        #region 鼠标及键盘事件
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_IsCapture)
            {
                return;
            }
            var point = e.GetPosition(this);
            _X0 = point.X;
            _Y0 = point.Y;
            _IsMouseDown = true;
            Canvas.SetLeft(maskingRegion, _X0);
            Canvas.SetTop(maskingRegion, _Y0);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_IsMouseDown || _IsCapture)
            {
                return;
            }
            _IsMouseDown = false;
            if (maskingRegion.Width >= MinSize && maskingRegion.Height >= MinSize)
            {
                _IsCapture = true;
                Cursor = Cursors.Arrow;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this);

            if (_IsCapture)
            {
                return;
            }

            if (_IsMouseDown)
            {
                var w = point.X - _X0;
                var h = point.Y - _Y0;
                if (w < MinSize || h < MinSize)
                {
                    return;
                }
                if (maskingRegion.Visibility == Visibility.Collapsed)
                {
                    maskingRegion.Visibility = Visibility.Visible;
                }

                maskingRegion.Width = w;
                maskingRegion.Height = h;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
        #endregion
    }
}
