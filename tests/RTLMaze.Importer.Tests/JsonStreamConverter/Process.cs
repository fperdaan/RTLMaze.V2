using System.Text;
using System.Text.Json;

namespace RTLMaze.Importer.Tests.JsonStreamConverter;

[TestClass]
public class Process_Should
{
    [TestMethod]
    public void Convert_OnValidJsonInput()
    {
		// Arrange 
		var expectedResult = new JsonMockObject{ ID = 10, Name = "Test" };

		var json = JsonSerializer.Serialize( expectedResult );
		var stream = new MemoryStream( Encoding.UTF8.GetBytes( json ) );

		var actor = new SUT.Importer.Models.JsonStreamConverter<JsonMockObject>();

		// Act 
		var result = actor.Process( stream );

		// Assert
		Assert.IsNotNull( result );
		Assert.AreEqual( expected: expectedResult.GetType().Name, actual: result.GetType().Name );
		Assert.AreEqual( expected: expectedResult.ID, actual: result.ID );
		Assert.AreEqual( expected: expectedResult.Name, actual: result.Name );
    }

	[TestMethod]
    public void ThrowException_OnInValidJsonInput()
    {
		// Arrange 
		var json = "Hello world!";
		var stream = new MemoryStream( Encoding.UTF8.GetBytes( json ) );

		var actor = new SUT.Importer.Models.JsonStreamConverter<JsonMockObject>();

		// Act & Assert
		Assert.ThrowsException<SUT.Importer.Exceptions.JsonParseException>( () => actor.Process( stream ) );
    }

	[TestMethod]
    public void ThrowException_OnInUnexpectedJsonInput()
    {
		// Arrange 
		var json = JsonSerializer.Serialize( new[]{1,2,3} );
		var stream = new MemoryStream( Encoding.UTF8.GetBytes( json ) );

		var actor = new SUT.Importer.Models.JsonStreamConverter<JsonMockObject>();

		// Act & Assert
		Assert.ThrowsException<SUT.Importer.Exceptions.JsonParseException>( () => actor.Process( stream ) );
    }
}

internal class JsonMockObject 
{
	public int ID { get; init; }
	public string? Name { get; init; }
}