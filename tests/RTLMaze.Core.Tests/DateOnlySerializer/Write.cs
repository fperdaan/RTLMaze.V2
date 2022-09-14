using System.Text.Json;

namespace RTLMaze.Core.Tests.DateOnlySerializer;

[TestClass]
public class Write_Should
{
	[TestMethod]
	public void SerializeProperly_WhenSuppliedCorrect()
	{
		// Arrange 
		var options = new JsonSerializerOptions { Converters = { new SUT.Core.Serializers.DateOnlySerializer() } };
		
		// Format yyyy-MM-dd
		var date = DateOnly.Parse( "2022-06-01" );
		const string expectedResult = "\"2022-06-01\"";

		// Act
		var dateJson = JsonSerializer.Serialize( date, options );

		// Assert
		Assert.IsNotNull( dateJson );
		Assert.AreEqual( expected: expectedResult, actual: dateJson );
	}
}