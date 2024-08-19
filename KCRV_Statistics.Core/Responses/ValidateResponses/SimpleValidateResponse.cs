namespace KCRV_Statistics.Core.Responses.ValidateResponses
{
    /// <summary>
    /// Позволяет в случае возникновения ошибки доставить сообщение там, где такой возможности нет, например из методов,
    /// в которых не предусматривается вывод каких-либо сообщений, по этой причине реализовано в виде веб-подобного respons'а
    /// </summary>
    public class SimpleValidateResponse
    {
        /// <summary>
        /// Статус исполнения в формате true-false
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Должен содержать сообщение об ошибке
        /// </summary>
        public string? Message { get; set; }
    }
}