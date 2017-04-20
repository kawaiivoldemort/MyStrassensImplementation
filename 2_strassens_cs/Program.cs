using System;
using StrassensLibrary;

namespace ConsoleApplication {
    public class Program {
        public static void Main(string[] args) {
            Random r = new Random();
            int di = 2048;
            int dj = 2048;
            int dk = 2048;
            int[][] a = new int[di][];
            int[][] b = new int[dk][];
            for(int i = 0; i < di; i++) {
                int[] x = new int[dk];
                for(int j = 0; j < dk; j++) {
                    x[j] = r.Next() % 5;
                }
                a[i] = x;
            }
            for(int i = 0; i < dk; i++) {
                int[] x = new int[dj];
                for(int j = 0; j < dj; j++) {
                    x[j] = r.Next() % 5;
                }
                b[i] = x;
            }
            var m = new Strassens();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            int[][] f = m.Multiply(a, b, di, dj, dk);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            int[][] g = m.NaiveMultiply(a, 0, 0, b, 0, 0, di, dj, dk);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            sw.Reset();
            Console.WriteLine(Equals(f, g, di, dj));
        }
        public static void Print(int[][] c, int di, int dj) {
            Console.WriteLine();
            for(int i = 0; i < di; i++) {
                Console.Write("{" + c[i][0].ToString());
                for(int j = 1; j < dj; j++) {
                    Console.Write(",\t" + c[i][j].ToString());
                }
                Console.WriteLine("}");
            }
            Console.WriteLine();
        }
        public static bool Equals(int[][] a, int[][] b, int di, int dj) {
            for(int i = 0; i < di; i++) {
                for(int j = 1; j < dj; j++) {
                    if(a[i][j] != b[i][j]) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}