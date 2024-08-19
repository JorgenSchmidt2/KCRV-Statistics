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
        public static string GetContentFromFile(string DirectoryName, string FileName)
        {
            string ActuallyFilePath = Environment.CurrentDirectory + @"\" + DirectoryName + @"\" + FileName;
            string Result = "";

            var file = new FileInfo(ActuallyFilePath);
            if (!file.Exists || file.Length == 0)
            {
                GetMessageBox.Show("Ошибка, файл не существует, либо его содержимое пустое.");
                return "";
            }

            try
            {
                Result = File.ReadAllText(ActuallyFilePath);
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка при чтении файла " + FileName + ":\n" + e.Message);
                return "";
            }

            return Result;
        }
    }
}