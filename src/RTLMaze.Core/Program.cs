// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Collections.Concurrent;
using Polly;
using Polly.RateLimit;

var producer = new Producer();
var consumer = new Consumer();

var bag = new BlockingCollection<int>();

var rateLimit = Policy
		.RateLimit( 5, TimeSpan.FromSeconds( 10 ), 5 );

var retryPolicy = Policy
		.Handle<RateLimitRejectedException>()
		.RetryForever(onRetry: ex => {
			
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine( $"Waiting {((RateLimitRejectedException)ex).RetryAfter}" );

			Task.Delay( ((RateLimitRejectedException)ex).RetryAfter ).Wait();
		});
			
var policy = Policy.Wrap( retryPolicy, rateLimit );


var task1 = Task.Run( async () => {
	await foreach( int i in producer.Produce() ) 
		bag.Add( i );

	bag.CompleteAdding();
});

var task2 = Task.Run( async () => {
	int number;

	while( !bag.IsCompleted )
	{
		if( bag.TryTake( out number, TimeSpan.FromSeconds(2) ) )
		{
			await policy.Execute( () => consumer.Process( number ) );
		}
	}
});

Task.WaitAll( task1, task2 );

Console.ResetColor();
Console.WriteLine();
Console.WriteLine( new string( '-', 10 ) );
Console.WriteLine("Program done");



public class Producer
{
	public async IAsyncEnumerable<int> Produce()
	{
		var items = new Random().Next( 10, 20 );

		for( int i = 0; i < items; i++ )
		{
			await Task.Delay( 100 );

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine( $"[produce] Thread: {Thread.CurrentThread.ManagedThreadId}, Item: ${i}" );

			yield return i;
		}
	}
}


public class Consumer 
{
	public async Task Process( int i )
	{
		await Task.Delay( 500 );

		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine( $"[consume] Thread: {Thread.CurrentThread.ManagedThreadId}, Item: ${i}" );
	}
}
