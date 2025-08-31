using KCRV_Statistics.Core.Entities.DataEntities.OtherDataEntities;
using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using System.Collections.Generic;

namespace KCRV_Statistics.UI.AppService
{
    /// <summary>
    /// Содержит основные поля, используемые непосредственно при работе окон программы. Не годятся для обнуления.
    /// </summary>
    public class AppData
    {
        #region Управление файлами
        /// <summary>
        /// Содержит имена файлов, которые удовлетворяют критериям: 
        /// 1."Находится в одной из выбранных папок"; 
        /// 2."При наличии запроса, содержит только те файлы, которые в имени содержат содержимое запроса.
        /// </summary>
        public static List<FileDataEntity> AppFileData = new List<FileDataEntity>();
        /// <summary>
        /// Список директорий, указанных в конфигурации
        /// </summary>
        public static List<ViewedDirectoryData> AppDirectoryData = new List<ViewedDirectoryData>();
        /// <summary>
        /// Содержит список выбранных пользователем директорий.
        /// </summary>
        public static List<string> ChosenFolders = new List<string>();
        #endregion

        #region Программные данные (используются по ходу работы программы
        /// <summary>
        /// Определяет является ли первый столбец входных двухстолбчатых данных "времяобразным" (значения нарастают по любому закону)
        /// </summary>
        public static bool IsTemporaryTwoColumnData = true;
        /// <summary>
        /// Содержит полученные данные от лабораторий.
        /// </summary>
        public static List<RegularData> CurrentData = new List<RegularData>();
        /// <summary>
        /// Содержит значения МСИ.
        /// </summary>
        public static List<OutputData> OutputData = new List<OutputData>();
        #endregion

        #region Данные для вывода
        /// <summary>
        /// Содержит значения результатов лаборатории на каждой итерации "отсеивания" данных по алгоритму Кокса до нахождения согласованного подмножества
        /// </summary>
        public static List<CoxResultsEntity> COX_Datas = new List<CoxResultsEntity>();
        #endregion
    }
}