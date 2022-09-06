namespace RTLMaze.Importer.Models;

public interface ISource<out TOutput>
{
	public TOutput GetData();
}