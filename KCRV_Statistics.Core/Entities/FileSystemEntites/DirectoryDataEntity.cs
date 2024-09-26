namespace KCRV_Statistics.Core.Entities.FileSystemEntites
{
    /// <summary>
    /// Базовая сущность для хранения имён директорий
    /// </summary>
    public class DirectoryDataEntity
    {
        /// <summary>
        /// Имя директории. Может быть задано всего один раз.
        /// </summary>
        public string DirectoryName { get; init; }
    }
}