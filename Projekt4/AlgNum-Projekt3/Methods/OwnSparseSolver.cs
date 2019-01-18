using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgNum_Projekt3.Methods
{
    public class OwnSparseSolver
    {
        #region Properties
        public double OwnSparseMatrixTiming { get; set; }
        public SparseMatrix OwnSparseMatrix { get; set; }
        public double[] OwnSparseMatrixSolution { get; set; }
        public double[] OwnSparseVector { get; set; }
        public int MaxIterations { get; set; }
        public double Epsilon { get; set; }
        #endregion

        #region Constructor
        public OwnSparseSolver(SparseMatrix sparseMatrix, double[] vector, int maxIterations, double epsilon)
        {
            OwnSparseMatrix = sparseMatrix;
            OwnSparseVector = vector;
            Epsilon = epsilon;
            MaxIterations = maxIterations;
            OwnSparseMatrixSolution = GaussSeidelOwnSparse();
        }
        #endregion

        #region Method
        public double[] GaussSeidelOwnSparse()
        {
            double[] result = new double[OwnSparseMatrix.RowCount];
            double[] previous = new double[OwnSparseMatrix.RowCount];
            double iterations = 0;
            bool run = true;

            var timer = System.Diagnostics.Stopwatch.StartNew();
            while (run)
            {
                int rowCounter = 0;
                double sum = 0;
                int column = 0;
                double diagonalValue = 0;
                int start = OwnSparseMatrix.RowIndexes[0];
                int end = OwnSparseMatrix.RowIndexes[1];

                while (rowCounter < OwnSparseMatrix.RowIndexes.Length)
                {
                    sum = OwnSparseVector[rowCounter];

                    for (int i = start; i < end; i++)
                    {
                        column = OwnSparseMatrix.ColumnIndexes.ElementAt(i);
                        if (rowCounter != column)
                            sum -= OwnSparseMatrix.Values[i] * result[column];
                        else
                            diagonalValue = OwnSparseMatrix.Values[i];
                    }

                    result[rowCounter] = 1 / diagonalValue * sum;

                    rowCounter++;

                    if (rowCounter >= OwnSparseMatrix.RowIndexes.Length - 1)
                        break;

                    start = OwnSparseMatrix.RowIndexes[rowCounter];
                    end = OwnSparseMatrix.RowIndexes[rowCounter + 1];
                }

                // Last row (we know last row contains only one element - which is 1 (becouse of the probability = 1).
                sum = OwnSparseVector[rowCounter];
                column = OwnSparseMatrix.ColumnIndexes.Last();
                if (rowCounter != column)
                    sum -= OwnSparseMatrix.Values.LastOrDefault() * result[column];
                result[rowCounter] = 1 / OwnSparseMatrix.Values.LastOrDefault() * sum;

                iterations++;
                run = false;

                // Check expected result by iterations or epsilon.
                for (int i = 0; i < OwnSparseMatrix.RowCount; i++)
                {
                    if (Math.Abs(result[i] - previous[i]) > Epsilon || iterations == MaxIterations)
                    {
                        run = true;
                    }
                }

                previous = (double[])result.Clone();
            }

            timer.Stop();

            OwnSparseMatrixTiming = timer.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
            return result;
        }
        #endregion
    }
}
