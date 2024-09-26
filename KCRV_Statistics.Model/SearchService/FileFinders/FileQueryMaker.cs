using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace KCRV_Statistics.Model.SearchService.FileFinders
{
    /// <summary>
    /// Отвечает за выполнение запросов по списку.
    /// </summary>
    public class FileQueryMaker
    {
        /// <summary>
        /// Выполняет поиск включений определённого набора символов Query в списке FileList, содержащего имена файлов.
        /// </summary>
        public static List<FileDataEntity> DoQuery (string Query, List<FileDataEntity> FileList)
        {
            List<FileDataEntity> Result = new List<FileDataEntity>();

            // Инициализация счётчика для переписывания ID результрующего списка файлов
            int FileCounter = 1;

            // Перебор элементов входного списка
            foreach (var item in FileList)
            {
                // Проверка включен ли в элемент списка поданный на вход запрос как подстрока
                if (item.FileName.Contains(Query))
                {
                    Result.Add(
                        new FileDataEntity ()
                        {
                            ID = FileCounter,
                            FileName = item.FileName,
                            Directory= item.Directory
                        }    
                    );
                    FileCounter += 1;
                }
            }

            return Result;
        }
    }
}