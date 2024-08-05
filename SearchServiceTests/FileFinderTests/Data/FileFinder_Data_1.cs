using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace SearchServiceTests.FileFinderTests.Data
{
    public class FileFinder_Data_1
    {
        public static List<EFileData> TestData = new List<EFileData>()
        {
            new EFileData()
            {
                ID = 1,
                Directory = "",
                FileName= "abcdef",
            },
            new EFileData()
            {
                ID = 2,
                Directory = "",
                FileName= "bcdefg",
            },
            new EFileData()
            {
                ID = 3,
                Directory = "",
                FileName= "cdefgh",
            },
            new EFileData()
            {
                ID = 4,
                Directory = "",
                FileName= "defghi",
            },
            new EFileData()
            {
                ID = 5,
                Directory = "",
                FileName= "efghij",
            },
            new EFileData()
            {
                ID = 6,
                Directory = "",
                FileName= "fghijk",
            },
            new EFileData()
            {
                ID = 7,
                Directory = "",
                FileName= "ghijkl",
            },
        };

        public static List<EFileData> Ethalon_DoQueryTest_DEF = new List<EFileData>()
        {
            new EFileData()
            {
                ID = 1,
                Directory = "",
                FileName= "abcdef",
            },
            new EFileData()
            {
                ID = 2,
                Directory = "",
                FileName= "bcdefg",
            },
            new EFileData()
            {
                ID = 3,
                Directory = "",
                FileName= "cdefgh",
            },
            new EFileData()
            {
                ID = 4,
                Directory = "",
                FileName= "defghi",
            },
        };

        public static List<EFileData> Ethalon_DoQueryTest_GHI = new List<EFileData>
        {
            new EFileData()
            {
                ID = 1,
                Directory = "",
                FileName= "defghi",
            },
            new EFileData()
            {
                ID = 2,
                Directory = "",
                FileName= "efghij",
            },
            new EFileData()
            {
                ID = 3,
                Directory = "",
                FileName= "fghijk",
            },
            new EFileData()
            {
                ID = 4,
                Directory = "",
                FileName= "ghijkl" 
            }
        };

        public static List<EFileData> Ethalon_DoQueryTest_FGH = new List<EFileData>
        {
            new EFileData()
            {
                ID = 1,
                Directory = "",
                FileName= "cdefgh",
            },
            new EFileData()
            {
                ID = 2,
                Directory = "",
                FileName= "defghi",
            },
            new EFileData()
            {
                ID = 3,
                Directory = "",
                FileName= "efghij",
            },
            new EFileData()
            {
                ID = 4,
                Directory = "",
                FileName= "fghijk",
            },
        };
    }
}