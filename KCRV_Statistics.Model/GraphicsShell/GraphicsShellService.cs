using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Core.Responses.DataResponses;
using KCRV_Statistics.Model.MessageService.MessageBoxService;

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
        public static ListDataResponse<ViewedOutputData> GetViewedOutputData (List<OutputData> Data)
        {
            var Result = new ListDataResponse<ViewedOutputData>();

            try
            {
                Result.Data = new List<ViewedOutputData>();

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
                    Result.Data.Add(obj);

                    counter += 1;
                }

                Result.Status = true;
                return Result;
            }
            catch (Exception e)
            {
                Result.Message = "Ошибка при составлении отображаемого списка элементов результатов расчётов: \n" + e.Message;
                Result.Status = false;
                return Result;
            }
        }
    }
}