CC=gcc
CFLAGS=-O3 -std=c11
SIMDFLAGS=-march=native
SSE41FLAGS=-msse4.1
AVX2FLAGS=-mavx2
OMPFLAGS=-fopenmp
LIB=./src
BIN=./bin
ASM=./asm
CFILES_NOPAR=$(LIB)/main/main.c $(LIB)/nopar/matrix.c $(LIB)/nopar/strassens.c $(LIB)/nopar/naive.c
HFILES_NOPAR=$(LIB)/main/main.h $(LIB)/nopar/matrix.h $(LIB)/nopar/strassens.h $(LIB)/nopar/naive.h
CFILES_SIMD=$(LIB)/main/main.c $(LIB)/simd/matrix.c $(LIB)/simd/strassens.c $(LIB)/simd/naive.c
HFILES_SIMD=$(LIB)/main/main.h $(LIB)/simd/matrix.h $(LIB)/simd/strassens.h $(LIB)/simd/naive.h $(LIB)/vector/vector.h
CFILES_OMP=$(LIB)/main/main.c $(LIB)/nopar/matrix.c $(LIB)/omp/strassens.c $(LIB)/omp/naive.c
HFILES_OMP=$(LIB)/main/main.h $(LIB)/nopar/matrix.h $(LIB)/omp/strassens.h $(LIB)/omp/naive.h
CFILES_SIMD_OMP=$(LIB)/main/main.c $(LIB)/simd/matrix.c $(LIB)/simd_omp/strassens.c $(LIB)/simd_omp/naive.c
HFILES_SIMD_OMP=$(LIB)/main/main.h $(LIB)/simd/matrix.h $(LIB)/simd_omp/strassens.h $(LIB)/simd_omp/naive.h $(LIB)/vector/vector.h

nopar: $(CFILES_NOPAR) $(HFILES_NOPAR)
	$(CC) $(CFILES_NOPAR) $(CFLAGS) -o $(BIN)/program.exe

simd: $(CFILES_SIMD) $(HFILES_SIMD)
	$(CC) $(CFILES_SIMD) $(CFLAGS) $(SIMDFLAGS) -o $(BIN)/program.exe

avx2: $(CFILES_SIMD) $(HFILES_SIMD)
	$(CC) $(CFILES_SIMD) $(CFLAGS) $(AVX2FLAGS) -o $(BIN)/program.exe

sse4.1: $(CFILES_SIMD) $(HFILES_SIMD)
	$(CC) $(CFILES_SIMD) $(CFLAGS) $(SSE41FLAGS) -o $(BIN)/program.exe

omp: $(CFILES_OMP) $(HFILES_OMP)
	$(CC) $(CFILES_OMP) $(CFLAGS) $(OMPFLAGS) -o $(BIN)/program.exe

simd-omp: $(CFILES_SIMD_OMP) $(HFILES_SIMD_OMP)
	$(CC) $(CFILES_SIMD_OMP) $(CFLAGS) $(SIMDFLAGS) $(OMPFLAGS) -o $(BIN)/program.exe

avx2-omp: $(CFILES_SIMD_OMP) $(HFILES_SIMD_OMP)
	$(CC) $(CFILES_SIMD_OMP) $(CFLAGS) $(AVX2FLAGS) $(OMPFLAGS) -o $(BIN)/program.exe

sse4.1-omp: $(CFILES_SIMD_OMP) $(HFILES_SIMD_OMP)
	$(CC) $(CFILES_SIMD_OMP) $(CFLAGS) $(SSE41FLAGS) $(OMPFLAGS) -o $(BIN)/program.exe

nopar-asm: $(CFILES_NOPAR) $(HFILES_NOPAR)
	$(CC) -S $(CFILES_NOPAR) $(CFLAGS) -o $(ASM)/program.s

simd-asm: $(CFILES_SIMD) $(HFILES_SIMD)
	$(CC) -S $(CFILES_SIMD) $(CFLAGS) $(SIMDFLAGS) -o $(ASM)/program.s

avx2-asm: $(CFILES_SIMD) $(HFILES_SIMD)
	$(CC) -S $(CFILES_SIMD) $(CFLAGS) $(AVX2FLAGS) -o $(ASM)/program.s

sse4.1-asm: $(CFILES_SIMD) $(HFILES_SIMD)
	$(CC) -S $(CFILES_SIMD) $(CFLAGS) $(SSE41FLAGS) -o $(ASM)/program.s

omp-asm: $(CFILES_OMP) $(HFILES_OMP)
	$(CC) -S $(CFILES_OMP) $(CFLAGS) $(OMPFLAGS) -o $(ASM)/program.s

simd-omp-asm: $(CFILES_SIMD_OMP) $(HFILES_SIMD_OMP)
	$(CC) -S $(CFILES_SIMD_OMP) $(CFLAGS) $(SIMDFLAGS) $(OMPFLAGS) -o $(ASM)/program.s

avx2-omp-asm: $(CFILES_SIMD_OMP) $(HFILES_SIMD_OMP)
	$(CC) -S $(CFILES_SIMD_OMP) $(CFLAGS) $(AVX2FLAGS) $(OMPFLAGS) -o $(ASM)/program.s

sse4.1-omp-asm: $(CFILES_SIMD_OMP) $(HFILES_SIMD_OMP)
	$(CC) -S $(CFILES_SIMD_OMP) $(CFLAGS) $(SSE41FLAGS) $(OMPFLAGS) -o $(ASM)/program.s

run-strassens: $(BIN)/program.exe
	$(BIN)/program.exe 4096 -type=strassens

run-naive: $(BIN)/program.exe
	$(BIN)/program.exe 4096 -type=naive

run-test: $(BIN)/program.exe
	$(BIN)/program.exe 4096 -type=test

clean:
	rm -f $(BIN)/* $(ASM)/*