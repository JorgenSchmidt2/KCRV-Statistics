using KCRV_Statistics.Core.AppConfiguration;
using System.Windows.Media;

namespace KCRV_Statistics.Core.Entities.GraphicsShellEntities
{
    /// <summary>
    /// Объект, по которому происходит построение точек на графике.
    /// </summary>
    public class PointGraphicsEntity
    {
        // Координаты точки
        public double X { get; set; }
        public double Y { get; set; }

        // Отвечает за местоположение объекта на plot'е
        public object? PointMargin { get; set; } 

        // Радиус точки
        public int Radius
        {
            get
            {
                return GraphicsShellConfiguration.PointRadius;
            }
        }

        // Ширина обрамления точки
        public int StrokeThicknessValue
        {
            get
            {
                return GraphicsShellConfiguration.PointThickness;
            }
        }

        // Обрамление (рамка) точки
        public SolidColorBrush Color
        {
            get
            {
                return GraphicsShellConfiguration.PointColor;
            }
        }

        // Заливка точки
        public SolidColorBrush Fill
        {
            get
            {
                return GraphicsShellConfiguration.Fill;
            }
        }
    }
}