using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace KCRV_Statistics.Model.DataOperatorsService.Lists
{
    /// <summary>
    /// Содержит основные операции со списками, применяемые конкретно в данной программе.
    /// </summary>
    public class ListOperators
    {
        #region Для сущностей FileDataEntity

        /// <summary>
        /// Позволяет отфильтровать входной список файлов по переданным в метод расширениям файлов
        /// </summary>
        public static List<FileDataEntity> FilterFileListByExtension(List<FileDataEntity> InputList, List<string> Extensions)
        {
            List<FileDataEntity> Result = new List<FileDataEntity>();

            // Задаём счётчик для присвоения идентификаторов элементам списка файлов
            var counter = 1;

            // Перебор элементов входного списка
            foreach (var Item in InputList)
            {
                // Разбиваем конкретное имя файла на части по символу "точка", если количество полученных элементов в массиве оказалось
                // больше или равно двум, - следующим шагом перебираем все нужные расширения файлов, при обнаружении такового 
                // копируем элементы текущего файла в результрующий объект
                var splits = Item.FileName.Split('.');
                if (splits.Length >= 2)
                {
                    // Перебор списка расширений
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

        /// <summary>
        /// Позволяет скопировать список файлов, применяется когда недопустимо создавать ссылку на первичный список.
        /// </summary>
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

        #region Для сущностей RegularData

        public static List<RegularData> CopyRegularDataListEntities(List<RegularData> Input)
        {
            List<RegularData> Result = new List<RegularData>();

            foreach (var Item in Input)
            {
                Result.Add(
                    new RegularData
                    {
                        LaboratoryNumber = Item.LaboratoryNumber,
                        Value = Item.Value,
                        Uncertanity = Item.Uncertanity,
                        E = Item.E
                    }    
                );
            }

            return Result;
        }

        #endregion
    }
}