# MyStrassensImplementation

This is a matrix multiplication benchmark tool. It can be built with and without parallelism and with and without simd acceleration. It supports SSE2 (simd), SSE4.1 and AVX2 extensions and uses OpenMP for parallelism.

## Makefile options

syntax: `make {RULE}; ./program.exe` or `make {TEST}`

### Flag Meanings

- nopar - no parallelism
- omp - use openmp parallelize

- simd - use SSE2 vectors
- avx2 - use AVX2 vectors
- sse4.1 - use SSE4.1 vectors

### Make rules

#### Flag based compilation

- nopar
- simd
- avx2
- sse4.1
- omp
- simd-omp
- avx2-omp
- sse4.1-omp
- nopar-asm
- simd-asm
- avx2-asm
- sse4.1-asm
- omp-asm
- simd-omp-asm
- avx2-omp-asm
- sse4.1-omp-asm

#### Tests

- run-strassens - Benchmark with strassens algorithm only
- run-naive - Benchmark with naive + divide and conquer only
- run-test - Benchmark both in sequence and display all running times
