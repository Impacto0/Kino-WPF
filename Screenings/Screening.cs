namespace Kino.Screenings;

public class Screening
{
	public Guid Id { get; set; }
	public Guid MovieId { get; set; }
	public DateTime Date { get; set; }
	public List<Guid> ReservedSeats { get; set; } = new();
}