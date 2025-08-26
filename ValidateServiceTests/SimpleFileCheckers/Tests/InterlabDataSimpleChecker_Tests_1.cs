using KCRV_Statistics.Model.ValidateService.FileCheckers;
using ValidateServiceTests.SimpleFileCheckers.Data;

namespace ValidateServiceTests.SimpleFileCheckers.Tests
{
    [TestClass]
    public class InterlabDataSimpleChecker_Tests_1
    {
        [TestMethod]
        public void CheckData_Test_1_Int_Corr () 
        {
            var Result = SimpleFileDataValidator.IsTwoColumnsFormat(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_1_Int_Corr_Clean
                ).Status;
            Assert.AreEqual(true, Result);
        }

        [TestMethod]
        public void CheckData_Test_2_Int_Corr ()
        {
            var Result = SimpleFileDataValidator.IsTwoColumnsFormat(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_2_Int_Corr_NotClean
                ).Status;
            Assert.AreEqual(true, Result);
        }

        [TestMethod]
        public void CheckData_Test_3_Int_Incorr()
        {
            var Result = SimpleFileDataValidator.IsTwoColumnsFormat(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_3_Int_Incorr
                ).Status;
            Assert.AreEqual(false, Result);
        }

        [TestMethod]
        public void CheckData_Test_4_Double_Corr()
        {
            var Result = SimpleFileDataValidator.IsTwoColumnsFormat(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_4_Double_Corr_Clean
                ).Status;
            Assert.AreEqual(
                true, Result
            );
        }

        [TestMethod]
        public void CheckData_Test_5_Double_Incorr()
        {
            var Result = SimpleFileDataValidator.IsTwoColumnsFormat(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_5_Double_Incorr
                ).Status;
            Assert.AreEqual(false, Result);
        }

        [TestMethod]
        public void CheckData_Test_6_FirstColIsTemporary()
        {
            var Result = SimpleFileDataValidator.IsStrictlyIncreasingFirstColumn(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_6_FirstColIsTemporary
                ).Status;
            Assert.AreEqual(true, Result);
        }

        [TestMethod]
        public void CheckData_Test_7_FirstColIsntTemporary()
        {
            var Result = SimpleFileDataValidator.IsStrictlyIncreasingFirstColumn(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_7_FirstColIsntTemporary
                ).Status;
            Assert.AreEqual(false, Result);
        }

        [TestMethod]
        public void CheckData_Test_8_NearDataIsEquals_FirstColumn()
        {
            var Result = SimpleFileDataValidator.IsStrictlyIncreasingFirstColumn(
                    InterlabDataSimpleChecker_Data_1.CheckData_Test_8_NearDataIsEquals_FirstColumn
                ).Status;
            Assert.AreEqual(false, Result);
        }

    }
}