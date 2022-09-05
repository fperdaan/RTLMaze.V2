namespace RTLMaze.Importer.Models;

public interface ISource<T>
{
	public T GetData();
}