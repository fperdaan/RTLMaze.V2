namespace RTLMaze.Core.Models.MazeScraper;

public partial class TitleDetailsProcessor : IProcessor<int, Title>
{
	public virtual Stream GetSource( int titleId )
	{
		// Not sure wether we should make this path configurable
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
		// should the processor be static shared accross all classes; memory / performance wise?

		var processor = new JsonStreamConverter<Title>();

		return processor.Process( GetSource( titleId ) );
	}

	# endregion
}