using KCRV_Statistics.Core.Entities.FileSystemEntites;
using System.Collections.Generic;

namespace KCRV_Statistics.UI.AppService
{
    public class AppData
    {
        public static List<EFileData> AppFileData = new List<EFileData>();
        public static List<string> ChoisedFolders = new List<string>();
    }
}