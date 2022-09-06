using System.Text.Json;
using System.Text.Json.Nodes;
using RTLMaze.Importer;
using RTLMaze.Importer.Models;
using RTLMaze.MazeScraper.Models;

// -- Though #1

// Naming: Converter vs Processor, what is better?
// Would be cool if we create a linked list trough the processors, so they can invoke each-other using the output. 
// Thus allowing a chain of processing

// Next?.Process()

// Can we make this work in the generic interface? Not with a return chain I think, so maybe the processor indeed is a
// different class. Not returning anything but do allow chaining of results into one another? 


// -- Thought #2
// Not really sure how the async methods in IProcessor and ISource really benefit in the current setup. This should
// probably be determined by  the source / processor. Does that mean however that we also need to split up the
// interfaces into a sync and async interface? and how will that affect our coupling? 


// -- Thought #3 
// The title updated source is probably to specific, to focused on updated ( might not be though )
// What we also want is a list containing the deleted items. That can be extracted from the same source by running a
// diff between the current register and the fetched register. Items whom are no longer on there are deleted

// --------------------------------------------------

// var source = new TitleUpdatedSource();
// 	source.Since( DsteTime.Now.AddDays( -1 ) );
//
// source.Count();

// foreach( var title in source.GetData() )
// {
// 	Console.WriteLine( title );
// }

// var url = $"https://api.tvmaze.com/shows/10?embed=cast";
//
// var source = new HttpStreamSource();
// 	source.FromUrl( url );
//
// var processor = new JsonStreamConverter<JsonObject>();
//
// var result = processor.Process( source.GetData() );
//
// var options = new JsonSerializerOptions { WriteIndented = true };
//
// Console.WriteLine( JsonSerializer.Serialize( result, options ) );



var result = Enumerable.Range( 1, 200 ).ToList();

// As per rate-limiter; allow 10 items per 5 seconds
var tracker = new ProgressTracker( itemCount: result.Count(), avgItemsPerSecond: 10 / 5 );

var pos = Console.GetCursorPosition();

foreach( var itemId in result )
{
	await Task.Delay(200);

	tracker.Next();	
	
	Console.SetCursorPosition( pos.Left, pos.Top );
	Console.WriteLine( tracker );
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
// 			["Name"] = "Some person",
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