using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.MessageService.MessageClasses;

namespace KCRV_Statistics.Model.GraphicsShell
{
    public class GraphicsSketchers
    {
        /// <summary>
        /// Задаёт координаты для подписей. По оси Х откладываются номера лабораторий, по оси Y от минимального до максимального значений.
        /// Количество подписей 
        /// </summary>
        public static List<TextLabelEntity> GetLabels(double value_min, double value_max, double LaboratoriesCount)
        {
            try
            {
                var Result = new List<TextLabelEntity>();

                int XFieldInitPoints = (GraphicsShellConfiguration.ExternalCanvasWidth - GraphicsShellConfiguration.InternalCanvasWidth) / 2;
                int YFieldInitPoints = (GraphicsShellConfiguration.ExternalCanvasHeight - GraphicsShellConfiguration.InternalCanvasHeight) / 2;

                if (LaboratoriesCount > GraphicsShellConfiguration.MaxXLabelCount)
                {
                    var xStep = Convert.ToInt32(
                        Math.Round(LaboratoriesCount / GraphicsShellConfiguration.MaxXLabelCount)
                    );

                }
                else
                {
                    var kx = (GraphicsShellConfiguration.InternalCanvasWidth)
                        / Math.Sqrt(Math.Pow(LaboratoriesCount - 1, 2));

                    for (double x = 0; x <= LaboratoriesCount - 1; x++)
                    {
                        var XCoord = Convert.ToInt32(x * kx);

                        Result.Add(
                            new TextLabelEntity
                            {
                                X = XCoord + XFieldInitPoints + 30 - GraphicsShellConfiguration.PointRadius / 2,
                                Y = YFieldInitPoints / 4,
                                Content = (x + 1).ToString()
                            }
                        );
                    }
                }

                var yStep = Math.Sqrt(
                        Math.Pow(value_max - value_min, 2)
                    )
                    / GraphicsShellConfiguration.YLabelCount;

                var ky = (GraphicsShellConfiguration.InternalCanvasHeight)
                    / Math.Sqrt(Math.Pow(value_max - value_min, 2));

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

        public static List<SphereGraphicsEntity> GetSpheres(List<RegularData> Data)
        {
            var Result = new List<SphereGraphicsEntity>();

            foreach (var item in Data)
            {
                Result.Add(
                    new SphereGraphicsEntity
                    {
                        X = item.LaboratoryNumber,
                        Y = item.Value
                    }
                );
            }

            return Result;
        }


    }
}