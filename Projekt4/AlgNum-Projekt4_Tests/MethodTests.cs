using System;
using AlgNum_Projekt3;
using AlgNum_Projekt3.Methods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgNum_Projekt4_Tests
{
    [TestClass]
    public class MethodTests
    {
        private static int _generatorTotalCases;
        private static double[,] _generatorMatrix;
        private static double[] _generatorVector;
        private static Generator _ownSparseGenerator;
        private static Generator _generator;

        [TestInitialize]
        public void Init()
        {
            _generator = new Generator(5);
            _ownSparseGenerator = new Generator(5, true);
            _generatorTotalCases = _generator.TotalCases;
            _generatorMatrix = (double[,])_generator.OriginalMatrix.Clone();
            _generatorVector = (double[])_generator.OriginalVector.Clone();
        }

        [TestMethod]
        public void AllMethodsTest()
        {
            double epsilon = Math.Pow(10, -10);
            int maxIterations = 10000;
            GaussSeidel gaussSeidel = new GaussSeidel(_generatorTotalCases, _generatorMatrix, _generatorVector, maxIterations, epsilon);
            PartialGauss sparsePartialGauss = new PartialGauss(_generatorTotalCases, _generatorMatrix, _generatorVector, true);
            PartialGauss partialGauss = new PartialGauss(_generatorTotalCases, _generatorMatrix, _generatorVector, false);
            SparseLUSolver sparseLUSolver = new SparseLUSolver(_generatorTotalCases, _generatorMatrix, _generatorVector);
            OwnSparseSolver ownSparseSolver = new OwnSparseSolver(_ownSparseGenerator.GenerateOwnSparseMatrix(), _generatorVector, maxIterations, epsilon);
        }
    }
}
