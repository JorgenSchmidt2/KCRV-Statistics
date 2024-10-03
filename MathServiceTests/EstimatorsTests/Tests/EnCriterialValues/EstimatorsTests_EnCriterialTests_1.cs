using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Model.DataOperatorsService.Lists;
using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;
using MathServiceTests.EstimatorsTests.TestEntites;

namespace MathServiceTests.EstimatorsTests.Tests.EnCriterialValues
{
    [TestClass]
    public class EstimatorsTests_EnCriterialTests_1
    {
        [TestMethod]
        public void EnCriterial_test_1_DEF()
        {
            int resdigits = 8;

            List<RegularData> actuallyData = ListOperators.CopyRegularDataListEntities(Estimators_Data_1.TestData);
            List<OutputData> characteristics = Estimators.CalculateAllMethods(ref actuallyData, 15, resdigits);

            List<CheckData<double>> checkDatas = new List<CheckData<double>>();
            for (var i = 0; i < actuallyData.Count; i++)
            {
                if (actuallyData[i].E == Math.Round(Estimators_Data_1.TestData[i].E, resdigits))
                {
                    checkDatas.Add(
                        new CheckData<double>
                        { 
                            IsCorrect = true,
                            TestValue = Math.Round(Estimators_Data_1.TestData[i].E, resdigits),
                            ActuallyValue = actuallyData[i].E
                        }
                    );
                }
                else
                {
                    checkDatas.Add(
                        new CheckData<double>
                        {
                            IsCorrect = false,
                            TestValue = Math.Round(Estimators_Data_1.TestData[i].E, resdigits),
                            ActuallyValue = actuallyData[i].E
                        }
                    );
                }
            }

            var CorrectValues = checkDatas.Where(x => x.IsCorrect).Select(x => x).ToList();
            var IncorrectValues = checkDatas.Where(x => !x.IsCorrect).Select(x => x).ToList();

            Assert.AreEqual(true, IncorrectValues.Count() == 0);
        }
    }
}