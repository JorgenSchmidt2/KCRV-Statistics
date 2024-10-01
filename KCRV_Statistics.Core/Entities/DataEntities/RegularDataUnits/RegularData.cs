namespace KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits
{
    /// <summary>
    /// Содержит описание входного набора данных для программы 
    /// </summary>
    public class RegularData
    {
        /// <summary>
        /// Номер лаборатории
        /// </summary>
        public double LaboratoryNumber { get; set; }
        /// <summary>
        /// Значение, полученное в лаборатории
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// Неопределённость результата лаборатории
        /// </summary>
        public double Uncertanity { get; set; }
        /// <summary>
        /// Значение критерия En. Вычисляется только после расчёта аттестуемой характеристики по методу Мандель-Пауля
        /// </summary>
        public double E { get; set; }
    }
}