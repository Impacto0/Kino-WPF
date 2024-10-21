using LiteDB;

namespace Kino.Screenings;

public class ScreeningsManager
{
	public static ScreeningsManager Instance { get; } = new ScreeningsManager();
	private LiteDatabase _db;
	
	public ScreeningsManager()
	{
		_db = new LiteDatabase("screenings.db");
		// CreateScreening(Guid.Parse("8e4e80c5-b466-434e-a4de-a737f04f8f6c"), DateTime.Parse("2024-07-01T12:00:00"));
	}
	
	public List<Screening> GetScreenings()
	{
		ILiteCollection<Screening>? screenings = _db.GetCollection<Screening>("screenings");
		return screenings.FindAll().ToList();
	}

	public Screening GetScreening(Guid screeningId)
	{
		ILiteCollection<Screening>? screenings = _db.GetCollection<Screening>("screenings");
		return screenings.FindById(screeningId);
	}
	
	public List<Screening> GetScreenings(Guid movieId)
	{
		ILiteCollection<Screening>? screenings = _db.GetCollection<Screening>("screenings");
		return screenings.Find(s => s.MovieId == movieId).ToList();
	}
	
	public void AddScreening(Screening screening)
	{
		ILiteCollection<Screening>? screenings = _db.GetCollection<Screening>("screenings");
		screenings.Insert(screening);
	}
	
	public void RemoveScreening(Guid screeningId)
	{
		ILiteCollection<Screening>? screenings = _db.GetCollection<Screening>("screenings");
		screenings.Delete(screeningId);
	}
	
	public Screening CreateScreening(Guid movieId, DateTime date)
	{
		Screening screening = new Screening
		{
			Id = Guid.NewGuid(),
			MovieId = movieId,
			Date = date
		};
		AddScreening(screening);
		return screening;
	}
	
	public void ReserveSeat(Guid screeningId, Guid seatId)
	{
		ILiteCollection<Screening>? screenings = _db.GetCollection<Screening>("screenings");
		Screening screening = GetScreening(screeningId);
		screening.ReservedSeats.Add(seatId);
		screenings.Update(screening);
	}
	
	public void UnreserveSeat(Guid screeningId, Guid seatId)
	{
		ILiteCollection<Screening>? screenings = _db.GetCollection<Screening>("screenings");
		Screening screening = GetScreening(screeningId);
		screening.ReservedSeats.Remove(seatId);
		screenings.Update(screening);
	}
}