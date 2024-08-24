using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.FileSystemEntites;
using System.Collections.Generic;

namespace KCRV_Statistics.UI.AppService
{
    /// <summary>
    /// Содержит основные поля, используемые непосредственно при работе окон программы.
    /// </summary>
    public class AppData
    {
        /// <summary>
        /// Содержит имена файлов, которые удовлетворяют критериям: 
        /// 1."Находится в одной из выбранных папок"; 
        /// 2."При наличии запроса, содержит только те файлы, которые в имени содержат содержимое запроса.
        /// </summary>
        public static List<EFileData> AppFileData = new List<EFileData>();
        /// <summary>
        /// Содержит список выбранных пользователем директорий.
        /// </summary>
        public static List<string> ChoisedFolders = new List<string>();
        /// <summary>
        /// Содержит полученные данные.
        /// </summary>
        public static List<RegularData> CurrentData = new List<RegularData>();
        /// <summary>
        /// Содержит значения KCRV.
        /// </summary>
        public static List<OutputData> OutputData = new List<OutputData>();
    }
}