using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.UI.Views;
using KCRV_Statistics.UI.Views.IntermediateWindows;
using KCRV_Statistics.UI.Views.MasterWindows;
using System.Collections.Generic;
using System.Windows.Documents;

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
        private static EntryWindow? EntryWindow;
        /// <summary>
        /// Окно ввода координат начальных точек, если открыт файл формата .xlsx
        /// </summary>
        private static TwoColumnDataIntermediateWindow? TwoColumnDataIntermediateWindow;
        /// <summary>
        /// Окно расчёта показателей межлабораторных KCRV
        /// </summary>
        private static InterlabMasterWindow? InterlabMasterWindow;

        public static void StartEntryWindow() 
        {
            // Открываем главное окно (также прописан алгоритм закрытия главного окна, при котором вся программа заканчивает работу).
            EntryWindow = new();
            if (EntryWindow.ShowDialog() == true)
            {
                EntryWindow.Show();
            }
            EntryWindow = null;
        }



        public static void StartTwoColumnDataIntermediateWindow(List<RegularData> Data)
        {
            AppData.CurrentData.Clear();
            AppData.CurrentData = Data;

            // Открываем окно указания начала координат (для xlsx файла)
            TwoColumnDataIntermediateWindow = new();
            if (TwoColumnDataIntermediateWindow.ShowDialog() == true)
            {
                TwoColumnDataIntermediateWindow.Show();
            }
            TwoColumnDataIntermediateWindow = null;
            AppData.CurrentData.Clear();
        }

        public static void StartInterlabMasterWindow(List<OutputData> Data)
        {
            AppData.OutputData.Clear();
            AppData.OutputData = Data;

            // Открытие окна
            InterlabMasterWindow = new();
            if (InterlabMasterWindow.ShowDialog() == true)
            {
                InterlabMasterWindow.Show();
            }
            InterlabMasterWindow = null;
            AppData.OutputData.Clear();
            AppData.COX_Datas.Clear();
        }
    }
}