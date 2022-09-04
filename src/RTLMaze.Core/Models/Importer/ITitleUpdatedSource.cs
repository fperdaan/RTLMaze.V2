namespace RTLMaze.Core.Models.Importer;

public interface ITitleUpdatedSource : ISource<ICollection<int>>
{
	public ITitleUpdatedSource Since( DateTime? date );	
}