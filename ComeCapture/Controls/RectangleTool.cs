using ComeCapture.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ComeCapture.Controls
{
    public class RectangleTool : StackPanel
    {
        static RectangleTool()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleTool), new FrameworkPropertyMetadata(typeof(RectangleTool)));
        }

        public RectangleTool()
        {
            _Current = this;
        }

        #region 属性 Current
        private static RectangleTool _Current = null;
        public static RectangleTool Current
        {
            get
            {
                return _Current;
            }
        }
        #endregion

        #region LineThickness DependencyProperty
        public double LineThickness
        {
            get { return (double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        public static readonly DependencyProperty LineThicknessProperty =
                DependencyProperty.Register("LineThickness", typeof(double), typeof(RectangleTool),
                new PropertyMetadata(5.0, new PropertyChangedCallback(OnLineThicknessPropertyChanged)));

        private static void OnLineThicknessPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is RectangleTool rectangleTool)
            {
                rectangleTool.OnLineThicknessValueChanged();
            }
        }

        protected void OnLineThicknessValueChanged()
        {

        }
        #endregion

        #region LineBrush DependencyProperty
        public SolidColorBrush LineBrush
        {
            get { return (SolidColorBrush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        public static readonly DependencyProperty LineBrushProperty =
                DependencyProperty.Register("LineBrush", typeof(SolidColorBrush), typeof(RectangleTool),
                new PropertyMetadata(ColorBrushHelper.GenerateDefaultColorBrush(), new PropertyChangedCallback(OnLineBrushPropertyChanged)));

        private static void OnLineBrushPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is RectangleTool rectangleTool)
            {
                rectangleTool.OnLineBrushValueChanged();
            }
        }

        protected void OnLineBrushValueChanged()
        {

        }
        #endregion
    }
}
