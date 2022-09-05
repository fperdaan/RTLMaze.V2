namespace RTLMaze.Importer.Models.Importer;

public interface ITitleUpdatedSource : ISource<IEnumerable<Title>>
{
	public ITitleUpdatedSource Since( DateTime? date );	
}