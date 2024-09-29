﻿using KCRV_Statistics.Model.MathService;
using MathServiceTests.EstimatorsTests.Data;

namespace MathServiceTests.EstimatorsTests.Tests.MandelPaule
{
    /// <summary>
    /// Первая тестовая выборка. Имеется 15 лабораторий, для которых уже рассчитаны необходимые значения, 
    /// требуется сравнить результаты работы алгоритмов с истинными результатами
    /// </summary>
    [TestClass]
    public class EstimatorsTests_MandelPaule_1
    {
        [TestMethod]
        public void MandelPaule_X_test_1_DEF()
        {
            double result = Estimators.MandelPaule(Estimators_Data_1.TestData, 10.1921646106683, 15, 4).X;
            Assert.AreEqual(Math.Round(10.017421554264, 4), result);
        }
        [TestMethod]
        public void MandelPaule_U_test_2_DEF()
        {
            double result = Estimators.MandelPaule(Estimators_Data_1.TestData, 10.1921646106683, 15, 4).U;
            Assert.AreEqual(Math.Round(0.12008449913461, 4), result);
        }
        [TestMethod]
        public void MandelPaule_λ_test_3_DEF()
        {
            double result = Estimators.MandelPaule(Estimators_Data_1.TestData, 10.1921646106683, 15, 4).InterLabVariance;
            Assert.AreEqual(Math.Round(0.350737906431726, 4), result);
        }
    }
}