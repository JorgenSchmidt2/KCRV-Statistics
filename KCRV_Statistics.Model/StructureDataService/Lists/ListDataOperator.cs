using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace KCRV_Statistics.Model.StructureDataService.Lists
{
    public class ListDataOperator
    {
        public static List<EFileData> CopyFileDataListEntities (List<EFileData> Input)
        {
            List<EFileData> Result = new List<EFileData>();

            foreach (EFileData item in Input)
            {
                Result.Add(
                    new EFileData
                    {
                        ID = item.ID,
                        Directory = item.Directory,
                        FileName= item.FileName
                    }    
                ); 
            }

            return Result;
        }
    }
}