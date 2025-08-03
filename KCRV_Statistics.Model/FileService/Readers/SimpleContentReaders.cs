using KCRV_Statistics.Core.Responses.DataResponses;
using KCRV_Statistics.Model.MessageService.MessageBoxService;

namespace KCRV_Statistics.Model.FileService.Readers
{
    /// <summary>
    /// Содержит методы чтения данных из файла
    /// </summary>
    public class SimpleContentReaders
    {
        /// <summary>
        /// ожидает получить на вход имя директории (не полный путь!!!) и имя файла с расширением. 
        /// Решение не передавать путь принято из за относительной адресации и потому что программисту так удобнее
        /// управлять потоками данных в приложении.
        /// </summary>
        public static SimpleDataResponse<string> GetContentFromFile(string DirectoryName, string FileName)
        {
            string ActuallyFilePath = "";

            if (!DirectoryName.Equals(""))
                ActuallyFilePath = Environment.CurrentDirectory + @"\" + DirectoryName + @"\" + FileName;
            else
                ActuallyFilePath = Environment.CurrentDirectory + @"\" + FileName;

            SimpleDataResponse<string> Result = new SimpleDataResponse<string>();

            var file = new FileInfo(ActuallyFilePath);
            if (!file.Exists || file.Length == 0)
            {
                return new SimpleDataResponse<string> 
                { 
                    Message = "Ошибка, файл " + FileName + " не существует, либо его содержимое пустое.",
                    Status = false
                };
            }

            try
            {
                Result.Data = File.ReadAllText(ActuallyFilePath);
                Result.Status = true;
                return Result;
            }
            catch (Exception e)
            {
                Result.Message = "Ошибка при чтении файла " + FileName + ":\n" + e.Message;
                Result.Status = true;
                return Result;
            }

        }
    }
}