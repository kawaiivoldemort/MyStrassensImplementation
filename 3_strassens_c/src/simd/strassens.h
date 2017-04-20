#include "./naive.h"
#include "../vector/vector.h"
#include "./matrix.h"

int** multiply_strassens_nooffset(int**, int**, int, int, int);
int** multiply_strassens_square(int**, int, int, int**, int, int, int);
int*** compute_m(int**, int, int, int**, int, int, int);
int** compute_c(int***, int);