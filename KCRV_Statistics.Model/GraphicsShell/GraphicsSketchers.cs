using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.MessageService.MessageClasses;

namespace KCRV_Statistics.Model.GraphicsShell
{
    public class GraphicsSketchers
    {
        #region Основные скетчеры

        /// <summary>
        /// Задаёт координаты для подписей. По оси Х откладываются номера лабораторий, по оси Y от минимального до максимального значений.
        /// </summary>
        public static List<TextLabelEntity> GetLabels(double value_min, double value_max, double LaboratoriesCount)
        {
            try
            {
                var Result = new List<TextLabelEntity>();

                // Вычисление начальных точек расположения подписей на внешнем поле отрисовки
                int XFieldInitPoints = (GraphicsShellConfiguration.ExternalCanvasWidth - GraphicsShellConfiguration.InternalCanvasWidth) / 2;
                int YFieldInitPoints = (GraphicsShellConfiguration.ExternalCanvasHeight - GraphicsShellConfiguration.InternalCanvasHeight) / 2;

                // Определение "цены" деления на координатной плоскости КАНВАСА под имеющиеся данные координаты
                var kx = (GraphicsShellConfiguration.InternalCanvasWidth)
                    / Math.Sqrt(Math.Pow(LaboratoriesCount - 1, 2));

                // Шаг по оси Х внешнего канваса
                var xStep = Convert.ToInt32(
                    Math.Floor(LaboratoriesCount / GraphicsShellConfiguration.MaxXLabelCount) + 1
                );

                // Определение координат подписей номеров лабораторий на канвасе
                for (double x = 0; x <= LaboratoriesCount - 1; x += xStep)
                {
                    // Текущая координата Х
                    var XCoord = Convert.ToInt32(x * kx);

                    Result.Add(
                        new TextLabelEntity
                        {
                            X = XCoord + XFieldInitPoints + GraphicsShellConfiguration.LabelStartCoordinate
                                - GraphicsShellConfiguration.PointRadius / 2,
                            Y = YFieldInitPoints / 4,
                            Content = (x + 1).ToString()
                        }
                    );
                }


                // Определение шага по Y, по полученным данным
                var yStep = Math.Sqrt(
                        Math.Pow(value_max - value_min, 2)
                    )
                    / GraphicsShellConfiguration.YLabelCount;

                // Определение цены деления на КАНВАСЕ для подписей значений
                var ky = (GraphicsShellConfiguration.InternalCanvasHeight)
                    / Math.Sqrt(Math.Pow(value_max - value_min, 2));

                // Определение координат подписей значений по каждой из лабораторий на канвасе
                for (double y = value_min; y <= value_max; y += Math.Round(yStep, 6))
                {
                    var YCoord = Convert.ToInt32((y - value_min) * ky);

                    Result.Add(
                        new TextLabelEntity
                        {
                            X = XFieldInitPoints / 4,
                            Y = YCoord + YFieldInitPoints,
                            Content = Math.Round(y, 2).ToString()
                        }
                    );
                }

                return Result;
            }
            catch (Exception ex)
            {
                MessageObjects.Sender.SendMessage("Ошибка: \n" + ex.Message);
                return new List<TextLabelEntity>();
            }
        }

        /// <summary>
        /// Задаёт положение точек на координатной плоскости
        /// </summary>
        public static List<PointGraphicsEntity> GetPoints(List<RegularData> Data)
        {
            var Result = new List<PointGraphicsEntity>();

            // Задание координат точек
            foreach (var item in Data)
            {
                Result.Add(
                    new PointGraphicsEntity
                    {
                        X = item.LaboratoryNumber,
                        Y = item.Value
                    }
                );
            }

            return Result;
        }

        #endregion
    }
}