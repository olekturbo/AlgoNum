using System;
using System.IO;

namespace AlgNum_Projekt3
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            int totalAgents;
            int startAgents;
            int maxIterations;
            double epsilon;
            Generator generator;
            string filePath;

            //For start memory
            //MonteCarlo monteCarlo = new MonteCarlo(1000, 5);

            /************** ERRORS *************/
            totalAgents = 30;
            startAgents = 3;
            maxIterations = 1000;
            filePath = "all-methods-errors.csv";

            using (var w = new StreamWriter(filePath))
            {
                var newLine = "Total Agents, Jacobi, Gauss-Seidel, Sparse Partial Gauss, Partial Gauss";
                w.WriteLine(newLine);
                w.Flush();
                for (int i = startAgents; i <= totalAgents; i++)
                {
                    generator = new Generator(i, maxIterations);
                    newLine = $"{i},{generator.JacobiError},{generator.GaussSeidelError},{generator.SparseGaussPartialError},{generator.GaussPartialError}";
                    w.WriteLine(newLine);
                    w.Flush();
                }
            }

            /************** PRECISIONS *************/
            totalAgents = 10;
            startAgents = 5;
            maxIterations = 500;
            filePath = "precisions.csv";

            using (var w = new StreamWriter(filePath))
            {
                var newLine = "Total Agents, Max Iterations, Precision, Jacobi, Gauss-Seidel";
                w.WriteLine(newLine);
                w.Flush();
                for (int i = startAgents; i <= totalAgents; i++)
                {
                    for (int j = -6; j >= -14; j -= 4)
                    {
                        epsilon = Math.Pow(10, j);
                        generator = new Generator(i, maxIterations, epsilon);
                        newLine = $"{i},{maxIterations}, {j},{generator.JacobiError},{generator.GaussSeidelError}";
                        w.WriteLine(newLine);
                        w.Flush();
                    }
                }
            }

            /************** TIMINGS *************/
            totalAgents = 10;
            startAgents = 5;
            filePath = "timings.csv";

            using (var w = new StreamWriter(filePath))
            {
                var newLine = "Total Agents, Sparse Partial Gauss,Partial Gauss";
                w.WriteLine(newLine);
                w.Flush();
                for (int i = startAgents; i <= totalAgents; i++)
                {
                    generator = new Generator(i, maxIterations);
                    newLine = $"{i},{generator.SparseGaussPartialTiming},{generator.GaussPartialTiming}";
                    w.WriteLine(newLine);
                    w.Flush();
                }
            }

            /************** PRECISIONS FOR ITERATION METHODS *************/
            totalAgents = 10;
            startAgents = 5;
            maxIterations = 500;
            filePath = "precisions-iteration-methods.csv";

            using (var w = new StreamWriter(filePath))
            {
                var newLine = "Total Agents, Max Iterations, Precision, Jacobi, Gauss-Seidel";
                w.WriteLine(newLine);
                w.Flush();
                for (int i = startAgents; i <= totalAgents; i++)
                {
                    for (int j = -6; j >= -14; j -= 4)
                    {
                        epsilon = Math.Pow(10, j);
                        generator = new Generator(i, maxIterations, epsilon);
                        newLine = $"{i},{maxIterations},{j},{generator.JacobiTiming},{generator.GaussSeidelTiming}";
                        w.WriteLine(newLine);
                        w.Flush();
                    }
                }
            }

            /************** MONTE CARLO VALIDATION *************/
            totalAgents = 15;
            startAgents = 3;
            maxIterations = 1000;
            filePath = "monte-carlo-vs-methods-error.csv";

            using (var w = new StreamWriter(filePath))
            {
                var newLine = "Total Agents, Iterations, GaussError, SparseGaussError, JacobiError, GaussSeidelError";
                w.WriteLine(newLine);
                w.Flush();
                for (int i = startAgents; i <= totalAgents; i++)
                {
                    generator = new Generator(i, maxIterations);
                    newLine = $"{i},{maxIterations},{generator.GaussMonteCarloError},{generator.SparseGaussMonteCarloError}, {generator.JacobiMonteCarloError}, {generator.GaussSeidelMonteCarloError}";
                    w.WriteLine(newLine);
                    w.Flush();
                }
            }
        }
    }
}
