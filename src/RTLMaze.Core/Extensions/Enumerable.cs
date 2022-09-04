namespace RTLMaze.Core;

static public class Enumerable
{	
	static public IEnumerable<TSource> WhereIf<TSource>( this IEnumerable<TSource> source, bool cond,  Func<TSource, bool> predicate )
	{	
		return cond ? source.Where( predicate ) : source;
	}

	static public IEnumerable<TSource> WhereIf<TSource>( this IEnumerable<TSource> source, bool cond,  Func<TSource, int, bool> predicate )
	{	
		return cond ? source.Where( predicate ) : source;
	}
}