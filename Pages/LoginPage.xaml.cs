using System.Windows;
using System.Windows.Controls;
using Kino.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;

namespace Kino.Pages;

public partial class LoginPage : Page
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private void OnLoginButtonClick(object sender, RoutedEventArgs e)
	{
		string login = LoginInput.Text;
		string password = PasswordInput.Password;
		
		if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
		{
			ServiceManager.ShowSnackbar("Błąd", "Login i hasło nie mogą być puste.");
			return;
		}
		
		if (!AccountsManager.Instance.Login(login, password))
		{
			ServiceManager.ShowSnackbar("Błąd", "Niepoprawne dane logowania.");
			return;
		}
		
		ServiceManager.NavigationView.Navigate(typeof(MainPage));
	}

	private void OnRegisterButtonClick(object sender, RoutedEventArgs e)
	{
		ServiceManager.NavigationView.Navigate(typeof(RegisterPage));
	}
}