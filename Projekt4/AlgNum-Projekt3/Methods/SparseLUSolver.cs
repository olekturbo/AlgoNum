using CSparse.Double;
using CSparse.Double.Factorization;
using CSparse.Storage;
using MathNet.Numerics.LinearAlgebra.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgNum_Projekt3.Methods
{
    public class SparseLUSolver
    {
        #region Properties

        public double SparseLUTiming { get; set; }

        public int TotalCases { get; set; }

        public SparseLU SparseLUMatrix { get; set; }

        public double[] SparseLUResultVector { get; set; }

        public double[] SparseLUVector { get; set; }
        #endregion

        #region Constructor
        public SparseLUSolver(int totalCases, double[,] matrix, double[] vector)
        {
            TotalCases = totalCases;
            CompressedColumnStorage<double> e = CompressedColumnStorage<double>.OfArray(matrix);
            SparseLUMatrix = SparseLU.Create(e, CSparse.ColumnOrdering.MinimumDegreeAtPlusA, 1.0);
            SparseLUVector = vector;
            SparseLUResultVector = Vector.Create(vector.Length, 1.0);
            SparseLUMethod();
        }
        #endregion

        #region Method
        public void SparseLUMethod()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            SparseLUMatrix.Solve(SparseLUVector, SparseLUResultVector);
            timer.Stop();
            SparseLUTiming = timer.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
        }
        #endregion

    }
}
