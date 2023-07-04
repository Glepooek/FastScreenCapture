using System.Collections.Generic;
using System.Windows.Media;

namespace ComeCapture.Helpers
{
    public class ColorBrushHelper
    {
        public static List<SolidColorBrush> GenerateColorBrushes()
        {
            return new List<SolidColorBrush>()
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5222D")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FA541C")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FA8C16")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAAD14")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FADB14")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0D911")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#52C41A")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#13C2C2")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),

                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1677FF")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2F54EB")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9254DE")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F759AB")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#445567")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91D5FF")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A24E56")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CDCDCD"))
            };
        }

        public static SolidColorBrush GenerateDefaultColorBrush()
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5222D"));
        }
    }
}
