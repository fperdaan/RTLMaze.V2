namespace RTLMaze.Importer.Models.Importer;

public interface ITitleDetailConverter : IProcessor<int, Title>, IProcessor<IEnumerable<int>, IEnumerable<Title>>
{
	
}