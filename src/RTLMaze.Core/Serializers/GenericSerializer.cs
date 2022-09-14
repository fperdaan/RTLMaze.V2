using System.Text.Json;
using System.Text.Json.Serialization;

namespace RTLMaze.Core.Serializers;

public abstract class GenericSerializer<T> : JsonConverter<T>
{
	public override T? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		=> JsonSerializer.Deserialize<T?>( ref reader, _RemoveSelfFromOptions( options ) );

	public override void Write( Utf8JsonWriter writer, T value, JsonSerializerOptions options )
		=> JsonSerializer.Serialize<T>( writer, value, _RemoveSelfFromOptions( options ) );

	protected virtual JsonSerializerOptions _RemoveSelfFromOptions( JsonSerializerOptions options )
	{
		var copy = new JsonSerializerOptions( options );
		copy.Converters.Remove( this );
		
		return copy;
	}
}