namespace KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits
{
    /// <summary>
    /// Выходной набор данных для одного результата по алгоритму
    /// </summary>
    public class OutputData
    {
        /// <summary>
        /// Содержит название метода
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Содержит результаты работы алгоритмов
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Содержит неопределённость результатов работы алгоритмы
        /// </summary>
        public double U { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double InterLabVariance { get; set; }
    }
}