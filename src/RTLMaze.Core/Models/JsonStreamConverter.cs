using System.Text.Json;
using System.Text.Json.Serialization;
using RTLMaze.Core.Exceptions;

namespace RTLMaze.Core.Models;

public partial class JsonStreamConverter<TOutput> : IProcessor<Stream, TOutput>
{
	private JsonSerializerOptions _serializerOptions;

	public JsonStreamConverter()
	{
		// -- TODO; inject with DI
		_serializerOptions = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true
		};
	}

	# region Configuration interface

	public virtual JsonStreamConverter<TOutput> AddSerializerConverter( JsonConverter converter )
	{
		_serializerOptions.Converters.Add( converter );

		return this;
	}	

	public virtual JsonStreamConverter<TOutput> SetSerializerOptions( JsonSerializerOptions options )
	{
		_serializerOptions = options;

		return this;
	}

	# endregion

	# region Processor interface implementation

	public virtual TOutput Process( Stream source )
	{
		TOutput? result;

		try 
		{
			result = JsonSerializer.Deserialize<TOutput>( source, _serializerOptions );
		}
		catch( Exception e )
		{
			throw new JsonParseException( "The supplied stream does not match the expected json format", e );
		}

		if( result == null )
			throw new JsonParseException( "The serialization resolved in an empty result" );

		return result;
	}

	# endregion
}