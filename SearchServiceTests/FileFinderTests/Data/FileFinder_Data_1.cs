using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace SearchServiceTests.FileFinderTests.Data
{
    public class FileFinder_Data_1
    {
        public static List<FileDataEntity> TestData = new List<FileDataEntity>()
        {
            new FileDataEntity()
            {
                ID = 1,
                Directory = "",
                FileName= "abcdef",
            },
            new FileDataEntity()
            {
                ID = 2,
                Directory = "",
                FileName= "bcdefg",
            },
            new FileDataEntity()
            {
                ID = 3,
                Directory = "",
                FileName= "cdefgh",
            },
            new FileDataEntity()
            {
                ID = 4,
                Directory = "",
                FileName= "defghi",
            },
            new FileDataEntity()
            {
                ID = 5,
                Directory = "",
                FileName= "efghij",
            },
            new FileDataEntity()
            {
                ID = 6,
                Directory = "",
                FileName= "fghijk",
            },
            new FileDataEntity()
            {
                ID = 7,
                Directory = "",
                FileName= "ghijkl",
            },
        };

        public static List<FileDataEntity> Ethalon_DoQueryTest_DEF = new List<FileDataEntity>()
        {
            new FileDataEntity()
            {
                ID = 1,
                Directory = "",
                FileName= "abcdef",
            },
            new FileDataEntity()
            {
                ID = 2,
                Directory = "",
                FileName= "bcdefg",
            },
            new FileDataEntity()
            {
                ID = 3,
                Directory = "",
                FileName= "cdefgh",
            },
            new FileDataEntity()
            {
                ID = 4,
                Directory = "",
                FileName= "defghi",
            },
        };

        public static List<FileDataEntity> Ethalon_DoQueryTest_GHI = new List<FileDataEntity>
        {
            new FileDataEntity()
            {
                ID = 1,
                Directory = "",
                FileName= "defghi",
            },
            new FileDataEntity()
            {
                ID = 2,
                Directory = "",
                FileName= "efghij",
            },
            new FileDataEntity()
            {
                ID = 3,
                Directory = "",
                FileName= "fghijk",
            },
            new FileDataEntity()
            {
                ID = 4,
                Directory = "",
                FileName= "ghijkl" 
            }
        };

        public static List<FileDataEntity> Ethalon_DoQueryTest_FGH = new List<FileDataEntity>
        {
            new FileDataEntity()
            {
                ID = 1,
                Directory = "",
                FileName= "cdefgh",
            },
            new FileDataEntity()
            {
                ID = 2,
                Directory = "",
                FileName= "defghi",
            },
            new FileDataEntity()
            {
                ID = 3,
                Directory = "",
                FileName= "efghij",
            },
            new FileDataEntity()
            {
                ID = 4,
                Directory = "",
                FileName= "fghijk",
            },
        };
    }
}