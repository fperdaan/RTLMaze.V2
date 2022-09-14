namespace RTLMaze.Models;

// Temp placeholder file
public class Title 
{
	public int ID { get; set; }
	public string Name { get; set; } = "";
	public DateOnly? Premiered { get; set; }
	public virtual IEnumerable<Cast> Cast { get; set; } = new List<Cast>();

	public override string ToString()
	{
		return $"{ID,8} / {Name} / {Premiered}";
	}
}