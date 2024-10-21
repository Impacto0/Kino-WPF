using System.Windows;
using Kino.Accounts;
using Kino.Pages;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace Kino;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
	public MainWindow()
	{
		DataContext = this;
		
		ServiceManager.SnackbarService = new SnackbarService();
		
		InitializeComponent();
		
		ServiceManager.SnackbarService.SetSnackbarPresenter(SnackbarPresenter);
		
		Loaded += (_, _) =>
		{
			RootNavigation.Navigate(typeof(MainPage));
		};
		
		RootNavigation.Navigated += (_, e) =>
		{
			UpdateAccountInfo();
		};
		
		ServiceManager.NavigationView = RootNavigation;
		
		AccountsManager.Instance.LoggedIn += (_, _) => UpdateAccountInfo();
	}
	
	private void UpdateAccountInfo()
	{
		if(AccountsManager.Instance.CurrentAccount == null)
		{
			LoginItem.Visibility = Visibility.Visible;
			AccountItem.Visibility = Visibility.Collapsed;
			AdminItem.Visibility = Visibility.Collapsed;
			LogoutItem.Visibility = Visibility.Collapsed;
		}
		else
		{
			if (AccountsManager.Instance.CurrentAccount?.HasAdminPermissions == true)
				AdminItem.Visibility = Visibility.Visible;
			
			AccountItem.Visibility = Visibility.Visible;
			LogoutItem.Visibility = Visibility.Visible;
			LoginItem.Visibility = Visibility.Collapsed;
		}
	}
	
	public void OnLogoutButtonClick(object sender, RoutedEventArgs e)
	{
		AccountsManager.Instance.CurrentAccount = null;
		RootNavigation.Navigate(typeof(MainPage));
		UpdateAccountInfo();
	}
}