using KCRV_Statistics.Core.AppConfiguration;

namespace KCRV_Statistics.Core.Entities.GraphicsShellEntities
{
    /// <summary>
    /// Объект, по которому происходит отрисовка различных надписей на графике.
    /// </summary>
    public class TextLabelEntity
    {
        // Координаты верхнего левого угла надписи
        public int X { get; set; }
        public int Y { get; set; }

        // Содержимое надписи
        public string? Content { get; set; }

        // Отвечает за положение точки на координатной плоскости
        public object? LabelMargin { get; set; }

        // Размер надписи
        public int FontSize
        {
            get
            {
                return GraphicsShellConfiguration.NumLabelFontSize;
            }
        }
    }
}