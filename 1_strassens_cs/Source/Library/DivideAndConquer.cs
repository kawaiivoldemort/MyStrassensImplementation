using System.Threading.Tasks;
using System.Numerics;

namespace StrassensOptimized
{
    public partial class Matrix
    {
        public static Matrix DivideAndConquer(Matrix x, Matrix y)
        {
            var xv = new Vector<double>[x.rows][];
            var yv = new Vector<double>[y.rows][];
            Vectorize(x, y, xv, yv);
            var ov = DivideAndConquerStep(xv, 0, 0, yv, 0, 0, x.rows, (x.columns / Vector<double>.Count));
            var o = new double[x.rows][];
            Devectorize(ov, o);
            return new Matrix(o);
        }
        private static Vector<double>[][] DivideAndConquerStep(Vector<double>[][] x, int x1, int x2, Vector<double>[][] y, int y1, int y2, int d1, int d2)
        {
            var o = new Vector<double>[d1][];
            if (d1 <= 16)
            {
                NaiveMultiply(x, x1, x2, y, y1, y2, o, d1, d2);
            }
            else
            {
                d1 /= 2;
                d2 /= 2;
                var c0 = DivideAndConquerStep(x, x1, x2, y, y1, y2, d1, d2);
                var c1 = DivideAndConquerStep(x, x1, x2 + d2, y, y1 + d1, y2, d1, d2);
                var c2 = DivideAndConquerStep(x, x1, x2, y, y1, y2 + d2, d1, d2);
                var c3 = DivideAndConquerStep(x, x1, x2 + d2, y, y1 + d1, y2 + d2, d1, d2);
                var c4 = DivideAndConquerStep(x, x1 + d1, x2, y, y1, y2, d1, d2);
                var c5 = DivideAndConquerStep(x, x1 + d1, x2 + d2, y, y1 + d1, y2, d1, d2);
                var c6 = DivideAndConquerStep(x, x1 + d1, x2, y, y1, y2 + d2, d1, d2);
                var c7 = DivideAndConquerStep(x, x1 + d1, x2 + d2, y, y1 + d1, y2 + d2, d1, d2);
                DivideAndConquerCombine(c0, c1, c2, c3, c4, c5, c6, c7, o, d1, d2);
            }
            return o;
        }
        public static Matrix DivideAndConquerP(Matrix x, Matrix y, int threads)
        {
            var xv = new Vector<double>[x.rows][];
            var yv = new Vector<double>[y.rows][];
            Vectorize(x, y, xv, yv);
            var task = DivideAndConquerPStep(xv, 0, 0, yv, 0, 0, x.rows, (x.columns / Vector<double>.Count), threads);
            task.Wait();
            var o = new double[x.rows][];
            Devectorize(task.Result, o);
            return new Matrix(o);
        }
        private static async Task<Vector<double>[][]> DivideAndConquerPStep(Vector<double>[][] x, int x1, int x2, Vector<double>[][] y, int y1, int y2, int d1, int d2, int threads)
        {
            var o = new Vector<double>[d1][];
            if (threads < 2)
            {
                return DivideAndConquerStep(x, x1, x2, y, y1, y2, d1, d2);
            }
            if (d1 <= 16)
            {
                NaiveMultiply(x, x1, x2, y, y1, y2, o, d1, d2);
            }
            else
            {
                d1 /= 2;
                d2 /= 2;
                Vector<double>[][] c0 = null;
                Vector<double>[][] c1 = null;
                Vector<double>[][] c2 = null;
                Vector<double>[][] c3 = null;
                Vector<double>[][] c4 = null;
                Vector<double>[][] c5 = null;
                Vector<double>[][] c6 = null;
                Vector<double>[][] c7 = null;
                var tasks = new Task[2];
                tasks[0] = Task.Run(() =>
                    {
                        c0 = DivideAndConquerPStep(x, x1, x2, y, y1, y2, d1, d2, threads / 2).Result;
                        c1 = DivideAndConquerPStep(x, x1, x2 + d2, y, y1 + d1, y2, d1, d2, threads / 2).Result;
                        c2 = DivideAndConquerPStep(x, x1, x2, y, y1, y2 + d2, d1, d2, threads / 2).Result;
                        c3 = DivideAndConquerPStep(x, x1, x2 + d2, y, y1 + d1, y2 + d2, d1, d2, threads / 2).Result;
                    });
                tasks[1] = Task.Run(() =>
                    {
                        c4 = DivideAndConquerPStep(x, x1 + d1, x2, y, y1, y2, d1, d2, threads / 2).Result;
                        c5 = DivideAndConquerPStep(x, x1 + d1, x2 + d2, y, y1 + d1, y2, d1, d2, threads / 2).Result;
                        c6 = DivideAndConquerPStep(x, x1 + d1, x2, y, y1, y2 + d2, d1, d2, threads / 2).Result;
                        c7 = DivideAndConquerPStep(x, x1 + d1, x2 + d2, y, y1 + d1, y2 + d2, d1, d2, threads / 2).Result;
                    });
                await Task.WhenAll(tasks);
                DivideAndConquerCombine(c0, c1, c2, c3, c4, c5, c6, c7, o, d1, d2);
            }
            return o;
        }
        private static void DivideAndConquerCombine(Vector<double>[][] c0, Vector<double>[][] c1, Vector<double>[][] c2, Vector<double>[][] c3, Vector<double>[][] c4, Vector<double>[][] c5, Vector<double>[][] c6, Vector<double>[][] c7, Vector<double>[][] o, int d1, int d2)
        {
            var d22 = d2 * 2;
            for (var i = 0; i < d1; i++)
            {
                o[i] = new Vector<double>[d22];
                o[i + d1] = new Vector<double>[d22];
                for (var j = 0; j < d2; j ++)
                {
                    o[i][j] = c0[i][j] + c1[i][j];
                    o[i][d2 + j] = c2[i][j] + c3[i][j];
                    o[d1 + i][j] = c4[i][j] + c5[i][j];
                    o[d1 + i][d2 + j] = c6[i][j] + c7[i][j];
                }
            }
        }
    }
}