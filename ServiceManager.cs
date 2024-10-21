using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Kino;

public class ServiceManager
{
	public static INavigationView NavigationView { get; set; }
	public static ISnackbarService SnackbarService { get; set; }
	public static IconElement SnackbarIcon = new SymbolIcon(SymbolRegular.Info16);
	public static Guid SelectedMovieId { get; set; }
	
	public static void ShowSnackbar(string title, string message)
	{
		SnackbarService.Show(title, message, ControlAppearance.Secondary, SnackbarIcon, TimeSpan.FromMilliseconds(2500));
	}
}