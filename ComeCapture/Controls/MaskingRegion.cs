using ComeCapture.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ComeCapture.Controls
{
    /// <summary>
    /// 遮罩区域控件
    /// </summary>
    public class MaskingRegion : Control
    {
        private Direction mDirection;
        private MaskingRegionModel regionModel;

        static MaskingRegion()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(MaskingRegion),
                new FrameworkPropertyMetadata(typeof(MaskingRegion))
            );
        }

        public MaskingRegion()
        {
            AddHandler(Thumb.DragStartedEvent, new DragStartedEventHandler(OnDragStart));
            AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(OnDragCompleted));
            AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(OnDragDelta));
            this.Loaded += (s, args) => { regionModel = this.DataContext as MaskingRegionModel; };
        }

        #region MoveCursor DependencyProperty
        public Cursor MoveCursor
        {
            get { return (Cursor)GetValue(MoveCursorProperty); }
            set { SetValue(MoveCursorProperty, value); }
        }

        public static readonly DependencyProperty MoveCursorProperty =
                DependencyProperty.Register(nameof(MoveCursor), typeof(Cursor), typeof(MaskingRegion),
                new PropertyMetadata(Cursors.SizeAll, new PropertyChangedCallback(OnMoveCursorPropertyChanged)));

        private static void OnMoveCursorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is MaskingRegion mainImage)
            {
                mainImage.OnMoveCursorValueChanged();
            }
        }

        protected void OnMoveCursorValueChanged()
        {

        }
        #endregion

        #region ZoomThumbVisibility DependencyProperty
        public Visibility ZoomThumbVisibility
        {
            get { return (Visibility)GetValue(ZoomThumbVisibilityProperty); }
            set { SetValue(ZoomThumbVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ZoomThumbVisibilityProperty =
            DependencyProperty.Register(
                "ZoomThumbVisibility",
                typeof(Visibility),
                typeof(MaskingRegion),
                new PropertyMetadata(
                    Visibility.Visible,
                    new PropertyChangedCallback(OnZoomThumbVisibilityPropertyChanged)
                )
            );

        private static void OnZoomThumbVisibilityPropertyChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (obj is MaskingRegion region && e.NewValue is Visibility visibility)
            {
                region.OnZoomThumbVisibilityValueChanged(visibility);
            }
        }

        protected void OnZoomThumbVisibilityValueChanged(Visibility visibility)
        {
            MoveCursor = visibility == Visibility.Visible ? Cursors.SizeAll : Cursors.Arrow;
        }
        #endregion

        #region MaskingRegionBackground DependencyProperty
        public SolidColorBrush MaskingRegionBackground
        {
            get { return (SolidColorBrush)GetValue(MaskingRegionBackgroundProperty); }
            set { SetValue(MaskingRegionBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MaskingRegionBackgroundProperty =
            DependencyProperty.Register(
                "MaskingRegionBackground",
                typeof(SolidColorBrush),
                typeof(MaskingRegion),
                new PropertyMetadata(
                    new SolidColorBrush()
                    {
                        Color = (Color)ColorConverter.ConvertFromString("#FFFFFF"),
                        Opacity = 0.5
                    },
                    new PropertyChangedCallback(OnMaskingRegionBackgroundPropertyChanged)
                )
            );

        private static void OnMaskingRegionBackgroundPropertyChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (obj is MaskingRegion region)
            {
                region.OnMaskingRegionBackgroundValueChanged();
            }
        }

        protected void OnMaskingRegionBackgroundValueChanged() { }
        #endregion

        #region 开始滑动事件
        private void OnDragStart(object sender, DragStartedEventArgs e)
        {
            mDirection = (e.OriginalSource as ZoomThumb).Direction;
        }
        #endregion

        #region 滑动中事件
        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            // HorizontalChange表示的是拖动操作的水平变化量，而不是绝对的坐标位置。
            // HorizontalChange的值是根据鼠标或触摸交互的实际移动距离来计算的。
            // 无论拖动的起点是什么位置，只要向左移动，水平变化量就会被计算为负值；只要向右移动，水平变化量就会被计算为正值。
            var X = e.HorizontalChange;
            // 向上拖动时负值，向下拖动时正值
            var Y = e.VerticalChange;
            switch (mDirection)
            {
                case Direction.Null:
                    break;
                case Direction.Move:
                    if (ZoomThumbVisibility != Visibility.Collapsed)
                    {
                        OnMove(X, Y);
                    }
                    break;
                default:
                    var str = mDirection.ToString();
                    if (X != 0)
                    {
                        if (str.Contains("Left"))
                        {
                            Left(X);
                        }
                        if (str.Contains("Right"))
                        {
                            Right(X);
                        }
                    }
                    if (Y != 0)
                    {
                        if (str.Contains("Top"))
                        {
                            Top(Y);
                        }
                        if (str.Contains("Bottom"))
                        {
                            Bottom(Y);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region 滑动结束事件
        private void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            mDirection = Direction.Null;
        }
        #endregion

        #region 拖动截图区域
        private void OnMove(double X, double Y)
        {
            #region X轴移动
            if (X > 0)
            {
                var max = regionModel.MaxScreenWidth - Canvas.GetLeft(this) - Width;
                if (X > max)
                {
                    X = max;
                }
            }
            else
            {
                var max = Canvas.GetLeft(this);
                if (-X > max)
                {
                    X = -max;
                }
            }
            if (X != 0)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + X);
                regionModel.ShowToolbarLeft += X;
            }
            #endregion

            #region Y轴移动
            if (Y > 0)
            {
                var max = regionModel.MaxScreenHeight - Canvas.GetTop(this) - Height;
                if (Y > max)
                {
                    Y = max;
                }
            }
            else
            {
                var max = Canvas.GetTop(this);
                if (-Y > max)
                {
                    Y = -max;
                }
            }
            if (Y != 0)
            {
                Canvas.SetTop(this, Canvas.GetTop(this) + Y);
                // 67是toolbar与遮罩区域的间距10 + toolbar的高度57
                if (regionModel.MaxScreenHeight - (Canvas.GetTop(this) + Height) < 67)
                {
                    regionModel.ShowToolbarTop = Canvas.GetTop(this) - 67;
                }
                else
                {
                    regionModel.ShowToolbarTop = Canvas.GetTop(this) + Height + 10;
                }
            }
            #endregion
        }
        #endregion

        #region 左缩放
        private void Left(double X)
        {
            if (X > 0)
            {
                var max = Width - regionModel.MinScreenSize;
                if (X > max)
                {
                    X = max;
                }
            }
            else
            {
                var max = Canvas.GetLeft(this);
                if (-X > max)
                {
                    X = -max;
                }
            }
            if (X != 0)
            {
                Width -= X;
                Canvas.SetLeft(this, Canvas.GetLeft(this) + X);
            }
        }
        #endregion

        #region 右缩放
        private void Right(double X)
        {
            if (X > 0)
            {
                var max = regionModel.MaxScreenWidth - Canvas.GetLeft(this) - Width;
                if (X > max)
                {
                    X = max;
                }
            }
            else
            {
                var max = Width - regionModel.MinScreenSize;
                if (-X > max)
                {
                    X = -max;
                }
            }
            if (X != 0)
            {
                Width += X;
                regionModel.ShowToolbarLeft += X;
            }
        }
        #endregion

        #region 上缩放
        private void Top(double Y)
        {
            if (Y > 0)
            {
                var max = Height - regionModel.MinScreenSize;
                if (Y > max)
                {
                    Y = max;
                }
            }
            else
            {
                var max = Canvas.GetTop(this);
                if (-Y > max)
                {
                    Y = -max;
                }
            }
            if (Y != 0)
            {
                Height -= Y;
                Canvas.SetTop(this, Canvas.GetTop(this) + Y);
            }
        }
        #endregion

        #region 下缩放
        private void Bottom(double Y)
        {
            if (Y > 0)
            {
                var max = regionModel.MaxScreenHeight - Canvas.GetTop(this) - Height;
                if (Y > max)
                {
                    Y = max;
                }
            }
            else
            {
                var max = Height - regionModel.MinScreenSize;
                if (-Y > max)
                {
                    Y = -max;
                }
            }
            if (Y != 0)
            {
                Height += Y;
                // 67是toolbar与遮罩区域的间距10 + toolbar的高度57
                if (regionModel.MaxScreenHeight - (Canvas.GetTop(this) + Height) < 67)
                {
                    regionModel.ShowToolbarTop = Canvas.GetTop(this) - 67;
                }
                else
                {
                    regionModel.ShowToolbarTop = Canvas.GetTop(this) + Height + 10;
                }
            }
        }
        #endregion
    }
}
