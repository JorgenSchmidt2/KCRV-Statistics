using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Core.Entities.DataEntities.OtherDataEntities;
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

        #region Оценка Mean

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

        #region Оценка WeightedMean

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

        #region Оценка Median и всё что с ней связано

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
            OutputData Result = new OutputData();
            Result.MethodName = KCRV_MethodsNames.Median;

            try
            {
                if (Data.Count() == 0) throw new Exception("Длина списка входных элементов равнялась нулю.");

                // Если оба входных показателя, отвечающих за округление равны 0, то 
                // им присваивается значение, равное соответствующим константам
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

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

        #region Оценка DerSimonian

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

        #region Оценка MandelPaule и всё что с ней связано (треб. правки)

        /// <summary>
        /// Находит значение KCRV, его неопределённость и добавочную по методу Мандель-Пауля.
        /// </summary>
        public static OutputData MandelPaule(List<RegularData> Data, int IterationDigits, int ResultDigits)
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

            try
            {
                // Расчёт добавочной дисперсии для метода Мандель-Пауля
                double AddDispersion = CalculateAddDispForMandelPaule(Data, 9);
                if (AddDispersion == 0) throw new Exception("К сожалению, не удалось рассчитать добавочную дисперсию для метода Мандель-Пауля.");
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
            catch (Exception e)
            {
                GetMessageBox.Show("Метод " + Result.MethodName + " был рассчитан с ошибкой: \n" + e.Message);
                return Result;
            }
        }

        /// <summary>
        /// Позволяет с заданной точностью до 15 знаков рассчитать значение добавочной дисперсии для метода Мандель-Пауля.
        /// Данному методу подчиняются два приватных метода GetLambdaCharacteristics_MandelPaule и GetNaturalSquared_MandelPaule 
        /// </summary>
        public static double CalculateAddDispForMandelPaule(List<RegularData> Data, int Accuracy)
        {
            double Result = 0;

            // Интервал доверия для расчёта критического значение Хи^2 - 5%
            double CriticalSquared = ChiSquared.InvCDF(Data.Count() - 1, 1 - 0.05);

            // Добавочная дисперсия и шаг по ней
            double AddDispersion = 10;
            double StepDisp = AddDispersion / 2;

            // Инициализация определителя шага (позитивный или негативный)
            bool IsNegativeStep;
            double InitialNaturalSquared = GetNaturalSquared_MandelPaule(
                Data,
                AddDispersion,
                GetLambdaCharacteristics_MandelPaule(Data, AddDispersion)
            );
            if (CriticalSquared > InitialNaturalSquared)
                IsNegativeStep = true;
            else
                IsNegativeStep = false;

            int counter = 0;

            // Начало вычисления добавочной дисперсии
            while (true)
            {
                // Расчёт характеристики по добавочной дисперсии
                double LambdaCharacter = GetLambdaCharacteristics_MandelPaule(Data, AddDispersion);

                // Расчёт натурального значения Хи^2
                double NaturalSquared = GetNaturalSquared_MandelPaule(Data, AddDispersion, LambdaCharacter);

                // Проверка полученного натурального значения
                if (Math.Round(CriticalSquared, Accuracy) == Math.Round(NaturalSquared, Accuracy))
                {
                    Result = Math.Sqrt(AddDispersion);
                    break;
                }
                else if (CriticalSquared >= NaturalSquared)
                {
                    AddDispersion -= StepDisp;

                    if (AddDispersion < 0)
                    {
                        AddDispersion = Math.Abs(AddDispersion);
                        StepDisp /= 2;
                    }

                    if (!IsNegativeStep)
                        StepDisp /= 2;
                }
                else if (CriticalSquared <= NaturalSquared)
                {
                    AddDispersion += StepDisp;
                    if (IsNegativeStep)
                        StepDisp /= 2;
                }

                counter++;
                if (counter == 100)
                {
                    Result = 0;
                    break;
                }
            }

            return Result;
        }

        /// <summary>
        /// Вычисляет характеристику от лямбды, подчинён методу CalculateAddDispForMandelPaule
        /// </summary>
        private static double GetLambdaCharacteristics_MandelPaule(List<RegularData> Data, double AddDispersion)
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
        private static double GetNaturalSquared_MandelPaule(List<RegularData> Data, double AddDispersion, double LambdaCharacter)
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

        #region Оценка Huber
        /// <summary>
        /// Расчёт показателя по методу Хубера
        /// </summary>
        public static OutputData Huber(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            OutputData Result = new OutputData();
            Result.MethodName = KCRV_MethodsNames.Huber;

            try
            {
                if (Data.Count() == 0) throw new Exception("Длина списка входных элементов равнялась нулю.");

                // Если оба входных показателя, отвечающих за округление равны 0, то 
                // им присваивается значение, равное соответствующим константам
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

                var MedianValue = Median(Data, IterationDigits, ResultDigits).X;
                var SkoEstimate = RobustSKO(Data, MedianValue, IterationDigits, ResultDigits);
                var IntermediateResult = MedianValue;

                var Counter = 0;
                while (true)
                {
                    var W = 0.0;
                    var LocalHUB = IntermediateResult;
                    IntermediateResult = 0;

                    foreach (var item in Data) 
                    {
                        var current_v = 0.0;
                        if (1.345 * SkoEstimate < Math.Abs(item.Value - LocalHUB))
                            current_v = Math.Round(1.345 * SkoEstimate / Math.Abs(item.Value - LocalHUB), IterationDigits);
                        else
                            current_v = 1;

                        IntermediateResult = Math.Round(IntermediateResult + item.Value * current_v, IterationDigits);
                        W = Math.Round(W + current_v, IterationDigits);
                    }

                    IntermediateResult = Math.Round(IntermediateResult / W, IterationDigits);
                    SkoEstimate = RobustSKO(Data, IntermediateResult, IterationDigits, ResultDigits);

                    Counter++;
                    if (Math.Abs(IntermediateResult - LocalHUB) <= 0.00001) 
                        break;
                    if (Counter >= 4000) 
                        throw new Exception("Количество итераций превысило допустимые пределы (4000).");
                }

                Result.X = Math.Round(
                    IntermediateResult,
                    ResultDigits
                );
                Result.U = Math.Round(
                    Math.Sqrt(Math.Pow(SkoEstimate, 2) / (0.95 * Data.Count)),
                    ResultDigits
                );

                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Метод " + Result.MethodName + " был рассчитан с ошибкой: \n" + e.Message);
                return Result;
            }
        } 

        
        #endregion

        #region Оценка GOST

        public static OutputData GOST(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            OutputData Result = new OutputData();
            Result.MethodName = KCRV_MethodsNames.GOST;

            try
            {
                // Если оба входных показателя, отвечающих за округление равны 0, то 
                // им присваивается значение, равное соответствующим константам
                if (IterationDigits == 0 && ResultDigits == 0)
                {
                    IterationDigits = DefaultIterationDigits;
                    ResultDigits = DefaultResultDigits;
                }

                if (Data.Count() == 0) throw new Exception("Длина списка входных элементов равнялась нулю.");

                List<RegularData> DataDiff = new List<RegularData>();

                // Медиана ненулевых отклонений
                double Mad = 0;

                // Значение медианы выборки
                double MedianValue = Median(Data, IterationDigits, ResultDigits).X;

                // Расчёт значений для медианы ненулевых отклонений
                foreach (var item in Data) DataDiff.Add(new RegularData { Value = Math.Abs(item.Value - MedianValue) });

                List<RegularData> DataDiffWithoutZero = new List<RegularData>();

                foreach (var item in DataDiff)
                    if (item.Value != 0) DataDiffWithoutZero.Add(item);

                Mad = Median(DataDiffWithoutZero, IterationDigits, ResultDigits).X;

                if (DataDiff.Max(obj => obj.Value) <= 3 * Mad)
                {
                    var MeanValue = Mean(Data, IterationDigits, ResultDigits);
                    Result.X = MeanValue.X;
                    Result.U = MeanValue.U;
                }
                else
                {
                    double summ_w = 0;
                    double gos = 0;
                    int Counter = 0;
                    int Used_W_Counter = DataDiff.Count();
                    foreach (var item in DataDiff)
                    {
                        double w_value = 0;
                        if (item.Value < 5.2 * Mad)
                        {
                            w_value = Math.Pow(
                                1 - Math.Pow(item.Value / 5.2 / Mad, 2), 2
                            );
                        }
                        else Used_W_Counter--;

                        summ_w = Math.Round(summ_w + w_value, IterationDigits);

                        if (Counter >= DataDiff.Count())
                            throw new Exception("Количество значений исходных данных превысило количество значений модулей от разницы значения и медианы выборки.");
                        gos = Math.Round(gos + w_value * Data[Counter].Value, IterationDigits);

                        Counter += 1;
                    }
                    gos /= summ_w;
                    Result.X = gos;
                    Result.U = RobustSKO(Data, gos, IterationDigits, ResultDigits) / Math.Sqrt(Used_W_Counter);
                }

                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Метод " + Result.MethodName + " был рассчитан с ошибкой: \n" + e.Message);
                return Result;
            }
        }

        #endregion

        #region Оценка A

        public static OutputData A(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            // Если оба входных показателя, отвечающих за округление равны 0, то 
            // им присваивается значение, равное соответствующим константам
            if (IterationDigits == 0 && ResultDigits == 0)
            {
                IterationDigits = DefaultIterationDigits;
                ResultDigits = DefaultResultDigits;
            }

            OutputData Result = new OutputData();
            Result.MethodName = KCRV_MethodsNames.A;

            try
            {
                var InternalAccuracy = 3;
                var MedianValue = Median(Data, IterationDigits, ResultDigits).X;
                var RobustValue = RobustSKO(Data, MedianValue, IterationDigits, ResultDigits);
                var CountSQRT = Math.Sqrt(Data.Count());
                var CountList = Data.Count();

                int Counter = 0;
                double MeanA = 0;
                double SKO = 0;

                while (true)
                {
                    List<double> ChangedList = Data.Select(obj => obj.Value).ToList();

                    MeanA = MedianValue;
                    SKO = RobustValue;

                    for (var i = 0; i < ChangedList.Count; i++)
                    {
                        var bet = 1.5 * SKO;
                        if (ChangedList[i] < MeanA - bet) ChangedList[i] = MeanA - bet;
                        else if (ChangedList[i] > MeanA + bet) ChangedList[i] = MeanA + bet;
                    }

                    MedianValue = ChangedList.Sum() / CountList;

                    var s_summ = ChangedList.Select(y => Math.Pow(y - MedianValue, 2)).Sum();
                    RobustValue = 1.134 * Math.Sqrt(s_summ/(CountList - 1));

                    if (Math.Round(MeanA, InternalAccuracy) == Math.Round(MedianValue, InternalAccuracy) &&
                        Math.Round(SKO, InternalAccuracy) == Math.Round(RobustValue, InternalAccuracy))
                    {
                        break;
                    }
                    Counter++;
                    if (Counter >= 800)
                        throw new Exception("Количество итераций превысило допустимые пределы (800).");
                }

                Result.X = Math.Round(MeanA, ResultDigits);
                Result.U = Math.Round(SKO / CountSQRT, ResultDigits);
                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Метод " + Result.MethodName + " был рассчитан с ошибкой: \n" + e.Message);
                return Result;
            }
        }

        #endregion

        #region Группа оценок PMA + оценка Кокса_1 (треб. правки)

        public static OutputData PMA(List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            // Если оба входных показателя, отвечающих за округление равны 0, то 
            // им присваивается значение, равное соответствующим константам
            if (IterationDigits == 0 && ResultDigits == 0)
            {
                IterationDigits = DefaultIterationDigits;
                ResultDigits = DefaultResultDigits;
            }

            OutputData Result = new OutputData();
            Result.MethodName = KCRV_MethodsNames.PMA;

            try
            {
                var FiltredCoxData = COX_1(Data);

            }
            catch (Exception e)
            {
                GetMessageBox.Show("Метод " + Result.MethodName + " был рассчитан с ошибкой: \n" + e.Message);
                return Result;
            }

            return Result;
        }

        public static List<RegularData> COX_1(List<RegularData> Data)
        {
            List<RegularData> Result = new List<RegularData>();

            try
            {
                ChiSquareDataEntity ActuallyDataObject = new ChiSquareDataEntity(Data);

                while (ActuallyDataObject.GetDataListCount() > 0) 
                {
                    // Значение по Дер-Симониану (после "починки" метода Мандель-Пауля, нужно будет использовать именно его) 
                    var NewData = ActuallyDataObject.GetData();
                    var PreEstimateValue = WeightedMean(NewData, 8, 8).X;

                    // Расчёт хи-значений для выборки
                    ActuallyDataObject.CalculateAll(PreEstimateValue);

                    // Интервал доверия для расчёта критического значение Хи^2 - 5%
                    double CriticalSquared = ChiSquared.InvCDF(ActuallyDataObject.GetDataListCount() - 1, 1 - 0.05);

                    // Проверка условия
                    bool IsEqualsCriticalAndChiSquare = false;
                    ActuallyDataObject.CheckAndDo(CriticalSquared, out IsEqualsCriticalAndChiSquare);
                    
                    if (IsEqualsCriticalAndChiSquare) 
                        break; 
                }

                if (ActuallyDataObject.GetDataListCount() <= 0) throw new Exception("Количество элементов оказалось равно нулю 0.");
                
                Result = ActuallyDataObject.GetData();
            }
            catch (Exception e) 
            {
                GetMessageBox.Show("Метод COX_1 был рассчитан с ошибкой: \n" + e.Message);
                return Result;
            }

            return Result;
        }

        /*
                 * Остаточный код для COX_Standart, когда тот понадобится (не удалять!!!)
                 * double reverse_uncertanity = 0;
                double relative_x_squ = 0;
                foreach (var Item in ActualData)
                {
                    reverse_uncertanity += Math.Pow(Item.Uncertanity, -2);
                    relative_x_squ += Item.Value / Math.Pow(Item.Uncertanity, 2);
                }

                Result.X = relative_x_squ / reverse_uncertanity;
                Result.U = Math.Sqrt(ActuallyDataObject.GetChiSquare() / (ActuallyDataObject.GetDataListCount() * reverse_uncertanity));
        */

        #endregion

        #region Другие операции

        /// <summary>
        /// Робастное среднеквадратичное отклонение
        /// </summary>
        private static double RobustSKO(List<RegularData> Data, double Value, int IterationDigits, int ResultDigits)
        {
            List<RegularData> Yonda = new List<RegularData>();

            foreach (var Item in Data) Yonda.Add(new RegularData { Value = Math.Abs(Item.Value - Value) });

            return Median(Yonda, IterationDigits, ResultDigits).X * 1.483;
        }

        /// <summary>
        /// Позволяет рассчитать En критерий для каждого результата лабораторий
        /// </summary>
        public static List<RegularData> GetEnValues(List<RegularData> Data, OutputData CertifiedCharacteristic, int ResultDigits)
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
                var E = (Item.Value - CertifiedCharacteristic.X) / 2 / Math.Sqrt(Math.Pow(Item.Uncertanity, 2) + Math.Pow(CertifiedCharacteristic.U, 2));
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

        #endregion

        #region Расчёт всех показателей

        /// <summary>
        /// Метод для составления списка всех показателей для полученной выборки.
        /// </summary>
        public static List<OutputData> CalculateAllMethods (List<RegularData> Data, int IterationDigits, int ResultDigits)
        {
            List<OutputData> Result = new List<OutputData>();

            var mean            = Mean          (Data, IterationDigits, ResultDigits);
            var weightedMean    = WeightedMean  (Data, IterationDigits, ResultDigits);
            var median          = Median        (Data, IterationDigits, ResultDigits);
            var derSimonian     = DerSimonian   (Data, weightedMean.X, IterationDigits, ResultDigits);
            //var mandelPaule     = MandelPaule   (Data, IterationDigits, ResultDigits);
            var huber           = Huber         (Data, IterationDigits, ResultDigits);
            var gost            = GOST          (Data, IterationDigits, ResultDigits);
            var a               = A             (Data, IterationDigits, ResultDigits);

            Result.Add(mean);
            Result.Add(weightedMean);
            Result.Add(median);
            Result.Add(derSimonian);
            //Result.Add(mandelPaule);
            Result.Add(huber);
            Result.Add(gost);
            Result.Add(a);

            return Result;
        }

        #endregion
    }
}