using KCRV_Statistics.Model.ValidateService.SimpleFileCheckers;
using ValidateServiceTests.SimpleFileCheckers.Data;

namespace ValidateServiceTests.SimpleFileCheckers.Tests
{
    [TestClass]
    public class InterlabDataSimpleChecker_Tests_1
    {
        [TestMethod]
        public void CheckData_Test_1_Int_Corr () 
        {
            var Result = InterlabDataSimpleChecker.CheckSimpleData(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_1_Int_Corr_Clean
                ).Status;
            Assert.AreEqual(true, Result);
        }

        [TestMethod]
        public void CheckData_Test_2_Int_Corr ()
        {
            var Result = InterlabDataSimpleChecker.CheckSimpleData(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_2_Int_Corr_NotClean
                ).Status;
            Assert.AreEqual(true, Result);
        }

        [TestMethod]
        public void CheckData_Test_3_Int_Incorr()
        {
            var Result = InterlabDataSimpleChecker.CheckSimpleData(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_3_Int_Incorr
                ).Status;
            Assert.AreEqual(false, Result);
        }

        [TestMethod]
        public void CheckData_Test_4_Double_Corr()
        {
            var Result = InterlabDataSimpleChecker.CheckSimpleData(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_4_Double_Corr_Clean
                ).Status;
            Assert.AreEqual(
                true, Result
            );
        }

        [TestMethod]
        public void CheckData_Test_5_Double_Incorr()
        {
            var Result = InterlabDataSimpleChecker.CheckSimpleData(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_5_Double_Incorr
                ).Status;
            Assert.AreEqual(false, Result);
        }
    }
}