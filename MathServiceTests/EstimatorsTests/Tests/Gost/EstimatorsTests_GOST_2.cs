using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.Gost
{
    [TestClass]
    public class EstimatorsTests_GOST_2
    {
        private int ResDigits = 8;

        [TestMethod]
        public void GOST_X_test_1_DEF()
        {
            double Result = Estimators.GOST(Estimators_Data_2.Data, 15, ResDigits).X;
            Assert.AreEqual(Math.Round(40.0356911641, ResDigits), Result);
        }

        [TestMethod]
        public void GOST_U_test_2_DEF()
        {
            double Result = Estimators.GOST(Estimators_Data_2.Data, 15, ResDigits).U;
            Assert.AreEqual(Math.Round(0.8627018073, ResDigits), Result);
        }
    }
}