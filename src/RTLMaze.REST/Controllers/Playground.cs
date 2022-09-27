using Microsoft.AspNetCore.Mvc;

namespace RTLMaze.REST.Controllers;

[ApiController]
[Route("[controller]")]
public class Playground : ControllerBase
{

	[HttpGet, Route("string")]
	public string ResponseString()
	{
		Console.WriteLine("API Request");
		return "Sample response";
	}
	
	[HttpGet, Route("action")]
	public IActionResult ResponseAction()
	{
		Console.WriteLine("API Request");
		return Ok("With response data");
	}
	
	[HttpGet, Route("array")]
	public int[] ResponseArray()
	{
		Console.WriteLine("API Request");
		return new int[]{ 1, 2, 3 };
	}
	
	[HttpGet, Route("exception")]
	public void ResponseException()
	{
		Console.WriteLine("API Request");
		throw new Exception("This is a test" );
	}
}