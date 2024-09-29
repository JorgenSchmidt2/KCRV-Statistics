using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.DerSimonian
{
    /// <summary>
    /// Первая тестовая выборка. Имеется 15 лабораторий, для которых уже рассчитаны необходимые значения, 
    /// требуется сравнить результаты работы алгоритмов с истинными результатами
    /// </summary>
    [TestClass]
    public class EstimatorsTests_DerSimonian_1
    {
        [TestMethod]
        public void DerSimonian_X_test_1_DEF()
        {
            double result = Estimators.DerSimonian(Estimators_Data_1.TestData, 10.1921646106683, 15, 4).X;
            Assert.AreEqual(Math.Round(9.987067086659160, 4), result);
        }
        [TestMethod]
        public void DerSimonian_U_test_2_DEF()
        {
            double result = Estimators.DerSimonian(Estimators_Data_1.TestData, 10.1921646106683, 15, 4).U;
            Assert.AreEqual(Math.Round(0.158489340165526, 4), result);
        }
        [TestMethod]
        public void DerSimonian_λ_test_3_DEF()
        {
            double result = Estimators.DerSimonian(Estimators_Data_1.TestData, 10.1921646106683, 15, 4).InterLabVariance;
            Assert.AreEqual(Math.Round(0.307858749037278, 4), result);
        }
    }
}