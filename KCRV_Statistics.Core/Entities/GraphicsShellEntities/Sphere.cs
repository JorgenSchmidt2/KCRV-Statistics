using KCRV_Statistics.Core.AppConfiguration;
using System.Windows.Media;

namespace KCRV_Statistics.Core.Entities.GraphicsShellEntities
{
    public class Sphere
    {
        public double X { get; set; }
        public double Y { get; set; }

        // Параметры графической составляющей, используемой при отображении UI
        public object? SphereMargin { get; set; } // Отвечает за местоположение объекта на plot'е

        // Радиус точки
        public int Radius
        {
            get
            {
                return GraphicsShellConfiguration.PointRadius;
            }
        }
        public int StrokeThicknessValue
        {
            get
            {
                return GraphicsShellConfiguration.PointThickness;
            }
        }
        public SolidColorBrush Color
        {
            get
            {
                return GraphicsShellConfiguration.PointColor;
            }
        }
    }
}