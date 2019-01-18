using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2
{
    public class Randomize<T>
    {
        
        public Randomize(Type type, int size, int[] randomizedVectorArray, int[,] randomizedMatrixArray)
        {
            switch (type.FullName)
            {
                case "System.Single":
                    {
                        Matrix = Randomize<T>.RandomizeMatrix(typeof(float), false, size, randomizedVectorArray, randomizedMatrixArray);
                        Vector = Randomize<T>.RandomizeMatrix(typeof(float), true, size, randomizedVectorArray, randomizedMatrixArray);
                        break;
                    }
                case "System.Double":
                    {
                        Matrix = Randomize<T>.RandomizeMatrix(typeof(double), false, size, randomizedVectorArray, randomizedMatrixArray);
                        Vector = Randomize<T>.RandomizeMatrix(typeof(double), true, size, randomizedVectorArray, randomizedMatrixArray);
                        break;
                    }
                case "Projekt2.Fraction":
                    {
                        Matrix = Randomize<T>.RandomizeMatrix(typeof(Fraction), false, size, randomizedVectorArray, randomizedMatrixArray);
                        Vector = Randomize<T>.RandomizeMatrix(typeof(Fraction), true, size, randomizedVectorArray, randomizedMatrixArray);
                        break;
                    }
            }
        }

        public static T[,] RandomizeMatrix(Type type, bool isVector, int size, int[] randomizedVectorArray, int[,] randomizedMatrixArray)
        {
            T[,] matrix = new T[0, 0];
            switch (type.FullName)
            {
                case "System.Single":
                    {
                        if (isVector)
                        {
                            matrix = new T[size, 1];
                            for (int i = 0; i < size; i++)
                            {
                                matrix[i, 0] = (T)(object)((float)randomizedVectorArray[i] / (float)65536);
                            }
                            break;
                        }
                        else
                        {
                            matrix = new T[size, size];
                            for (int i = 0; i < size; i++)
                            {
                                for (int j = 0; j < size; j++)
                                {
                                    matrix[i, j] = (T)(object)((float)randomizedMatrixArray[i, j] / (float)65536);
                                }
                            }
                            break;
                        }
                    }
                case "System.Double":
                    {
                        if (isVector)
                        {
                            matrix = new T[size, 1];
                            for (int i = 0; i < size; i++)
                            {
                                matrix[i, 0] = (T)(object)((double)randomizedVectorArray[i] / (double)65536);
                            }
                            break;
                        }
                        else
                        {
                            matrix = new T[size, size];
                            for (int i = 0; i < size; i++)
                            {
                                for (int j = 0; j < size; j++)
                                {
                                    matrix[i, j] = (T)(object)((double)randomizedMatrixArray[i, j] / (double)65536);
                                }
                            }
                            break;
                        }
                    }
                case "Projekt2.Fraction":
                    {
                        if (isVector)
                        {
                            matrix = new T[size, 1];
                            for (int i = 0; i < size; i++)
                            {
                                matrix[i, 0] = (dynamic)new Fraction(randomizedVectorArray[i], BigInteger.Pow(2,16));
                            }
                            break;
                        }
                        else
                        {
                            matrix = new T[size, size];
                            for (int i = 0; i < size; i++)
                            {
                                for (int j = 0; j < size; j++)
                                {
                                    matrix[i, j] = (dynamic)new Fraction(randomizedMatrixArray[i,j], BigInteger.Pow(2,16));
                                }
                            }
                            break;
                        }
                    }
            }
            return matrix;
        }

        public T[,] Matrix;
        public T[,] Vector;
    }
}
