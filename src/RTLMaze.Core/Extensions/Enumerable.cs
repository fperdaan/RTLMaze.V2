namespace RTLMaze.Core;

public static class EnumerableExtension
{	
	public static IEnumerable<TSource> WhereIf<TSource>( this IEnumerable<TSource> source, bool cond,  Func<TSource, bool> predicate )
	{	
		return cond ? source.Where( predicate ) : source;
	}

	public static IEnumerable<TSource> WhereIf<TSource>( this IEnumerable<TSource> source, bool cond,  Func<TSource, int, bool> predicate )
	{	
		return cond ? source.Where( predicate ) : source;
	}
}

