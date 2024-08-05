using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace KCRV_Statistics.Model.SearchService.FileFinders
{
    public class FileQueryMaker
    {
        public static List<EFileData> DoQuery (string Query, List<EFileData> FileList)
        {
            List<EFileData> Result = new List<EFileData>();

            int FileCounter = 1;

            foreach (var item in FileList)
            {
                if (item.FileName.Contains(Query))
                {
                    Result.Add(
                        new EFileData ()
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