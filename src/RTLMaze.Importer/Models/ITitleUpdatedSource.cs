using RTLMaze.Models;
namespace RTLMaze.Importer.Models;

public interface ITitleUpdatedSource : ISource<IEnumerable<Title>>
{
	public int EstimatedCount();
	public ITitleUpdatedSource Since( DateTime? date );
}