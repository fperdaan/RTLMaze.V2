﻿using System.Collections.Concurrent;
using System.Net;
using System.Net.Security;
using System.Text.Json;
using Polly;
using Polly.RateLimit;

using RTLMaze.Core.Models;


var source = new HttpStreamSource()
					.FromUrl("https://api.tvmaze.com/updates/shows?since=day");

var processor = new JsonStreamConverter<Dictionary<string,long>>();
var updated = processor.Convert( source );

long offset = new DateTimeOffset( DateTime.UtcNow ).AddHours( -12 ).ToUnixTimeSeconds();

var updatedSinceOffset = updated
		.Where( kp => kp.Value > offset )
		.Select( kp => Int32.Parse( kp.Key ) );

Console.WriteLine( JsonSerializer.Serialize( updatedSinceOffset ) );

/*
var producer = new Producer();
var consumer = new Consumer();

var bag = new BlockingCollection<int>();

var rateLimit = Policy
		.RateLimitAsync( 5, TimeSpan.FromSeconds( 10 ), 5 );

var retryPolicy = Policy
		.Handle<RateLimitRejectedException>()
		.RetryForeverAsync(onRetry: ex => {
			
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine( $"Waiting {((RateLimitRejectedException)ex).RetryAfter}" );

			Task.Delay( ((RateLimitRejectedException)ex).RetryAfter ).Wait();
		});
			

var policy = Policy.WrapAsync( retryPolicy, rateLimit );


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
