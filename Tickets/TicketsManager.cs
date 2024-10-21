using Kino.Screenings;
using LiteDB;

namespace Kino.Tickets;

public class TicketsManager
{
	public static TicketsManager Instance { get; } = new TicketsManager();
	private LiteDatabase _db;
	
	public TicketsManager()
	{
		_db = new LiteDatabase("tickets.db");
	}
	
	public List<Ticket> GetUserTickets(Guid accountId)
	{
		var tickets = _db.GetCollection<Ticket>("tickets");
		return tickets.Find(t => t.AccountId == accountId).ToList();
	}
	
	public Ticket GetTicket(Guid ticketId)
	{
		var tickets = _db.GetCollection<Ticket>("tickets");
		return tickets.FindById(ticketId);
	}
	
	public void AddTicket(Ticket ticket)
	{
		var tickets = _db.GetCollection<Ticket>("tickets");
		tickets.Insert(ticket);
	}
	
	public void RemoveTicket(Guid ticketId)
	{
		var tickets = _db.GetCollection<Ticket>("tickets");
		ScreeningsManager.Instance.UnreserveSeat(GetTicket(ticketId).ScreeningId, GetTicket(ticketId).SeatId);
		tickets.Delete(ticketId);
	}
	
	public Ticket CreateTicket(Guid accountId, Guid seatId, Guid movieId, Guid screeningId)
	{
		Ticket ticket = new Ticket
		{
			Id = Guid.NewGuid(),
			AccountId = accountId,
			SeatId = seatId,
			MovieId = movieId,
			ScreeningId = screeningId,
			PurchaseDate = DateTime.Now
		};
		AddTicket(ticket);
		ScreeningsManager.Instance.ReserveSeat(screeningId, seatId);
		return ticket;
	}
}