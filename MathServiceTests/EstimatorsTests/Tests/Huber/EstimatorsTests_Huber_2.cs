using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.Huber
{
    [TestClass]
    public class EstimatorsTests_Huber_2
    {
        private int ResDigits = 4;

        [TestMethod]
        public void Huber_X_test_1_DEF()
        {
            double Result = Estimators.Huber(Estimators_Data_2.Data, 15, ResDigits).X;
            Assert.AreEqual(Math.Round(39.5727272727, ResDigits), Result);
        }

        [TestMethod]
        public void Huber_U_test_2_DEF()
        {
            double Result = Estimators.Huber(Estimators_Data_2.Data, 15, ResDigits).U;
            Assert.AreEqual(Math.Round(0.7235102860, ResDigits), Result);
        }
    }
}