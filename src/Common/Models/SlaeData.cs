using System;
using System.Collections.Generic;

namespace Common.Models
{
    /// <summary>
    /// Модель данных для матрицы коэффициентов и вектора свободных членов.
    /// </summary>
    public class SlaeData
    {
        /// <summary>
        /// Инициализирует новый объект типа <see cref="SlaeData"/>.
        /// </summary>
        /// <param name="matrix">Матрица коэффициентов.</param>
        /// <param name="vector">Вектор свободных членов.</param>
        /// <exception cref="ArgumentNullException">Если матрица коэффициентов или вектор свободных членов являются null.</exception>
        /// <exception cref="ArgumentException">Если количество уравнений не совпадает с количеством свободных членов.</exception>
        public SlaeData(IReadOnlyList<float[]> matrix, float[] vector)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            if (vector == null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            if (matrix.Count != vector.Length)
            {
                throw new ArgumentException("Количество уравнений не совпадает с количеством свободных членов");
            }

            Matrix = matrix;
            Vector = vector;
        }

        /// <summary>
        /// Возвращает матрицу коэффициентов.
        /// </summary>
        public IReadOnlyList<float[]> Matrix { get; }

        /// <summary>
        /// Возвращает вектор свободных членов.
        /// </summary>
        public float[] Vector { get; }
    }
}