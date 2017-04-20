#include "naive.h"

int** multiply_naive_nooffset(int** a, int** b, int di, int dj, int dk) {
    return multiply_naive(a, 0, 0, b, 0, 0, di, dj, dk);
}
int** multiply_naive_noparallel_nooffset(int** a, int** b, int di, int dj, int dk) {
    return multiply_naive_noparallel(a, 0, 0, b, 0, 0, di, dj, dk);
}
int** multiply_naive(int** a, int ax, int ay, int** b, int bx, int by, int di, int dj, int dk) {
    int** c = (int**) malloc(di * sizeof(int*));
    int i;
    #pragma omp parallel for
    for(i = 0; i < di; i++) {
        int* ct, * ai, * bk;
        int j, k, aik;
        ct = (int*) malloc(dj * sizeof(int));
        for(j = 0; j < dj; j++) {
            ct[j] = 0;
        }
        ai = a[i + ax];
        for(k = 0; k < dk; k++) {
            bk = b[k + bx];
            aik = ai[k + ay];
            for(j = 0; j < dj; j++) {
                ct[j] += aik * bk[j + by];
            }
        }
        c[i] = ct;
    }
    return c;
}
int** multiply_naive_noparallel(int** a, int ax, int ay, int** b, int bx, int by, int di, int dj, int dk) {
    int** c = (int**) malloc(di * sizeof(int*));
    int* ct, * ai, * bk;
    int i, j, k, aik;
    for(i = 0; i < di; i++) {
        ct = (int*) malloc(dj * sizeof(int));
        for(j = 0; j < dj; j++) {
            ct[j] = 0;
        }
        ai = a[i + ax];
        for(k = 0; k < dk; k++) {
            bk = b[k + bx];
            aik = ai[k + ay];
            for(j = 0; j < dj; j++) {
                ct[j] += aik * bk[j + by];
            }
        }
        c[i] = ct;
    }
    return c;
}