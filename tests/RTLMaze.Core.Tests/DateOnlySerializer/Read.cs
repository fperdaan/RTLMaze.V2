using System.Text.Json;

namespace RTLMaze.Core.Tests.DateOnlySerializer;

[TestClass]
public class Read_Should
{
	[TestMethod]
	public void FormatProperly_WhenSuppliedCorrect()
	{
		// Arrange 
		var options = new JsonSerializerOptions { Converters = { new SUT.Core.Serializers.DateOnlySerializer() } };
		
		// Format yyyy-MM-dd
		const string jsonString = "\"2022-06-01\"";
		const string expectedResult = "2022-06-01";

		// Act
		var date = JsonSerializer.Deserialize<DateOnly>( jsonString, options );

		// Assert
		Assert.IsNotNull( date );
		Assert.AreEqual( expected: expectedResult, actual: date.ToString("yyyy-MM-dd") );
	}
	
	[TestMethod]
	public void ThrowException_WhenSuppliedIncorrect()
	{
		// Arrange 
		var options = new JsonSerializerOptions { Converters = { new SUT.Core.Serializers.DateOnlySerializer() } };
		
		// -- Format yyyy-MM-dd
		const string jsonString = "\"2022/06/01\"";

		// Act & Assert
		Assert.ThrowsException<FormatException>( () => JsonSerializer.Deserialize<DateOnly>( jsonString, options ) );
	}
}