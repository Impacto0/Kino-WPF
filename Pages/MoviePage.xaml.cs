using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Kino.Accounts;
using Kino.Movies;

namespace Kino.Pages;

public partial class MoviePage : Page
{
	public List<string> BreadcrumbBarPath { get; } = new() { "Filmy" };
	private Movie _movie;
	
	public MoviePage()
	{
		DataContext = this;
		
		_movie = MoviesManager.Instance.GetMovie(ServiceManager.SelectedMovieId);
		
		BitmapImage bitmap = new();
		bitmap.BeginInit();
		bitmap.StreamSource = MoviesManager.Instance.GetMoviePoster(_movie.PosterPath);
		bitmap.EndInit();
		
		BreadcrumbBarPath.Add(_movie.Title);
		
		InitializeComponent();
		
		MovieTitle.Text = _movie.Title;
		MovieDescription.Text = _movie.Description;
		MovieRatingStars.Value = _movie.Rating;
		MovieRatingCount.Text = $"({_movie.RatingCount} głosów)";
		MoviePoster.ImageSource = bitmap;
	}
	
	public void OnBuyTicketButtonClicked(object sender, RoutedEventArgs e)
	{
		if (!AccountsManager.Instance.IsLoggedIn)
			return;
		
		ServiceManager.SelectedMovieId = _movie.Id;
		ServiceManager.NavigationView.Navigate(typeof(TicketPurchasePage));
	}
}