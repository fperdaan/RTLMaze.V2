namespace RTLMaze.Importer.Models;

public interface IProcessor<TInput, TOutput>
{
	public TOutput Process( TInput source );
}