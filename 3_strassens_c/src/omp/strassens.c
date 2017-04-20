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
int** multiply_strassens_square_noparallel(int** a, int ax, int ay, int** b, int bx, int by, int d) {
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
    int*** m = compute_m_noparallel(a, ax, ay, b, bx, by, dh);
    int** c = compute_c_noparallel(m, dh);
    for(int i = 0; i < 7; i++) {
        free_mat(m[i], dh);
    }
    return c;
}
int*** compute_m(int** a, int ax, int ay, int** b, int bx, int by, int d) {
    int*** m = (int***) malloc(7 * sizeof(int**));
    #pragma omp parallel sections
    {
        #pragma omp section
        {
            int** x1 = sum(a, ax, ay, a, ax + d, ay + d, d);
            int** x2 = sum(b, bx, by, b, bx + d, by + d, d);
            m[0] = multiply_strassens_square_noparallel(x1, 0, 0, x2, 0, 0, d);
            free_mat(x1, d);
            free_mat(x2, d);
        }
        #pragma omp section
        {
            int** x3 = sum(a, ax + d, ay, a, ax + d, ay + d, d);
            m[1] = multiply_strassens_square_noparallel(x3, 0, 0, b, bx, by, d);
            free_mat(x3, d);
        }
        #pragma omp section
        {
            int** x4 = difference(b, bx, by + d, b, bx + d, by + d, d);
            m[2] = multiply_strassens_square_noparallel(a, ax, ay, x4, 0, 0, d);
            free_mat(x4, d);
        }
        #pragma omp section
        {
            int** x5 = difference(b, bx + d, by, b, bx, by, d);
            m[3] = multiply_strassens_square_noparallel(a, ax + d, ay + d, x5, 0, 0, d);
            free_mat(x5, d);
        }
        #pragma omp section
        {
            int** x6 = sum(a, ax, ay, a, ax, ay + d, d);
            m[4] = multiply_strassens_square_noparallel(x6, 0, 0, b, bx + d, by + d, d);
            free_mat(x6, d);
        }
        #pragma omp section
        {
            int** x7 = difference(a, ax + d, ay, a, ax, ay, d);
            int** x8 = sum(b, bx, by, b, bx, by + d, d);
            m[5] = multiply_strassens_square_noparallel(x7, 0, 0, x8, 0, 0, d);
            free_mat(x7, d);
            free_mat(x8, d);
        }
        #pragma omp section
        {
            int** x9 = difference(a, ax, ay + d, a, ax + d, ay + d, d);
            int** x10 = sum(b, bx + d, by, b, bx + d, by + d, d);
            m[6] = multiply_strassens_square_noparallel(x9, 0, 0, x10, 0, 0, d);
            free_mat(x9, d);
            free_mat(x10, d);
        }
    }
    return m;
}
int*** compute_m_noparallel(int** a, int ax, int ay, int** b, int bx, int by, int d) {
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
    int** m0 = m[0];
    int** m1 = m[1];
    int** m2 = m[2];
    int** m3 = m[3];
    int** m4 = m[4];
    int** m5 = m[5];
    int** m6 = m[6];
    int i;
    #pragma omp parallel for
    for(i = 0; i < d; i++) {
        int j, k;
        int* ct = (int*) malloc(d2 * sizeof(int));
        int* m0i = m0[i];
        int* m2i = m2[i];
        int* m3i = m3[i];
        int* m4i = m4[i];
        int* m6i = m6[i];
        for(j = 0; j < d; j++) {
            ct[j] = m0i[j] + m3i[j] - m4i[j] + m6i[j];
        }
        for(k = 0; j < d2; j++, k++) {
            ct[j] = m2i[k] + m4i[k];
        }
        c[i] = ct;
    }
    #pragma omp parallel for
    for(i = d; i < d2; i++) {
        int j, k;
        int l = i - d;
        int* ct = (int*) malloc(d2 * sizeof(int));
        int* m0i = m0[l];
        int* m1i = m1[l];
        int* m2i = m2[l];
        int* m3i = m3[l];
        int* m5i = m5[l];
        for(j = 0; j < d; j++) {
            ct[j] = m1i[j] + m3i[j];
        }
        for(k = 0; j < d2; j++, k++) {
            ct[j] = m0i[k] - m1i[k] + m2i[k] + m5i[k];
        }
        c[i] = ct;
        l++;
    }
    return c;
}
int** compute_c_noparallel(int*** m, int d) {
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
    for(i = 0; i < d; i++) {
        ct = (int*) malloc(d2 * sizeof(int));
        int* m0i = m0[i];
        int* m2i = m2[i];
        int* m3i = m3[i];
        int* m4i = m4[i];
        int* m6i = m6[i];
        for(j = 0; j < d; j++) {
            ct[j] = m0i[j] + m3i[j] - m4i[j] + m6i[j];
        }
        for(k = 0; j < d2; j++, k++) {
            ct[j] = m2i[k] + m4i[k];
        }
        c[i] = ct;
    }
    for(l = 0; i < d2; i++, l++) {
        ct = (int*) malloc(d2 * sizeof(int));
        int* m0i = m0[l];
        int* m1i = m1[l];
        int* m2i = m2[l];
        int* m3i = m3[l];
        int* m5i = m5[l];
        for(j = 0; j < d; j++) {
            ct[j] = m1i[j] + m3i[j];
        }
        for(k = 0; j < d2; j++, k++) {
            ct[j] = m0i[k] - m1i[k] + m2i[k] + m5i[k];
        }
        c[i] = ct;
    }
    return c;
}