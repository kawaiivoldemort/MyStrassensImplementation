#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <string.h>
#include "../simd/matrix.h"
#include "../simd/strassens.h"
#include "../simd/naive.h"

#define TIMER_INIT clock_t time_v
#define TIMER_START time_v = clock()
#define TIMER_END time_v = clock() - time_v
#define TIMER_PRINT printf("%Lf\n", (long double)time_v / (double)CLOCKS_PER_SEC)

int main();