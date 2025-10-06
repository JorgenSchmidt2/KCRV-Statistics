using KCRV_Statistics.Core.Entities.DataEntities.OtherDataEntities;
using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Model.MathService;
using KCRV_Statistics.UI.AppService;
using System;
using System.Collections.Generic;
using System.Windows;

namespace KCRV_Statistics.UI.ViewModels.IntermediateWindows
{
    public class TwoColumnDataIntermediateWindowViewModel : NotifyPropertyChanged
    {
        #region Поля ввода значений округления при итерации и выводе результата

        public int iterationDigits = 0;
        /// <summary>
        /// Количество знаков после запятой при итерациях
        /// </summary>
        public int IterationDigits
        {
            get => iterationDigits;
            set
            {
                iterationDigits = value;
                CheckChanges();
            }
        }

        public int resultDigits = 0;
        /// <summary>
        /// Количество знаков после запятой для результата
        /// </summary>
        public int ResultDigits
        {
            get => resultDigits;
            set
            {
                resultDigits = value;
                CheckChanges();
            }
        }

        #endregion

        #region Поля ввода координат для xlsx

        public int coordinateDataBeginX;
        /// <summary>
        /// Указание начальной столбца данных в XLSX файле
        /// </summary>
        public int CoordinateDataBeginX
        {
            get => coordinateDataBeginX;
            set
            {
                coordinateDataBeginX = value;
                CheckChanges();
            }
        }

        public int coordinateDataBeginY;
        /// <summary>
        /// Указание начальной строки данных в XLSX файле
        /// </summary>
        public int CoordinateDataBeginY
        {
            get => coordinateDataBeginY;
            set
            {
                coordinateDataBeginY = value;
                CheckChanges();
            }
        }

        #endregion

        #region Кнопки управления

        /// <summary>
        /// Для кнопки расчёта значений аттестуемой характеристики
        /// </summary>
        public Command Calculate
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        try
                        {
                            // Прочие операции проверки
                            if (IsShouldBeOpenedAsILC && IsShouldBeOpenedAsStabFile)
                            {
                                MessageBox.Show("Может быть открыто только одно окно типа файла.");
                                return;
                            }

                            // Проверка списка на наличие в нём элементов
                            if (AppData.CurrentData.Count == 0)
                            {
                                MessageBox.Show("Не удалось загрузить данные из файла.");
                                return;
                            }

                            if (IterationDigits > 15 || ResultDigits > 15)
                            {
                                var Message =     "Максимально-возможное значение цифр после запятой равно 15. "
                                                + "Введите число меньшее или равное 15-ти.";

                                MessageBox.Show(Message);
                                return;
                            }

                            if (IterationDigits < 0 || ResultDigits < 0)
                            {
                                var Message = "Количество знаков после запятой может быть только положительным числом.";
                                MessageBox.Show(Message);
                                return;
                            }

                            if (IsShouldBeOpenedAsILC)
                                OpenCLIWindow();

                            if (isShouldBeOpenedAsStabFile)
                                OpenStabFile();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Возникла неустранимая ошибка: \n" + e.Message);
                        }
                    }
                );
            }
        }

        public void OpenCLIWindow ()
        {
            // Расчёт значений, присвоение результатов вычислений статическому полю 
            AppData.COX_Datas = new List<CoxResultsEntity>();
            var Result = Estimators.CalculateAllMethods(AppData.CurrentData, out AppData.COX_Datas, IterationDigits, ResultDigits);
            AppData.OutputData.Clear();
            AppData.OutputData = Result;

            // Открытие окна
            WindowsObjects.InterlabMasterWindow = new();
            if (WindowsObjects.InterlabMasterWindow.ShowDialog() == true)
            {
                WindowsObjects.InterlabMasterWindow.Show();
            }
        }

        public void OpenStabFile()
        {
            MessageBox.Show("Hadn't implement");
        }

        #region Кнопки получения информации об окне

        public Command Help
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var Content = "";
                        MessageBox.Show(Content);
                    }
                );
            }
        }

        public Command Close
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        AppData.CurrentData.Clear();
                        WindowsObjects.OpenInterlabIntermediateWindow.Close();
                        WindowsObjects.OpenInterlabIntermediateWindow = null;
                    }
                );
            }
        }

        #endregion

        #endregion

        #region Чекбоксы для выбора представления данных

        public bool isShouldBeOpenedAsILC = true;
        /// <summary>
        /// Определяет должно ли быть открыто окно расчёта значений для МСИ
        /// </summary>
        public bool IsShouldBeOpenedAsILC
        {
            get => isShouldBeOpenedAsILC;
            set
            {
                if (!ChangeOpenDataVarianceInProcess && value != false)
                {
                    ChangeOpenDataVarianceInProcess = true;
                    CheckChoises(value, !value);
                }
                CheckChanges();
            }
        }

        public bool isShouldBeOpenedAsStabFile = false;
        /// <summary>
        /// Определяет должно ли быть открыто окно расчёта значений для стабильности
        /// </summary>
        public bool IsShouldBeOpenedAsStabFile
        {
            get => isShouldBeOpenedAsStabFile; 
            set
            {
                if (!ChangeOpenDataVarianceInProcess && value != false)
                {
                    ChangeOpenDataVarianceInProcess = true;
                    CheckChoises(!value, value);
                }
                CheckChanges();
            }
        }

        /// <summary>
        /// Проверяет запущен ли процесс выбора нужного варианта
        /// </summary>
        private bool ChangeOpenDataVarianceInProcess;
        /// <summary>
        /// Выбирает нужный вариант, действуя по методу взаимоисключения вариантов выше
        /// </summary>
        public void CheckChoises(bool ILC, bool Stab)
        {
            if (ChangeOpenDataVarianceInProcess) 
            {
                isShouldBeOpenedAsILC       = ILC;
                IsShouldBeOpenedAsILC       = isShouldBeOpenedAsILC;
                isShouldBeOpenedAsStabFile  = Stab;
                IsShouldBeOpenedAsStabFile  = isShouldBeOpenedAsStabFile;
                ChangeOpenDataVarianceInProcess = false;
            }
        }

        public bool isTemporaryData = AppData.IsTemporaryTwoColumnData;
        /// <summary>
        /// Определяет является ли первая колонка данных возрастающей по её значениям. 
        /// В зависимости от того, является ли утверждение выше верным, блокирует или открывает возможность выбрать нужное окно.
        /// </summary>
        public bool IsTemporaryData
        {
            get => isTemporaryData;
            set
            {
                isTemporaryData = value;
                CheckChanges();
            }
        }

        #endregion
    }
}