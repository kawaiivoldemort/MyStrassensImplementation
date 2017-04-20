using System.Numerics;

namespace StrassensOptimized
{
    public partial class Matrix
    {
        public bool AlmostEquals(Matrix otherMatrix, double precision)
        {
            if ((this.rows != otherMatrix.rows) || (this.columns != otherMatrix.columns)) return false;
            for (int i = 0; i < this.rows; i++)
            {
                int j = 0;
                for (; j < this.columns - Vector<double>.Count; j += Vector<double>.Count)
                {
                    var diff = Vector.Abs<double>((new Vector<double>(this.matrix[i], j) - new Vector<double>(otherMatrix.matrix[i], j)));
                    for (int k = 0; k < Vector<double>.Count; k++)
                    {
                        if (diff[k] > precision)
                        {
                            return false;
                        }
                    }
                }
                for (; j < this.columns; j++)
                {
                    if (System.Math.Abs(this.matrix[i][j] - otherMatrix.matrix[i][j]) > precision)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}