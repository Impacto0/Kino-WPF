using System.Windows;
using System.Windows.Controls;
using Kino.Movies;

namespace Kino.Pages;

public partial class MainPage : Page
{
	public MainPage()
	{
		InitializeComponent();

		List<Movie> topMovies = MoviesManager.Instance.GetTop3Movies();
		Top1MovieTitle.Text = topMovies[0].Title;
		Top1MovieRating.Value = topMovies[0].Rating;
		Top1MovieMoreButton.Click += (sender, args) => ShowMoviePage(topMovies[0].Id);
		Top1MoviePoster.ImageSource = MoviesManager.Instance.GetMoviePosterBitmap(topMovies[0].PosterPath);
		Top2MovieTitle.Text = topMovies[1].Title;
		Top2MovieRating.Value = topMovies[1].Rating;
		Top2MovieMoreButton.Click += (sender, args) => ShowMoviePage(topMovies[1].Id);
		Top2MoviePoster.ImageSource = MoviesManager.Instance.GetMoviePosterBitmap(topMovies[1].PosterPath);
		Top3MovieTitle.Text = topMovies[2].Title;
		Top3MovieRating.Value = topMovies[2].Rating;
		Top3MovieMoreButton.Click += (sender, args) => ShowMoviePage(topMovies[2].Id);
		Top3MoviePoster.ImageSource = MoviesManager.Instance.GetMoviePosterBitmap(topMovies[2].PosterPath);
	}
	
	private void ShowMoviePage(Guid movieId)
	{
		ServiceManager.SelectedMovieId = movieId;
		ServiceManager.NavigationView.Navigate(typeof(MoviePage));
	}
}