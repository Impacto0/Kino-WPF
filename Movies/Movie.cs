namespace Kino.Movies;

public class Movie
{
	public Guid Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string PosterPath { get; set; }
	public float Rating { get; set; }
	public int RatingCount { get; set; }
}