using MathNet.Numerics.LinearAlgebra;
using System;

namespace AlgNum_Projekt3.Approximation
{
    public class Approximation
    {
        public ApproximationFunction GetApproximation(int degree, double[] arguments, double[] values)
        {
            var sVector = new double[2 * degree + 1];

            for (var i = 0; i < arguments.Length; i++)
            {
                for (var j = 0; j < 2 * degree + 1; j++)
                {
                    sVector[j] += Math.Pow(arguments[i], j);
                }
            }

            var tVector = new double[2 * degree + 1];

            for (var i = 0; i < arguments.Length; i++)
            {
                for (var j = 0; j < degree + 1; j++)
                {
                    tVector[j] += values[i] * Math.Pow(arguments[i], j);
                }
            }

            var A = new double[degree + 1, degree + 1];
            var B = new double[degree + 1];

            for (var i = 0; i < degree + 1; i++)
            {
                for (var j = 0; j < degree + 1; j++)
                {
                    A[i, j] = sVector[j + i];
                }

                B[i] = tVector[i];
            }

            Matrix<double> matrixA = Matrix<double>.Build.DenseOfArray(A);
            Vector<double> vectorB = Vector<double>.Build.DenseOfArray(B);
            double[] solution = matrixA.Solve(vectorB).ToArray();

            return new ApproximationFunction(solution);
        }

        public ApproximationFunction getLinearRegression(double[] arguments, double[] values)
        {
            double sumX = 0;
            double sumPowX = 0;
            double sumFx = 0;
            double sumXFx = 0;
            double a0;
            double a1;

            for(int i = 0; i < arguments.Length; i++)
            {
                sumX += arguments[i];
                sumPowX += Math.Pow(arguments[i], 2);
                sumFx += values[i];
                sumXFx += arguments[i] * values[i];
            }

            a0 = (sumPowX * sumFx - sumX * sumXFx) / (arguments.Length * sumPowX - Math.Pow(sumX, 2));
            a1 = (arguments.Length * sumXFx - sumX * sumFx) / (arguments.Length * sumPowX - Math.Pow(sumX, 2));

            double[] solution = { a0, a1 };

            return new ApproximationFunction(solution);

        }

    }
}
