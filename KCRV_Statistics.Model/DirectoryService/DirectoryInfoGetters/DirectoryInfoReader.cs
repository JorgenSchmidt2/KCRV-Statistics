using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Model.MessageService.MessageBoxService;

namespace KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters
{
    public class DirectoryInfoReader
    {
        public static List<EFileData> GetDirectoryList (List<string> DirectoryNames)
        {
            List<EFileData> Result = new List<EFileData> ();
            List<string> NonEmptyDirectories = new List<string>();
            
            try
            {
                foreach (var item in DirectoryNames)
                {
                    if (!Directory.Exists(Environment.CurrentDirectory + @"\" + item))
                    {
                        GetMessageBox.Show("Директории " + item + " не существует.");
                    }
                    else
                    {
                        NonEmptyDirectories.Add(item);
                    }
                }

                int FileCounter = 1;
                foreach (var item in NonEmptyDirectories)
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
                            var concat = "";

                            for (int i = 0; i <= splits.Length - 1 - 1; i++)
                            {
                                concat += splits[i] + @"\";
                            }

                            Result.Add( new EFileData() { ID = FileCounter, Directory = concat, FileName = splits[splits.Length - 1] });
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
    }
}