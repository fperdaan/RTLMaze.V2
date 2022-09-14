using System.Text.Json;
using System.Text.Json.Nodes;
using RTLMaze.Core.Serializers;
using RTLMaze.Models;

namespace RTLMaze.MazeScraper.Deserializer;

public class CastDeserializer : GenericSerializer<Cast>
{
	public override Cast? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		var data = JsonSerializer.Deserialize<JsonObject>( ref reader, options );

		if ( data?["character"] == null || data?["person"] == null )
			return null;
		
		var person = data["person"].Deserialize<Person>( options );

		if ( person == null )
			return null;

		var cast = data["character"].Deserialize<Cast>( _RemoveSelfFromOptions( options ) );

		if ( cast == null )
			return null;
		
		// Set reference
		cast.Person = person;
		
		return cast;
	}
}