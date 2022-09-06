namespace RTLMaze.Core.Tests.EnumerableExtension;

[TestClass]
public class WhereIf_Should
{
    [TestMethod]
    public void Apply_WhenConditionMet()
    {
		// Arrange 
		var actor = new[]{ 1, 2, 3 };
		var filteredValue = 2;

		// Act 
		var result = actor.WhereIf( true, ( i ) => i != filteredValue ).ToList();

		// Assert
		Assert.IsFalse( result.Contains( filteredValue ) );
    }


    [TestMethod]
    public void Skip_WhenConditionNotMet()
    {
		// Arrange 
		var actor = new List<int>{ 1, 2, 3 };
		var filteredValue = 2;

		// Act 
		var result = actor.WhereIf( false, ( i ) => i != filteredValue ).ToList();

		// Assert
		Assert.IsTrue( result.Contains( filteredValue ) );
    }

}