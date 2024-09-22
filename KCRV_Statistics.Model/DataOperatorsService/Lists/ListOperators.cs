using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace KCRV_Statistics.Model.DataOperatorsService.Lists
{
    public class ListOperators
    {
        #region Для сущностей FileDataEntity

        public static List<FileDataEntity> FilterFileListByExtension(List<FileDataEntity> InputList, List<string> Extensions)
        {
            List<FileDataEntity> Result = new List<FileDataEntity>();

            var counter = 1;
            foreach (var Item in InputList)
            {
                var splits = Item.FileName.Split('.');
                if (splits.Length >= 2)
                {
                    foreach (var CurrentExtension in Extensions)
                    {
                        if (CurrentExtension.Equals(splits[splits.Length - 1]))
                        {
                            Result.Add(
                                new FileDataEntity()
                                {
                                    ID = counter,
                                    Directory = Item.Directory,
                                    FileName = Item.FileName
                                }
                            );
                            counter++;
                            break;
                        }
                    }
                }
            }

            return Result;
        }

        public static List<FileDataEntity> CopyFileDataListEntities(List<FileDataEntity> Input)
        {
            List<FileDataEntity> Result = new List<FileDataEntity>();

            foreach (FileDataEntity item in Input)
            {
                Result.Add(
                    new FileDataEntity
                    {
                        ID = item.ID,
                        Directory = item.Directory,
                        FileName = item.FileName
                    }
                );
            }

            return Result;
        }

        #endregion
    }
}