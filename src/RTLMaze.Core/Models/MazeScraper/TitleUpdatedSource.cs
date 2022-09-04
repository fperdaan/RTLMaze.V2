using RTLMaze.Core.Models.Importer;

namespace RTLMaze.Core.Models.MazeScraper;

public partial class TitleUpdatedSource : ITitleUpdatedSource
{
	protected long? _sinceDate { get; private set; }

	# region Configuration interface

	public virtual ITitleUpdatedSource Since( DateTime? date )
	{
		if( date != null )
			_sinceDate = new DateTimeOffset( (DateTime)date ).ToUnixTimeSeconds();
		else 
			_sinceDate = null;

		return this;
	}

	# endregion

	# region Source interface implementation 

	public virtual ICollection<int> GetData()
	{
		// Not sure wether we should make this path configurable
		// After all the import and parsing is tightly coupled to the source

		var source = new HttpStreamSource()
					.FromUrl("https://api.tvmaze.com/updates/shows?since=day");

		var processor = new JsonStreamConverter<Dictionary<string,long>>();
		
		Dictionary<string,long> updated = processor.Process( source );

		// Cast the list to our format and possibly filter by update date
	
		return updated
				.WhereIf( _sinceDate != null, kp => kp.Value > _sinceDate )
				.Select( kp => Int32.Parse( kp.Key ) )
				.ToList();
	}

	public virtual Task<ICollection<int>> GetDataAsync() => Task.Run( () => GetData() );

	# endregion
}