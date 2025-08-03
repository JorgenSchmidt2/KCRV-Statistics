namespace KCRV_Statistics.Core.Responses.DataResponses
{
    /// <summary>
    /// Для передачи ответов, от методов, возвращающих список объектов
    /// </summary>
    public class ListDataResponse <T>
    {
        /// <summary>
        /// Статус исполнения в формате true-false
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Данные в виде списка однообразных объектов
        /// </summary>
        public List<T>? Data { get; set; }
        /// <summary>
        /// Должен содержать сообщение об ошибке
        /// </summary>
        public string? Message { get; set; }
    }
}