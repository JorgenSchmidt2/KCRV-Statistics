using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.MessageService.MessageBoxService;
using System.Collections.ObjectModel;

namespace KCRV_Statistics.Model.GraphicsShell
{
    /// <summary>
    /// Содержит основные операции (в т.ч. манипуляции с ними) с данными, отображаемыми в интерфейсе пользователя
    /// </summary>
    public class GraphicsShellService
    {
        /// <summary>
        /// Преобразует исходный список результатов KCRV методов расчёта в отображаемый, дополнительно определяет какой метод будет
        /// отображён в первую очередь
        /// </summary>
        public static ObservableCollection<ViewedOutputData> GetViewedOutputData (List<OutputData> Data)
        {
            try
            {
                ObservableCollection<ViewedOutputData> Result = new ObservableCollection<ViewedOutputData>();

                // Инициализация счётчика для определения какой из методов будет отображён при открытии окна отображения
                int counter = 1;
                foreach (var item in Data)
                {
                    // Копирование объекта с данными расчётов во внутриитерационный объект с целью его добавления в результирующий список
                    ViewedOutputData obj = new ViewedOutputData();
                    obj.MethodName = item.MethodName;
                    obj.X = item.X;
                    obj.U = item.U;
                    
                    // Определение какой из объектов будет отображём при открытии окна отображения
                    if (counter == 1) 
                        obj.IsChoised = true;
                    else 
                        obj.IsChoised = false;

                    // Добавление объекта в результирующий список
                    Result.Add(obj);

                    counter += 1;
                }
                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка при составлении отображаемого списка элементов результатов расчётов: \n" + e.Message);
                return new ObservableCollection<ViewedOutputData>();
            }
        }
    }
}