namespace KCRV_Statistics.Core.Responses.DataResponses
{
    /// <summary>
    /// Для передачи ответов, от методов, возвращающих объект
    /// </summary>
    public class SimpleDataResponse <T>
    {
        /// <summary>
        /// Статус исполнения в формате true-false
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Данные 
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Должен содержать сообщение об ошибке
        /// </summary>
        public string? Message { get; set; }
    }
}