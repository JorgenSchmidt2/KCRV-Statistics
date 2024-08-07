using KCRV_Statistics.Model.MessageService.MessageBoxService;

namespace KCRV_Statistics.Model.FileService.Readers
{
    public class SimpleContentReaders
    {
        public static string GetContentFromFile(string DirectoryName, string FileName)
        {
            string ActuallyFilePath = DirectoryName + @"\" + FileName;
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