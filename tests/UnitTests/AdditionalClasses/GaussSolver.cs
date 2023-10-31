using System.Collections.Generic;

namespace UnitTests.AdditionalClasses
{
    public static class GaussSolver
    {
        public static float[] Solve(IReadOnlyList<float[]> matrix, float[] b)
        {
            matrix = CreateNewMatrix(matrix, b);
            var n = matrix.Count;
            var matrixClone = new float[n, n + 1];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    matrixClone[i, j] = matrix[i][j];
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n + 1; i++)
                {
                    if (matrix[k][k] != 0)
                    {
                        matrixClone[k, i] = matrixClone[k, i] / matrix[k][k];
                    }
                }

                for (int i = k + 1; i < n; i++)
                {
                    if (matrixClone[k, k] != 0)
                    {
                        var K = matrixClone[i, k] / matrixClone[k, k];

                        for (int j = 0; j < n + 1; j++)
                        {
                            matrixClone[i, j] = matrixClone[i, j] - matrixClone[k, j] * K;
                        }
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n + 1; j++)
                    {
                        matrix[i][j] = matrixClone[i, j];
                    }
                }
            }

            for (int k = n - 1; k > -1; k--)
            {
                for (int i = n; i > -1; i--)
                {
                    if (matrix[k][k] != 0)
                    {
                        matrixClone[k, i] = matrixClone[k, i] / matrix[k][k];
                    }
                }

                for (int i = k - 1; i > -1; i--)
                {
                    if (matrixClone[k, k] != 0)
                    {
                        var K = matrixClone[i, k] / matrixClone[k, k];

                        for (int j = n; j > -1; j--)
                        {
                            matrixClone[i, j] = matrixClone[i, j] - matrixClone[k, j] * K;
                        }
                    }
                }
            }

            var x = new float[n];

            for (int i = 0; i < n; i++)
            {
                x[i] = matrixClone[i, n];
            }

            return x;
        }

        private static IReadOnlyList<float[]> CreateNewMatrix(IReadOnlyList<float[]> matrix, float[] b)
        {
            var newMatrix = new float[matrix.Count][];

            for (int i = 0; i < matrix.Count; i++)
            {
                newMatrix[i] = new float[matrix.Count + 1];

                for (int j = 0; j < matrix.Count + 1; j++)
                {
                    newMatrix[i][j] = j == matrix.Count ? b[i] : matrix[i][j];
                }
            }

            return newMatrix;
        }
    }
}