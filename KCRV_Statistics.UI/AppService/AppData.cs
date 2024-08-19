using KCRV_Statistics.Core.Entities.FileSystemEntites;
using System.Collections.Generic;

namespace KCRV_Statistics.UI.AppService
{
    /// <summary>
    /// Содержит основные поля, используемые непосредственно при работе окон программы.
    /// </summary>
    public class AppData
    {
        public static List<EFileData> AppFileData = new List<EFileData>();
        public static List<string> ChoisedFolders = new List<string>();
    }
}