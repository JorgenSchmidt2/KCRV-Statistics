using KCRV_Statistics.Core.AppConfiguration;
using System.Windows;
using System;

namespace KCRV_Statistics.UI.AppService
{
    public class ThicknessGetter
    {
        public static object GetTranslatedCoords(
            double x,
            double y,
            double x_min,
            double x_max,
            double y_min,
            double y_max
        )
        {
            var kx = (GraphicsShellConfiguration.InternalCanvasWidth)
                / Math.Sqrt(Math.Pow(x_max - x_min, 2));

            var ky = (GraphicsShellConfiguration.InternalCanvasHeight)
                / Math.Sqrt(Math.Pow(y_max - y_min, 2));

            var X = Convert.ToInt32((x - x_min) * kx);
            var Y = Convert.ToInt32((y - y_min) * ky);

            return new Thickness(X, 0, 0, Y);
        }

        public static object GetCoords(int x, int y)
        {
            return new Thickness(x, 0, 0, y);
        }
    }
}