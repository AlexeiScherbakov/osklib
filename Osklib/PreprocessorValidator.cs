#if NET40

#if !NO_INLINING
#error NO_INLINING must be defined .NET 40 
#endif

#else

#if NO_INLINING
#error NO_INLINING must be NOT defined for .NET Version > 4.0
#endif

#endif

#if NET471 || NETCOREAPP3_0

#if !USE_REF_STRUCT
#error USE_REF_STRUCT must be defined
#endif

#else

#if USE_REF_STRUCT
#error USE_REF_STRUCT must be not defined
#endif

#endif