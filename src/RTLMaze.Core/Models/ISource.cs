namespace RTLMaze.Core.Models;

public interface ISource<T>
{
	public T GetData();
}