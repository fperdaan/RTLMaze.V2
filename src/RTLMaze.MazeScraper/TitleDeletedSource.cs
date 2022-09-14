using RTLMaze.Importer;

namespace RTLMaze.MazeScraper;

public partial class TitleDeletedSource : ITitleDeletedSource
{
	public virtual IEnumerable<int> GetData()
	{
		// TODO 
		// Set source by DI 
		var source = new TitleUpdatedSource();
		
		// Retrieve a full index of all updated items
		var items = source.GetDataAsIndex();
		

		// TODO 
		// Compare with active title index (database) so we can calculate a difference
		
		return items;
	}
}