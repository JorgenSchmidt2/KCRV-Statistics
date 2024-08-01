using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Model.MessageService.MessageBoxService;

namespace KCRV_Statistics.Model.MathService
{
    public class Estimators
    {

        private static readonly int DefaultIterationDigits = 8;
        private static readonly int DefaultResultDigits = 4;

        #region Переопределения метода Mean

        /// <summary>
        /// Находит среднее значение выборки и её неопределённость 
        /// (округление задаётся через параметры метода, в случае если оба значения сразу оказываются равны нулю,
        /// округляем до дефолтных значений)
        /// </summary>
        /// <param name="Data"> Входные данные типа "Значение-неопределённость"</param>
        /// <param name="IterationDigits"> Насколько округляются получаемые суммы при итерациях </param>
        /// <param name="ResultDigits"> Насколько округляется ответ </param>
        public static OutputData Mean(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            try
            {
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

                OutputData Result = new OutputData();
                Result.Content = "Mean";

                // Вычисление итогового значения
                foreach (RegularData item in Data)
                {
                    Result.X = Math.Round(Result.X + item.Value, IterationDigits);
                }
                Result.X = Math.Round(Result.X / Data.Count, ResultDigits);

                // Вычисление dU
                foreach (RegularData item in Data)
                {
                    Result.U = Math.Round(Result.U + Math.Pow(item.Value - Result.X, 2), IterationDigits);
                }
                Result.U = Math.Round(
                        Math.Pow(Result.U / (Data.Count - 1) / (Data.Count), 0.5),
                        ResultDigits
                    );

                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка в методе Mean: \n" + e.Message);
                return new OutputData() { Content = "Error (Mean)" };
            }
        }

        #endregion


        #region Переопределения метода WeightedMean

        /// <summary>
        /// Находит средневзвешенное значение выборки и её неопределённость
        /// (округление задаётся через параметры метода, в случае если оба значения сразу оказываются равны нулю,
        /// округляем до дефолтных значений)
        /// </summary>
        /// <param name="Data"> Входные данные типа "Значение-неопределённость"</param>
        /// <param name="IterationDigits"> Насколько округляются получаемые суммы при итерациях </param>
        /// <param name="ResultDigits"> Насколько округляется ответ </param>
        public static OutputData WeightedMean(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            try
            {
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

                OutputData Result = new OutputData();
                Result.Content = "WeightedMean";

                double Numerator = 0;
                double Denominator = 0;

                // Нахождение ключевых для расчёта параметров сумм
                foreach (RegularData item in Data)
                {
                    Numerator = Math.Round(Numerator + item.Value / Math.Pow(item.Uncertanity, 2), IterationDigits);
                    Denominator = Math.Round(Denominator + 1 / Math.Pow(item.Uncertanity, 2), IterationDigits);
                }

                // Нахождение ключевого значения и его неопределённости 
                Result.X = Math.Round(Numerator / Denominator, ResultDigits);
                Result.U = Math.Round(Math.Pow(1 / Denominator, 0.5), ResultDigits);

                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка в методе WeightedMean: \n" + e.Message);
                return new OutputData() { Content = "Error (WeightedMean)" };
            }
        }

        #endregion


        #region Переопределения метода Median и всё что с ними связано

        /// <summary>
        /// Достаёт средний элемент списка без нахождения медианы (в противном случае нарушение принципа единственной ответственности)
        /// </summary>
        /// <param name="Data"> Для RegularData </param>
        private static double GetMediavalElement (List<RegularData> Data)
        {
            double Result = 0;

            if (Data.Count % 2 == 1)
                Result = Data[(Data.Count - 1) / 2].Value;
            else
                Result = (Data[(Data.Count - 1) / 2].Value
                              + Data[Data.Count / 2].Value)
                              / 2;

            return Result;
        }

        /// <summary>
        /// Достаёт средний элемент списка без нахождения медианы (в противном случае нарушение принципа единственной ответственности)
        /// </summary>
        /// <param name="Data"> Для double </param>
        private static double GetMediavalElement(List<double> Data)
        {
            double Result = 0;

            if (Data.Count % 2 == 1)
                Result = Data[(Data.Count - 1) / 2];
            else
                Result = (Data[(Data.Count - 1) / 2]
                              + Data[Data.Count / 2])
                              / 2;

            return Result;
        }

        /// <summary>
        /// Находит медиану списка значений и их неопределённость
        /// (округление задаётся через параметры метода, в случае если оба значения сразу оказываются равны нулю,
        /// округляем до дефолтных значений)
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="IterationDigits"></param>
        /// <param name="ResultDigits"></param>
        /// <returns></returns>
        public static OutputData Median(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            try
            {
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

                OutputData Result = new OutputData();
                Result.Content = "Median";

                // Нахождение медианы
                Data = Data.OrderBy(x => x.Value).ToList();
                Result.X = Math.Round(GetMediavalElement(Data), ResultDigits);

                // Нахождение оценки неопределённости медианы
                List<double> Dispersion = new List<double>();
                foreach (var item in Data)
                    Dispersion.Add(
                        Math.Round(
                            Math.Abs(item.Value - Result.X), IterationDigits
                        )
                    );

                Dispersion.Sort();
                Result.U = Math.Round(
                    Math.Sqrt(Math.PI * Math.Pow(GetMediavalElement(Dispersion) * 1.483, 2) / 2 / Dispersion.Count)
                    , ResultDigits
                );

                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка в методе Median: \n" + e.Message);
                return new OutputData() { Content = "Error (Median)" };
            }

        }

        #endregion
    }
}