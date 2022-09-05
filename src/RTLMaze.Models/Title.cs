namespace RTLMaze.Models;

// Temp placeholder file
public class Title 
{
	public int ID { get; set; }
	public string Name { get; set; } = "";


	public override string ToString()
	{
		return $"{ID,8} / {Name}";
	}
}