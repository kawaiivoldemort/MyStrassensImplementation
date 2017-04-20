#include "matrix.h"

int** squarify(int** a, int di, int dj, int d) {
    int** ar = (int**) malloc(d * sizeof(int*));
    int* at, * ai;
    int i, j;
    for(i = 0; i < di; i++) {
        at = (int*) MALLOC_A(d * sizeof(int));
        ai = a[i];
        for(j = 0; j < dj; j++) {
            at[j] = ai[j];
        }
        for(; j < d; j++) {
            at[j] = 0;
        }
        ar[i] = at;
    }
    for(; i < d; i++) {
        at = (int*) MALLOC_A(d * sizeof(int));
        for(j = 0; j < d; j++) {
            at[j] = 0;
        }
        ar[i] = at;
    }
    return ar;
}
int** evenify(int** a, int ax, int ay, int d) {
    int* at, * ai;
    int i, j;
    int dp = d + 1;
    int** ar = (int**) malloc(dp * sizeof(int*));
    for(i = 0; i < d; i++) {
        at = (int*) MALLOC_A(dp * sizeof(int));
        ai = a[i + ax];
        for(j = 0; j < d; j++) {
            at[j] = ai[j + ay];
        }
        at[j] = 0;
        ar[i] = at;
    }
    at = (int*) MALLOC_A(dp * sizeof(int));
    for(j = 0; j < dp; j++) {
        at[j] = 0;
    }
    ar[i] = at;
    free_mat(a, d);
    return ar;
}
int** compactify(int** a, int di, int dj) {
    int* at, * ai;
    int i, j;
    int** ar = (int**) malloc(di * sizeof(int*));
    for(i = 0; i < di; i++) {
        at = (int*) MALLOC_A(dj * sizeof(int));
        ai = a[i];
        for(j = 0; j < dj; j++) {
            at[j] = ai[j];
        }
        ar[i] = at;
    }
    return ar;
}
int** sum(int** a, int ax, int ay, int** b, int bx, int by, int d) {
    int i, j;
    int* resulti, * ai, * bi;
    int** result = (int**) malloc(d * sizeof(int*));
    VECTOR ai_vec, bi_vec, res_vec;
    int q = (d % VECTOR_SIZE == 0) ? d : (d - VECTOR_SIZE);
    for(i = 0; i < d; i++) {
        ai = a[ax + i];
        bi = b[bx + i];
        resulti = (int*) MALLOC_A(d * sizeof(int));
        for(j = 0; j < q; j += VECTOR_SIZE) {
            ai_vec = LOAD_VECTOR(ai, ay + j);
            bi_vec = LOAD_VECTOR(bi, by + j);
            res_vec = ADD_VEC(ai_vec, bi_vec);
            STORE_VECTOR(res_vec, resulti, j);
        }
        for(j = 0; j < d; j += VECTOR_SIZE) {
            resulti[j] = ai[ay + j] + bi[by + j];
        }
        result[i] = resulti;
    }
    return result;
}
int** difference(int** a, int ax, int ay, int** b, int bx, int by, int d) {
    int i, j;
    int* resulti, * ai, * bi;
    int** result = (int**) malloc(d * sizeof(int*));
    VECTOR ai_vec, bi_vec, res_vec;
    int q = (d % VECTOR_SIZE == 0) ? d : (d - VECTOR_SIZE);
    for(i = 0; i < d; i++) {
        ai = a[ax + i];
        bi = b[bx + i];
        resulti = (int*) MALLOC_A(d * sizeof(int));
        for(j = 0; j < q; j += VECTOR_SIZE) {
            ai_vec = LOAD_VECTOR(ai, ay + j);
            bi_vec = LOAD_VECTOR(bi, by + j);
            res_vec = SUB_VEC(ai_vec, bi_vec);
            STORE_VECTOR(res_vec, resulti, j);
        }
        for(j = 0; j < d; j += VECTOR_SIZE) {
            resulti[j] = ai[ay + j] - bi[by + j];
        }
        result[i] = resulti;
    }
    return result;
}
void free_mat(int ** a, int dx) {
    for(int i = 0; i < dx; i++) {
        ALIGNED_FREE(a[i]);
    }
    free(a);
}
void print(int** c, int di, int dj) {
    for(int i = 0; i < di; i++) {
        printf("{%d", c[i][0]);
        for(int j = 1; j < dj; j++) {
            printf(",\t%d", c[i][j]);
        }
        printf("}\n");
    }
    printf("\n");
}
int equals(int** a, int** b, int di, int dj) {
    for(int i = 0; i < di; i++) {
        for(int j = 1; j < dj; j++) {
            if(a[i][j] != b[i][j]) {
                return 0;
            }
        }
    }
    return 1;
}