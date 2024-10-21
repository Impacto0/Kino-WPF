using LiteDB;

namespace Kino.Seats;

public class SeatsManager
{
	private const int ROWS = 2;
	private const int COLUMNS = 3;
	
	public static SeatsManager Instance { get; } = new SeatsManager();
	private LiteDatabase _db;
	private List<Seat> _seats = new();
	
	public SeatsManager()
	{
		_db = new LiteDatabase("seats.db");
		// GenerateSeats();
		_seats = _db.GetCollection<Seat>("seats").FindAll().ToList();
	}
	
	public void GenerateSeats()
	{
		_seats = new List<Seat>();
		for (int i = 0; i < ROWS; i++)
		{
			for (int j = 0; j < COLUMNS; j++)
			{
				_seats.Add(new Seat { Row = i, Column = j });
			}
		}
		
		ILiteCollection<Seat>? seats = _db.GetCollection<Seat>("seats");
		seats.Insert(_seats);
	}
	
	public List<Seat> GetSeats()
	{
		return _seats;
	}

	public Seat? GetSeat(int row, int col)
	{
		Seat? seat = _seats.Find(s => s.Row == row && s.Column == col);
		return seat;
	}
	
	public Seat? GetSeat(Guid seatId)
	{
		Seat? seat = _seats.Find(s => s.Id == seatId);
		return seat;
	}
}