using System.Windows;

namespace ComeCapture.Models
{
    public class MaskingRegionModel : EntityBase
    {
        #region 属性 MaxScreenWidth
        public double MaxScreenWidth => SystemParameters.WorkArea.Width;
        #endregion

        #region 属性 MaxScreenHeight
        public double MaxScreenHeight => SystemParameters.WorkArea.Height;
        #endregion

        #region 属性 MinScreenSize
        public double MinScreenSize => 10;
        #endregion

        #region 属性 ShowToolbarLeft
        private double _ShowToolbarLeft = 0;
        public double ShowToolbarLeft
        {
            get
            {
                return _ShowToolbarLeft;
            }
            set
            {
                _ShowToolbarLeft = value;
                RaisePropertyChanged(() => ShowToolbarLeft);
            }
        }
        #endregion

        #region 属性 ShowToolbarTop
        private double _ShowToolbarTop = 0;
        public double ShowToolbarTop
        {
            get
            {
                return _ShowToolbarTop;
            }
            set
            {
                _ShowToolbarTop = value;
                RaisePropertyChanged(() => ShowToolbarTop);
            }
        }
        #endregion
    }
}
