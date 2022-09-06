using System.Text.Json;
using System.Text.Json.Serialization;

namespace RTLMaze.Core.Serializers;

public class DateOnlySerializer : JsonConverter<DateOnly>
{
	public const string DATE_FORMAT = "yyyy-MM-dd";

	public override DateOnly Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		=> DateOnly.ParseExact( reader.GetString()!, DATE_FORMAT );

	public override void Write( Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options )
		=> writer.WriteStringValue( value.ToString( DATE_FORMAT ) );
}