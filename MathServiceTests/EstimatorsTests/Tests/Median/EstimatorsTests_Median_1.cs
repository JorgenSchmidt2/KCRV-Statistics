using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.Median
{
    /// <summary>
    /// Первая тестовая выборка. Имеется 15 лабораторий, для которых уже рассчитаны необходимые значения, 
    /// требуется сравнить результаты работы алгоритмов с истинными результатами
    /// </summary>
    [TestClass]
    public class EstimatorsTests_Median_1
    {
        [TestMethod]
        public void WeightedMean_X_test_1_DEF()
        {
            double result = Estimators.Median(Data_1.TestData, 0, 0).X;
            Assert.AreEqual(9.8147, result);
        }
        [TestMethod]
        public void WeightedMean_U_test_2_DEF()
        {
            double result = Estimators.Median(Data_1.TestData, 0, 0).U;
            Assert.AreEqual(0.2056, result);
        }
    }
}