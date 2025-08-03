using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;

namespace KCRV_Statistics.Core.Entities.DataEntities.OtherDataEntities
{
    /// <summary>
    /// Класс с полной инкапсуляцией, служащий для организации работы с алгоритмом отсеивания лабораторий в оценке Кокса.
    /// </summary>
    public class ChiSquareDataEntity
    {
        /// <summary>
        /// Содержит сумму хи-квадрат всех элементов выборки.
        /// </summary>
        private double ChiSquareSum { get; set; }
        /// <summary>
        /// Содержит значение "внешней" оценки, с помощью которой рассчитываются ключевые значения
        /// Обычно используется оценка по средне-взвешенному.
        /// </summary>
        private double EstimateValue { get; set; }
        /// <summary>
        /// Критическое значение хи-квадрат
        /// </summary>
        private double CriticalValue { get; set; }
        /// <summary>
        /// Список основных данных
        /// </summary>
        private List<ChiData> ChiDataList { get; set; }

        /// <summary>
        /// Класс записи (record-like) для хранения элементов с учётом специфики оценок Кокса и Мандель-Пауля
        /// </summary>
        private class ChiData
        {
            internal double NumberLab { get; set; }
            internal double DataValue { get; set; }
            internal double Uncertanity { get; set; }
            internal double ChiSquareValue { get; set; }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ChiSquareDataEntity(List<RegularData> InputData)
        {
            ChiDataList = new List<ChiData>();
            foreach (var Item in InputData)
                ChiDataList.Add(
                    new ChiData { NumberLab = Item.LaboratoryNumber, DataValue = Item.Value, Uncertanity = Item.Uncertanity });
        }

        /// <summary>
        /// Реализация расчётов оценки Кокса
        /// </summary>
        public void CalculateAll(double EstimateValueParam)
        {
            double ChiSquare = 0;
            EstimateValue = EstimateValueParam;
            foreach (var Item in ChiDataList)
            {
                var value = Math.Pow(Item.DataValue - EstimateValueParam, 2) / Math.Pow(Item.Uncertanity, 2);
                Item.ChiSquareValue = value;
                ChiSquare += value;
            }
            ChiSquareSum = ChiSquare;
        }

        /// <summary>
        /// Проверка значений на соответствие критерию
        /// </summary>
        public void CheckAndDo(double CriticalValueParam, out bool IsEqualsCriticalAndChiSquare)
        {
            CriticalValue = CriticalValueParam;
            IsEqualsCriticalAndChiSquare = ChiSquareSum <= CriticalValueParam;
            if (!IsEqualsCriticalAndChiSquare)
            {
                var element = ChiDataList.MaxBy(x => x.ChiSquareValue);
                if (element != null) ChiDataList.Remove(element);
            }
        }

        /// <summary>
        /// Геттер, возвращающий количество элементов списка на текущий момент выполнения программы
        /// </summary>
        public int GetDataListCount() { return ChiDataList.Count; }
        /// <summary>
        /// Геттер, возвращающий значение суммы квадратов для текущей выборки
        /// </summary>
        public double GetChiSquare() { return ChiSquareSum; }
        /// <summary>
        /// Геттер, возвращающий значение сторонней оценки
        /// </summary>
        public double GetEstimateValue() { return EstimateValue; }
        /// <summary>
        /// Геттер, возвращающий рассчитанное критическое значение для выборки
        /// </summary>
        public double GetCriticalValue() {  return CriticalValue; }

        /// <summary>
        /// Геттер, возвращающий текущий список данных в общем для программы виде исходных данных
        /// </summary>
        public List<RegularData> GetData() 
        {  
            var Result = new List<RegularData>();

            foreach (var Item in ChiDataList)
                Result.Add(new RegularData { LaboratoryNumber = Item.NumberLab, Value = Item.DataValue, Uncertanity = Item.Uncertanity, ChiSquareValue = Item.ChiSquareValue });

            return Result;
        }
    }
}