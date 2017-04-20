#include "main.h"

int main(int argc, char **argv) {
    srand(time(NULL));
    int di, dj, dk, t;
    TIMER_INIT;
    if(argc == 3) {
        int d = atoi(argv[1]);
        di = d;
        dj = d;
        dk = d;
        if(strcmp(argv[2], "-type=strassens") == 0) {
            t = 0;
        } else if(strcmp(argv[2], "-type=naive") == 0) {
            t = 1;
        } else if(strcmp(argv[2], "-type=test") == 0) {
            t = 2;
        } else {
            perror("wrong params");
            return 1;
        }
    } else if(argc == 5) {
        di = atoi(argv[1]);
        dj = atoi(argv[2]);
        dk = atoi(argv[3]);
        if(strcmp(argv[4], "-type=strassens") == 0) {
            t = 0;
        } else if(strcmp(argv[4], "-type=naive") == 0) {
            t = 1;
        } else if(strcmp(argv[4], "-type=test") == 0) {
            t = 2;
        } else {
            perror("wrong params");
            return 1;
        }
    } else {
        perror("wrong number of params");
        return 2;
    }
    int** a = (int**) malloc(di * sizeof(int*));
    int** b = (int**) malloc(dk * sizeof(int*));
    for(int i = 0; i < di; i++) {
        int* x = (int*) malloc(dk * sizeof(int));
        for(int j = 0; j < dk; j++) {
            x[j] = rand() % 5;
        }
        a[i] = x;
    }
    for(int i = 0; i < dk; i++) {
        int* x = (int*) malloc(dj * sizeof(int));
        for(int j = 0; j < dj; j++) {
            x[j] = rand() % 5;
        }
        b[i] = x;
    }
    if(t == 0) {
        TIMER_START;
        int** f = multiply_strassens_nooffset(a, b, di, dj, dk);
        TIMER_END;
        TIMER_PRINT;
        free_mat(f, di);
    } else if(t == 1) {
        TIMER_START;
        int** g = multiply_naive_nooffset(a, b, di, dj, dk);
        TIMER_END;
        TIMER_PRINT;
        free_mat(g, di);
    } else if(t == 2) {
        int** f = multiply_strassens_nooffset(a, b, di, dj, dk);
        int** g = multiply_naive_nooffset(a, b, di, dj, dk);
        printf(equals(f, g, di, dj) ? "Equal\n" : "Inequal\n");
        free_mat(f, di);
        free_mat(g, di);
    }
    free_mat(a, di);
    free_mat(b, dk);
    return 0;
}