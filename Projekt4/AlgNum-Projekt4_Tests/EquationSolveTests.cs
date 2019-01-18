using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlgNum_Projekt3;
using AlgNum_Projekt3.Approximation;
using AlgNum_Projekt3.Methods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgNum_Projekt4_Tests
{
    [TestClass]
    public class EquationSolveTests
    {
        [TestMethod]
        public void TimesTest()
        {
            /** EXCHANGE SEPARATOR FOR CSV **/
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            /** CONFIGURATION **/
            int totalAgents = 60;
            int startAgents = 5;
            int loopJump = 5;
            int loopCounter = 0;
            int maxIterations;
            double epsilon = Math.Pow(10, -10);
            Generator generator;
            Generator ownSparseGenerator;
            maxIterations = 1000;
            double[] agents = new double[totalAgents / startAgents];
            double[] cases = new double[totalAgents / startAgents];

            /** TIMES LISTS **/
            List<double> partialGaussTimes = new List<double>();
            List<double> sparsePartialGaussTimes = new List<double>();
            List<double> gaussSeidelTimes = new List<double>();
            List<double> sparseLUTimes = new List<double>();
            List<double> ownSparseTimes = new List<double>();

            /** X AND F(X) FOR APPROXIMATION **/
            Approximation approximation = new Approximation();
            double[] arguments = new double[totalAgents - startAgents + 1];
            double[] partialGaussValues = new double[totalAgents - startAgents + 1];
            double[] sparsePartialGaussValues = new double[totalAgents - startAgents + 1];
            double[] gaussSeidelValues = new double[totalAgents - startAgents + 1];
            double[] sparseLUValues = new double[totalAgents - startAgents + 1];
            double[] ownSparseValues = new double[totalAgents - startAgents + 1];

            /** COUNTING TIMES **/
            using (var w = new StreamWriter("times.csv"))
            {
                var newLine = "Total Agents, Total Cases, Generate Equation, SparseLU, Gauss-Seidel, Sparse Partial Gauss, Partial Gauss, Own Sparse";
                w.WriteLine(newLine);
                w.Flush();

                loopCounter = 0;
                for (int i = startAgents; i <= totalAgents; i += loopJump)
                {
                    partialGaussTimes.Clear();
                    sparsePartialGaussTimes.Clear();
                    gaussSeidelTimes.Clear();
                    sparseLUTimes.Clear();
                    ownSparseTimes.Clear();

                    generator = new Generator(i);
                    ownSparseGenerator = new Generator(i, true);

                    for (int j = 0; j < 100; j++)
                    {
                        GaussSeidel gaussSeidel = new GaussSeidel(generator.TotalCases, generator.OriginalMatrix, generator.OriginalVector, maxIterations, epsilon);
                        PartialGauss partialGauss = new PartialGauss(generator.TotalCases, generator.OriginalMatrix, generator.OriginalVector, false);
                        PartialGauss sparsePartialGauss = new PartialGauss(generator.TotalCases, generator.OriginalMatrix, generator.OriginalVector, true);
                        SparseLUSolver sparseLUSolver = new SparseLUSolver(generator.TotalCases, generator.OriginalMatrix, generator.OriginalVector);
                        OwnSparseSolver ownSparseSolver = new OwnSparseSolver(ownSparseGenerator.GenerateOwnSparseMatrix(), generator.OriginalVector, maxIterations, epsilon);

                        partialGaussTimes.Add(partialGauss.GaussPartialTiming);
                        sparsePartialGaussTimes.Add(sparsePartialGauss.SparseGaussPartialTiming);
                        gaussSeidelTimes.Add(gaussSeidel.GaussSeidelTiming);
                        sparseLUTimes.Add(sparseLUSolver.SparseLUTiming);
                        ownSparseTimes.Add(ownSparseSolver.OwnSparseMatrixTiming);
                    }

                    partialGaussTimes.Sort();
                    sparsePartialGaussTimes.Sort();
                    gaussSeidelTimes.Sort();
                    sparseLUTimes.Sort();
                    ownSparseTimes.Sort();

                    partialGaussTimes.Remove(partialGaussTimes.FirstOrDefault());
                    partialGaussTimes.Remove(partialGaussTimes.LastOrDefault());

                    sparsePartialGaussTimes.Remove(sparsePartialGaussTimes.FirstOrDefault());
                    sparsePartialGaussTimes.Remove(sparsePartialGaussTimes.LastOrDefault());

                    gaussSeidelTimes.Remove(gaussSeidelTimes.FirstOrDefault());
                    gaussSeidelTimes.Remove(gaussSeidelTimes.LastOrDefault());

                    sparseLUTimes.Remove(sparseLUTimes.FirstOrDefault());
                    sparseLUTimes.Remove(sparseLUTimes.LastOrDefault());

                    ownSparseTimes.Remove(ownSparseTimes.FirstOrDefault());
                    ownSparseTimes.Remove(ownSparseTimes.LastOrDefault());

                    newLine = $"{i},{generator.TotalCases},{generator.GenerateEquationTiming},{sparseLUTimes.Average()},{gaussSeidelTimes.Average()},{sparsePartialGaussTimes.Average()}, {partialGaussTimes.Average()}, {ownSparseTimes.Average()}";
                    w.WriteLine(newLine);
                    w.Flush();

                    arguments[loopCounter] = generator.TotalCases;
                    partialGaussValues[loopCounter] = partialGaussTimes.Average();
                    sparsePartialGaussValues[loopCounter] = sparsePartialGaussTimes.Average();
                    gaussSeidelValues[loopCounter] = gaussSeidelTimes.Average();
                    sparseLUValues[loopCounter] = sparseLUTimes.Average();
                    ownSparseValues[loopCounter] = ownSparseTimes.Average();

                    agents[loopCounter] = generator.TotalAgents;
                    cases[loopCounter] = generator.TotalCases;

                    loopCounter++;

                }

            }

            /** APPROXIMATION **/
            var partialGaussApproximationTest = approximation.GetApproximation(3, arguments, partialGaussValues);
            var sparsePartialGaussApproximationTest = approximation.GetApproximation(2, arguments, sparsePartialGaussValues);
            var gaussSeidelApproximationTest = approximation.GetApproximation(2, arguments, gaussSeidelValues);
            var sparseLUApproximationTest = approximation.getLinearRegression(arguments, sparseLUValues);
            var ownSparseApproximationTest = approximation.GetApproximation(2, arguments, ownSparseValues);

            /** toString **/
            using (var s = new StreamWriter("strings.txt"))
            {
                s.WriteLine("PARTIAL GAUSS " + partialGaussApproximationTest.GetString());
                s.WriteLine("SPARSE PARTIAL GAUUS " + sparsePartialGaussApproximationTest.GetString());
                s.WriteLine("GAUSS SEIDEL " + gaussSeidelApproximationTest.GetString());
                s.WriteLine("SPARSE LU " + sparseLUApproximationTest.GetString());
                s.WriteLine("OWN SPARSE " + ownSparseApproximationTest.GetString());
                s.Flush();
            }

            /** toValue **/
            using (var v = new StreamWriter("approximation.csv"))
            {
                var line = "Total Agents, Total Cases, SparseLU, Gauss-Seidel, Sparse Partial Gauss, Partial Gauss, Own Sparse";
                v.WriteLine(line);
                v.Flush();

                for (int i = 0; i < (totalAgents / startAgents); i++)
                {
                    line = $"{agents[i]}, {cases[i]}, {sparseLUApproximationTest.GetResult(cases[i])}, {gaussSeidelApproximationTest.GetResult(cases[i])}, {sparsePartialGaussApproximationTest.GetResult(cases[i])}, {partialGaussApproximationTest.GetResult(cases[i])}, {ownSparseApproximationTest.GetResult(cases[i])}";
                    v.WriteLine(line);
                    v.Flush();
                }
            }

            /** ERRORS **/
            double partialGaussError = 0;
            double sparsePartialGaussError = 0;
            double gaussSeidelError = 0;
            double sparseLUError = 0;
            double ownSparseError = 0;

            for (int i = 0; i < (totalAgents / startAgents); i++)
            {
                partialGaussError += Math.Sqrt(Math.Pow(partialGaussValues[i] - partialGaussApproximationTest.GetResult(cases[i]), 2));
                sparsePartialGaussError += Math.Sqrt(Math.Pow(sparsePartialGaussValues[i] - sparsePartialGaussApproximationTest.GetResult(cases[i]), 2));
                gaussSeidelError += Math.Sqrt(Math.Pow(gaussSeidelValues[i] - gaussSeidelApproximationTest.GetResult(cases[i]), 2));
                sparseLUError += Math.Sqrt(Math.Pow(sparseLUValues[i] - sparseLUApproximationTest.GetResult(cases[i]), 2));
                ownSparseError += Math.Sqrt(Math.Pow(ownSparseValues[i] - ownSparseApproximationTest.GetResult(cases[i]), 2));
            }

            using (var e = new StreamWriter("errors.txt"))
            {
                e.WriteLine("PARTIAL GAUSS ERROR " + partialGaussError);
                e.WriteLine("SPARSE PARTIAL GAUSS ERROR " + sparsePartialGaussError);
                e.WriteLine("GAUSS SEIDEL ERROR " + gaussSeidelError);
                e.WriteLine("SPARSE LU ERROR " + sparseLUError);
                e.WriteLine("OWN SPARSE ERROR " + ownSparseError);
                e.Flush();
            }

            /** COUNTING TIME FOR 100K */
            using (var w = new StreamWriter("100k.txt"))
            {
                w.WriteLine("PARTIAL GAUSS 100k " + partialGaussApproximationTest.GetResult(100000) / Math.Pow(10, 6) + " SEKUND");
                w.WriteLine("SPARSE PARTIAL 100k " + sparsePartialGaussApproximationTest.GetResult(100000) / Math.Pow(10, 6) + " SEKUND");
                w.WriteLine("GAUSS SEIDEL 100k " + gaussSeidelApproximationTest.GetResult(100000) / Math.Pow(10, 6) + " SEKUND");
                w.WriteLine("SPARSE LU 100k " + sparseLUApproximationTest.GetResult(100000) + " MIKROSEKUND");
                w.WriteLine("OWN SPARSE 100k " + ownSparseApproximationTest.GetResult(100000) / Math.Pow(10, 6) + " SEKUND");
                w.Flush();
            }

        }

        [TestMethod]
        public void GaussSeidelSparseTest()
        {
            int maxIterations = 100;
            double epsilon = Math.Pow(10, -10);
            Generator generator = new Generator(30);
            Generator ownSparseGenerator = new Generator(30, true);
            GaussSeidel gaussSeidel = new GaussSeidel(generator.TotalCases, generator.OriginalMatrix, generator.OriginalVector, maxIterations, epsilon);
            OwnSparseSolver ownSparseSolver = new OwnSparseSolver(ownSparseGenerator.GenerateOwnSparseMatrix(), generator.OriginalVector, maxIterations, epsilon);

            Assert.AreEqual(gaussSeidel.GaussSeidelSolution.Length, ownSparseSolver.OwnSparseMatrixSolution.Length);

            for (int i = 0; i < gaussSeidel.GaussSeidelSolution.Length; i++)
            {
                if (gaussSeidel.GaussSeidelSolution[i] != ownSparseSolver.OwnSparseMatrixSolution[i])
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void OwnSparseMatrixLargeTest()
        {
            int maxIterations = 100;
            double epsilon = Math.Pow(10, -10);
            Generator generator = new Generator(500, true);
            OwnSparseSolver ownSparseSolver = new OwnSparseSolver(generator.GenerateOwnSparseMatrix(), generator.LargeVector, maxIterations, epsilon);
        }

        [TestMethod]
        public void EquationMatrixTest()
        {
            Generator generator = new Generator(20);
            bool isNumeric = false;
            for (int i = 0; i < generator.TotalCases; i++)
            {
                for (int j = 0; j < generator.TotalCases; j++)
                {
                    if (generator.OriginalMatrix[i, j] > 0)
                        isNumeric = true;
                }

                if (!isNumeric)
                    Assert.Fail();
            }
        }

    }
}

