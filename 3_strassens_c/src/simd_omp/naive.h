#include <stdlib.h>
#include <omp.h>
#include "../vector/vector.h"

int** multiply_naive_nooffset(int**, int**, int, int, int);
int** multiply_naive_noparallel_nooffset(int**, int**, int, int, int);
int** multiply_naive(int**, int, int, int**, int, int, int, int, int);
int** multiply_naive_noparallel(int**, int, int, int**, int, int, int, int, int);