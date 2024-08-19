using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KCRV_Statistics.UI.AppService
{
    /// <summary>
    /// Содержит основную логику работы программы при изменении данных в окне. По сути является "проводником" 
    /// между моделью визуального представления и интерфейсом.
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void CheckChanges([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}