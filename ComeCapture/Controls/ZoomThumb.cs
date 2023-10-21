using ComeCapture.Models;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ComeCapture.Controls
{
    public class ZoomThumb : Thumb
    {
        #region Direction DependencyProperty
        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
                DependencyProperty.Register("Direction", typeof(Direction), typeof(ZoomThumb),
                new PropertyMetadata(Direction.Null, new PropertyChangedCallback(OnDirectionPropertyChanged)));

        private static void OnDirectionPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ZoomThumb zoomThumb)
            {
                zoomThumb.OnDirectionValueChanged();
            }
        }

        protected void OnDirectionValueChanged()
        {
            if (Direction == Direction.Move)
            {
                MouseDoubleClick += (sender, e) =>
                {
                    MainWindow.Current?.OnOK();
                };
            }
        }
        #endregion
    }
}
