using RTLMaze.Core;
using RTLMaze.Models;
using RTLMaze.Importer.Models;

namespace RTLMaze.MazeScraper.Models;

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

	/// <summary>
	/// Helper method to return an index of the updated items.
	/// </summary>
	/// <see cref="GetData"/>
	protected virtual IEnumerable<int> _GetUpdatedItemIndex()
	{
		// Not sure wether we should make this path configurable
		// After all the import and parsing is tightly coupled to the source

		var source = new HttpStreamSource()
					.FromUrl("https://api.tvmaze.com/updates/shows");

		var processor = new JsonStreamConverter<Dictionary<string,long>>();
		
		Dictionary<string,long> updated = processor.Process( source.GetData() );

		// Cast the list to our format and possibly filter by update date
	
		return updated
				.WhereIf( _sinceDate != null, kp => kp.Value > _sinceDate )
				.Select( kp => Int32.Parse( kp.Key ) )
				.ToList();
	}


	# region Source interface implementation 

	public virtual IEnumerable<Title> GetData() => GetData( _GetUpdatedItemIndex() );

	/// <summary>
	/// Alternative interface, allowing the class to be used with specific titleIds, more of a passtrough converting the title ids to a title
	/// </summary>
	public virtual IEnumerable<Title> GetData( IEnumerable<int> titleIds ) => new TitleDetailsConverter().Process( titleIds );

	#endregion
}