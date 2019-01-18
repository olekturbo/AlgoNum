using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgNum_Projekt3.Methods
{
    public class GaussSeidel
    {
        #region Properties
        public double[,] Matrix { get; set; }

        public double[] Vector { get; set; }

        public double[] GaussSeidelSolution { get; set; }

        public double GaussSeidelTiming { get; set; }

        public int TotalCases { get; set; }

        public int MaxIterations { get; set; }

        public double Epsilon { get; set; }
        #endregion

        #region Constructor
        public GaussSeidel(int totalCases, double[,] matrix, double[] vector, int maxIterations, double epsilon)
        {
            TotalCases = totalCases;
            Matrix = matrix;
            Vector = vector;
            MaxIterations = maxIterations;
            Epsilon = epsilon;
            GaussSeidelSolution = GaussSeidelMethod();
        }
        #endregion

        #region Method
        public double[] GaussSeidelMethod()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            double[] result = new double[TotalCases];
            double[] previous = new double[TotalCases];
            int iterations = 0;
            bool run = true;

            while (run)
            {
                for (int i = 0; i < TotalCases; i++)
                {
                    double sum = Vector[i];

                    for (int j = 0; j < TotalCases; j++)
                    {
                        if (j != i)
                        {
                            sum -= Matrix[i, j] * result[j];
                        }
                    }

                    result[i] = 1 / Matrix[i, i] * sum;
                }

                iterations++;
                run = false;

                for (int i = 0; i < TotalCases; i++)
                {
                    if (Math.Abs(result[i] - previous[i]) > Epsilon || iterations == MaxIterations)
                    {
                        run = true;
                    }
                }

                previous = (double[])result.Clone();
            }

            timer.Stop();

            GaussSeidelTiming = timer.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
            return result;
        }
        #endregion
    }
}
