namespace StrassensLibrary {
    public class Strassens {
        const int THRESHOLD = 128;
        public int[][] Multiply(int[][] a, int[][] b, int di, int dj, int dk) {
            int d;
            int[][] ar, br;
            d = di > dj ? di : dj;
            d = d > dk ? d : dk;
            d += d % 2;
            if(d != dk) {
                ar = Squarify(a, di, dk, d);
                br = Squarify(b, dk, dj, d);
            } else if(d != di) {
                ar = Squarify(a, di, dk, d);
                if(d != dj) {
                    br = Squarify(b, dk, dj, d);
                } else {
                    br = b;
                }
            } else if(d != dj) {
                ar = a;
                br = Squarify(b, dk, dj, d);
            } else {
                ar = a;
                br = b;
            }
            return Compactify(Multiply(ar, 0, 0, br, 0, 0, d), di, dj);
        }
        int[][] Squarify(int[][] a, int di, int dj, int d) {
            int[][] ar = new int[d][];
            int[] at, ai;
            int i, j;
            for(i = 0; i < di; i++) {
                at = new int[d];
                ai = a[i];
                for(j = 0; j < dj; j++) {
                    at[j] = ai[j];
                }
                ar[i] = at;
            }
            for(; i < d; i++) {
                ar[i] = new int[d];
            }
            return ar;
        }
        int[][] Evenify(int[][] a, int ax, int ay, int d) {
            int[] at, ai;
            int i, j;
            int dp = d + 1;
            int[][] ar = new int[dp][];
            for(i = 0; i < d; i++) {
                at = new int[dp];
                ai = a[i + ax];
                for(j = 0; j < d; j++) {
                    at[j] = ai[j + ay];
                }
                ar[i] = at;
            }
            ar[i] = new int[dp];
            return ar;
        }
        int[][] Compactify(int[][] a, int di, int dj) {
            int[] at, ai;
            int i, j;
            int[][] ar = new int[di][];
            for(i = 0; i < di; i++) {
                at = new int[dj];
                ai = a[i];
                for(j = 0; j < dj; j++) {
                    at[j] = ai[j];
                }
                ar[i] = at;
            }
            return ar;
        }
        int[][] Multiply(int[][] a, int ax, int ay, int[][] b, int bx, int by, int d) {
            if(d < THRESHOLD) {
                return NaiveMultiply(a, ax, ay, b, bx, by, d, d, d);
            }
            var dh = d / 2;
            if(d % 2 == 1) {
                a = Evenify(a, ax, ay, d);
                b = Evenify(b, bx, by, d);
                dh += 1;
                ax = 0;
                ay = 0;
                bx = 0;
                by = 0;
            }
            int[][] c = ComputeC(ComputeM(a, ax, ay, b, bx, by, dh), dh);
            // Return C
            return c;
        }
        public int[][] NaiveMultiply(int[][] a, int ax, int ay, int[][] b, int bx, int by, int di, int dj, int dk) {
            var c = new int[di][];
            int[] ct, ai, bk;
            int aik;
            for(var i = 0; i < di; i++) {
                ct = new int[dj];
                ai = a[i + ax];
                for(var k = 0; k < dk; k++) {
                    bk = b[k + bx];
                    aik = ai[k + ay];
                    for(var j = 0; j < dj; j++) {
                        ct[j] += aik * bk[j + by];
                    }
                }
                c[i] = ct;
            }
            return c;
        }
        int[][][] ComputeM(int[][] a, int ax, int ay, int[][] b, int bx, int by, int d) {
            // compute m
            var m = new int[7][][];
            // m1 = (a11 + a22) * (b11 + b22)
            // m2 = (a21 + a22) * b11
            // m3 = a11 * (b12 - b22)
            // m4 = a22 * (b21 - b11)
            // m5 = (a11 + a12) * b22
            // m6 = (a21 - a11) * (b11 + b12)
            // m7 = (a12 - a22) * (b21 + b22)
            var x1 = Sum(a, ax, ay, a, ax + d, ay + d, d);
            var x2 = Sum(b, bx, by, b, bx + d, by + d, d);
            var x3 = Sum(a, ax + d, ay, a, ax + d, ay + d, d);
            var x4 = Difference(b, bx, by + d, b, bx + d, by + d, d);
            var x5 = Difference(b, bx + d, by, b, bx, by, d);
            var x6 = Sum(a, ax, ay, a, ax, ay + d, d);
            var x7 = Difference(a, ax + d, ay, a, ax, ay, d);
            var x8 = Sum(b, bx, by, b, bx, by + d, d);
            var x9 = Difference(a, ax, ay + d, a, ax + d, ay + d, d);
            var x10 = Sum(b, bx + d, by, b, bx + d, by + d, d);
            // multiply
            m[0] = Multiply(x1, 0, 0, x2, 0, 0, d);
            m[1] = Multiply(x3, 0, 0, b, bx, by, d);
            m[2] = Multiply(a, ax, ay, x4, 0, 0, d);
            m[3] = Multiply(a, ax + d, ay + d, x5, 0, 0, d);
            m[4] = Multiply(x6, 0, 0, b, bx + d, by + d, d);
            m[5] = Multiply(x7, 0, 0, x8, 0, 0, d);
            m[6] = Multiply(x9, 0, 0, x10, 0, 0, d);
            // return m
            return m;
        }
        int[][] ComputeC(int[][][] m, int d) {
            // compute c
            int d2 = 2 * d;
            var c = new int[d2][];
            int i, j, k, l;
            int[] ct;
            var m0 = m[0];
            var m1 = m[1];
            var m2 = m[2];
            var m3 = m[3];
            var m4 = m[4];
            var m5 = m[5];
            var m6 = m[6];
            for(i = 0; i < d; i++) {
                ct = new int[d2];
                var m0i = m0[i];
                var m2i = m2[i];
                var m3i = m3[i];
                var m4i = m4[i];
                var m6i = m6[i];
                // c11 = m1 + m4 - m5 + m7
                for(j = 0; j < d; j++) {
                    ct[j] = m0i[j] + m3i[j] - m4i[j] + m6i[j];
                }
                // c12 = m3 + m5
                for(k = 0; j < d2; j++, k++) {
                    ct[j] = m2i[k] + m4i[k];
                }
                c[i] = ct;
            }
            for(l = 0; i < d2; i++, l++) {
                ct = new int[d2];
                var m0i = m0[l];
                var m1i = m1[l];
                var m2i = m2[l];
                var m3i = m3[l];
                var m5i = m5[l];
                // c21 = m2 + m4
                for(j = 0; j < d; j++) {
                    ct[j] = m1i[j] + m3i[j];
                }
                // c22 = m1 - m2 + m3 + m6
                for(k = 0; j < d2; j++, k++) {
                    ct[j] = m0i[k] - m1i[k] + m2i[k] + m5i[k];
                }
                c[i] = ct;
            }
            return c;
        }
        int[][] Sum(int[][] a, int ax, int ay, int[][] b, int bx, int by, int d) {
            var result = new int[d][];
            for(var i = 0; i < d; i++) {
                result[i] = new int[d];
                for(var j = 0; j < d; j++) {
                    result[i][j] = a[ax + i][ay + j] + b[bx + i][by + j];
                }
            }
            return result;
        }
        int[][] Difference(int[][] a, int ax, int ay, int[][] b, int bx, int by, int d) {
            var result = new int[d][];
            for(var i = 0; i < d; i++) {
                result[i] = new int[d];
                for(var j = 0; j < d; j++) {
                    result[i][j] = a[ax + i][ay + j] - b[bx + i][by + j];
                }
            }
            return result;
        }
    }
}