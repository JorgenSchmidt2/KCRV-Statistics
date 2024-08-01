using KCRV_Statistics.Model.MessageService.MessageBoxService;

namespace KCRV_Statistics.Model.FileService.Writers
{
    public class SimpleContentWriters
    {
        /// <summary>
        /// Использует относительную адресацию, для данного статического метода требуется указать: 
        /// контент, записываемый в файл; 
        /// название папки, в которую записывается контент; 
        /// имя генерируемого файла; 
        /// расширение файла.
        /// </summary>
        public static void WriteContentToFile(string Content, string DirectoryName, string FileName, string Extension)
        {

            string ActuallyFilePath = Environment.CurrentDirectory + @"\" + DirectoryName + @"\" + FileName + "." + Extension;

            try
            {
                using (StreamWriter stream = new StreamWriter(ActuallyFilePath))
                {
                    stream.Write(Content);
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка при записи файла " + FileName + ":\n" + e.Message);
            }
        }
    }
}