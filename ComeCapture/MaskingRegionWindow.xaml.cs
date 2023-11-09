using System.Windows;
using System.Windows.Input;

namespace ComeCapture
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaskingRegionWindow : Window
    {
        public MaskingRegionWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                maskingRegionControl.CloseAction = () => { this.Close(); };
            };
        }

        #region 鼠标及键盘事件
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
