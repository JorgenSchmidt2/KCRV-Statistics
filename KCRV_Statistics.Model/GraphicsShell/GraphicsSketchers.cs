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
            catch (Exception e)
            {
                MessageObjects.Sender.SendMessage("Ошибка при попытке получить данные для отрисовки подписей: \n" + e.Message);
                return new List<TextLabelEntity>();
            }
        }

        /// <summary>
        /// Задаёт положение точек на координатной плоскости (не производит вычисление координат непосредственно на канвасе).
        /// </summary>
        public static List<PointGraphicsEntity> GetPoints(List<RegularData> Data)
        {
            try
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
            catch (Exception e)
            {
                MessageObjects.Sender.SendMessage("Ошибка при попытке получить данные для отрисовки точек: \n" + e.Message);
                return new List<PointGraphicsEntity>();
            }
        }

        /// <summary>
        /// Даёт данные для отрисовки линий, которые обозначают на графике неопределённость результатов измерений лабораторий.
        /// </summary>
        public static List<LineGraphicsEntity> GetUncertanityLines(List<RegularData> regdata)
        {
            try
            {
                List<LineGraphicsEntity> Result = new List<LineGraphicsEntity>();

                // Определение минимального, максимального значений по оси Y
                var min = regdata.Min(x => x.Value - x.Uncertanity);
                var max = regdata.Max(x => x.Value + x.Uncertanity);

                // Определение цены одного деления на отображаемой оси
                var kx = (GraphicsShellConfiguration.InternalCanvasWidth)
                    / Math.Sqrt(Math.Pow(regdata.Count - 1, 2));
                var ky = (GraphicsShellConfiguration.InternalCanvasHeight)
                    / Math.Sqrt(Math.Pow(max - min, 2));

                foreach (var Item in regdata)
                {
                    // Определяем положение линии на оси Х, исходя из номера лабораторий
                    // (не забываем, что по формуле расчёта координат нужно обязательно искать разность между
                    // максимальным и минимальным значениями по осям координат, и да пускай то, что под таковыми
                    // имеюстя ввиду номера лабораторий не вводит в заблуждение :D )
                    var X = Convert.ToInt32((Item.LaboratoryNumber - 1) * kx) + GraphicsShellConfiguration.PointRadius / 2;

                    // Определяем положение линии на оси Y, исходя из значений результата лаборатории и его неопределённости
                    var Y1_Coord = Item.Value - Item.Uncertanity;
                    var Y2_Coord = Item.Value + Item.Uncertanity;

                    var Y1 = GraphicsShellConfiguration.InternalCanvasWidth - Convert.ToInt32((Y1_Coord - min) * ky)
                        + GraphicsShellConfiguration.PointRadius / 2;
                    var Y2 = GraphicsShellConfiguration.InternalCanvasWidth - Convert.ToInt32((Y2_Coord - min) * ky)
                        + GraphicsShellConfiguration.PointRadius / 2;

                    // Добавляем полученное в результирующий список
                    Result.Add(
                        new LineGraphicsEntity
                        {
                            X1 = X,
                            Y1 = Y1,
                            X2 = X,
                            Y2 = Y2,
                            Color = GraphicsShellConfiguration.LabUncertanityLinesColor,
                            StrokeThicknessValue = GraphicsShellConfiguration.LabUncertanityLinesThickness
                        }
                    );
                }

                return Result;
            }
            catch (Exception e)
            {
                MessageObjects.Sender.SendMessage("Ошибка при попытке получить данные для отрисовки линий "
                    + "для неопределённости результатов лабораторий: \n" 
                    + e.Message);
                return new List<LineGraphicsEntity>();
            }
        }

        /// <summary>
        /// Даёт координаты для отрисовки интервала показателя KCRV. Входной объект (obj) - результаты применения одного из методов KCRV,
        /// regdata - для определения минимального, максимального значений по осям.
        /// </summary>
        public static List<LineGraphicsEntity> GetKCRV_Lines (OutputData obj, List<RegularData> regdata)
        {
            try
            {
                List<LineGraphicsEntity> Result = new List<LineGraphicsEntity>();

                // Определение минимального, максимального значений по оси Y
                var min = regdata.Min(x => x.Value - x.Uncertanity);
                var max = regdata.Max(x => x.Value + x.Uncertanity);

                // Определение цены одного деления на отображаемой оси
                var ky = (GraphicsShellConfiguration.InternalCanvasHeight)
                    / Math.Sqrt(Math.Pow(max - min, 2));

                // Задаёт положение линии регрессии на оси данных
                var Y_Reg = GraphicsShellConfiguration.InternalCanvasWidth - Convert.ToInt32((obj.X - min) * ky);
                Result.Add(
                    new LineGraphicsEntity()
                    {
                        X1 = 1,
                        Y1 = Y_Reg,
                        X2 = GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius,
                        Y2 = Y_Reg,
                        Color = GraphicsShellConfiguration.RegressionLinesColor,
                        StrokeThicknessValue = GraphicsShellConfiguration.RegressionLinesThickness
                    }
                );

                // Задаёт положение нижней части доверительного интервала
                var Y_MinTrust = GraphicsShellConfiguration.InternalCanvasWidth - Convert.ToInt32((obj.X - obj.U - min) * ky);
                Result.Add(
                    new LineGraphicsEntity()
                    {
                        X1 = 1,
                        Y1 = Y_MinTrust,
                        X2 = GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius,
                        Y2 = Y_MinTrust,
                        Color = GraphicsShellConfiguration.TrustLinesColor,
                        StrokeThicknessValue = GraphicsShellConfiguration.TrustLinesThickness
                    }
                );

                // Задаёт положение верхней части доверительного интервала
                var Y_MaxTrust = GraphicsShellConfiguration.InternalCanvasWidth - Convert.ToInt32((obj.X + obj.U - min) * ky);
                Result.Add(
                    new LineGraphicsEntity()
                    {
                        X1 = 1,
                        Y1 = Y_MaxTrust,
                        X2 = GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius,
                        Y2 = Y_MaxTrust,
                        Color = GraphicsShellConfiguration.TrustLinesColor,
                        StrokeThicknessValue = GraphicsShellConfiguration.TrustLinesThickness
                    }
                );

                return Result;
            }
            catch (Exception e)
            {
                MessageObjects.Sender.SendMessage("Ошибка при попытке получить данные для отрисовки линий для показателей KCRV: \n" + e.Message);
                return new List<LineGraphicsEntity>();
            }
        }

        #endregion

    }
}