using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2
{
    public class MyMatrix<T> : ICloneable
    {
        public MyMatrix(Type type, int size, T[,] matrix, T[,] vector)
        {
            Type = type;
            Size = size;
            Matrix = matrix;
            Vector = vector;
            OriginalMatrix = (T[,])Matrix.Clone();
            OriginalVector = (T[,])Vector.Clone();
            CalculatedVector = new T[size, 1];
            Error = default(T);
        }

        public void GaussWithoutPivoting()
        {
            T[,] solution = new T[Size, 1];

            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < Size; i++)
            {
                ReduceMatrix(i);
            }

            CalculateSolution(solution);

            watch.Stop();
            MethodExecutionTime = watch.ElapsedMilliseconds;

            CalculateVector(solution);
            CalculateError();
        }

        public void GaussWithPartialPivoting()
        {
            T[,] solution = new T[Size, 1];

            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < Size; i++)
            {
                int max = i;

                for (int j = i + 1; j < Size; j++)
                {
                    if (Type.FullName.Equals("Projekt2.Fraction"))
                    {
                        if (Fraction.Abs((dynamic)(object)Matrix[j, i]) > Fraction.Abs((dynamic)(object)Matrix[max, i]))
                        {
                            max = j;
                        }
                    }
                    else
                    {
                        if (Math.Abs((dynamic)(object)Matrix[j, i]) > Math.Abs((dynamic)(object)Matrix[max, i]))
                        {
                            max = j;
                        }
                    }
                }

                T temp;

                for (int j = 0; j < Size; j++)
                {
                    temp = Matrix[i, j];
                    Matrix[i, j] = Matrix[max, j];
                    Matrix[max, j] = temp;
                }


                temp = Vector[i, 0];
                Vector[i, 0] = Vector[max, 0];
                Vector[max, 0] = temp;


                ReduceMatrix(i);
            }

            CalculateSolution(solution);

            watch.Stop();
            MethodExecutionTime = watch.ElapsedMilliseconds;

            CalculateVector(solution);
            CalculateError();
        }

        public void GaussWithTotalPivoting()
        {
            T[,] solution = new T[Size, 1];
            T[,] orderedVector = new T[Size, 1];
            int[] columnOrder = new int[Size];

            for (int i = 0; i < Size; i++)
            {
                columnOrder[i] = i;
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < Size; i++)
            {
                int maxRow = i;
                int maxColumn = i;

                for (int j = i; j < Size; j++)
                {
                    for (int k = i; k < Size; k++)
                    {
                        if (Type.FullName.Equals("Projekt2.Fraction"))
                        {
                            if (Fraction.Abs((dynamic)(object)Matrix[j, k]) > Fraction.Abs((dynamic)(object)Matrix[maxRow, maxColumn]))
                            {
                                maxRow = j;
                                maxColumn = k;
                            }
                        }
                        else
                        {
                            if (Math.Abs((dynamic)(object)Matrix[j, k]) > Math.Abs((dynamic)(object)Matrix[maxRow, maxColumn]))
                            {
                                maxRow = j;
                                maxColumn = k;
                            }
                        }
                    }
                }

                int tempPosition = columnOrder[i];
                columnOrder[i] = columnOrder[maxColumn];
                columnOrder[maxColumn] = tempPosition;

                T temp;

                for (int j = 0; j < Size; j++)
                {
                    temp = Matrix[i, j];
                    Matrix[i, j] = Matrix[maxRow, j];
                    Matrix[maxRow, j] = temp;
                }

                temp = Vector[i, 0];
                Vector[i, 0] = Vector[maxRow, 0];
                Vector[maxRow, 0] = temp;

                for (int j = 0; j < Size; j++)
                {
                    temp = Matrix[j, i];
                    Matrix[j, i] = Matrix[j, maxColumn];
                    Matrix[j, maxColumn] = temp;
                }

                ReduceMatrix(i);
            }

            CalculateSolution(solution);

            watch.Stop();
            MethodExecutionTime = watch.ElapsedMilliseconds;

            for (int i = 0; i < Size; i++)
            {
                orderedVector[columnOrder[i], 0] = (T)(object)solution[i, 0];
            }

            CalculateVector(orderedVector);
            CalculateError();
        }

        private void CalculateVector(T[,] solution)
        {
            for (int i = 0; i < Size; i++)
            {
                dynamic result = 0;
                if (Type.FullName.Equals("Projekt2.Fraction"))
                    result = new Fraction(0, 1);
                for (int j = 0; j < Size; j++)
                {
                    result += (dynamic)(object)OriginalMatrix[i, j] * (dynamic)(object)solution[j, 0];
                }
                CalculatedVector[i, 0] = (T)(object)result;
            }
        }

        private void ReduceMatrix(int i)
        {
            for (int j = i + 1; j < Size; j++)
            {
                dynamic factor = (dynamic)(object)Matrix[j, i] / (dynamic)(object)Matrix[i, i];
                Vector[j, 0] = (T)(object)((dynamic)(object)Vector[j, 0] - factor * (dynamic)(object)Vector[i, 0]);

                for (int k = i; k < Size; k++)
                    Matrix[j, k] = (T)(object)((dynamic)(object)Matrix[j, k] - factor * (dynamic)(object)Matrix[i, k]);
            }
        }

        private void CalculateSolution(T[,] solution)
        {
            for (int i = Size - 1; i >= 0; i--)
            {
                dynamic result = 0;
                if (Type.FullName.Equals("Projekt2.Fraction"))
                    result = new Fraction(0, 1);
                for (int j = i + 1; j < Size; j++)
                {
                    result += (dynamic)(object)Matrix[i, j] * (dynamic)(object)solution[j, 0];
                }
                solution[i, 0] = (T)(object)(((dynamic)(object)Vector[i, 0] - result) / (dynamic)(object)Matrix[i, i]);
            }
        }

        private void CalculateError()
        {
            if (Type.FullName.Equals("Projekt2.Fraction"))
            {
                Fraction result = new Fraction(0, 1);
                for (int i = 0; i < Size; i++)
                {
                    OriginalVector[i, 0] = (T)(object)Fraction.Abs((Fraction)(object)OriginalVector[i, 0] - (Fraction)(object)CalculatedVector[i, 0]);

                    if ((Fraction)(object)OriginalVector[i, 0] > (Fraction)(object)result)
                        result = (Fraction)(object)OriginalVector[i, 0];
                }
                Error = (T)(object)result;
                ErrorString = Error.ToString();
                return;
            }
            else
            {
                for (int i = 0; i < Size; i++)
                {
                    OriginalVector[i, 0] = (T)(object)Math.Abs((dynamic)(object)OriginalVector[i, 0] - (dynamic)(object)CalculatedVector[i, 0]);
                }
                Error = (T)(object)OriginalVector.Cast<dynamic>().Max();
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Type Type;
        public int Size;
        public T[,] Matrix;
        public T[,] CalculatedVector;
        public T[,] Vector;
        public T VectorLength;
        public T Error;
        public T[,] OriginalMatrix;
        public T[,] OriginalVector;
        public string ErrorString;
        public long MethodExecutionTime;
    }
}
