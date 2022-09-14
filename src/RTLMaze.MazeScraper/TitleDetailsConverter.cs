using RTLMaze.Models;
using RTLMaze.Importer;

namespace RTLMaze.MazeScraper;

public partial class TitleDetailsConverter : ITitleDetailConverter
{
	/// <summary>
	/// Helper method used to converts the specified title id to a streaming source.
	/// </summary>
	/// <param name="titleId">The id the title gets referred by.</param>
	/// <see>
	///     <cref>Process</cref>
	/// </see>
	protected virtual Stream _GetSource( int titleId )
	{
		// Not sure whether we should make this path configurable
		// After all the import and parsing is tightly coupled to the source
		var url = $"https://api.tvmaze.com/shows/{titleId}?embed=cast";

		var source = new HttpStreamSource();
			source.FromUrl( url );
		
		return source.GetData();
	}

	# region Processor interface implementation

	public virtual Title Process( int titleId )
	{
		// -- TODO
		// should the processor be static shared across all classes; memory / performance wise?

		var processor = new JsonStreamConverter<Title>();

		return processor.Process( _GetSource( titleId ) );
	}

	public IEnumerable<Title> Process( IEnumerable<int> titles ) => Process( titles, false );

	public IEnumerable<Title> Process( IEnumerable<int> titles, bool skipOnException )
	{
		// -- TODO
		// should the processor be static shared across all classes; memory / performance wise?
		// for this method it would mean we can call the singular process recursively with minimal additional cost
		var processor = new JsonStreamConverter<Title>();

		foreach( int titleId in titles )
		{
			Title title;

			try 
			{
				title = processor.Process( _GetSource( titleId ) );
			}
			catch( Exception )
			{
				if( skipOnException )
					continue;

                // -- TODO 
                // Hmm but what if we want another handler on exception?
                // or what if we want to log it and then continue? Not 100% happy with current approach

                throw;
            }

			yield return title;
		}
	}

	#endregion
}