using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.Mean
{
    /// <summary>
    /// Первая тестовая выборка. Имеется 15 лабораторий, для которых уже рассчитаны необходимые значения, 
    /// требуется сравнить результаты работы алгоритмов с истинными результатами
    /// </summary>
    [TestClass]
    public class EstimatorsTests_Mean_1
    {
        [TestMethod]
        public void Mean_X_test_1_DEF()
        {
            double result = Estimators.Mean(Data_1.TestData, 0, 0).X;
            Assert.AreEqual(9.9616, result);
        }
        [TestMethod]
        public void Mean_U_test_2_DEF()
        {
            double result = Estimators.Mean(Data_1.TestData, 0, 0).U;
            Assert.AreEqual(0.1565, result);
        }

        private int mean_iteration_calibr = 14;
        private int mean_result_calibr = 14;

        [TestMethod]
        public void Mean_X_test_3_Calibration()
        {
            double resultcalibr = 9.96158898572207;
            double result = Estimators.Mean(Data_1.TestData, mean_iteration_calibr, mean_result_calibr).X;

            Assert.AreEqual(Math.Round(resultcalibr, mean_result_calibr),
                            result);
        }
        [TestMethod]
        public void Mean_U_test_4_Calibration()
        {
            double resultcalibr = 0.156494754481712;
            double result = Estimators.Mean(Data_1.TestData, mean_iteration_calibr, mean_result_calibr).U;

            Assert.AreEqual(Math.Round(resultcalibr, mean_result_calibr),
                            result);
        }

    }
}