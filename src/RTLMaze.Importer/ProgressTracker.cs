using System.Diagnostics;
using System.Text;

namespace RTLMaze.Importer;

public class ProgressTracker
{
	# region Properties

	const char BLOCK = '■';
	const int BLOCK_COUNT = 15;

	public int ItemCount { get; }
	public int? AvgItemsPerSecond { get; }
	public int CurrentItem { get; private set; }
	public Stopwatch Timer { get; private set; }

	public double EstimatedTotalTime 
	{ 
		get 
		{
			if( AvgItemsPerSecond == null )
				return 0;

			return ItemCount / (double)AvgItemsPerSecond;
		}
	}

	public double EstimatedRemainingTime 
	{
		get 
		{
			if( AvgItemsPerSecond == null )
				return 0;

			return ( ItemCount - CurrentItem ) / (double)AvgItemsPerSecond;
		}
	}

	public decimal Progress 
	{
		get 
		{
			return CurrentItem > 0 ? ( (decimal)CurrentItem / ItemCount ) * 100m : 0m;
		}
	}

	# endregion
	# region Constructors

	public ProgressTracker( int itemCount )
	{
		ItemCount = itemCount;
		CurrentItem = 0;
		Timer = new Stopwatch();

		if( itemCount <= 0 )
			throw new ArgumentException( "Please supply a count higer than 0 to use the tracker", "itemCount" );
	}

	public ProgressTracker( int itemCount, int avgItemsPerSecond ) : this( itemCount )
	{
		AvgItemsPerSecond = avgItemsPerSecond;
	}

	# endregion

	# region Progression interface

	public ProgressTracker Next()
	{
		if( CurrentItem == 0 )
			Timer.Start();

		if( CurrentItem >= ItemCount )
			return this;

		++CurrentItem;

		if( CurrentItem == ItemCount )
			Stop(); 

		return this;	
	}

	/// <summary>
	/// Manually stop ( abort ) the tracker
	/// </summary>
	public ProgressTracker Stop()
	{
		if( Timer != null )
			Timer.Stop();

		return this;
	}

	# endregion


	public override string ToString()
	{
		var content = new StringBuilder();

		content.AppendLine( $"Estimated time: {EstimatedTotalTime:F2} seconds, Estimated time remaining {EstimatedRemainingTime:F2} seconds" );
		content.AppendLine( $"Time elapsed: {Timer!.Elapsed.TotalSeconds:F2} seconds" );
		content.AppendLine();

		content.Append( '[' );

		int progress = (int)Progress * BLOCK_COUNT / 100;

		for ( int i = 0; i < BLOCK_COUNT; ++i )
		{
			if ( i >= progress )
				content.Append( ' ' );
			else
				content.Append( BLOCK );
		}

		content.Append( $"] {Progress:F2}%, {CurrentItem}/{ItemCount}" );    

		return content.ToString();
	}
}