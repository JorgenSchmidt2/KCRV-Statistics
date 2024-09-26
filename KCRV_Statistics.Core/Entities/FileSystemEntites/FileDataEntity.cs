namespace KCRV_Statistics.Core.Entities.FileSystemEntites
{
    /// <summary>
    /// Сущность для отображения имён директорий
    /// </summary>
    public class FileDataEntity
    {
        /// <summary>
        /// ID файла
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Имя папки в корневой директории приложения
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; set; }
    }
}