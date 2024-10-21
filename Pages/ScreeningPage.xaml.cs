using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Kino.Movies;
using Kino.Screenings;
using Wpf.Ui.Controls;
using Button = System.Windows.Controls.Button;
using TextBlock = System.Windows.Controls.TextBlock;

namespace Kino.Pages;

public partial class ScreeningPage : Page
{
	public ScreeningPage()
	{
		InitializeComponent();
		
		Loaded += (sender, args) => Update();
	}

	public void Update()
	{
		ScreeningGrid.Children.Clear();
		List<Screening> screenings = ScreeningsManager.Instance.GetScreenings();
		screenings = screenings.OrderBy(x => x.Date).ToList();
		int calculatedRows = (int) Math.Ceiling((double) screenings.Count / 3);
		for (int i = 0; i < calculatedRows; i++)
		{
			ScreeningGrid.RowDefinitions.Add(new RowDefinition());
		}
		for (int i = 0; i < screenings.Count; i++)
		{
			SetupScreeningCard(screenings[i], i / 3, i % 3);
		}
	}
	
	/*
	 <ui:Card
					Grid.Column="2">
					<Grid>
						<Grid.RowDefinitions>
							<RowDefinition Height="Auto"></RowDefinition>
							<RowDefinition Height="Auto"></RowDefinition>
							<RowDefinition Height="*"></RowDefinition>
						</Grid.RowDefinitions>
						<Border
							Height="400"
							Width="290"
							CornerRadius="20">
							<Border.Background>
								<ImageBrush
									ImageSource="C:\Users\Barto\RiderProjects\Kino\Assets\movie-poster.jpg"
									RenderOptions.BitmapScalingMode="HighQuality"
									Stretch="Fill"/>
							</Border.Background>
						</Border>
						<ui:TextBlock
							Grid.Row="1"
							Text="Schachnovelle"
							HorizontalAlignment="Center"
							FontSize="32"></ui:TextBlock>
						<Border 
							Grid.Row="2"
							Height="60">
							<Grid
								Margin="0,10,0,0">
								<Grid.ColumnDefinitions>
									<ColumnDefinition Width="*"></ColumnDefinition>
									<ColumnDefinition Width="*"></ColumnDefinition>
								</Grid.ColumnDefinitions>
								<ui:RatingControl 
									HalfStarEnabled="True"
									HorizontalAlignment="Center"
									IsHitTestVisible="False"
									Value="4"></ui:RatingControl>
								<ui:Button
									Grid.Column="1"
									Width="100"
									Height="40"
									FontSize="24"
									HorizontalAlignment="Center"
									Content="Więcej"></ui:Button>
							</Grid>
						</Border>
					</Grid>
				</ui:Card>
	 */
	public void SetupScreeningCard(Screening screening, int row, int column)
	{
		Card card = new Card();
		Grid grid = new Grid();
		RowDefinition row1 = new RowDefinition();
		row1.Height = GridLength.Auto;
		RowDefinition row2 = new RowDefinition();
		row2.Height = GridLength.Auto;
		RowDefinition row3 = new RowDefinition();
		row3.Height = new GridLength(1, GridUnitType.Star);
		grid.RowDefinitions.Add(row1);
		grid.RowDefinitions.Add(row2);
		grid.RowDefinitions.Add(row3);
		Border posterBorder = new Border();
		posterBorder.Height = 400;
		posterBorder.Width = 290;
		posterBorder.CornerRadius = new CornerRadius(20);
		ImageBrush posterBrush = new ImageBrush();
		posterBrush.ImageSource = MoviesManager.Instance.GetMoviePosterBitmap(MoviesManager.Instance.GetMovie(screening.MovieId).PosterPath);
		posterBrush.Stretch = Stretch.Fill;
		posterBorder.Background = posterBrush;
		TextBlock titleTextBlock = new TextBlock();
		titleTextBlock.Text = MoviesManager.Instance.GetMovie(screening.MovieId).Title;
		titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
		titleTextBlock.FontSize = 32;
		Border ratingBorder = new Border();
		ratingBorder.Height = 60;
		Grid ratingGrid = new Grid();
		ratingGrid.Margin = new Thickness(0, 10, 0, 0);
		ColumnDefinition ratingColumn1 = new ColumnDefinition();
		ratingColumn1.Width = new GridLength(1, GridUnitType.Star);
		ColumnDefinition ratingColumn2 = new ColumnDefinition();
		ratingColumn2.Width = new GridLength(1, GridUnitType.Star);
		ratingGrid.ColumnDefinitions.Add(ratingColumn1);
		ratingGrid.ColumnDefinitions.Add(ratingColumn2);
		RatingControl ratingControl = new RatingControl();
		ratingControl.HalfStarEnabled = true;
		ratingControl.HorizontalAlignment = HorizontalAlignment.Center;
		ratingControl.IsHitTestVisible = false;
		ratingControl.Value = MoviesManager.Instance.GetMovie(screening.MovieId).Rating;
		Button moreButton = new Button();
		moreButton.Content = "Więcej";
		moreButton.FontSize = 24;
		moreButton.Width = 100;
		moreButton.Height = 40;
		moreButton.HorizontalAlignment = HorizontalAlignment.Center;
		moreButton.SetValue(Grid.ColumnProperty, 1);
		moreButton.VerticalAlignment = VerticalAlignment.Center;
		moreButton.Margin = new Thickness(0, 0, 0, 0);
		ratingGrid.Children.Add(ratingControl);
		ratingGrid.Children.Add(moreButton);
		
		posterBorder.SetValue(Grid.RowProperty, 0);
		titleTextBlock.SetValue(Grid.RowProperty, 1);
		ratingBorder.SetValue(Grid.RowProperty, 2);
		ratingGrid.SetValue(Grid.ColumnProperty, 1);
		ratingBorder.Child = ratingGrid;
		moreButton.Click += (sender, args) =>
		{
			ServiceManager.SelectedMovieId = screening.MovieId;
			ServiceManager.NavigationView.Navigate(typeof(TicketPurchasePage));
		};
		grid.Children.Add(posterBorder);
		grid.Children.Add(titleTextBlock);
		grid.Children.Add(ratingBorder);
		card.Content = grid;
		card.SetValue(Grid.ColumnProperty, column);
		card.SetValue(Grid.RowProperty, row);
		card.Margin = new Thickness(30);
		ScreeningGrid.Children.Add(card);
	}
}