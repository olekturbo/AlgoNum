using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 10;
            int[] randomizedVectorArray = new int[size];
            int[,] randomizedMatrixArray = new int[size,size];
            Random random = new Random();

            for(int i = 0; i < size; i++)
            {
                randomizedVectorArray[i] = random.Next(-65536, 65535);
                for(int j = 0; j < size; j++)
                {
                    randomizedMatrixArray[i, j] = random.Next(-65536, 65535);
                }
            }

            Randomize<double> doubleRandomize = new Randomize<double>(typeof(double), size, randomizedVectorArray, randomizedMatrixArray);
            var doubleRandomizeMatrix = doubleRandomize.Matrix;
            var doubleRandomizeVector = doubleRandomize.Vector;

            var doubleRandomizeMatrix2 = (double[,])doubleRandomizeMatrix.Clone();
            var doubleRandomizeVector2 = (double[,])doubleRandomizeVector.Clone();

            var doubleRandomizeMatrix3 = (double[,])doubleRandomizeMatrix.Clone();
            var doubleRandomizeVector3 = (double[,])doubleRandomizeVector.Clone();

            MyMatrix<double> doubleMatrix = new MyMatrix<double>(typeof(double), size, doubleRandomizeMatrix, doubleRandomizeVector);
            MyMatrix<double> doubleMatrix2 = new MyMatrix<double>(typeof(double), size, doubleRandomizeMatrix2, doubleRandomizeVector2);
            MyMatrix<double> doubleMatrix3 = new MyMatrix<double>(typeof(double), size, doubleRandomizeMatrix3, doubleRandomizeVector3);


            doubleMatrix3.GaussWithTotalPivoting();
            Console.WriteLine("Gauss With Total Pivoting (DOUBLE) ERROR: " + doubleMatrix3.Error);
            Console.WriteLine("Gauss With Total Pivoting (DOUBLE) TIME: " + doubleMatrix3.MethodExecutionTime + "ms");
            doubleMatrix2.GaussWithPartialPivoting();
            Console.WriteLine("Gauss With Partial Pivoting (DOUBLE) ERROR: " + doubleMatrix2.Error);
            Console.WriteLine("Gauss With Partial Pivoting (DOUBLE) TIME: " + doubleMatrix2.MethodExecutionTime + "ms");
            doubleMatrix.GaussWithoutPivoting();
            Console.WriteLine("Gauss Without Pivoting (DOUBLE) ERROR: " + doubleMatrix.Error);
            Console.WriteLine("Gauss Without Pivoting (DOUBLE) TIME: " + doubleMatrix.MethodExecutionTime + "ms");


            Randomize<float> floatRandomize = new Randomize<float>(typeof(float),size, randomizedVectorArray, randomizedMatrixArray);
            var floatRandomizeMatrix = floatRandomize.Matrix;
            var floatRandomizeVector = floatRandomize.Vector;

            var floatRandomizeMatrix2 = (float[,])floatRandomizeMatrix.Clone();
            var floatRandomizeVector2 = (float[,])floatRandomizeVector.Clone();

            var floatRandomizeMatrix3 = (float[,])floatRandomizeMatrix.Clone();
            var floatRandomizeVector3 = (float[,])floatRandomizeVector.Clone();

            MyMatrix<float> floatMatrix = new MyMatrix<float>(typeof(float), size, floatRandomizeMatrix, floatRandomizeVector);
            MyMatrix<float> floatMatrix2 = new MyMatrix<float>(typeof(float), size, floatRandomizeMatrix2, floatRandomizeVector2);
            MyMatrix<float> floatMatrix3 = new MyMatrix<float>(typeof(float), size, floatRandomizeMatrix3, floatRandomizeVector3);

            floatMatrix3.GaussWithTotalPivoting();
            Console.WriteLine("Gauss With Total Pivoting (FLOAT) ERROR: " + floatMatrix3.Error);
            Console.WriteLine("Gauss With Total Pivoting (FLOAT) TIME: " + floatMatrix3.MethodExecutionTime + "ms");

            floatMatrix2.GaussWithPartialPivoting();
            Console.WriteLine("Gauss With Partial Pivoting (FLOAT) ERROR: " + floatMatrix2.Error);
            Console.WriteLine("Gauss With Partial Pivoting (FLOAT) TIME: " + floatMatrix2.MethodExecutionTime + "ms");

            floatMatrix.GaussWithoutPivoting();
            Console.WriteLine("Gauss Without Pivoting (FLOAT) ERROR: " + floatMatrix.Error);
            Console.WriteLine("Gauss Without Pivoting (FLOAT) TIME: " + floatMatrix.MethodExecutionTime + "ms");
            
            Randomize<Fraction> fractionRandomize = new Randomize<Fraction>(typeof(Fraction), size, randomizedVectorArray, randomizedMatrixArray);

            var fractionRandomizeMatrix = fractionRandomize.Matrix;
            var fractionRandomizeVector = fractionRandomize.Vector;

            var fractionRandomizeMatrix2 = (Fraction[,])fractionRandomize.Matrix.Clone();
            var fractionRandomizeVector2 = (Fraction[,])fractionRandomize.Vector.Clone();

            var fractionRandomizeMatrix3 = (Fraction[,])fractionRandomize.Matrix.Clone();
            var fractionRandomizeVector3 = (Fraction[,])fractionRandomize.Vector.Clone();

            MyMatrix<Fraction> fractionMatrix = new MyMatrix<Fraction>(typeof(Fraction), size, fractionRandomizeMatrix, fractionRandomizeVector);
            MyMatrix<Fraction> fractionMatrix2 = new MyMatrix<Fraction>(typeof(Fraction), size, fractionRandomizeMatrix2, fractionRandomizeVector2);
            MyMatrix<Fraction> fractionMatrix3 = new MyMatrix<Fraction>(typeof(Fraction), size, fractionRandomizeMatrix3, fractionRandomizeVector3);

            fractionMatrix3.GaussWithTotalPivoting();
            Console.WriteLine("Gauss With Total Pivoting (FRACTION) ERROR: " + fractionMatrix3.ErrorString);
            Console.WriteLine("Gauss With Total Pivoting (FRACTION) TIME: " + fractionMatrix3.MethodExecutionTime + "ms");

            fractionMatrix2.GaussWithPartialPivoting();
            Console.WriteLine("Gauss With Partial Pivoting (FRACTION) ERROR: " + fractionMatrix2.ErrorString);
            Console.WriteLine("Gauss With Partial Pivoting (FRACTION) TIME: " + fractionMatrix2.MethodExecutionTime + "ms");

            fractionMatrix.GaussWithoutPivoting();
            Console.WriteLine("Gauss Without Pivoting (FRACTION) ERROR: " + fractionMatrix.ErrorString);
            Console.WriteLine("Gauss Without Pivoting (FRACTION) TIME: " + fractionMatrix.MethodExecutionTime + "ms");

            Console.ReadLine();
        }
    }
}
