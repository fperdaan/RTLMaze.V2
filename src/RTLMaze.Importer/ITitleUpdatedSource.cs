using RTLMaze.Models;
namespace RTLMaze.Importer;

public interface ITitleUpdatedSource : ISource<IEnumerable<Title>>
{
	public int EstimatedCount();
	public ITitleUpdatedSource Since( DateTime? date );
	public IEnumerable<int> GetDataAsIndex();
}