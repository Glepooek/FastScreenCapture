using ComeCapture.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ComeCapture.Controls
{
    /// <summary>
    /// MaskingRegionControl.xaml 的交互逻辑
    /// </summary>
    public partial class MaskingRegionControl : UserControl
    {
        #region Fields
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
        private const int MinSize = 10;

        private double _X0 = 0;
        private double _Y0 = 0;

        private MaskingRegionModel maskingRegionModel;

        public Action CloseAction;
        #endregion

        #region Constructor
        public MaskingRegionControl()
        {
            InitializeComponent();

            maskingRegionModel = new MaskingRegionModel();
            this.DataContext = maskingRegionModel;
        }
        #endregion

        #region EventHandler
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

        private void OnMouseMove(object sender, MouseEventArgs e)
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
                if (toolBar.Visibility == Visibility.Collapsed)
                {
                    toolBar.Visibility = Visibility.Visible;
                }

                maskingRegion.Width = w;
                maskingRegion.Height = h;

                maskingRegionModel.ShowToolbarLeft = point.X - toolBar.ActualWidth;
                maskingRegionModel.ShowToolbarTop = point.Y + 10;
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            CloseAction?.Invoke();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            mainCanvas.Background = null;
            maskingRegion.MaskingRegionBackground = new SolidColorBrush(Colors.White);
            maskingRegion.ZoomThumbVisibility = Visibility.Collapsed;

            maskingRegionModel.ShowToolbarLeft += 55;
            ok.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
