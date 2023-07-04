using ComeCapture.Controls;
using ComeCapture.Helpers;
using ComeCapture.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComeCapture
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static double ScreenWidth = SystemParameters.PrimaryScreenWidth;
        public static double ScreenHeight = SystemParameters.PrimaryScreenHeight;
        public static double ScreenScale = 1;
        public static int MinSize = 10;

        /// <summary>
        /// 画图注册名称集合
        /// </summary>
        public List<NameAndLimit> list = new List<NameAndLimit>();
        /// <summary>
        /// 画图注册名称
        /// </summary>
        public int num = 1;

        /// <summary>
        /// 是否截图开始
        /// </summary>
        private bool _IsMouseDown = false;
        /// <summary>
        /// 是否截图完毕
        /// </summary>
        private bool _IsCapture = false;

        private double _X0 = 0;
        private double _Y0 = 0;

        public MainWindow()
        {
            _Current = this;
            InitializeComponent();
            DataContext = new AppModel();
            Background = new ImageBrush(ImageHelper.GetFullBitmapSource());
            WpfHelper.MainDispatcher = Dispatcher;
            MaxWindow();
            MaskLeft.Height = ScreenHeight;
            MaskRight.Height = ScreenHeight;
            //计算Windows项目缩放比例
            ScreenHelper.ResetScreenScale();
        }

        #region 属性 Current
        private static MainWindow _Current = null;
        public static MainWindow Current
        {
            get
            {
                return _Current;
            }
        }
        #endregion

        #region 全屏+置顶
        private void MaxWindow()
        {
            Left = 0;
            Top = 0;
            Width = ScreenWidth;
            Height = ScreenHeight;
            Activate();
        }
        #endregion

        #region 注册画图
        public static void Register(object control)
        {
            var name = "Draw" + _Current.num;
            //_Current.MainCanvas.RegisterName(name, control);
            _Current.MainImage.MainImageCanvas.RegisterName(name, control);
            _Current.list.Add(new NameAndLimit(name));
            _Current.num++;
        }
        #endregion

        #region 截图区域添加画图
        public static void AddControl(UIElement e)
        {
            //_Current.MainCanvas.Children.Add(e);
            _Current.MainImage.MainImageCanvas.Children.Add(e);
        }
        #endregion

        #region 截图区域移除画图
        public static void RemoveControl(UIElement e)
        {
            //_Current.MainCanvas.Children.Remove(e);
            _Current.MainImage.MainImageCanvas.Children.Remove(e);
        }
        #endregion

        #region 撤回
        public void OnRevoke()
        {
            if (list.Count > 0)
            {
                var name = list[list.Count - 1].Name;
                //var obj = MainCanvas.FindName(name);
                var obj = _Current.MainImage.MainImageCanvas.FindName(name);
                if (obj != null)
                {
                    //MainCanvas.Children.Remove(obj as UIElement);
                    //MainCanvas.UnregisterName(name);
                    _Current.MainImage.MainImageCanvas.Children.Remove(obj as UIElement);
                    _Current.MainImage.MainImageCanvas.UnregisterName(name);
                    list.RemoveAt(list.Count - 1);
                    MainImage.Limit = list.Count == 0 ? new Limit() : list[list.Count - 1].Limit;
                }
            }
        }
        #endregion

        #region 保存
        public void OnSave()
        {
            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "截图" + DateTime.Now.ToString("yyyyMMddhhmmss"),
                Filter = "png|*.png",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (sfd.ShowDialog() == true)
            {
                Hidden();
                Thread t = new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(200);
                    WpfHelper.SafeRun(() =>
                    {
                        var source = GetCapture();
                        if (source != null)
                        {
                            ImageHelper.SaveToPng(source, sfd.FileName);
                        }
                        Close();
                    });
                }))
                {
                    IsBackground = true
                };
                t.Start();
            }
        }
        #endregion

        #region 获取截图
        private BitmapSource GetCapture()
        {
            return ImageHelper.GetBitmapSource((int)AppModel.Current.MaskLeftWidth + 1, (int)AppModel.Current.MaskTopHeight + 1, (int)MainImage.ActualWidth - 2, (int)MainImage.ActualHeight - 2); ;
        }
        #endregion

        #region 退出截图
        public void OnCancel()
        {
            Close();
        }
        #endregion

        #region 完成截图
        public void OnOK()
        {
            Hidden();
            Thread t = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(50);
                WpfHelper.SafeRun(() =>
                {
                    var source = GetCapture();
                    if (source != null)
                    {
                        Clipboard.SetImage(source);
                    }
                    Close();
                });
            }))
            {
                IsBackground = true
            };
            t.Start();
        }
        #endregion

        #region 开关灯
        public void OnTurnOnLight()
        {
            if (MaskLeft.Opacity == 0.9)
            {
                MaskLeft.Opacity = 0.5;
                MaskRight.Opacity = 0.5;
                MaskTop.Opacity = 0.5;
                MaskBottom.Opacity = 0.5;
            }
            else
            {
                MaskLeft.Opacity = 0.9;
                MaskRight.Opacity = 0.9;
                MaskTop.Opacity = 0.9;
                MaskBottom.Opacity = 0.9;
            }
        }
        #endregion

        #region 最大化截图区域

        private bool _IsMax = false;
        private double _TempWidth = 0;
        private double _TempHeight = 0;
        private double _TempLeft = 0;
        private double _TempTop = 0;
        public void OnMaxMainImage()
        {
            if (_IsCapture)
            {
                if (_IsMax)
                {
                    _IsMax = false;
                    _Current.MainImage.MainImageBackground.Source = null;
                    _Current.MainImage.ZoomThumbVisibility = Visibility.Visible;
                    scaleTrans.ScaleX = 1;
                    scaleTrans.ScaleY = 1;
                    Canvas.SetLeft(MainImage, _TempLeft);
                    Canvas.SetTop(MainImage, _TempTop);
                    //MainImage.Height = _TempHeight;
                    //MainImage.Width = _TempWidth;
                    AppModel.Current.MaskLeftWidth = _TempLeft;
                    AppModel.Current.MaskRightWidth = ScreenWidth - _TempLeft - _TempWidth;
                    AppModel.Current.MaskTopHeight = _TempTop;
                    AppModel.Current.MaskTopWidth = _TempWidth;
                    AppModel.Current.MaskBottomHeight = ScreenHeight - _TempTop - _TempHeight;
                    ResetToolBarPosition();
                }
                else
                {
                    _IsMax = true;
                    _TempWidth = MainImage.Width;
                    _TempHeight = MainImage.Height;
                    _TempLeft = Canvas.GetLeft(MainImage);
                    _TempTop = Canvas.GetTop(MainImage);

                    //var scale = ScreenWidth / ScreenHeight;
                    //var h = ScreenHeight;
                    //var w = ScreenHeight * scale;

                    //Canvas.SetLeft(MainImage, (ScreenWidth - w) / 2);
                    //Canvas.SetTop(MainImage, 0);
                    //MainImage.Height = h;
                    //MainImage.Width = w;

                    //AppModel.Current.MaskLeftWidth = 0;
                    //AppModel.Current.MaskRightWidth = 0;
                    //AppModel.Current.MaskTopWidth = w;
                    //AppModel.Current.MaskTopHeight = 0;
                    //AppModel.Current.MaskBottomHeight = 0;
                    //AppModel.Current.ChangeShowSize();

                    _Current.MainImage.MainImageCanvas.Visibility = Visibility.Collapsed;
                    _Current.MainImage.ZoomThumbVisibility = Visibility.Collapsed;
                    Task.Delay(200).ContinueWith(t =>
                    {
                        _Current.MainImage.MainImageBackground.Source = GetCapture();
                        _Current.MainImage.MainImageCanvas.Visibility = Visibility.Visible;

                        var scaleX = ScreenWidth / _Current.MainImage.ActualWidth;
                        var scaleY = ScreenHeight / _Current.MainImage.ActualHeight;
                        scaleTrans.ScaleX = scaleX;
                        scaleTrans.ScaleY = scaleY;

                        Canvas.SetLeft(_Current.MainImage, (ScreenWidth - _Current.MainImage.Width) / 2);
                        Canvas.SetTop(_Current.MainImage, (ScreenHeight - _Current.MainImage.Height) / 2);

                        AppModel.Current.MaskLeftWidth = 0;
                        AppModel.Current.MaskRightWidth = 0;
                        AppModel.Current.MaskTopWidth = ScreenWidth;
                        AppModel.Current.MaskTopHeight = 0;
                        AppModel.Current.MaskBottomHeight = 0;
                        AppModel.Current.ChangeShowSize();
                        ResetToolBarPosition();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private void ResetToolBarPosition()
        {
            if (MainImage.Width >= MinSize && MainImage.Height >= MinSize)
            {
                ImageEditBar.Current.Visibility = Visibility.Visible;
                ImageEditBar.Current.ResetCanvas();
                SizeColorBar.Current.ResetCanvas();
                Cursor = Cursors.Arrow;
            }
        }

        #endregion

        #region 截图前隐藏窗口
        private void Hidden()
        {
            //隐藏尺寸RGB框
            if (AppModel.Current.MaskTopHeight < 40)
            {
                SizeRGB.Visibility = Visibility.Collapsed;
            }
            // ImageEditBar.Height(40) 上Margin(5)
            // SizeColorBar.Height(44) 上Margin(2)
            var need = SizeColorBar.Current.Selected == Tool.Null ? 45 : 91;
            if (AppModel.Current.MaskBottomHeight < need && AppModel.Current.MaskTopHeight < need)
            {
                ImageEditBar.Current.Visibility = Visibility.Collapsed;
                SizeColorBar.Current.Visibility = Visibility.Collapsed;
            }
            MainImage.ZoomThumbVisibility = Visibility.Collapsed;
        }
        #endregion

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
            Canvas.SetLeft(MainImage, _X0);
            Canvas.SetTop(MainImage, _Y0);
            AppModel.Current.MaskLeftWidth = _X0;
            AppModel.Current.MaskRightWidth = ScreenWidth - _X0;
            AppModel.Current.MaskTopHeight = _Y0;
            Show_Size.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_IsMouseDown || _IsCapture)
            {
                return;
            }
            _IsMouseDown = false;
            if (MainImage.Width >= MinSize && MainImage.Height >= MinSize)
            {
                _IsCapture = true;
                ImageEditBar.Current.Visibility = Visibility.Visible;
                ImageEditBar.Current.ResetCanvas();
                Cursor = Cursors.Arrow;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            var screenP = PointToScreen(point);
            AppModel.Current.ShowRGB = ImageHelper.GetRGB((int)screenP.X, (int)screenP.Y);
            if (_IsCapture)
            {
                return;
            }

            if (Show_RGB.Visibility == Visibility.Collapsed)
            {
                Show_RGB.Visibility = Visibility.Visible;
            }

            if (_IsMouseDown)
            {
                var w = point.X - _X0;
                var h = point.Y - _Y0;
                if (w < MinSize || h < MinSize)
                {
                    return;
                }
                if (MainImage.Visibility == Visibility.Collapsed)
                {
                    MainImage.Visibility = Visibility.Visible;
                }
                AppModel.Current.MaskRightWidth = ScreenWidth - point.X;
                AppModel.Current.MaskTopWidth = w;
                AppModel.Current.MaskBottomHeight = ScreenHeight - point.Y;
                AppModel.Current.ChangeShowSize();
                MainImage.Width = w;
                MainImage.Height = h;
            }
            else
            {
                AppModel.Current.ShowSizeLeft = point.X;
                AppModel.Current.ShowSizeTop = ScreenHeight - point.Y < 30 ? point.Y - 30 : point.Y + 10;
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
