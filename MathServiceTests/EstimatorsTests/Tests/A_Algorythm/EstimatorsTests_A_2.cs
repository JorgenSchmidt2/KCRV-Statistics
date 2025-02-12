using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.A_Algorythm
{
    [TestClass]
    public class EstimatorsTests_A_2
    {
        
        private int ResDigits = 8;

        [TestMethod]
        public void A_X_test_1_DEF()
        {
            double Result = Estimators.A(Estimators_Data_2.Data, 15, ResDigits).X;
            Assert.AreEqual(Math.Round(39.0484633046, ResDigits), Result);
        }

        [TestMethod]
        public void A_Y_test_2_DEF()
        {
            double Result = Estimators.A(Estimators_Data_2.Data, 15, ResDigits).X;
            Assert.AreEqual(Math.Round(1.2655890831, ResDigits), Result);
        }
    }
}