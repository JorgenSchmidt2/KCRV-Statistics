using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Model.SearchService.FileFinders;
using SearchServiceTests.FileFinderTests.Data;
using SearchServiceTests.FileFinderTests.Methods;

namespace SearchServiceTests.FileFinderTests.Tests
{
    [TestClass]
    public class FileQuery_Tests_1
    {
        [TestMethod]
        public void FileFinder_DoQuery_Test_1 ()
        {
            Assert.AreEqual(
                true,
                FileFinderTests_Methods.FileListsAreEquals_ByFileName(
                    FileFinder_Data_1.Ethalon_DoQueryTest_DEF,
                    FileQueryMaker.DoQuery("def", FileFinder_Data_1.TestData)
                )
            );
        }

        [TestMethod]
        public void FileFinder_DoQuery_Test_2()
        {
            Assert.AreEqual(
                true,
                FileFinderTests_Methods.FileListsAreEquals_ByFileName(
                    FileFinder_Data_1.Ethalon_DoQueryTest_FGH,
                    FileQueryMaker.DoQuery("fgh", FileFinder_Data_1.TestData)
                )
            );
        }

        [TestMethod]
        public void FileFinder_DoQuery_Test_3()
        {
            Assert.AreEqual(
                true,
                FileFinderTests_Methods.FileListsAreEquals_ByFileName (

                    FileFinder_Data_1.Ethalon_DoQueryTest_GHI,
                    FileQueryMaker.DoQuery("ghi", FileFinder_Data_1.TestData)
                 )
            );
        }
    }
}