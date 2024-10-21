namespace Kino.Tickets;

public class Ticket
{
	public Guid Id { get; set; }
	public Guid AccountId { get; set; }
	public Guid SeatId { get; set; }
	public Guid MovieId { get; set; }
	public Guid ScreeningId { get; set; }
	public DateTime PurchaseDate { get; set; }
}