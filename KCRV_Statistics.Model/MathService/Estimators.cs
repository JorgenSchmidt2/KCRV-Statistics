﻿using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Model.MessageService.MessageBoxService;
using MathNet.Numerics.Distributions;

namespace KCRV_Statistics.Model.MathService
{
    /// <summary>
    /// Содержит основные методы расчёта оценок межлабораторных исследований и их неопределённости.
    /// </summary>
    public class Estimators
    {
        // Поля, отвечающие за настройки округления по умолчанию, в данном случае
        // при итерациях полученное будет округляться до 8, а при выводе до 4
        private static readonly int DefaultIterationDigits = 8;
        private static readonly int DefaultResultDigits = 4;

        #region Метод Mean

        /// <summary>
        /// Находит среднее значение выборки и её неопределённость 
        /// (округление задаётся через параметры метода, в случае если оба значения сразу оказываются равны нулю,
        /// округляем до дефолтных значений).
        /// </summary>
        /// <param name="Data"> Входные данные типа "Значение-неопределённость"</param>
        /// <param name="IterationDigits"> Насколько округляются получаемые суммы при итерациях </param>
        /// <param name="ResultDigits"> Насколько округляется ответ </param>
        public static OutputData Mean(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            try
            {
                // Если оба входных показателя, отвечающих за округление равны 0, то 
                // им присваивается значение, равное соответствующим константам
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

                OutputData Result = new OutputData();       // Объявляем выходную переменную    | для следующих методов      |
                Result.MethodName = KCRV_MethodsNames.Mean; // Подписываем имя метода           | это подписываться не будет |

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
                return new OutputData() { MethodName = "Error (Mean)" };
            }
        }

        #endregion


        #region Метод WeightedMean

        /// <summary>
        /// Находит средневзвешенное значение выборки и её неопределённость
        /// (округление задаётся через параметры метода, в случае если оба значения сразу оказываются равны нулю,
        /// округляем до дефолтных значений).
        /// </summary>
        /// <param name="Data"> Входные данные типа "Значение-неопределённость"</param>
        /// <param name="IterationDigits"> Насколько округляются получаемые суммы при итерациях </param>
        /// <param name="ResultDigits"> Насколько округляется ответ </param>
        public static OutputData WeightedMean(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            try
            {
                // Если оба входных показателя, отвечающих за округление равны 0, то 
                // им присваивается значение, равное соответствующим константам
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

                OutputData Result = new OutputData();
                Result.MethodName = KCRV_MethodsNames.WeightedMean;

                double Numerator = 0;
                double Denominator = 0;

                // Нахождение ключевых для расчёта сумм, для числителя и знаменателя основной оценки
                // Применяются и при расчёте неопределённости результата (только знаменатель)
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
                return new OutputData() { MethodName = "Error (WeightedMean)" };
            }
        }

        #endregion


        #region Метод Median и всё что с ними связано

        /// <summary>
        /// Достаёт средний элемент списка без нахождения медианы (в противном случае нарушение принципа единственной ответственности).
        /// Для получения непосредственно медианы, необходимо перед применением метода отсортировать список, например методом SomeList.Sort(),
        /// где SomeList - некоторый определённый список.
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
        /// Для получения непосредственно медианы, необходимо перед применением метода отсортировать список, например методом SomeList.Sort(),
        /// где SomeList - некоторый определённый список.
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
        /// округляем до дефолтных значений).
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="IterationDigits"></param>
        /// <param name="ResultDigits"></param>
        /// <returns></returns>
        public static OutputData Median(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            try
            {
                // Если оба входных показателя, отвечающих за округление равны 0, то 
                // им присваивается значение, равное соответствующим константам
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

                OutputData Result = new OutputData();
                Result.MethodName = KCRV_MethodsNames.Median;

                // Нахождение медианы
                Data = Data.OrderBy(x => x.Value).ToList();
                Result.X = Math.Round(GetMediavalElement(Data), ResultDigits);

                // Нахождение оценки неопределённости медианы
                List<double> Dispersion = new List<double>();   // Вычисляем степени удалённости переменных от медианы
                foreach (var item in Data)
                    Dispersion.Add(
                        Math.Round(
                            Math.Abs(item.Value - Result.X), IterationDigits
                        )
                    );

                Dispersion.Sort();      // В данной строке сортируем список, в следующей уже вычисляем неопределённость
                Result.U = Math.Round(
                    Math.Sqrt(Math.PI * Math.Pow(GetMediavalElement(Dispersion) * 1.483, 2) / 2 / Dispersion.Count)
                    , ResultDigits
                );

                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка в методе Median: \n" + e.Message);
                return new OutputData() { MethodName = "Error (Median)" };
            }

        }

        #endregion

        #region Метод DerSimonian

        /// <summary>
        /// Находит значение KCRV, его неопределённость и InterLabVariance по методу Дер-Симониана.
        /// Наличие значения средне-взешенного значения по выборке - обязательно.
        /// </summary>
        public static OutputData DerSimonian(List<RegularData> Data, double WeightedMeanValue, int IterationDigits, int ResultDigits)
        {
            // Если оба входных показателя, отвечающих за округление равны 0, то 
            // им присваивается значение, равное соответствующим константам
            if (IterationDigits == 0 && ResultDigits == 0)
            {
                IterationDigits = DefaultIterationDigits;
                ResultDigits = DefaultResultDigits;
            }

            OutputData Result = new OutputData();
            Result.MethodName = KCRV_MethodsNames.DerSimonian;

            // Начало расчёта показателя InterLabVariance
            double GrayBillValue = 0;
            double W1 = 0;
            double W2 = 0;

            // Расчёт необходимых сумм, участвующих в расчёте InterLabVariance
            foreach (var Item in Data)
            {
                var w = 1 / Math.Pow(Item.Uncertanity, 2);

                GrayBillValue = Math.Round(
                    GrayBillValue + w * Math.Pow(Item.Value - WeightedMeanValue, 2), 
                    IterationDigits
                );

                W1 = Math.Round(W1 + w, IterationDigits);
                W2 = Math.Round(W2 + Math.Pow(w, 2), 
                    IterationDigits
                );
            }

            // Расчёт полученного результата для InterLabVariance и сравнение его с нулём, 
            // если полученный результат отрицательный, - итоговое значение InterLabVariance будет равно нулю
            var LambdaValue = (GrayBillValue - Data.Count() + 1) / (W1 - W2 / W1);

            if (LambdaValue > 0) Result.InterLabVariance = Math.Round(LambdaValue, ResultDigits);
            else Result.InterLabVariance = 0;

            // Начало расчёта показателя KCRV и его неопределённости
            double Usqr_LAMBDA_Summ = 0; // Usqr означает, что берётся квадрат неопределённости,
                                         // название метода неполное, т.к. суммироваться будет сумма под степенью -1

            // Считаем сумму, которая приходится на знаменатель переменной w
            foreach (var Item in Data)
            {
                Usqr_LAMBDA_Summ = Math.Round(
                    Usqr_LAMBDA_Summ + Math.Pow(Math.Pow(Item.Uncertanity, 2) + LambdaValue, -1), IterationDigits
                );
            }

            // Начинаем расчитывать значение KCRV
            double X = 0;
            foreach (var Item in Data)
            {
                var w = Math.Pow(Math.Pow(Item.Uncertanity, 2) + LambdaValue, -1) / Usqr_LAMBDA_Summ;
                X = Math.Round(X + w * Item.Value, IterationDigits);
            }
            Result.X = Math.Round(X, ResultDigits);

            // Начинаем расчитывать значение неопределённости KCRV
            double U = 0;
            foreach (var Item in Data)
            {
                var w = Math.Pow(Math.Pow(Item.Uncertanity, 2) + LambdaValue, -1) / Usqr_LAMBDA_Summ;

                U = Math.Round( U + Math.Pow(w, 2) * Math.Pow(Item.Value - Result.X, 2) / (1 - w)
                    , IterationDigits
                );
            }
            Result.U = Math.Round(Math.Sqrt(U), ResultDigits);

            return Result;
        }

        #endregion

        #region Метод MandelPaule

        public static OutputData MandelPaule(List<RegularData> Data, double WeightedMeanValue, int IterationDigits, int ResultDigits)
        {
            // Если оба входных показателя, отвечающих за округление равны 0, то 
            // им присваивается значение, равное соответствующим константам
            if (IterationDigits == 0 && ResultDigits == 0)
            {
                IterationDigits = DefaultIterationDigits;
                ResultDigits = DefaultResultDigits;
            }

            OutputData Result = new OutputData();
            Result.MethodName = KCRV_MethodsNames.MandelPaule;

            // Временное значение
            double AddDispersion = CalculateAddDispForMandelPaule(Data, 15);
            Result.InterLabVariance = Math.Round(AddDispersion, ResultDigits);

            // Расчёт показателя KCRV и его неопределённости, в первую очередь посчитаем значение знаменателя числа w
            double w_denominator = 0;
            foreach (var Item in Data)
            {
                w_denominator = Math.Round(w_denominator + 1 / (Math.Pow(Item.Uncertanity, 2) + Math.Pow(AddDispersion, 2)), IterationDigits);
            }

            // Начало расчёта показателя KCRV
            double X = 0;
            foreach (var Item in Data)
            {
                var w = (1 / (Math.Pow(Item.Uncertanity, 2) + Math.Pow(AddDispersion, 2))) / w_denominator;
                X = Math.Round(X + w * Item.Value, IterationDigits);
            }
            Result.X = Math.Round(X, ResultDigits);

            // Расчёт показателя неопределённости KCRV
            double U = Math.Sqrt(1 / w_denominator);
            Result.U = Math.Round(U, ResultDigits);

            return Result;
        }

        #endregion

        #region Другие операции

        /// <summary>
        /// Позволяет рассчитать En критерий для каждого результата лабораторий
        /// </summary>
        public static List<RegularData> GetEnValues(List<RegularData> Data, OutputData MandelPaule, int ResultDigits)
        {
            // Если оба входных показателя, отвечающих за округление равны 0, то 
            // им присваивается значение, равное соответствующим константам
            if (ResultDigits == 0)
            {
                ResultDigits = DefaultResultDigits;
            }

            List<RegularData> Result = new List<RegularData>();

            foreach (var Item in Data)
            {
                var E = (Item.Value - MandelPaule.X) / 2 / Math.Sqrt(Math.Pow(Item.Uncertanity, 2) + Math.Pow(MandelPaule.U, 2));
                Result.Add(
                    new RegularData()
                    {
                        LaboratoryNumber = Item.LaboratoryNumber,
                        Value = Item.Value,
                        Uncertanity = Item.Uncertanity,
                        E = Math.Round(E, ResultDigits)
                    }
                );
            }

            return Result;
        }

        /// <summary>
        /// Позволяет с заданной точностью до 15 знаков рассчитать значение добавочной дисперсии для метода Мандель-Пауля.
        /// Данному методу подчиняются два приватных метода GetLambdaCharacteristics_MandelPaule и GetNaturalSquared_MandelPaule 
        /// </summary>
        public static double CalculateAddDispForMandelPaule (List<RegularData> Data, int Accuracy)
        {
            double Result = 0;

            // Интервал доверия для расчёта критического значение Хи^2 - 5%
            double CriticalSquared = ChiSquared.InvCDF(Data.Count() - 1, 1 - 0.05);

            // Добавочная дисперсия и шаг по ней
            double AddDispersion = 0.5;
            double StepDisp = AddDispersion / 2;

            // Начало вычисления добавочной дисперсии
            while (true)
            {
                // Расчёт характеристики по добавочной дисперсии
                double LambdaCharacter = GetLambdaCharacteristics_MandelPaule(Data, AddDispersion);

                // Расчёт натурального значения Хи^2
                double NaturalSquared = GetNaturalSquared_MandelPaule(Data, AddDispersion, LambdaCharacter);

                // Проверка полученного натурального значения
                if (CriticalSquared == NaturalSquared)
                {
                    Result = Math.Sqrt(AddDispersion);
                    break;
                }
                else if (CriticalSquared >= NaturalSquared)
                {
                    AddDispersion -= StepDisp;
                    StepDisp /= 2;
                }
                else if (CriticalSquared <= NaturalSquared)
                {
                    AddDispersion += StepDisp;
                    StepDisp /= 2;
                }
            }

            return Result;
        }

        /// <summary>
        /// Вычисляет характеристику от лямбды, подчинён методу CalculateAddDispForMandelPaule
        /// </summary>
        private static double GetLambdaCharacteristics_MandelPaule (List<RegularData> Data, double AddDispersion) 
        {
            double LambdaCharacter_numerator = 0;
            double LambdaCharacter_denomerator = 0;
            foreach (var Item in Data)
            {
                LambdaCharacter_numerator += Item.Value / (Math.Pow(Item.Uncertanity, 2) + AddDispersion);
                LambdaCharacter_denomerator += 1 / (Math.Pow(Item.Uncertanity, 2) + AddDispersion);
            }
            return LambdaCharacter_numerator / LambdaCharacter_denomerator;
        }

        /// <summary>
        /// Вычисляет натуральное значение Хи^2, подчинён методу CalculateAddDispForMandelPaule
        /// </summary>
        private static double GetNaturalSquared_MandelPaule (List<RegularData> Data, double AddDispersion, double LambdaCharacter)
        {
            double NaturalSquared = 0;

            // Расчёт натурального значения Хи^2
            foreach (var Item in Data)
            {
                NaturalSquared += Math.Pow(Item.Value - LambdaCharacter, 2) / (Math.Pow(Item.Uncertanity, 2) + AddDispersion);
            }
            return NaturalSquared;
        }

        #endregion

        #region Расчёт всех показателей

        /// <summary>
        /// Метод для составления списка всех показателей для полученной выборки.
        /// </summary>
        public static List<OutputData> CalculateAllMethods (ref List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            List<OutputData> Result = new List<OutputData>();

            var mean            = Mean          (Data, IterationDigits, ResultDigits);
            var weightedMean    = WeightedMean  (Data, IterationDigits, ResultDigits);
            var median          = Median        (Data, IterationDigits, ResultDigits);
            var mandelPaule     = MandelPaule   (Data, weightedMean.X, IterationDigits, ResultDigits);
            var derSimonian     = DerSimonian   (Data, weightedMean.X, IterationDigits, ResultDigits);

            Data = GetEnValues(Data, mandelPaule, ResultDigits);

            Result.Add(mean);
            Result.Add(weightedMean);
            Result.Add(median);
            Result.Add(mandelPaule);
            Result.Add(derSimonian);

            return Result;
        }

        #endregion
    }
}