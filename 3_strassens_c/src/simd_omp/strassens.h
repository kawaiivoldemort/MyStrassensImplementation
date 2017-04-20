#include <omp.h>
#include "./naive.h"
#include "../vector/vector.h"
#include "../simd/matrix.h"

int** multiply_strassens_nooffset(int**, int**, int, int, int);
int** multiply_strassens_noparallel_nooffset(int**, int**, int, int, int);
int** multiply_strassens_square(int**, int, int, int**, int, int, int);
int** multiply_strassens_square_noparallel(int**, int, int, int**, int, int, int);
int*** compute_m(int**, int, int, int**, int, int, int);
int*** compute_m_noparallel(int**, int, int, int**, int, int, int);
int** compute_c(int***, int);
int** compute_c_noparallel(int***, int);