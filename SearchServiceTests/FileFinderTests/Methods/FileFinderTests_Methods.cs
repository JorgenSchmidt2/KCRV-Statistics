using KCRV_Statistics.Core.Entities.FileSystemEntites;

namespace SearchServiceTests.FileFinderTests.Methods
{
    public class FileFinderTests_Methods
    {
        public static bool FileListsAreEquals_ByFileName (List<EFileData> Ethalon, List<EFileData> Getted)
        {
            bool Result = true;

            if (Ethalon.Count != Getted.Count)
            {
                return false;
            }

            for (int i = 0; i < Ethalon.Count; i++)
            {
                if (!Ethalon[i].FileName.Equals(Getted[i].FileName))
                {
                    return false;
                }
            }

            return Result;
        }
    }
}