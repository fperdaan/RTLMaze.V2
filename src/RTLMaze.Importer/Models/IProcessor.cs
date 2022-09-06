namespace RTLMaze.Importer.Models;

public interface IProcessor<in TInput, out TOutput>
{
	public TOutput Process( TInput source );
}