using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Responses.DataResponses;
using KCRV_Statistics.Model.MessageService.MessageBoxService;

namespace KCRV_Statistics.Model.DataOperatorsService.Lists
{
    /// <summary>
    /// Содержит методы работы со списками, необходимые для работы программы.
    /// </summary>
    public class ListConverters
    {
        /// <summary>
        /// Переводит в RegularData контент переменной типа string, формат контента которой можно описать как 
        /// "2 столбца разделены табуляцией, произвольное количество строк разделены переносом строки".
        /// </summary>
        public static ListDataResponse<RegularData> StringToRegularData (string Content)
        {
            ListDataResponse<RegularData> Result = new ListDataResponse<RegularData>();
            Result.Data = new List<RegularData>();

            try
            {
                // Удаляем лишние знаки
                Content = Content.Replace('.', ',')
                    .Replace("\t\n", "")
                    .Replace("\r", "");

                // При наличии последнего переноса строки - удаляем и его
                if (Content[Content.Length - 1] == '\n')
                {
                    Content = Content.Remove(Content.Length - 1, 1);
                }

                // Переводим строки в массив для дальнейших операций
                var lines = Content.Split('\n');

                // Переводим содержимое массива lines в объекты RegularData
                int laboratoryCounter = 0;
                foreach (var item in lines)
                {
                    laboratoryCounter += 1;
                    var columns = item.Split('\t');
                    if (Double.TryParse(columns[0], out var number_1) && Double.TryParse(columns[1], out var number_2))
                    {
                        Result.Data.Add(new RegularData
                        {
                            LaboratoryNumber = laboratoryCounter,
                            Value = number_1,
                            Uncertanity = number_2
                        });
                    }
                }

                Result.Status = true;
                return Result;
            }
            catch (Exception e)
            {
                Result.Message = e.Message;
                Result.Status = false;
                return Result;
            }

        }
    }
}