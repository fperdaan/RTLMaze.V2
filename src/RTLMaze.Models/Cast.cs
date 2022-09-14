namespace RTLMaze.Models;

public class Cast
{
	public virtual Person Person { get; set; }
	
	// Character information
	public int ID { get; set; }
	public string Name { get; set; }
}