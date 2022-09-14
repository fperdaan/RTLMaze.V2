using System.Text.Json;
using System.Text.Json.Nodes;
using RTLMaze.Core.Serializers;
using RTLMaze.Models;

namespace RTLMaze.MazeScraper.Deserializer;

public class TitleDeserializer : GenericSerializer<Title>
{
	public override Title? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		var data  = JsonSerializer.Deserialize<JsonObject>( ref reader, options );
		
		// Lazy extract as much information into the object as possible
		var title = data?.Deserialize<Title>( _RemoveSelfFromOptions( options ) );

		if ( title == null )
			return null;

		if ( data!["_embedded"]?["cast"] == null ) 
			return title;
		
		var cast = data["_embedded"]!["cast"].Deserialize<Cast[]>( options );

		if ( cast == null )
			return title;
		
		title.Cast = cast.Where( e => e != null );

		// TODO
		// attach cast to title object

		return title;
	}
}