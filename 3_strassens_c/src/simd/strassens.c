#include "strassens.h"

int** multiply_strassens_nooffset(int** a, int** b, int di, int dj, int dk) {
    int d;
    int** ar, ** br;
    d = di > dj ? di : dj;
    d = d > dk ? d : dk;
    d += d % 2;
    ar = squarify(a, di, dk, d);
    br = squarify(b, dk, dj, d);
    int** t = multiply_strassens_square(ar, 0, 0, br, 0, 0, d);
    free_mat(ar, d);
    free_mat(br, d);
    int** r = compactify(t, di, dj);
    free_mat(t, d);
    return r;
}
int** multiply_strassens_square(int** a, int ax, int ay, int** b, int bx, int by, int d) {
    if(d < THRESHOLD) {
        return multiply_naive(a, ax, ay, b, bx, by, d, d, d);
    }
    int dh = d / 2;
    if(d % 2 == 1) {
        a = evenify(a, ax, ay, d);
        b = evenify(b, bx, by, d);
        dh += 1;
        ax = 0;
        ay = 0;
        bx = 0;
        by = 0;
    }
    int*** m = compute_m(a, ax, ay, b, bx, by, dh);
    int** c = compute_c(m, dh);
    for(int i = 0; i < 7; i++) {
        free_mat(m[i], dh);
    }
    return c;
}
int*** compute_m(int** a, int ax, int ay, int** b, int bx, int by, int d) {
    int*** m = (int***) malloc(7 * sizeof(int**));
    int** x1 = sum(a, ax, ay, a, ax + d, ay + d, d);
    int** x2 = sum(b, bx, by, b, bx + d, by + d, d);
    int** x3 = sum(a, ax + d, ay, a, ax + d, ay + d, d);
    int** x4 = difference(b, bx, by + d, b, bx + d, by + d, d);
    int** x5 = difference(b, bx + d, by, b, bx, by, d);
    int** x6 = sum(a, ax, ay, a, ax, ay + d, d);
    int** x7 = difference(a, ax + d, ay, a, ax, ay, d);
    int** x8 = sum(b, bx, by, b, bx, by + d, d);
    int** x9 = difference(a, ax, ay + d, a, ax + d, ay + d, d);
    int** x10 = sum(b, bx + d, by, b, bx + d, by + d, d);
    m[0] = multiply_strassens_square(x1, 0, 0, x2, 0, 0, d);
    free_mat(x1, d);
    free_mat(x2, d);
    m[1] = multiply_strassens_square(x3, 0, 0, b, bx, by, d);
    free_mat(x3, d);
    m[2] = multiply_strassens_square(a, ax, ay, x4, 0, 0, d);
    free_mat(x4, d);
    m[3] = multiply_strassens_square(a, ax + d, ay + d, x5, 0, 0, d);
    free_mat(x5, d);
    m[4] = multiply_strassens_square(x6, 0, 0, b, bx + d, by + d, d);
    free_mat(x6, d);
    m[5] = multiply_strassens_square(x7, 0, 0, x8, 0, 0, d);
    free_mat(x7, d);
    free_mat(x8, d);
    m[6] = multiply_strassens_square(x9, 0, 0, x10, 0, 0, d);
    free_mat(x9, d);
    free_mat(x10, d);
    return m;
}
int** compute_c(int*** m, int d) {
    int d2 = 2 * d;
    int** c = (int**) malloc(d2 * sizeof(int*));
    int i, j, k, l;
    int* ct;
    int** m0 = m[0];
    int** m1 = m[1];
    int** m2 = m[2];
    int** m3 = m[3];
    int** m4 = m[4];
    int** m5 = m[5];
    int** m6 = m[6];
    VECTOR m0i_vec, m1i_vec, m2i_vec, m3i_vec, m4i_vec, m5i_vec, m6i_vec, res_vec;
    int qd = (d % VECTOR_SIZE == 0) ? d : (d - VECTOR_SIZE);
    int qd2 = (d2 % VECTOR_SIZE == 0) ? d2 : (d2 - VECTOR_SIZE);
    for(i = 0; i < d; i++) {
        ct = (int*) MALLOC_A(d2 * sizeof(int));
        int* m0i = m0[i];
        int* m2i = m2[i];
        int* m3i = m3[i];
        int* m4i = m4[i];
        int* m6i = m6[i];
        for(j = 0; j < qd; j += VECTOR_SIZE) {
            m0i_vec = LOAD_VECTOR(m0i, j);
            m3i_vec = LOAD_VECTOR(m3i, j);
            res_vec = ADD_VEC(m0i_vec, m3i_vec);
            m4i_vec = LOAD_VECTOR(m4i, j);
            res_vec = SUB_VEC(res_vec, m4i_vec);
            m6i_vec = LOAD_VECTOR(m6i, j);
            res_vec = ADD_VEC(res_vec, m6i_vec);
            STORE_VECTOR(res_vec, ct, j);
        }
        for(; j < d; j++) {
            ct[j] = m0i[j] + m3i[j] - m4i[j] + m6i[j];
        }
        for(k = 0; j < qd2; j += VECTOR_SIZE, k += VECTOR_SIZE) {
            m2i_vec = LOAD_VECTOR(m2i, k);
            m4i_vec = LOAD_VECTOR(m4i, k);
            res_vec = ADD_VEC(m2i_vec, m4i_vec);
            STORE_VECTOR(res_vec, ct, j);
        }
        for(; j < d2; j++, k++) {
            ct[j] = m2i[k] + m4i[k];
        }
        c[i] = ct;
    }
    for(l = 0; i < d2; i++, l++) {
        ct = (int*) MALLOC_A(d2 * sizeof(int));
        int* m0i = m0[l];
        int* m1i = m1[l];
        int* m2i = m2[l];
        int* m3i = m3[l];
        int* m5i = m5[l];
        for(j = 0; j < qd; j += VECTOR_SIZE) {
            m1i_vec = LOAD_VECTOR(m1i, j);
            m3i_vec = LOAD_VECTOR(m3i, j);
            res_vec = ADD_VEC(m1i_vec, m3i_vec);
            STORE_VECTOR(res_vec, ct, j);
        }
        for(; j < d; j++) {
            ct[j] = m1i[j] + m3i[j];
        }
        for(k = 0; j < qd2; j += VECTOR_SIZE, k += VECTOR_SIZE) {
            m0i_vec = LOAD_VECTOR(m0i, k);
            m1i_vec = LOAD_VECTOR(m1i, k);
            res_vec = SUB_VEC(m0i_vec, m1i_vec);
            m2i_vec = LOAD_VECTOR(m2i, k);
            res_vec = ADD_VEC(res_vec, m2i_vec);
            m5i_vec = LOAD_VECTOR(m5i, k);
            res_vec = ADD_VEC(res_vec, m5i_vec);
            STORE_VECTOR(res_vec, ct, j);
        }
        for(; j < d2; j++, k++) {
            ct[j] = m0i[k] - m1i[k] + m2i[k] + m5i[k];
        }
        c[i] = ct;
    }
    return c;
}