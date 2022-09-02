namespace RTLMaze.Core.Models;

public interface IConverter<TInput, TOutput>
{
	public TOutput Convert( ISource<TInput> source );
	public Task<TOutput> ConvertAsync( ISource<TInput> source );
}