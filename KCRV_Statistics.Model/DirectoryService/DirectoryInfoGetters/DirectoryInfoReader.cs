using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Model.MessageService.MessageBoxService;
using System.Windows;

namespace KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters
{
    /// <summary>
    /// Содержит статические методы для получения информации из директорий
    /// </summary>
    public class DirectoryInfoReader
    {
        /// <summary>
        /// Получает список файлов из указанных директорий.
        /// Под "указанными директориями" в данном случае 
        /// имеется ввиду список директорий, передаваемый в функцию
        /// </summary>
        public static List<FileDataEntity> GetFileListFromDirectory (List<string> DirectoryNames)
        {
            // Содержит результат работы метода
            List<FileDataEntity> Result = new List<FileDataEntity>();
            // По логике должна содержать список реально существующих директорий из входного списка
            List<string> ExistsDirectories = new List<string>();
            
            try
            {
                // Проверка каждого элемента из полученного списка директорий на то существует ли элемент из списка
                // При положительном результате - папка добавляется в список существующих директорий (см. выше переменную ExistsDirectories)
                // При отрицательном результате - на экран пользователю выдаётся ошибка, исполнение программы продолжается
                foreach (var item in DirectoryNames)
                {
                    if (!Directory.Exists(Environment.CurrentDirectory + @"\" + item))
                    {
                        GetMessageBox.Show("Директории " + item + " не существует.");
                    }
                    else
                    {
                        ExistsDirectories.Add(item);
                    }
                }

                // Если из полученного списка не существует ни одной из директорий - ход метода останавливается
                // Пользователь получает уведомление об ошибке (см. выше)
                if (ExistsDirectories.Count <= 0)
                {
                    var Message = "Следующих директорий не существует: \n";
                    foreach (var item in DirectoryNames)
                    {
                        Message += item + "\n";
                    }
                    Message += "Считывание данных из директорий остановлено.";
                    MessageBox.Show(Message);
                    return new List<FileDataEntity>();
                }

                // Считывание файлов из существующих директорий
                int FileCounter = 1;
                foreach (var item in ExistsDirectories)
                {
                    var FileList = Directory.GetFiles(Environment.CurrentDirectory + @"\" + item);
                    if (FileList.Length == 0)
                    {
                        GetMessageBox.Show("Директория " + item + " оказалась пуста.");
                    }
                    else
                    {
                        foreach (var curFile in FileList)
                        {
                            var splits = curFile.Split('\\');

                            Result.Add( 
                                new FileDataEntity() { 
                                    ID = FileCounter, 
                                    Directory = splits[splits.Length - 2], 
                                    FileName = splits[splits.Length - 1] 
                                });

                            FileCounter += 1;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GetMessageBox.Show("При чтении данных из указанных директорий возникла ошибка: " + e.Message);
            }

            return Result;
        }

        /// <summary>
        /// Проверяет пуста ли локальная директория
        /// </summary>
        public static bool CheckDirForEmpty (string DirectoryName)
        {
            // Объявление переменных
            bool Result = true;
            string ActuallyDirectoryName = Environment.CurrentDirectory + "//" + DirectoryName;

            // Получаем список файлов из директории
            var Files = Directory.GetFiles(ActuallyDirectoryName);

            // Проверяем пуста ли директория сравнением с 0
            if (Files.Length <= 0)
            {
                Result = false;
            }

            return Result;
        }
    }
}