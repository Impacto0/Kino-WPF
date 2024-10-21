using System.IO;
using System.Windows.Media.Imaging;
using LiteDB;

namespace Kino.Movies;

public class MoviesManager
{
	public static MoviesManager Instance { get; } = new MoviesManager();
	private LiteDatabase _db;
	
	private MoviesManager()
	{
		_db = new LiteDatabase("movies.db");
		// ILiteCollection<Movie>? movies = _db.GetCollection<Movie>("movies");
		// ILiteStorage<string>? fs = _db.FileStorage;
		// movies.Insert(new Movie
		// {
		// 	Id = Guid.NewGuid(),
		// 	Title = "W głowie się nie mieści 2",
		// 	Description = "Riley jest już nastolatką. W jej życiu pojawiają się nowe emocje, które kształtują osobowość dorastającej dziewczyny.",
		// 	PosterPath = "Inside_Out_2.jpg",
		// 	Rating = 4.5f,
		// 	RatingCount = 81753
		// });
		// fs.Upload("$/posters/Inside_Out_2.jpg", "Inside_Out_2.jpg");
	}
	
	public List<Movie> GetMovies()
	{
		ILiteCollection<Movie>? movies = _db.GetCollection<Movie>("movies");
		return movies.FindAll().ToList();
	}
	
	public void AddMovie(Movie movie)
	{
		ILiteCollection<Movie>? movies = _db.GetCollection<Movie>("movies");
		movies.Insert(movie);
	}
	
	public void UpdateMovie(Movie movie)
	{
		ILiteCollection<Movie>? movies = _db.GetCollection<Movie>("movies");
		movies.Update(movie);
	}
	
	public void DeleteMovie(Guid id)
	{
		ILiteCollection<Movie>? movies = _db.GetCollection<Movie>("movies");
		movies.Delete(id);
	}
	
	public Movie GetMovie(Guid id)
	{
		ILiteCollection<Movie>? movies = _db.GetCollection<Movie>("movies");
		return movies.FindById(id);
	}
	
	public Movie GetMovie(string title)
	{
		ILiteCollection<Movie>? movies = _db.GetCollection<Movie>("movies");
		return movies.FindOne(x => x.Title == title);
	}
	
	public Stream GetMoviePoster(string imagePath)
	{
		ILiteStorage<string>? fs = _db.FileStorage;
		return fs.OpenRead($"$/posters/{imagePath}");
	}
	
	public BitmapImage GetMoviePosterBitmap(string imagePath)
	{
		Stream stream = GetMoviePoster(imagePath);
		BitmapImage image = new BitmapImage();
		image.BeginInit();
		image.StreamSource = stream;
		image.EndInit();
		return image;
	}
	
	public void UploadMoviePoster(string imagePath)
	{
		ILiteStorage<string>? fs = _db.FileStorage;
		fs.Upload($"$/posters/{imagePath.Split("\\").Last()}", imagePath);
	}
	
	public List<Movie> GetTop3Movies()
	{
		ILiteCollection<Movie>? movies = _db.GetCollection<Movie>("movies");
		return movies.FindAll().OrderByDescending(x => x.Rating).Take(3).ToList();
	}
}