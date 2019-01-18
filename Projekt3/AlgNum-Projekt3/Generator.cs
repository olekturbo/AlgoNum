using System;
using System.Collections.Generic;

namespace AlgNum_Projekt3
{
    public class Generator
    {
        #region Properties
        public int TotalAgents { get; set; }

        public int TotalCases { get; set; }

        public List<Protocol> Cases { get; set; }

        public double[,] OriginalMatrix { get; set; }

        public double[,] GaussPartialMatrix { get; set; }

        public double[] GaussPartialVector { get; set; }

        public double[] GaussPartialSolution { get; set; }

        public double GaussPartialTiming { get; set; }

        public double[,] SparseGaussPartialMatrix { get; set; }

        public double[] SparseGaussPartialVector { get; set; }

        public double[] SparseGaussPartialSolution { get; set; }

        public double SparseGaussPartialTiming { get; set; }

        public double[,] JacobiMatrix { get; set; }

        public double[] JacobiVector { get; set; }

        public double[] JacobiSolution { get; set; }

        public double JacobiTiming { get; set; }

        public double[,] GaussSeidelMatrix { get; set; }

        public double[] GaussSeidelVector { get; set; }

        public double[] GaussSeidelSolution { get; set; }

        public double GaussSeidelTiming { get; set; }

        public double[] OriginalVector { get; set; }

        public double GaussSeidelError { get; set; }

        public double JacobiError { get; set; }

        public double GaussPartialError { get; set; }

        public double SparseGaussPartialError { get; set; }

        public int MaxIterations { get; set; }

        public double GaussMonteCarloError { get; set; }

        public double SparseGaussMonteCarloError { get; set; }

        public double JacobiMonteCarloError { get; set; }

        public double GaussSeidelMonteCarloError { get; set; }

        public MonteCarlo MonteCarloSimulation { get; set; }

        public double Epsilon { get; set; }
        #endregion

        public Generator(int totalAgents, int maxIterations, double epsilon = 0)
        {
            TotalAgents = totalAgents;
            GenerateCases();
            TotalCases = Cases.Count;
            OriginalMatrix = new double[TotalCases, TotalCases];
            OriginalVector = new double[TotalCases];
            MaxIterations = maxIterations;
            Epsilon = epsilon;
            GenerateEquation();

            MonteCarloSimulation = new MonteCarlo(1, TotalAgents);

            GaussPartialMatrix = (double[,])OriginalMatrix.Clone();
            GaussPartialVector = (double[])OriginalVector.Clone();
            GaussPartialSolution = GaussWithPartialPivoting(GaussPartialMatrix, GaussPartialVector, false);
            GaussMonteCarloError = CalculateMonteCarloError(GaussPartialSolution);
            GaussPartialError = CalculateError(GaussPartialSolution);

            SparseGaussPartialMatrix = (double[,])OriginalMatrix.Clone();
            SparseGaussPartialVector = (double[])OriginalVector.Clone();
            SparseGaussPartialSolution = GaussWithPartialPivoting(SparseGaussPartialMatrix, SparseGaussPartialVector, true);
            SparseGaussMonteCarloError = CalculateMonteCarloError(SparseGaussPartialSolution);
            SparseGaussPartialError = CalculateError(SparseGaussPartialSolution);

            JacobiMatrix = (double[,])OriginalMatrix.Clone();
            JacobiVector = (double[])OriginalVector.Clone();
            JacobiSolution = Jacobi();
            JacobiMonteCarloError = CalculateMonteCarloError(JacobiSolution);
            JacobiError = CalculateError(JacobiSolution);

            GaussSeidelMatrix = (double[,])OriginalMatrix.Clone();
            GaussSeidelVector = (double[])OriginalVector.Clone();
            GaussSeidelSolution = GaussSeidel();
            GaussSeidelMonteCarloError = CalculateMonteCarloError(GaussSeidelSolution);
            GaussSeidelError = CalculateError(GaussSeidelSolution);
        }

        public void GenerateEquation()
        {
            //Always 0, last vector is an Case when we have 3 yes agents so P(3,0) will be 1.
            for (var i = 0; i < TotalCases - 1; i++)
                OriginalVector[i] = 0;
            OriginalVector[TotalCases - 1] = 1;

            for (var i = 0; i < TotalCases; i++)
            {
                for (var j = 0; j < TotalCases; j++)
                {
                    OriginalMatrix[i, j] = GenerateValue(i, j);
                }
            }
        }

        private double GenerateValue(int i, int j)
        {
            var yesCountsRow = Cases[i].YesVoters;
            var noCountsRow = Cases[i].NoVoters;
            var undecidedCountsRow = Cases[i].UndecidedVoters;

            var yesCountsColumn = Cases[j].YesVoters;
            var noCountsColumn = Cases[j].NoVoters;

            if ((yesCountsRow == TotalAgents || noCountsRow == TotalAgents) && i == j)
                return 1;

            if (yesCountsRow == 0 && noCountsRow == 0 && i == j)
                return 1;

            if (yesCountsRow == yesCountsColumn && noCountsRow == noCountsColumn
               && (yesCountsRow > 1 || noCountsRow > 1 || undecidedCountsRow > 1))
                return UndecidedAgentsChosenCase(Cases[i]);

            if (yesCountsRow + 1 == yesCountsColumn && noCountsRow == noCountsColumn
               && yesCountsRow > 0 && undecidedCountsRow > 0)
                return UndecidedAgentChosenCase(Cases[i], Cases[i].YesVoters);

            if (yesCountsRow == yesCountsColumn && noCountsRow + 1 == noCountsColumn
               && noCountsRow > 0 && undecidedCountsRow > 0)
                return UndecidedAgentChosenCase(Cases[i], Cases[i].NoVoters);

            if (yesCountsRow - 1 == yesCountsColumn && noCountsRow - 1 == noCountsColumn
               && yesCountsRow > 0 && noCountsRow > 0)
                return YesAndNoAgentChosenCase(Cases[i]);

            if (i == j)
                return -1;

            return 0;
        }

        private double UndecidedAgentsChosenCase(Protocol currentCase)
        {
            double result = -1;
            double baseProbability = (double)Newton((long)TotalAgents, 2);
            if (currentCase.YesVoters > 1)
                result += (double)Newton(currentCase.YesVoters, 2) / baseProbability;

            if (currentCase.NoVoters > 1)
                result += (double)Newton(currentCase.NoVoters, 2) / baseProbability;

            if (currentCase.UndecidedVoters > 1)
                result += (double)Newton(currentCase.UndecidedVoters, 2) / baseProbability;

            return result;
        }

        private double UndecidedAgentChosenCase(Protocol currentCase, int conditionValue)
        {
            double result = 0;
            double baseProbability = (double)Newton((long)TotalAgents, 2);

            if (conditionValue > 0 && currentCase.UndecidedVoters > 0)
                result = (double)(conditionValue * (currentCase.UndecidedVoters)) / baseProbability;

            return result;
        }

        private double YesAndNoAgentChosenCase(Protocol currentCase)
        {
            double baseProbability = (double)Newton((long)TotalAgents, 2);
            double result = 0;

            if (currentCase.NoVoters > 0 && currentCase.YesVoters > 0)
                result = (double)(currentCase.NoVoters * currentCase.YesVoters) / baseProbability;

            return result;
        }

        private long Newton(long n, long k)
        {
            if (k == n || k == 0)
                return 1;
            return Newton(n - 1, k - 1) + Newton(n - 1, k);
        }

        private void GenerateCases()
        {
            Cases = new List<Protocol>();
            for (var i = 0; i <= TotalAgents; i++)
            {
                for (var j = 0; j <= TotalAgents; j++)
                {
                    if (i + j <= TotalAgents)
                        Cases.Add(new Protocol(TotalAgents, i, j));
                }
            }
        }

        public double[] GaussWithPartialPivoting(double[,] Matrix, double[] Vector, bool isSparse)
        {
            DateTime startTime = DateTime.Now;
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
                    if ((isSparse && Matrix[j, i] != 0) || !isSparse)
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

            DateTime endTime = DateTime.Now;
            if (isSparse)
            {
                Console.Write("(SPARSE) ");
                SparseGaussPartialTiming = (endTime - startTime).TotalMilliseconds;
            }
            else
            {
                GaussPartialTiming = (endTime - startTime).TotalMilliseconds;
            }
            Console.WriteLine("Gauss With Partial Pivoting Time: " + (endTime - startTime).TotalMilliseconds + "ms");
            return solution;
        }

        private double CalculateError(double[] solution)
        {
            double[,] originalMatrix = (double[,])OriginalMatrix.Clone();
            double[] originalVector = (double[])OriginalVector.Clone();
            double[] vectorToCompare = new double[TotalCases];
            double max = 0;
            double result = 0;

            for (int i = 0; i < TotalCases; i++)
            {
                for (int j = 0; j < TotalCases; j++)
                {
                    result += originalMatrix[i, j] * solution[j];
                }
                vectorToCompare[i] = result;
            }

            for (int i = 0; i < TotalCases; i++)
            {
                if (Math.Abs(originalVector[i] - vectorToCompare[i]) > max)
                {
                    max = Math.Abs(originalVector[i] - vectorToCompare[i]);
                }
            }

            return max;
        }

        private double CalculateMonteCarloError(double[] solution)
        {
            double sum = 0;

            for (int i = 0; i < TotalCases; i++)
                sum += Math.Abs(MonteCarloSimulation.FinalVector[i] - solution[i]);

            return sum / TotalAgents;
        }

        public double[] Jacobi()
        {
            DateTime startTime = DateTime.Now;
            double[] previous = new double[TotalCases];
            double[] result = new double[TotalCases];
            double iterations = 0;
            bool run = true;

            while (run)
            {
                for (int i = 0; i < TotalCases; i++)
                {
                    double sum = JacobiVector[i];

                    for (int j = 0; j < TotalCases; j++)
                    {
                        if (j != i)
                        {
                            sum -= JacobiMatrix[i, j] * previous[j];
                        }
                    }
                    result[i] = 1 / JacobiMatrix[i, i] * sum;
                }

                iterations++;
                run = false;

                for (int i = 0; i < TotalCases; i++)
                {
                    if (Math.Abs(result[i] - previous[i]) > Epsilon || iterations == MaxIterations)
                        run = true;
                }


                previous = (double[])result.Clone();
            }
            DateTime endTime = DateTime.Now;

            JacobiTiming = (endTime - startTime).TotalMilliseconds;
            Console.WriteLine("Jacobi Time: " + (endTime - startTime).TotalMilliseconds + "ms");

            return result;
        }

        public double[] GaussSeidel()
        {
            DateTime startTime = DateTime.Now;
            double[] result = new double[TotalCases];
            double[] previous = new double[TotalCases];
            double iterations = 0;
            bool run = true;

            while (run)
            {
                for (int i = 0; i < TotalCases; i++)
                {
                    double sum = GaussSeidelVector[i];

                    for (int j = 0; j < TotalCases; j++)
                    {
                        if (j != i)
                        {
                            sum -= GaussSeidelMatrix[i, j] * result[j];
                        }
                    }

                    result[i] = 1 / GaussSeidelMatrix[i, i] * sum;
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

            DateTime endTime = DateTime.Now;

            GaussSeidelTiming = (endTime - startTime).TotalMilliseconds;
            Console.WriteLine("Gauss Seidel Time: " + (endTime - startTime).TotalMilliseconds + "ms");
            return result;
        }
    }
}
