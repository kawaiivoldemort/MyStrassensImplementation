#include <stdio.h>
#include <stdlib.h>

#define THRESHOLD 128

int** squarify(int**, int, int, int);
int** evenify(int**, int, int, int);
int** compactify(int**, int, int);
int** sum(int**, int, int, int**, int, int, int);
int** difference(int**, int, int, int**, int, int, int);
void free_mat(int ** a, int dx);
void print(int**, int, int);
int equals(int**, int**, int, int);