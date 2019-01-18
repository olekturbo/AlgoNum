using CSparse.Double.Factorization;
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Storage;
using System.Linq;

namespace AlgNum_Projekt3
{
    public class Generator
    {
        #region Properties
        public int TotalAgents { get; set; }

        public int TotalCases { get; set; }

        public List<Protocol> Cases { get; set; }

        public double[,] OriginalMatrix { get; set; }

        public double[] OriginalVector { get; set; }

        public double GenerateEquationTiming { get; set; }

        public SparseLU SparseLUMatrix { get; set; }

        SparseLU LargeMatrix { get; set; }

        public double[] LargeVector { get; set; }

        public List<double[]> RowsOfArray { get; set; }

        #endregion

        public Generator(int totalAgents)
        {
            TotalAgents = totalAgents;
            GenerateCases();
            TotalCases = Cases.Count;
            OriginalMatrix = new double[TotalCases, TotalCases];
            OriginalVector = new double[TotalCases];
            GenerateEquation();
        }

        public Generator(int totalAgents, bool isOwn = false)
        {
            TotalAgents = totalAgents;
            GenerateCases();
            TotalCases = Cases.Count;
            LargeVector = new double[TotalCases];
        }

        public void GenerateEquation()
        {        
            var timer = System.Diagnostics.Stopwatch.StartNew();
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

            timer.Stop();
            GenerateEquationTiming = timer.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
        }

        public SparseMatrix GenerateOwnSparseMatrix()
        {
            int startRowIndex = 0;
            bool newRowStarted;
            bool valueAdded;

            List<double> values = new List<double>();
            List<int> columnIndexes = new List<int>();
            List<int> rowIndexes = new List<int>();

            //Always 0, last vector is an Case when we have 3 yes agents so P(3,0) will be 1.
            for (var i = 0; i < TotalCases - 1; i++)
                LargeVector[i] = 0;
            LargeVector[TotalCases - 1] = 1;

            for (int i = 0; i < TotalCases; i++)
            {
                valueAdded = false;
                newRowStarted = false;

                for (int j = 0; j < TotalCases; j++)
                {
                    var value = GenerateValue(i, j);

                    if (value != 0)
                    {
                        if (!newRowStarted)
                        {
                            startRowIndex = values.Count;
                            newRowStarted = true;
                        }

                        values.Add(value);
                        columnIndexes.Add(j);
                        valueAdded = true;
                    }
                }

                if (valueAdded)
                    rowIndexes.Add(startRowIndex);
            }

            SparseMatrix sparseMatrix = new SparseMatrix(values.ToArray(), rowIndexes.ToArray(), columnIndexes.ToArray());

            return sparseMatrix;
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
            double baseProbability = TotalAgents * (TotalAgents - 1) / 2;
            if (currentCase.YesVoters > 1)
                result += (currentCase.YesVoters * (currentCase.YesVoters - 1) / 2) / baseProbability;

            if (currentCase.NoVoters > 1)
                result += (currentCase.NoVoters * (currentCase.NoVoters - 1) / 2) / baseProbability;

            if (currentCase.UndecidedVoters > 1)
                result += (currentCase.UndecidedVoters * (currentCase.UndecidedVoters-1) / 2) / baseProbability;

            return result;
        }

        private double UndecidedAgentChosenCase(Protocol currentCase, int conditionValue)
        {
            double result = 0;
            double baseProbability = TotalAgents * (TotalAgents - 1) / 2;

            if (conditionValue > 0 && currentCase.UndecidedVoters > 0)
                result = (double)(conditionValue * (currentCase.UndecidedVoters)) / baseProbability;

            return result;
        }

        private double YesAndNoAgentChosenCase(Protocol currentCase)
        {
            double baseProbability = TotalAgents * (TotalAgents-1) / 2;
            double result = 0;

            if (currentCase.NoVoters > 0 && currentCase.YesVoters > 0)
                result = (double)(currentCase.NoVoters * currentCase.YesVoters) / baseProbability;

            return result;
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
        
    }
}
