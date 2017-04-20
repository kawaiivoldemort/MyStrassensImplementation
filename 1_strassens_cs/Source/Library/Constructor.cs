using System.Numerics;

namespace StrassensOptimized
{
    public partial class Matrix
    {
        public static bool IsHardwareAccelerated = Vector.IsHardwareAccelerated;
        public static int ThreadCount = System.Environment.ProcessorCount;
        public double[][] matrix { get; }
        public int rows { get; }
        public int columns { get; }
        public Matrix(double[][] matrix)
        {
            this.matrix = matrix;
            this.columns = matrix.Length;
            this.rows = matrix[0].Length;
        }
    }
}