#define ALIGN_SIZE 32
#if defined(__AVX2__)
    #if defined(_WIN32)
        #define MALLOC_A(a) _aligned_malloc(a, ALIGN_SIZE)
        #define ALIGNED_FREE(a) _aligned_free(a)
    #else
        #define MALLOC_A(a) aligned_alloc(ALIGN_SIZE, a)
        #define ALIGNED_FREE(a) free(a)
    #endif
    #include <immintrin.h>
    #define VECTOR_SIZE 8
    #define VECTOR __m256i
    #define LOAD_VECTOR(a, b) _mm256_load_si256((VECTOR *) &a[b])
    #define MUL_VECTOR_NOOVFL(a, b) _mm256_mullo_epi32(a, b)
    #define ADD_VEC(a, b) _mm256_add_epi32(a, b)
    #define SUB_VEC(a, b) _mm256_sub_epi32(a, b)
    #define SET_VEC(a) _mm256_set1_epi32(a)
    #define STORE_VECTOR(f, a, b) _mm256_store_si256((VECTOR *) &a[b], f)
#elif defined(__SSE4_1__)
    #if defined(_WIN32)
        #define MALLOC_A(a) _aligned_malloc(a, ALIGN_SIZE)
        #define ALIGNED_FREE(a) _aligned_free(a)
    #else
        #define MALLOC_A(a) aligned_alloc(ALIGN_SIZE, a)
        #define ALIGNED_FREE(a) free(a)
    #endif
    #include <smmintrin.h>
    #define VECTOR_SIZE 4
    #define VECTOR __m128i
    #define LOAD_VECTOR(a, b) _mm_load_si128((VECTOR *) &a[b])
    #define MUL_VECTOR_NOOVFL(a, b) _mm_mullo_epi32(a, b)
    #define ADD_VEC(a, b) _mm_add_epi32(a, b)
    #define SUB_VEC(a, b) _mm_sub_epi32(a, b)
    #define SET_VEC(a) _mm_set1_epi32(a)
    #define STORE_VECTOR(f, a, b) _mm_store_si128((VECTOR *) &a[b], f)
#endif