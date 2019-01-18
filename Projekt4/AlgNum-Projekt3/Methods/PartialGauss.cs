using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgNum_Projekt3.Methods
{
    public class PartialGauss
    {
        #region Properties
        public double[,] Matrix { get; set; }

        public double[] Vector { get; set; }

        public double[] GaussPartialSolution { get; set; }

        public double GaussPartialTiming { get; set; }

        public double[] SparseGaussPartialSolution { get; set; }

        public double SparseGaussPartialTiming { get; set; }

        public int TotalCases { get; set; }

        public bool IsSparse { get; set; }
        #endregion

        #region Constructor
        public PartialGauss(int totalCases, double[,] matrix, double[] vector, bool isSparse)
        {
            TotalCases = totalCases;
            IsSparse = isSparse;
            Matrix = matrix;
            Vector = vector;
            if (isSparse)
                SparseGaussPartialSolution = GaussWithPartialPivoting();
            else
                GaussPartialSolution = GaussWithPartialPivoting();
        }
        #endregion

        #region Method
        public double[] GaussWithPartialPivoting()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            double[] solution = new double[TotalCases];

            for (int i = 0; i < TotalCases; i++)
            {
                int max = i;

                for (int j = i + 1; j < TotalCases; j++)
                {
                    if (Math.Abs(Matrix[j, i]) > Math.Abs(Matrix[max, i]))
                    {
                        max = j;
                    }
                }

                double temp;

                for (int j = 0; j < TotalCases; j++)
                {
                    temp = Matrix[i, j];
                    Matrix[i, j] = Matrix[max, j];
                    Matrix[max, j] = temp;
                }


                temp = Vector[i];
                Vector[i] = Vector[max];
                Vector[max] = temp;

                for (int j = i + 1; j < TotalCases; j++)
                {
                    if ((IsSparse && Matrix[j, i] != 0) || !IsSparse)
                    {
                        double factor = Matrix[j, i] / Matrix[i, i];
                        Matrix[j, 0] = Matrix[j, 0] - factor * Vector[i];

                        for (int k = i; k < TotalCases; k++)
                        {
                            Matrix[j, k] = Matrix[j, k] - factor * Matrix[i, k];
                        }
                    }
                }
            }

            for (int i = TotalCases - 1; i >= 0; i--)
            {
                double result = 0;
                for (int j = i + 1; j < TotalCases; j++)
                {
                    result += Matrix[i, j] * solution[j];
                }
                solution[i] = (Vector[i] - result) / Matrix[i, i];
            }

            timer.Stop();
            if (IsSparse)
            {
                Console.Write("(SPARSE) ");
                SparseGaussPartialTiming = timer.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
            }
            else
            {
                GaussPartialTiming = timer.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
            }
            return solution;
        }
    }
    #endregion
}
