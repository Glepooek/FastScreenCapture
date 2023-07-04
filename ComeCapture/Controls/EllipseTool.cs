﻿using ComeCapture.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ComeCapture.Controls
{
    public class EllipseTool : StackPanel
    {
        static EllipseTool()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseTool), new FrameworkPropertyMetadata(typeof(EllipseTool)));
        }

        public EllipseTool()
        {
            _Current = this;
        }

        #region 属性 Current
        private static EllipseTool _Current = null;
        public static EllipseTool Current
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
                DependencyProperty.Register("LineThickness", typeof(double), typeof(EllipseTool),
                new PropertyMetadata(5.0, new PropertyChangedCallback(OnLineThicknessPropertyChanged)));

        private static void OnLineThicknessPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is EllipseTool ellipseTool)
            {
                ellipseTool.OnLineThicknessValueChanged();
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
                DependencyProperty.Register("LineBrush", typeof(SolidColorBrush), typeof(EllipseTool),
                new PropertyMetadata(ColorBrushHelper.GenerateDefaultColorBrush(), new PropertyChangedCallback(OnLineBrushPropertyChanged)));

        private static void OnLineBrushPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is EllipseTool ellipseTool)
            {
                ellipseTool.OnLineBrushValueChanged();
            }
        }

        protected void OnLineBrushValueChanged()
        {

        }
        #endregion
    }
}
