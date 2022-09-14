namespace RTLMaze.Importer;

public interface ISource<out TOutput>
{
	public TOutput GetData();
}