namespace KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits
{
    /// <summary>
    /// Содержит результаты расчёта аттестуемой характеристики по одному из методов
    /// </summary>
    public class OutputData
    {
        /// <summary>
        /// Содержит название метода
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Значение аттестуемой характеристики
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Значение неопределённости аттестуемой характеристики
        /// </summary>
        public double U { get; set; }
        /// <summary>
        /// Значение добавочной дисперсии
        /// </summary>
        public double InterLabVariance { get; set; }
    }
}