using System.Windows;
using System.Windows.Controls;
using Kino.Movies;
using Kino.Screenings;
using Microsoft.Win32;

namespace Kino.Pages;

public partial class AdminPage : Page
{
	public string SelectedPosterPath { get; set; }
	private Dictionary<Guid, string> _screeningNames = new Dictionary<Guid, string>();
	
	public AdminPage()
	{
		InitializeComponent();
		Loaded += (sender, args) => Update();
	}

	private void Update()
	{
		List<Movie> movies = MoviesManager.Instance.GetMovies();
		Dictionary<Guid, string> movieNames = new Dictionary<Guid, string>();
		foreach (Movie movie in movies)
		{
			movieNames.Add(movie.Id, movie.Title);
		}
		movieNames = movieNames.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
		AddScreeningMovieComboBox.Items.Clear();
		DeleteMovieComboBox.Items.Clear();
		foreach (KeyValuePair<Guid, string> movie in movieNames)
		{
			AddScreeningMovieComboBox.Items.Add(movie.Value);
			DeleteMovieComboBox.Items.Add(movie.Value);
		}
		
		List<Screening> screenings = ScreeningsManager.Instance.GetScreenings();
		_screeningNames.Clear();
		foreach (Screening screening in screenings)
		{
			_screeningNames.Add(screening.Id, $"{MoviesManager.Instance.GetMovie(screening.MovieId).Title} - {screening.Date.ToString("HH:mm dd-MM-yyyy")}");
		}
		DeleteScreeningComboBox.Items.Clear();
		foreach (KeyValuePair<Guid, string> screening in _screeningNames)
		{
			DeleteScreeningComboBox.Items.Add(screening.Value);
		}
	}

	private void OnDeleteMovieButtonClick(object sender, RoutedEventArgs e)
	{
		string selectedMovie = DeleteMovieComboBox.SelectedItem.ToString();
		Guid movieId = MoviesManager.Instance.GetMovie(selectedMovie).Id;
		MoviesManager.Instance.DeleteMovie(movieId);
		
		ServiceManager.ShowSnackbar("Sukces", "Film został usunięty.");
		Update();
	}

	private void OnAddMovieImageButtonClick(object sender, RoutedEventArgs e)
	{
		FileDialog fileDialog = new OpenFileDialog();
		fileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
		if (fileDialog.ShowDialog() == true)
		{
			SelectedPosterPath = fileDialog.FileName;
		}
		
		ServiceManager.ShowSnackbar("Sukces", "Zdjęcie zostało dodane.");
	}
	
	private void OnAddMovieButtonClick(object sender, RoutedEventArgs e)
	{
		Movie movie = new Movie
		{
			Title = AddMovieTitleInput.Text,
			Description = AddMovieDescriptionInput.Text,
			PosterPath = SelectedPosterPath.Split('\\').Last(),
			Rating = 0,
			RatingCount = 0,
		};
		MoviesManager.Instance.AddMovie(movie);
		MoviesManager.Instance.UploadMoviePoster(SelectedPosterPath);
		
		ServiceManager.ShowSnackbar("Sukces", "Film został dodany.");
		Update();
	}

	private void AddNewScreeningButtonClick(object sender, RoutedEventArgs e)
	{
		Guid movieId = MoviesManager.Instance.GetMovie(AddScreeningMovieComboBox.SelectedItem.ToString()).Id;
		DateTime date = AddScreeningDatePicker.SelectedDate.Value;
		TimeSpan time = TimeSpan.Parse(AddScreeningTimePicker.Text);
		
		if (movieId == Guid.Empty || date == null || time == null)
		{
			ServiceManager.ShowSnackbar("Błąd", "Wypełnij wszystkie pola.");
			return;
		}
		
		date = date.Add(time);
		
		Screening screening = new Screening
		{
			Id = Guid.NewGuid(),
			MovieId = movieId,
			Date = date,
		};
		ScreeningsManager.Instance.AddScreening(screening);
		
		ServiceManager.ShowSnackbar("Sukces", "Seans został dodany.");
		Update();
	}

	private void DeleteScreeningButtonClick(object sender, RoutedEventArgs e)
	{
		string selectedScreening = DeleteScreeningComboBox.SelectedItem.ToString();
		Guid screeningId = ScreeningsManager.Instance.GetScreening(_screeningNames.FirstOrDefault(x => x.Value == selectedScreening).Key).Id;
		
		if (screeningId == Guid.Empty)
		{
			ServiceManager.ShowSnackbar("Błąd", "Wybierz seans.");
			return;
		}
		
		ScreeningsManager.Instance.RemoveScreening(screeningId);
		
		ServiceManager.ShowSnackbar("Sukces", "Seans został usunięty.");
		Update();
	}
}