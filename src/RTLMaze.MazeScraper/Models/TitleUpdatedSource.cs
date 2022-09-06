using RTLMaze.Core;
using RTLMaze.Models;
using RTLMaze.Importer.Models;

namespace RTLMaze.MazeScraper.Models;

public partial class TitleUpdatedSource : ITitleUpdatedSource
{
	protected long? _sinceDate { get; private set; }
	private ICollection<int>? _updatedIndex;

	# region Configuration interface

	public int EstimatedCount() => _GetUpdatedItemIndex().Count();

	public virtual ITitleUpdatedSource Since( DateTime? date )
	{
		if( date != null )
			_sinceDate = new DateTimeOffset( (DateTime)date ).ToUnixTimeSeconds();
		else 
			_sinceDate = null;

		// Reset possibly cached data
		_updatedIndex = null;
		
		return this;
	}

	# endregion

	/// <summary>
	/// Helper method to return an index of the updated items.
	/// </summary>
	/// <see cref="GetData"/>
	protected virtual ICollection<int> _GetUpdatedItemIndex()
	{
		if ( _updatedIndex != null )
			return _updatedIndex;
		
		// Not sure whether we should make this path configurable
		// After all the import and parsing is tightly coupled to the source

		var source = new HttpStreamSource()
					.FromUrl("https://api.tvmaze.com/updates/shows");

		var processor = new JsonStreamConverter<Dictionary<string,long>>();
		
		Dictionary<string,long> updated = processor.Process( source.GetData() );

		// Cast the list to our format and possibly filter by update date
	
		_updatedIndex = updated
				.WhereIf( _sinceDate != null, kp => kp.Value > _sinceDate )
				.Select( kp => Int32.Parse( kp.Key ) )
				.ToList();

		return _updatedIndex;
	}


	# region Source interface implementation 

	public virtual IEnumerable<Title> GetData() => new TitleDetailsConverter().Process( _GetUpdatedItemIndex(), skipOnException: true );

	#endregion
}