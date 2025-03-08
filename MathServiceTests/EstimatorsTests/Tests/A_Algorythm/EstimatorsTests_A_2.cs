using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.A_Algorythm
{
    [TestClass]
    public class EstimatorsTests_A_2
    {
        
        private int ResDigits = 2;

        [TestMethod]
        public void A_X_test_1_DEF()
        {
            var Result = Estimators.A(Estimators_Data_2.Data, 15, ResDigits);
            Assert.AreEqual(Math.Round(39.0484633046, ResDigits), Math.Round(Result.X, ResDigits));
        }

        [TestMethod]
        public void A_U_test_2_DEF()
        {
            var Result = Estimators.A(Estimators_Data_2.Data, 15, ResDigits);
            Assert.AreEqual(Math.Round(1.2655890831, ResDigits), Math.Round(Result.U, ResDigits));
        }
    }
}