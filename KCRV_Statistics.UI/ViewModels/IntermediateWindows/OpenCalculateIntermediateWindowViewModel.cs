﻿using KCRV_Statistics.Model.MathService;
using KCRV_Statistics.UI.AppService;
using System;
using System.Windows;

namespace KCRV_Statistics.UI.ViewModels.IntermediateWindows
{
    public class OpenCalculateIntermediateWindowViewModel : NotifyPropertyChanged
    {
        #region Поля ввода значений округления при итерации и выводе результата

        public int iterationDigits = 0;
        public int IterationDigits
        {
            get
            {
                return iterationDigits;
            }
            set
            {
                iterationDigits = value;
                CheckChanges();
            }
        }

        public int resultDigits = 0;
        public int ResultDigits
        {
            get
            {
                return resultDigits;
            }
            set
            {
                resultDigits = value;
                CheckChanges();
            }
        }

        #endregion

        #region Поля ввода координат для xlsx

        public int coordinateDataBeginX;
        public int CoordinateDataBeginX
        {
            get
            {
                return coordinateDataBeginX;
            }
            set
            {
                coordinateDataBeginX = value;
                CheckChanges();
            }
        }

        public int coordinateDataBeginY;
        public int CoordinateDataBeginY
        {
            get
            {
                return coordinateDataBeginY;
            }
            set
            {
                coordinateDataBeginY = value;
                CheckChanges();
            }
        }

        #endregion

        #region Кнопки управления

        public Command Calculate
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        try
                        {
                            // Проверка списка на наличие в нём элементов
                            if (AppData.CurrentData.Count == 0)
                            {
                                MessageBox.Show("Не удалось загрузить данные из файла.");
                                return;
                            }

                            // Расчёт значений, присвоение результатов вычислений статическому полю 
                            var Result = Estimators.CalculateAllMethods(AppData.CurrentData, IterationDigits, ResultDigits);
                            AppData.OutputData.Clear();
                            AppData.OutputData = Result;

                            // Открытие окна
                            WindowsObjects.CalculateMasterWindow = new();
                            if (WindowsObjects.CalculateMasterWindow.ShowDialog() == true)
                            {
                                WindowsObjects.CalculateMasterWindow.Show();
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Возникла неустранимая ошибка: \n" + e.Message);
                        }
                    }
                );
            }
        }

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
                        WindowsObjects.OpenCalculateIntermediateWindow.Close();
                        WindowsObjects.OpenCalculateIntermediateWindow = null;
                    }
                );
            }
        }

        #endregion
    }
}