using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class SeidelSolverTests
    {
        public const float Eps = 0.01f;

        [TestMethod]
        public void CompareVectors_First()
        {
            // Arrange.
            var matrix = GetMatrix("A1.txt");
            var vector = GetVector("B1.txt");
            var xExpected = GetVector("X1.txt");

            // Act.
            var xActual = SeidelSolver.Solve(matrix, vector);

            // Assert.
            for (int i = 0; i < vector.Length; i++)
            {
                Assert.AreEqual(xExpected[i], xActual[i], Eps);
            }
        }

        [TestMethod]
        public void CompareVectors_Second()
        {
            // Arrange.
            var matrix = GetMatrix("A2.txt");
            var vector = GetVector("B2.txt");
            var xExpected = GetVector("X2.txt");

            // Act.
            var xActual = SeidelSolver.Solve(matrix, vector);

            // Assert.
            for (int i = 0; i < vector.Length; i++)
            {
                Assert.AreEqual(xExpected[i], xActual[i], Eps);
            }
        }

        private IReadOnlyList<float[]> GetMatrix(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var reader = new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Files.{filename}")))
            {
                var matrix = new List<float[]>();

                while (!reader.EndOfStream)
                {
                    var elements = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    matrix.Add(new float[elements.Length]);

                    for (int i = 0; i < elements.Length; i++)
                    {
                        matrix[matrix.Count - 1][i] = Convert.ToSingle(elements[i]);
                    }
                }

                return matrix;
            }
        }

        private float[] GetVector(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var reader = new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Files.{filename}")))
            {
                var elements = reader.ReadToEnd().Split(new char[] { '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var vector = new float[elements.Length];

                for (int i = 0; i < elements.Length; i++)
                {
                    vector[i] = Convert.ToSingle(elements[i]);
                }

                return vector;
            }
        }
    }
}