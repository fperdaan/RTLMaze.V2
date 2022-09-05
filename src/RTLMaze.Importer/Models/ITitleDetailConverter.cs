using RTLMaze.Models;
namespace RTLMaze.Importer.Models;

public interface ITitleDetailConverter : IProcessor<int, Title>, IProcessor<IEnumerable<int>, IEnumerable<Title>>
{
	
}