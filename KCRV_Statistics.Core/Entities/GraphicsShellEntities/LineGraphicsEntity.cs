﻿using KCRV_Statistics.Core.AppConfiguration;
using System.Windows.Media;

namespace KCRV_Statistics.Core.Entities.GraphicsShellEntities
{
    /// <summary>
    /// Объект, по которому происходит построение линий на графике.
    /// </summary>
    public class LineGraphicsEntity
    {
        // Координаты ключевых точек
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        // Цвет линии
        public SolidColorBrush Color { get; set; }

        // Ширина линии
        public double StrokeThicknessValue { get; set; }
    }
}