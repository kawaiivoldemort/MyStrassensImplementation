using System.Threading.Tasks;
using System.Numerics;
using System.Collections.Concurrent;

namespace StrassensOptimized
{
    public partial class Matrix
    {
        public static Matrix Strassens(Matrix x, Matrix y)
        {
            var xv = new Vector<double>[x.rows][];
            var yv = new Vector<double>[y.rows][];
            Vectorize(x, y, xv, yv);
            var ov = StrassensStep(xv, 0, 0, yv, 0, 0, x.rows, (x.columns / Vector<double>.Count));
            var o = new double[x.rows][];
            Devectorize(ov, o);
            return new Matrix(o);
        }
        private static Vector<double>[][] StrassensStep(Vector<double>[][] a, int a1, int a2, Vector<double>[][] b, int b1, int b2, int d1, int d2)
        {
            var o = new Vector<double>[d1][];
            if (d1 <= 16)
            {
                NaiveMultiply(a, a1, a2, b, b1, b2, o, d1, d2);
            }
            else
            {
                d1 /= 2;
                d2 /= 2;
                var t0 = new Vector<double>[d1][];
                var t1 = new Vector<double>[d1][];
                var t2 = new Vector<double>[d1][];
                var t3 = new Vector<double>[d1][];
                var t4 = new Vector<double>[d1][];
                var t5 = new Vector<double>[d1][];
                var t6 = new Vector<double>[d1][];
                var t7 = new Vector<double>[d1][];
                var t8 = new Vector<double>[d1][];
                var t9 = new Vector<double>[d1][];
                ComputeT(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, a, a1, a2, b, b1, b2, d1, d2);
                var m0 = StrassensStep(t0, 0, 0, t1, 0, 0, d1, d2);
                var m1 = StrassensStep(t2, 0, 0, b, b1, b2, d1, d2);
                var m2 = StrassensStep(a, a1, a2, t3, 0, 0, d1, d2);
                var m3 = StrassensStep(a, a1 + d1, a2 + d2, t4, 0, 0, d1, d2);
                var m4 = StrassensStep(t5, 0, 0, b, b1 + d1, b2 + d2, d1, d2);
                var m5 = StrassensStep(t6, 0, 0, t7, 0, 0, d1, d2);
                var m6 = StrassensStep(t8, 0, 0, t9, 0, 0, d1, d2);
                StrassensCombine(m0, m1, m2, m3, m4, m5, m6, o, d1, d2);
            }
            return o;
        }        
        public static Matrix StrassensP(Matrix x, Matrix y, int threads)
        {
            var xv = new Vector<double>[x.rows][];
            var yv = new Vector<double>[y.rows][];
            Vectorize(x, y, xv, yv);
            var task = StrassensPStep(xv, 0, 0, yv, 0, 0, x.rows, (x.columns / Vector<double>.Count), threads);
            task.Wait();
            var o = new double[x.rows][];
            Devectorize(task.Result, o);
            return new Matrix(o);
        }
        private static async Task<Vector<double>[][]> StrassensPStep(Vector<double>[][] a, int a1, int a2, Vector<double>[][] b, int b1, int b2, int d1, int d2, int threads)
        {
            var o = new Vector<double>[d1][];
            if (threads < 2)
            {
                return StrassensStep(a, a1, a2, b, b1, b2, d1, d2);
            }
            if (d1 <= 16)
            {
                NaiveMultiply(a, a1, a2, b, b1, b2, o, d1, d2);
            }
            else
            {
                d1 /= 2;
                d2 /= 2;
                var t0 = new Vector<double>[d1][];
                var t1 = new Vector<double>[d1][];
                var t2 = new Vector<double>[d1][];
                var t3 = new Vector<double>[d1][];
                var t4 = new Vector<double>[d1][];
                var t5 = new Vector<double>[d1][];
                var t6 = new Vector<double>[d1][];
                var t7 = new Vector<double>[d1][];
                var t8 = new Vector<double>[d1][];
                var t9 = new Vector<double>[d1][];
                ComputeT(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, a, a1, a2, b, b1, b2, d1, d2);
                Vector<double>[][] m0 = null;
                Vector<double>[][] m1 = null;
                Vector<double>[][] m2 = null;
                Vector<double>[][] m3 = null;
                Vector<double>[][] m4 = null;
                Vector<double>[][] m5 = null;
                Vector<double>[][] m6 = null;
                var tasks = new Task[2];
                tasks[0] = Task.Run(() =>
                    {
                        m0 = StrassensPStep(t0, 0, 0, t1, 0, 0, d1, d2, threads / 2).Result;
                        m1 = StrassensPStep(t2, 0, 0, b, b1, b2, d1, d2, threads / 2).Result;
                        m2 = StrassensPStep(a, a1, a2, t3, 0, 0, d1, d2, threads / 2).Result;
                    });
                tasks[1] = Task.Run(() =>
                    {
                        m3 = StrassensPStep(a, a1 + d1, a2 + d2, t4, 0, 0, d1, d2, threads / 2).Result;
                        m4 = StrassensPStep(t5, 0, 0, b, b1 + d1, b2 + d2, d1, d2, threads / 2).Result;
                        m5 = StrassensPStep(t6, 0, 0, t7, 0, 0, d1, d2, threads / 2).Result;
                        m6 = StrassensPStep(t8, 0, 0, t9, 0, 0, d1, d2, threads / 2).Result;
                    });
                await Task.WhenAll(tasks);
                StrassensCombine(m0, m1, m2, m3, m4, m5, m6, o, d1, d2);
            }
            return o;
        }
        public static void Vectorize(Matrix x, Matrix y, Vector<double>[][] xv, Vector<double>[][] yv)
        {
            var osize = x.rows / Vector<double>.Count;
            for (var i = 0; i < x.rows; i++)
            {
                xv[i] = new Vector<double>[osize];
                yv[i] = new Vector<double>[osize];
                for (var j = 0; j < osize; j ++)
                {
                    xv[i][j] = new Vector<double>(x.matrix[i], j * Vector<double>.Count);
                    yv[i][j] = new Vector<double>(y.matrix[i], j * Vector<double>.Count);
                }
            }
        }
        public static void Devectorize(Vector<double>[][] ov, double[][] o)
        {
            int k;
            for (var i = 0; i < o.Length; i += 1)
            {
                o[i] = new double[ov[0].Length * Vector<double>.Count];
                k = 0;
                for (var j = 0; j < ov[0].Length; j++)
                {
                    ov[i][j].CopyTo(o[i], k);
                    k += Vector<double>.Count;
                }
            }
        }
        private static void ComputeT(Vector<double>[][] t0, Vector<double>[][] t1, Vector<double>[][] t2, Vector<double>[][] t3, Vector<double>[][] t4, Vector<double>[][] t5, Vector<double>[][] t6, Vector<double>[][] t7, Vector<double>[][] t8, Vector<double>[][] t9, Vector<double>[][] a, int a1, int a2, Vector<double>[][] b, int b1, int b2, int d1, int d2)
        {
            for (var i = 0; i < d1; i++)
            {
                t0[i] = new Vector<double>[d2];
                t1[i] = new Vector<double>[d2];
                t2[i] = new Vector<double>[d2];
                t3[i] = new Vector<double>[d2];
                t4[i] = new Vector<double>[d2];
                t5[i] = new Vector<double>[d2];
                t6[i] = new Vector<double>[d2];
                t7[i] = new Vector<double>[d2];
                t8[i] = new Vector<double>[d2];
                t9[i] = new Vector<double>[d2];
                for (var j = 0; j < d2; j++)
                {
                    t0[i][j] = a[a1 + i][a2 + j] + a[a1 + i + d1][a2 + j + d2];
                    t1[i][j] = b[b1 + i][b2 + j] + b[b1 + i + d1][b2 + j + d2];
                    t2[i][j] = a[a1 + i + d1][a2 + j] + a[a1 + i + d1][a2 + j + d2];
                    t3[i][j] = b[b1 + i][b2 + j + d2] - b[b1 + i + d1][b2 + j + d2];
                    t4[i][j] = b[b1 + i + d1][b2 + j] - b[b1 + i][b2 + j];
                    t5[i][j] = a[a1 + i][a2 + j] + a[a1 + i][a2 + j + d2];
                    t6[i][j] = a[a1 + i + d1][a2 + j] - a[a1 + i][a2 + j];
                    t7[i][j] = b[b1 + i][b2 + j] + b[b1 + i][b2 + j + d2];
                    t8[i][j] = a[a1 + i][a2 + j + d2] - a[a1 + i + d1][a2 + j + d2];
                    t9[i][j] = b[b1 + i + d1][b2 + j] + b[b1 + i + d1][b2 + j + d2];
                }
            }
        }
        private static void StrassensCombine(Vector<double>[][] m0, Vector<double>[][] m1, Vector<double>[][] m2, Vector<double>[][] m3, Vector<double>[][] m4, Vector<double>[][] m5, Vector<double>[][] m6, Vector<double>[][] o, int d1, int d2)
        {
            var d22 = d2 * 2;
            for (var i = 0; i < d1; i++)
            {
                o[i] = new Vector<double>[d22];
                o[i + d1] = new Vector<double>[d22];
                for (var j = 0; j < d2; j++)
                {
                    o[i][j] = m0[i][j] + m3[i][j]- m4[i][j]+ m6[i][j];
                    o[i][j + d2] = m2[i][j] + m4[i][j];
                    o[i + d1][j] = m1[i][j] + m3[i][j];
                    o[i + d1][j + d2] = m0[i][j] - m1[i][j]+ m2[i][j]+ m5[i][j];
                }
            }
        }
    }
}