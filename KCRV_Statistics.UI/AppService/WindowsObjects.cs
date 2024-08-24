using KCRV_Statistics.UI.Views;
using KCRV_Statistics.UI.Views.IntermediateWindows;
using KCRV_Statistics.UI.Views.MasterWindows;

namespace KCRV_Statistics.UI.AppService
{
    /// <summary>
    /// Содержит конкретные реализации, используемых в программе окон. Именно эти реализации окон будут открываться при работе программы.
    /// </summary>
    public class WindowsObjects
    {
        /// <summary>
        /// Начальное окно
        /// </summary>
        public static EntryWindow EntryWindow;
        /// <summary>
        /// Окно ввода координат начальных точек, если открыт файл формата .xlsx
        /// </summary>
        public static OpenCalculateIntermediateWindow OpenCalculateIntermediateWindow;
        /// <summary>
        /// Окно расчёта показателей межлабораторных KCRV
        /// </summary>
        public static CalculateMasterWindow CalculateMasterWindow;
    }
}