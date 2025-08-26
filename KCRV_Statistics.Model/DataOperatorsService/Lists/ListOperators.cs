using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Core.Responses.DataResponses;

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
        public static ListDataResponse<FileDataEntity> FilterFileListByExtension(List<FileDataEntity> InputList, List<string> Extensions)
        {
            ListDataResponse<FileDataEntity> Result = new ListDataResponse<FileDataEntity>();
            Result.Data = new List<FileDataEntity>();

            try 
            {
                if (InputList == null || InputList.Count == 0) throw new Exception("Входной лист оказался пустым.");

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
                                Result.Data.Add(
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

                Result.Status = true;
                return Result;
            }
            catch (Exception e)
            { 
                Result.Message = e.Message;
                Result.Status = false;
                return Result;
            }
        }

        /// <summary>
        /// Позволяет скопировать список файлов, применяется когда недопустимо создавать ссылку на первичный список.
        /// </summary>
        public static ListDataResponse<FileDataEntity> CopyFileDataListEntities(List<FileDataEntity> Input)
        {
            ListDataResponse<FileDataEntity> Result = new ListDataResponse<FileDataEntity>();
            Result.Data = new List<FileDataEntity>();

            try
            {
                if (Input.Count == 0)
                    throw new Exception("Входной список оказался пуст.");

                foreach (FileDataEntity item in Input)
                {
                    Result.Data.Add(
                        new FileDataEntity
                        {
                            ID = item.ID,
                            Directory = item.Directory,
                            FileName = item.FileName
                        }
                    );
                }

                Result.Status = true;
                return Result;
            }
            catch (Exception e) 
            {
                Result.Message = e.Message;
                Result.Status = false;
                return Result;
            }
        }

        #endregion

        #region Для сущностей RegularData

        /// <summary>
        /// Позволяет скопировать список сущностей типа RegularData в другой 
        /// из одной области памяти ЭВМ в другую с сохранением ссылки у объекта-указателя
        /// </summary>
        public static ListDataResponse<RegularData> CopyRegularDataListEntities(List<RegularData> Input)
        {
            ListDataResponse<RegularData> Result = new ListDataResponse<RegularData>();
            Result.Data = new List<RegularData>();

            try
            {
                foreach (var Item in Input)
                {
                    Result.Data.Add(
                        new RegularData
                        {
                            LaboratoryNumber = Item.LaboratoryNumber,
                            Value = Item.Value,
                            Uncertanity = Item.Uncertanity,
                            E = Item.E
                        }
                    );
                }

                Result.Status = true;
                return Result;
            }
            catch (Exception e) 
            { 
                Result.Message = e.Message;
                Result.Status = false;
                return Result;
            }
        }

        #endregion
    }
}