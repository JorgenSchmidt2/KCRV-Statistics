using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;

namespace KCRV_Statistics.Core.Entities.DataEntities.OtherDataEntities
{
    public class CoxResultsEntity
    {
        /// <summary>
        /// Данные объекта, должны быть задействованы поля, содержащие значение результата, неопределённость результата, значение Хи-Квадрат для данного значения
        /// </summary>
        public List<RegularData> Data { get; set; }
        /// <summary>
        /// Критическое значение 
        /// </summary>
        public double CriticalValue { get; set; }
        /// <summary>
        /// Значение хи квадрат всей выборки
        /// </summary>
        public double ChiSquareSum { get; set; }
        /// <summary>
        /// Опорное значение, которое строится по одной из аттестуемых характеристик (чаще по средневзвешенному)
        /// </summary>
        public double EstimateValue { get; set; }
    }
}