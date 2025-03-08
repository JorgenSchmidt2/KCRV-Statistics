using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.MandelPaule
{
    /// <summary>
    /// Первая тестовая выборка. Имеется 15 лабораторий, для которых уже рассчитаны необходимые значения, 
    /// требуется сравнить результаты работы алгоритмов с истинными результатами
    /// </summary>
    [TestClass]
    public class EstimatorsTests_MandelPaule_1
    {
        private int ResDigits = 5;

        [TestMethod]
        public void MandelPaule_X_test_1_DEF()
        {
            double result = Estimators.MandelPaule(Estimators_Data_1.TestData, 15, ResDigits).X;
            Assert.AreEqual(Math.Round(10.017421554264, ResDigits), Math.Round(result, ResDigits));
        }
        [TestMethod]
        public void MandelPaule_U_test_2_DEF()
        {
            double result = Estimators.MandelPaule(Estimators_Data_1.TestData, 15, ResDigits).U;
            Assert.AreEqual(Math.Round(0.12008449913461, ResDigits), Math.Round(result, ResDigits));
        }
        [TestMethod]
        public void MandelPaule_λ_test_3_DEF()
        {
            double result = Estimators.MandelPaule(Estimators_Data_1.TestData, 15, ResDigits).InterLabVariance;
            Assert.AreEqual(Math.Round(0.350737906431726, ResDigits), Math.Round(result, ResDigits));
        }
    }
}