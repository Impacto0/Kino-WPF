using System.Windows;
using System.Windows.Controls;
using Kino.Accounts;
using Kino.Movies;
using Kino.Screenings;
using Kino.Seats;
using Kino.Tickets;
using Wpf.Ui.Controls;
using Button = Wpf.Ui.Controls.Button;
using ListViewItem = Wpf.Ui.Controls.ListViewItem;
using TextBlock = Wpf.Ui.Controls.TextBlock;

namespace Kino.Pages;

public partial class MyTicketsPage : Page
{
	public MyTicketsPage()
	{
		InitializeComponent();
		Loaded += (sender, args) => DisplayTickets();
	}
	
	public void DisplayTickets()
	{
		if(AccountsManager.Instance.CurrentAccount == null)
			return;
		
		List<Ticket> tickets = TicketsManager.Instance.GetUserTickets(AccountsManager.Instance.CurrentAccount.Id);
		
		TicketsList.Items.Clear();
		
		/*
		 <ui:ListViewItem>
						<ui:Card>
							<Grid>
								<Grid.ColumnDefinitions>
									<ColumnDefinition Width="*"/>
									<ColumnDefinition Width="Auto"/>
								</Grid.ColumnDefinitions>
								<Grid.RowDefinitions>
									<RowDefinition Height="*"/>
									<RowDefinition Height="*"/>
								</Grid.RowDefinitions>
								<ui:TextBlock
									Text="Film"
									FontSize="24"
									Grid.Column="0"
									Grid.Row="0"/>
								<ui:TextBlock
									Text="06-10-2024 12:00"
									FontSize="24"
									Grid.Column="0"
									Grid.Row="1"/>
								<ui:Button
									Grid.Row="0"
									Grid.RowSpan="2"
									Grid.Column="1"
									VerticalAlignment="Center">
									<ui:TextBlock
										FontSize="32"
										Text="Więcej"/>
								</ui:Button>
							</Grid>
						</ui:Card>
					</ui:ListViewItem>
		 */

		foreach (Ticket ticket in tickets)
		{
			ListViewItem item = new ListViewItem();
			Card card = new Card();
			Grid grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			
			Movie movie = MoviesManager.Instance.GetMovie(ticket.MovieId);
			Screening screening = ScreeningsManager.Instance.GetScreening(ticket.ScreeningId);
			
			TextBlock movieTitle = new TextBlock
			{
				Text = movie.Title,
				FontSize = 24,
			};
			movieTitle.SetValue(Grid.ColumnProperty, 0);
			movieTitle.SetValue(Grid.RowProperty, 0);
			grid.Children.Add(movieTitle);
			
			TextBlock date = new TextBlock
			{
				Text = screening.Date.ToString("HH:mm dd-MM-yyyy"),
				FontSize = 24,
			};
			date.SetValue(Grid.ColumnProperty, 0);
			date.SetValue(Grid.RowProperty, 1);
			grid.Children.Add(date);
			
			Button moreButton = new Button
			{
				VerticalAlignment = VerticalAlignment.Center,
			};
			moreButton.SetValue(Grid.RowProperty, 0);
			moreButton.SetValue(Grid.RowSpanProperty, 2);
			moreButton.SetValue(Grid.ColumnProperty, 1);
			TextBlock moreText = new TextBlock
			{
				FontSize = 32,
				Text = "Więcej",
			};
			moreButton.Content = moreText;
			grid.Children.Add(moreButton);
			
			card.Content = grid;
			item.Content = card;
			TicketsList.Items.Add(item);
			
			moreButton.Click += (sender, args) =>
			{
				ShowMoreInfo(ticket);	
			};
		}
	}

	public void ShowMoreInfo(Ticket ticket)
	{
		MoreInfoGrid.Visibility = Visibility.Visible;
		MovieTitle.Text = MoviesManager.Instance.GetMovie(ticket.MovieId).Title;
		DateAndTime.Text = ScreeningsManager.Instance.GetScreening(ticket.ScreeningId).Date.ToString("HH:mm dd-MM-yyyy");
		Seat.Text = SeatsManager.Instance.GetSeat(ticket.SeatId).ToString();
		CancelTicketButton.Click += (sender, args) =>
		{
			TicketsManager.Instance.RemoveTicket(ticket.Id);
			MoreInfoGrid.Visibility = Visibility.Collapsed;
			DisplayTickets();
		};
	}
}