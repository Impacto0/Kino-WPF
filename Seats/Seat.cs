namespace Kino.Seats;

public class Seat
{
	public Guid Id { get; set; }
	public int Row { get; set; }
	public int Column { get; set; }

	public override string ToString()
	{
		return $"Rząd: {Row + 1}, Miejsce: {Column + 1}";
	}
}