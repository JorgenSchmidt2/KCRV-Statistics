using KCRV_Statistics.Core.Responses.ValidateResponses;

namespace KCRV_Statistics.Model.ValidateService.SimpleFileCheckers
{
    /// <summary>
    /// Содержит методы проверки простых файлов. Под простым файлом подразумевается обычный .txt файл, содержащий данные в 
    /// относительно понятном для человека формате (в отличии от двоичного кода .xlsx файлов).
    /// В простых файлах, данные часто разделяют пробелами, табуляцией, запятыми и т.п. по колонкам и переносом строки по строкам.
    /// </summary>
    public class InterlabDataSimpleChecker
    {
        /// <summary>
        /// Выполняет проверку входного контента из простого файла на соответствие его списку объектов типа RegularData (см. Core -> Entities).
        /// </summary>
        public static SimpleValidateResponse CheckSimpleData(string Content)
        {
            // Переменная для результатов
            SimpleValidateResponse Result = new SimpleValidateResponse();
            Result.Status = true;

            // Другие необходимые переменные
            int lines_counter = 0;              // Считает сколько строк было пройдено
            int error_counter = 0;              // Считает ошибки

            try
            {
                // Предварительные действия
                Content = Content.Replace('.', ',') 
                    .Replace("\t\n", "");  
                
                // Если последний символ строки равен переносу, перенос удаляется
                // Часто информация из Excel при копировании выглядит именно таким
                // образом, что последняя строка всегда остаётся
                if (Content[Content.Length - 1] == '\n')
                {
                    Content = Content.Remove(Content.Length - 1, 1);
                }

                // Переводит входной string в последовательный набор строк
                var lines = Content.Split('\n');    

                // Цикл проверки
                foreach (var item in lines)
                {
                    // Предварительные операции
                    lines_counter++;
                    var columns = item.Split('\t');

                    // Условия проверки
                    if (columns.Length != 2)
                    {
                        error_counter += 1;
                        Result.Status = false;
                        Result.Message += "Ошибка в строке №" + lines_counter + ". Элементов в строке должно быть равно 2.\n";
                    }
                    if (!Double.TryParse(columns[0], out var number_1) || !Double.TryParse(columns[1], out var number_2))
                    {
                        error_counter += 1;
                        Result.Status = false;
                        Result.Message += "Ошибка в строке №" + lines_counter + ". Содержимое элементов должно быть числом. ";
                        Result.Message += "Полученные данные: " + columns[0] + "\t" + columns[1] + ".\n";
                    }

                    // Ограничение по ошибкам (нужно чтобы текст с ошибками умещался в Textbox'е)
                    if (error_counter == 10)
                    {
                        Result.Status = false;
                        Result.Message += "Накопилось 10 ошибок и более, цикл проверки данных закончил работу.\n";
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Result.Status = false;
                Result.Message = "Во время проверки простого файла возникла следующая ошибка: \n";
                Result.Message += e;
            }

            return Result;
        }
    }
}