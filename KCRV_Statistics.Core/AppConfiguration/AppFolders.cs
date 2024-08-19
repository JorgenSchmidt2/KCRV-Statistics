namespace KCRV_Statistics.Core.AppConfiguration
{
    /// <summary>
    /// Содержит набор readonly полей, содержащих имена используемых в программе директорий
    /// </summary>
    public class AppFolders
    {
        /// <summary>
        /// Имя поля с названием директории с XLSX файлами
        /// </summary>
        public readonly static string InputFiles_XLSX = "Input Files XLSX";
        /// <summary>
        /// Имя поля с названием директории с CSV и JSON файлами
        /// </summary>
        public readonly static string InputFiles_CSV_JSON = "Input Files CSV-JSON";
        /// <summary>
        /// Имя поля с названием директории с простыми текстовыми файлами
        /// </summary>
        public readonly static string InputFiles_Simple = "Input Files Simple";
        /// <summary>
        /// Имя поля с названием директории с результатами
        /// </summary>
        public readonly static string Results = "Results";
        public readonly static string Reports = "Reports";
    }
}

// Задача на следующие коммиты: реализовать через конфигурационный файл возможность пользователю определять самому папки с исходными файлами
// Вместо вышеуказанных readonly полей будет реализован список папок произвольного размера (массив не будет readonly)
// Папки results и reports останутся неизменными