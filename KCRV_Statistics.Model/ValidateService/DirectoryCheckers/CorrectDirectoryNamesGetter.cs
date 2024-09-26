using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using System.Windows;

namespace KCRV_Statistics.Model.ValidateService.DirectoryCheckers
{
    /// <summary>
    /// Содержит методы проверки на валидность списка директорий, например содержатся ли в папке запрещённые системой Windows символы
    /// </summary>
    public class CorrectDirectoryNamesGetter
    {
        /// <summary>
        /// Проверяет содержатся ли в папке запрещённые системой Windows символы
        /// </summary>
        public static List<ViewedDirectoryData> GetCorrectDirectories (string[] DirectoryList)
        {
            List<ViewedDirectoryData> Result = new List<ViewedDirectoryData>();

            // Инициализация счётчика для определения того, какая из папок будет открыта приложением при его инициализации
            var counter = 1;

            // Перебор элементов входного первичного массива элементов
            foreach (var Item in DirectoryList)
            {
                // Проверка на содержание в объекте зарезервированных системой Windows символов
                if (!(Item.Contains('<') || Item.Contains('>') || Item.Contains(':') ||
                        Item.Contains('\"') || Item.Contains('\\') || Item.Contains('/') ||
                        Item.Contains('|') || Item.Contains('?') || Item.Contains('*') ||
                        Item.Contains('\t')
                    ))
                {
                    // Определение какие элементы будут открыты в первую очередь
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