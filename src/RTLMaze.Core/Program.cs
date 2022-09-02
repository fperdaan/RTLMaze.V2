// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Collections.Concurrent;

var producer = new Producer();
var consumer = new Consumer();

var bag = new BlockingCollection<int>();

var task1 = Task.Run( async () => {
	await foreach( int i in producer.Produce() ) 
		bag.Add( i );

	bag.CompleteAdding();
});

var task2 = Task.Run( () => {
	int number;

	while( !bag.IsCompleted )
	{
		if( bag.TryTake( out number, TimeSpan.FromSeconds(1) ) )
			consumer.Process( number );
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

	public async Task<int> GenerateNumber()
	{
		await Task.Delay( 200 );

		var i = new Random().Next( 10, 20 );

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine( $"[produce] Thread: {Thread.CurrentThread.ManagedThreadId}, Item: ${i}" );

		return i;
	}
}


public class Consumer 
{
	public void Process( int i )
	{
		Thread.Sleep( 500 );

		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine( $"[consume] Thread: {Thread.CurrentThread.ManagedThreadId}, Item: ${i}" );
	}
}
