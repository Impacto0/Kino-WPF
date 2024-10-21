using System.Windows;
using System.Windows.Controls;
using Kino.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace Kino.Pages;

public partial class RegisterPage : Page
{
	public RegisterPage()
	{
		InitializeComponent();
	}

	private void OnLoginButtonClick(object sender, RoutedEventArgs e)
	{
		ServiceManager.NavigationView.Navigate(typeof(LoginPage));
	}

	private void OnRegisterButtonClick(object sender, RoutedEventArgs e)
	{
		string login = LoginInput.Text;
		string password = PasswordInput.Password;
		
		
		if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
		{
			ServiceManager.ShowSnackbar("Błąd", "Login i hasło nie mogą być puste.");
			return;
		}

		if (AccountsManager.Instance.GetAccount(login) != null)
		{
			ServiceManager.ShowSnackbar("Błąd", "Konto o podanym loginie już istnieje.");
			return;
		}
		
		AccountsManager.Instance.CreateAccount(login, password, false);
		ServiceManager.NavigationView.Navigate(typeof(LoginPage));
	}
}