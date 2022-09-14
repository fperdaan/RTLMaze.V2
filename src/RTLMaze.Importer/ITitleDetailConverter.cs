using RTLMaze.Models;
namespace RTLMaze.Importer;

public interface ITitleDetailConverter : IProcessor<int, Title>, IProcessor<IEnumerable<int>, IEnumerable<Title>>
{
	public IEnumerable<Title> Process( IEnumerable<int> titles, bool skipOnException );
}