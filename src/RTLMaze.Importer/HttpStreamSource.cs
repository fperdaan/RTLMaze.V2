using System.Net;
using Polly;
using Polly.RateLimit;

namespace RTLMaze.Importer;

public partial class HttpStreamSource : ISource<Stream>
{
	static public AsyncPolicy<HttpResponseMessage> RequestPolicy { get; set; }
	private string? _sourceUrl;

	static HttpStreamSource()
	{
		// -- todo; inject with DI and make non-static
		# region TODO
		var codes = new[]{
			HttpStatusCode.RequestTimeout, // 408
			HttpStatusCode.InternalServerError, // 500
			HttpStatusCode.BadGateway, // 502
			HttpStatusCode.ServiceUnavailable, // 503
			HttpStatusCode.GatewayTimeout // 504
		};

		var requestPolicy = Policy
				.Handle<HttpRequestException>()
				.OrResult<HttpResponseMessage>( r => codes.Contains( r.StatusCode ) )
				.RetryAsync( 3 );

		

		var rateLimitPolicy = Policy
				.RateLimitAsync( 5, TimeSpan.FromSeconds( 10 ), 5 );

		var retryPolicy = Policy
				.Handle<RateLimitRejectedException>()
				.RetryForeverAsync(onRetry: ex => {
					
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine( $"Waiting {((RateLimitRejectedException)ex).RetryAfter}" );
					Console.ResetColor();

					Task.Delay( ((RateLimitRejectedException)ex).RetryAfter ).Wait();
				});
					

		RequestPolicy = Policy
				.WrapAsync( retryPolicy, rateLimitPolicy )
				.WrapAsync( requestPolicy );	

		# endregion
	}

	# region Configuration interface 

	public virtual HttpStreamSource FromUrl( string sourceUrl )
	{
		_sourceUrl = sourceUrl;

		return this;
	}

	# endregion

	# region Source interface implementation
	public virtual Stream GetData() => GetDataAsync().Result;

	public virtual async Task<Stream> GetDataAsync()
	{
		if( _sourceUrl == null )
			throw new ArgumentException( message: "Source is not set", nameof( _sourceUrl ) );

		var client = new HttpClient();		
		var result = await RequestPolicy.ExecuteAsync( () => client.GetAsync( _sourceUrl ) );

		return await result.Content.ReadAsStreamAsync();
	}

	# endregion
}