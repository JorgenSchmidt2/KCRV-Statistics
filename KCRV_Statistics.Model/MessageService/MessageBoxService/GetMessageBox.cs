using System.Windows;

namespace KCRV_Statistics.Model.MessageService.MessageBoxService
{
    /// <summary>
    /// Конкретная реализация message box'a, адаптированная под вывод сообщений на desktop.
    /// Реализация данного функционала может быть легко адаптирована, например, под консольную версию.
    /// </summary>
    public static class GetMessageBox
    {
        public static void Show(string Message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(Message);
            });
        }
    }
}