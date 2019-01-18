using System;
using AlgNum_Projekt3.Approximation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgNum_Projekt4_Tests
{
    [TestClass]
    public class ApproximationTests
    {
        // Test for example from PDF - Page 34 (https://mdl.ug.edu.pl/pluginfile.php/239499/course/section/75316/Aproksymacja.pdf)
        [TestMethod]
        public void ApproximationResultTest()
        {
            double[] arguments = {0, 0.25, 0.5, 0.75, 1};
            double[] values = {1, 1.284, 1.6487, 2.117, 2.7183};
            double[] expected = { 1.0051, 0.8647, 0.8432 };

            Approximation approximation = new Approximation();
            double[] actual = approximation.GetApproximation(2, arguments, values).Polynomial;

            Assert.AreEqual(expected[0], actual[0], 0.001);
            Assert.AreEqual(expected[1], actual[1], 0.001);
            Assert.AreEqual(expected[2], actual[2], 0.001);
        }
    }
}
