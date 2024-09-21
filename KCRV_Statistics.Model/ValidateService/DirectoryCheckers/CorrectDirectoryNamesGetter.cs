using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using System.Windows;

namespace KCRV_Statistics.Model.ValidateService.DirectoryCheckers
{
    public class CorrectDirectoryNamesGetter
    {
        public static List<ViewedDirectoryData> GetCorrectDirectories (string[] DirectoryList)
        {
            List<ViewedDirectoryData> Result = new List<ViewedDirectoryData>();
            var counter = 1;

            foreach (var Item in DirectoryList)
            {
                if (!(Item.Contains('<') || Item.Contains('>') || Item.Contains(':') ||
                        Item.Contains('\"') || Item.Contains('\\') || Item.Contains('/') ||
                        Item.Contains('|') || Item.Contains('?') || Item.Contains('*') ||
                        Item.Contains('\t')
                    ))
                {
                    if (counter == 1)
                    {
                        Result.Add(
                            new ViewedDirectoryData()
                            {
                                DirectoryName = Item,
                                IsChoised = true,
                            }
                        );
                    }
                    else
                    {
                        Result.Add(
                            new ViewedDirectoryData()
                            {
                                DirectoryName = Item,
                                IsChoised = false,
                            }
                        );
                    }
                }
                else
                {
                    MessageBox.Show("Директория " + Item + ", указанная в конфигурационном файле, содержит один из запрещённых символов:" +
                        "\n <, >, :, \", \\, /, |, ?, *, табуляция (tab) ");
                }
                counter++;
            }

            return Result;
        }
    }
}