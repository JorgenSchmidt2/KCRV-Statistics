using KCRV_Statistics.Core.AppConfiguration;

namespace KCRV_Statistics.Core.Entities.GraphicsShellEntities
{
    public class TextLabel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string? Content { get; set; }
        public object? LabelMargin { get; set; }
        public int FontSize
        {
            get
            {
                return GraphicsShellConfiguration.NumLabelFontSize;
            }
        }
    }
}