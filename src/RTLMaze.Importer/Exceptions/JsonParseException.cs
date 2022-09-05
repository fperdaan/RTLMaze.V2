namespace RTLMaze.Importer.Exceptions;

public class JsonParseException : Exception
{
	public JsonParseException() { }

    public JsonParseException( string message ) : base( message ) { }

    public JsonParseException( string message, Exception inner ) : base(message, inner) { }
}