using System.Net;
using Polly;
using Polly.Wrap;

namespace RTLMaze.Core.Models;

public partial class HttpStreamSource : ISource<Stream>
{
	public AsyncPolicy<HttpResponseMessage> RequestPolicy { get; set; }
	private string? _sourceUrl;

	public HttpStreamSource()
	{
		// -- todo; inject with DI
		# region TODO
		var codes = new HttpStatusCode[]{
			HttpStatusCode.RequestTimeout, // 408
			HttpStatusCode.InternalServerError, // 500
			HttpStatusCode.BadGateway, // 502
			HttpStatusCode.ServiceUnavailable, // 503
			HttpStatusCode.GatewayTimeout // 504
		};

		RequestPolicy = Policy
				.Handle<HttpRequestException>()
				.OrResult<HttpResponseMessage>( r => codes.Contains( r.StatusCode ) )
				.RetryAsync( 3 );

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
			throw new ArgumentException( message: "Source is not set", paramName: "_sourceUrl" );

		var client = new HttpClient();		
		var result = await RequestPolicy.ExecuteAsync( () => client.GetAsync( _sourceUrl ) );

		return result.Content.ReadAsStream();
	}

	# endregion
}