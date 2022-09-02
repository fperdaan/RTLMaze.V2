using System.Text.Json;
using System.Text.Json.Serialization;
using RTLMaze.Core.Exceptions;

namespace RTLMaze.Core.Models;

public partial class JsonStreamConverter<TOutput> : IConverter<Stream, TOutput>
{
	private JsonSerializerOptions _serializerOptions;

	public JsonStreamConverter()
	{
		// -- todo; inject with DI
		# region TODO
		_serializerOptions = new JsonSerializerOptions();
		# endregion
	}

	# region Fluid interface

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
	public virtual TOutput Convert( ISource<Stream> source )
	{
		TOutput? result;

		try 
		{
			using ( StreamReader reader = new StreamReader( source.GetData() ) )
			{
				string json = reader.ReadToEnd();
				result = JsonSerializer.Deserialize<TOutput>( json, _serializerOptions );
			}
		}
		catch( Exception e )
		{
			throw new JsonParseException( "The supplied stream does not match the expected json format", e );
		}


		if( result == null )
			throw new JsonParseException( "The serialization resolved in an empty result" );

		return result;
	}
	public Task<TOutput> ConvertAsync( ISource<Stream> source ) => Task.Run( () => Convert( source ) );
	# endregion
}