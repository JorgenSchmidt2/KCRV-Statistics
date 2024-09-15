using System.Windows.Media;

namespace KCRV_Statistics.Core.AppConfiguration
{
    /// <summary>
    /// Содержит конфигурацию основных графических элементов на графике.
    /// </summary>
    public class GraphicsShellConfiguration
    {
        // Размеры окна данных (для внешней части и внутренней)
        // Мерка по разнице между размерами внут. и внеш. частями проводится по внутренней части, т.к. она содержит основные данные в граф. виде
        public static readonly int InternalCanvasWidth = 500;
        public static readonly int InternalCanvasHeight = 500;
        public static readonly int ExternalCanvasWidth = 550;
        public static readonly int ExternalCanvasHeight = 550;

        // Количество точек для построения линии тренда
        public static readonly int TrendPointCount = 2;

        // Количество подписей для координатной оси
        public static readonly int MaxXLabelCount = 20;
        public static readonly int YLabelCount = 10;

        // Начальная точка по внешнему канвасу для отображения подписей
        public static readonly int LabelStartCoordinate = 30;

        // Конфигурация точек
        public static readonly int PointRadius = 8;
        public static readonly int PointThickness = 1;
        public static readonly SolidColorBrush PointColor = Brushes.Red; // "Рамка" точки 
        public static readonly SolidColorBrush Fill = Brushes.Red; // Заливка точки

        // Конфигурация численных подписей (по осям X, Y)
        public static readonly int NumLabelFontSize = 10;

        // Конфигурация линий
        public static readonly int LineStrokeThickness = 1;
        public static readonly SolidColorBrush RegressionLinesColor = Brushes.Blue;
        public static readonly SolidColorBrush TrustLinesColor = Brushes.Gray;
    }
}