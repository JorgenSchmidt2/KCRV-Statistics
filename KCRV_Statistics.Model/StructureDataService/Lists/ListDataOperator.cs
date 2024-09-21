using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace KCRV_Statistics.Model.StructureDataService.Lists
{
    public class ListDataOperator
    {
        public static List<FileDataEntity> CopyFileDataListEntities (List<FileDataEntity> Input)
        {
            List<FileDataEntity> Result = new List<FileDataEntity>();

            foreach (FileDataEntity item in Input)
            {
                Result.Add(
                    new FileDataEntity
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