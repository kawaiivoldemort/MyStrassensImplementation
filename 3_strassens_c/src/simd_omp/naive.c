#include "naive.h"

int** multiply_naive_nooffset(int** a, int** b, int di, int dj, int dk) {
    return multiply_naive(a, 0, 0, b, 0, 0, di, dj, dk);
}
int** multiply_naive_noparallel_nooffset(int** a, int** b, int di, int dj, int dk) {
    return multiply_naive_noparallel(a, 0, 0, b, 0, 0, di, dj, dk);
}
int** multiply_naive(int** a, int ax, int ay, int** b, int bx, int by, int di, int dj, int dk) {
    int** c = (int**) malloc(di * sizeof(int*));
    int q = (dj % VECTOR_SIZE == 0) ? dj : (dj - VECTOR_SIZE);
    int i;
    #pragma omp parallel for
    for(i = 0; i < di; i++) {
        int j, k;
        int* ct, * ai, * bk;
        VECTOR aik_vec, bk_vec, ct_vec, res_vec;
        ct = (int*) MALLOC_A(dj * sizeof(int));
        for(j = 0; j < dj; j++) {
            ct[j] = 0;
        }
        ai = a[i + ax];
        for(k = 0; k < dk; k++) {
            bk = b[k + bx];
            aik_vec = SET_VEC(ai[k + ay]);
            for(j = 0; j < q; j += VECTOR_SIZE) {
                bk_vec = LOAD_VECTOR(bk, by + j);
                res_vec = MUL_VECTOR_NOOVFL(aik_vec, bk_vec);
                ct_vec = LOAD_VECTOR(ct, j);
                res_vec = ADD_VEC(ct_vec, res_vec);
                STORE_VECTOR(res_vec, ct, j);
            }
            for(; j < dj; j++) {
                ct[j] += ai[k + ay] * bk[j + by];
            }
        }
        c[i] = ct;
    }
    return c;
}
int** multiply_naive_noparallel(int** a, int ax, int ay, int** b, int bx, int by, int di, int dj, int dk) {
    int** c = (int**) malloc(di * sizeof(int*));
    int* ct, * ai, * bk;
    int i, j, k;
    VECTOR aik_vec, bk_vec, ct_vec, res_vec;
    int q = (dj % VECTOR_SIZE == 0) ? dj : (dj - VECTOR_SIZE);
    for(i = 0; i < di; i++) {
        ct = (int*) MALLOC_A(dj * sizeof(int));
        for(j = 0; j < dj; j++) {
            ct[j] = 0;
        }
        ai = a[i + ax];
        for(k = 0; k < dk; k++) {
            bk = b[k + bx];
            aik_vec = SET_VEC(ai[k + ay]);
            for(j = 0; j < q; j += VECTOR_SIZE) {
                bk_vec = LOAD_VECTOR(bk, by + j);
                res_vec = MUL_VECTOR_NOOVFL(aik_vec, bk_vec);
                ct_vec = LOAD_VECTOR(ct, j);
                res_vec = ADD_VEC(ct_vec, res_vec);
                STORE_VECTOR(res_vec, ct, j);
            }
            for(; j < dj; j++) {
                ct[j] += ai[k + ay] * bk[j + by];
            }
        }
        c[i] = ct;
    }
    return c;
}