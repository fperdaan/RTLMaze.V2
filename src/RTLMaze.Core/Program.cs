using System.Collections.Concurrent;
using System.Net;
using System.Net.Security;
using System.Text.Json;
using System.Text.Json.Nodes;
using Polly;
using Polly.RateLimit;

using RTLMaze.Core.Models;

using RTLMaze.Core.Models.MazeScraper;



// -- Though #1

// Naming: Converter vs Processor, what is better?
// Would be cool if we create a linked list trough the processors, so they can invoke each-other using the output. 
// Thus allowing a chain of processing

// Next?.Process()

// Can we make this work in the generic interface? Not with a return chain I think, so maybe the processor indeed is a diffent class.
// Not returning anything but do allow chaining of results into one another? 


// -- Thought #2
// Not really sure how the async methods in IProcessor and ISource really benefit in the current setup. This should probably be determined by 
// the source / processor. Does that mean however that we also need to split up the interfaces into a sync and async interface? 
// and how will that affect our coupeling? 

// --------------------------------------------------

var source = new TitleUpdatedSource();
	source.Since( DateTime.Now.AddDays( -1 ) ); 

var result = source.GetData();


var processor = new TitleDetailsProcessor();

// The ratelimiter should be integrated into the http request policy. This because failed request/retries 
// should also contribute to the ratelimiter
# region TODO 

var rateLimitPolicy = Policy
		.RateLimitAsync( 5, TimeSpan.FromSeconds( 10 ), 5 );

var retryPolicy = Policy
		.Handle<RateLimitRejectedException>()
		.RetryForeverAsync(onRetry: ex => {
			
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine( $"Waiting {((RateLimitRejectedException)ex).RetryAfter}" );

			Task.Delay( ((RateLimitRejectedException)ex).RetryAfter ).Wait();
		});
			

var policy = Policy.WrapAsync( retryPolicy, rateLimitPolicy );	

# endregion

foreach( var itemId in result )
{
	var title = await policy.ExecuteAsync( () => Task.Run( () => processor.Process( itemId  ) ) );

	Console.ForegroundColor = ConsoleColor.Magenta;
	Console.WriteLine( JsonSerializer.Serialize( title ) );
}



// var test = new JsonObject
// {
// 	["ID"] = 10,
// 	["Name"] = "Under the dome",
// 	["Cast"] = new JsonArray
// 	{
// 		new JsonObject 
// 		{
// 			["ID"] = 20,
// 			["Name"] = "Some peroson",
// 			["BirthDay"] = DateOnly.FromDateTime( DateTime.Now ).ToString("yyyy-MM-dd")
// 		}
// 	} 
// };

// Console.WriteLine( test.ToJsonString() );

/*
var producer = new Producer();
var consumer = new Consumer();

var bag = new BlockingCollection<int>();

var rateLimitPolicy = Policy
		.RateLimitAsync( 5, TimeSpan.FromSeconds( 10 ), 5 );

var retryPolicy = Policy
		.Handle<RateLimitRejectedException>()
		.RetryForeverAsync(onRetry: ex => {
			
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine( $"Waiting {((RateLimitRejectedException)ex).RetryAfter}" );

			Task.Delay( ((RateLimitRejectedException)ex).RetryAfter ).Wait();
		});
			

var policy = Policy.WrapAsync( retryPolicy, rateLimitPolicy );


var task1 = Task.Run( async () => {
	await foreach( int i in producer.Produce() ) 
		bag.Add( i );

	bag.CompleteAdding();
});

var task2 = Task.Run( async () => {
	int number;

	while( !bag.IsCompleted )
	{
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine("Try fetch");

		if( bag.TryTake( out number, TimeSpan.FromSeconds(5) ) )
		{
			await policy.ExecuteAsync( () => consumer.Process( number ) );
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
}*/


public class Title 
{
	public int ID { get; set; }
	public string Name { get; set; } = "";
}