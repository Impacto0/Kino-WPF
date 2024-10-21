using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Kino.Accounts;
using Kino.Movies;
using Kino.Screenings;
using Kino.Seats;
using Kino.Tickets;
using Button = Wpf.Ui.Controls.Button;

namespace Kino.Pages;

public partial class TicketPurchasePage : Page
{
	public Dictionary<Guid, string> Screenings { get; } = new();
	public Dictionary<Guid, string> Seats = new();
	private List<Guid> _selectedSeats = new();
	private Movie _movie;
	
	public TicketPurchasePage()
	{
		DataContext = this;
		
		_movie = MoviesManager.Instance.GetMovie(ServiceManager.SelectedMovieId);
		
		List<Screening> screenings = ScreeningsManager.Instance.GetScreenings(ServiceManager.SelectedMovieId);
		foreach (Screening screening in screenings)
		{
			Screenings.Add(screening.Id, screening.Date.ToString());
		}
		
		BitmapImage bitmap = new();
		bitmap.BeginInit();
		bitmap.StreamSource = MoviesManager.Instance.GetMoviePoster(_movie.PosterPath);
		bitmap.EndInit();
		
		InitializeComponent();
		
		MovieTitle.Text = $"{_movie.Title}";
		MoviePoster.ImageSource = bitmap;
	}

	public void OnAddSeatButtonClick(object sender, RoutedEventArgs e)
	{
		if (_selectedSeats.Count >= 5)
		{
			return;
		}
		
		if (_selectedSeats.Contains(Seats.Keys.ElementAt(SeatSelection.SelectedIndex)))
		{
			return;
		}
		
		_selectedSeats.Add(Seats.Keys.ElementAt(SeatSelection.SelectedIndex));
		UpdateSelectedSeats();
	}
	
	public void OnSeatRemoveButtonClick(Guid seat)
	{
		_selectedSeats.Remove(seat);
		UpdateSelectedSeats();
	}
	
	public void UpdateSelectedSeats()
	{
		SelectedSeats.Items.Clear();
		foreach (Guid seat in _selectedSeats)
		{
			TextBlock textBlock = new();
			textBlock.Text = Seats[seat];
			textBlock.Margin = new Thickness(0, 0, 20, 0);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.FontSize = 18;
			Button button = new();
			TextBlock buttonTextBlock = new();
			buttonTextBlock.Text = "Usuń";
			button.Content = buttonTextBlock;
			button.Click += (sender, e) => OnSeatRemoveButtonClick(seat);
			button.HorizontalAlignment = HorizontalAlignment.Right;
			button.VerticalAlignment = VerticalAlignment.Center;
			button.Margin = new Thickness(0, 0, 0, 0);
			StackPanel stackPanel = new();
			stackPanel.Orientation = Orientation.Horizontal;
			stackPanel.Children.Add(textBlock);
			stackPanel.Children.Add(button);
			SelectedSeats.Items.Add(stackPanel);
		}
	}
	
	public void OnBuyTicketsButtonClick(object sender, RoutedEventArgs e)
	{
		if (!AccountsManager.Instance.IsLoggedIn)
			return;

		Guid screeningId = Screenings.Keys.ElementAt(ScreeningSelection.SelectedIndex);

		if (screeningId == Guid.Empty || _selectedSeats.Count <= 0)
		{
			ServiceManager.ShowSnackbar("Błąd", "Niepoprawne wybrane dane");
			return;
		}
		
		foreach (Guid seatId in _selectedSeats)
		{
			TicketsManager.Instance.CreateTicket(AccountsManager.Instance.CurrentAccount.Id, seatId, _movie.Id, screeningId);
		}
		
		ServiceManager.ShowSnackbar("Sukces", "Pomyślnie zakupiono bilety.");
		ServiceManager.NavigationView.Navigate(typeof(MainPage));
	}

	private void OnScreeningSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		Screening screening = ScreeningsManager.Instance.GetScreening(Screenings.Keys.ElementAt(ScreeningSelection.SelectedIndex));
		
		List<Seat> seats = SeatsManager.Instance.GetSeats();
		foreach (Seat seat in seats)
		{
			if (screening.ReservedSeats.Contains(seat.Id))
				continue;
			
			Seats.Add(seat.Id, $"Rząd {seat.Row + 1}, Miejsce {seat.Column + 1}");
		}
		Seats = Seats.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
		
		SeatSelection.Items.Clear();
		foreach (KeyValuePair<Guid, string> seat in Seats)
		{
			SeatSelection.Items.Add(seat);
		}
	}
}