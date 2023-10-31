using System;
using System.Collections.Generic;

namespace Server.Math
{
    public static class SeidelSolver
    {
        public const float Eps = 0.00001f;

        public static float[] Solve(IReadOnlyList<float[]> matrix, float[] b)
        {
            var x = new float[b.Length];
            var xNew = new float[b.Length];
            bool converge = false;

            while (!converge)
            {
                var loss = 0.0f;
                Array.Copy(x, xNew, x.Length);

                for (var i = 0; i < matrix.Count; i++)
                {
                    var sum1 = 0.0f;
                    var sum2 = 0.0f;

                    for (var j = 0; j < i; j++)
                    {
                        sum1 += matrix[i][j] * xNew[j];
                    }

                    for (var j = i + 1; j < matrix.Count; j++)
                    {
                        sum2 += matrix[i][j] * x[j];
                    }

                    xNew[i] = (b[i] - sum1 - sum2) / matrix[i][i];
                    loss += (float)System.Math.Pow(xNew[i] - x[i], 2);
                }

                converge = System.Math.Sqrt(loss) <= Eps;
                Array.Copy(xNew, x, xNew.Length);
            }

            return x;
        }
    }
}