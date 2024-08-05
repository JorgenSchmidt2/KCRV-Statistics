using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.WeightedMean
{
    /// <summary>
    /// Первая тестовая выборка. Имеется 15 лабораторий, для которых уже рассчитаны необходимые значения, 
    /// требуется сравнить результаты работы алгоритмов с истинными результатами
    /// </summary>
    [TestClass]
    public class EstimatorsTests_WeightedMean_1
    {
        [TestMethod]
        public void WeightedMean_X_test_1_DEF()
        {
            double result = Estimators.WeightedMean(Estimators_Data_1.TestData, 0, 0).X;
            Assert.AreEqual(10.1922, result);
        }
        [TestMethod]
        public void WeightedMean_U_test_2_DEF()
        {
            double result = Estimators.WeightedMean(Estimators_Data_1.TestData, 0, 0).U;
            Assert.AreEqual(0.064, result);
        }

        private int weighted_mean_iteration_calibr = 13;
        private int weighted_mean_result_calibr = 13;

        [TestMethod]
        public void Mean_X_test_3_Calibration()
        {
            double resultcalibr = 10.1921646106683;
            double result = Estimators.WeightedMean(Estimators_Data_1.TestData, weighted_mean_iteration_calibr, weighted_mean_result_calibr).X;

            Assert.AreEqual(Math.Round(resultcalibr, weighted_mean_result_calibr),
                            result);
        }
        [TestMethod]
        public void Mean_U_test_4_Calibration()
        {
            double resultcalibr = 0.0640207785559905;
            double result = Estimators.WeightedMean(Estimators_Data_1.TestData, weighted_mean_iteration_calibr, weighted_mean_result_calibr).U;

            Assert.AreEqual(Math.Round(resultcalibr, weighted_mean_result_calibr),
                            result);
        }
    }
}