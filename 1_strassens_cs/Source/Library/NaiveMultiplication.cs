using System.Numerics;

namespace StrassensOptimized
{
    public partial class Matrix
    {
        public static Matrix operator *(Matrix one, Matrix two)
        {
            var twoTranspose = new double[two.columns][];
            for (var i = 0; i < two.columns; i++)
            {
                twoTranspose[i] = new double[two.rows];
                for (var j = 0; j < two.rows; j++)
                {
                    twoTranspose[i][j] = two.matrix[j][i];
                }
            }
            var matrix = new double[one.rows][];
            for (var i = 0; i < one.rows; i++)
            {
                matrix[i] = new double[one.columns];
                for (var j = 0; j < two.columns; j++)
                {
                    matrix[i][j] = 0;
                    var k = 0;
                    for (; k < one.columns - Vector<double>.Count; k += Vector<double>.Count)
                    {
                        matrix[i][j] += Vector.Dot<double>(new Vector<double>(one.matrix[i], k), new Vector<double>(twoTranspose[j], k));
                    }
                    for (; k < one.columns; k++)
                    {
                        matrix[i][j] += one.matrix[i][k] * twoTranspose[j][k];
                    }
                }
            }
            return new Matrix(matrix);
        }
        private static void NaiveMultiply(Vector<double>[][] a, int a1, int a2, Vector<double>[][] b, int b1, int b2, Vector<double>[][] o, int d1, int d2)
        {
            var temp = new double[Vector<double>.Count];
            var bT = new Vector<double>[d1][];
            int m;
            var n = 0;
            for (var i = 0; i < d1; i++)
            {
                bT[i] = new Vector<double>[d2];
            }
            for (var i = 0; i < d2; i++)
            {
                m = 0;
                for (var j = 0; j < d2; j++)
                {
                    for (var k = 0; k < Vector<double>.Count; k++)
                    {
                        for (var l = 0; l < Vector<double>.Count; l++)
                        {
                            temp[l] = b[n + b1 + l][j + b2][k];
                        }
                        bT[m + k][i] = new Vector<double>(temp);
                    }
                    m += Vector<double>.Count;
                }
                n += Vector<double>.Count;
            }
            for (var i = 0; i < d1; i++)
            {
                o[i] = new Vector<double>[d2];
                m = 0;
                for (var j = 0; j < d1 / Vector<double>.Count; j++)
                {
                    for (var l = 0; l < Vector<double>.Count; l++)
                    {
                        temp[l] = 0;
                        for (var k = 0; k < d2; k++)
                        {
                            temp[l] += Vector.Dot<double>(a[i + a1][k + a2], bT[m + l][k]);
                        }
                    }
                    o[i][j] = new Vector<double>(temp);
                    m += Vector<double>.Count;
                }
            }
        }
    }
}